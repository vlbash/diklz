using App.Core.Data.Entities.Common;
using App.Core.Data.Enums;
using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Linq;

namespace App.Core.Data.Helpers
{
    public class DocumentHelper: IDocumentHelper
    {
        private readonly object locker = new object();
        private DbContext _context;

        public DocumentHelper(DbContext context)
        {
            _context = context;
        }

        public string GetRegNumber(string entityName, RegNumberCounterType counterType, NumberCounterPattern pattern)
        {
            lock (locker)
            {
                return GenerateNumber(entityName, counterType, pattern);
            }
        }

        public string InvokeRegNumberGenerator(IDocument document, string entityName)
        {
            var doc = document as IDocumentHelper_GenerateRegNumber;

            if (doc?.GenerateRegNumber != null)
            {
                lock (locker)
                {
                    var counter = GetLastNumberCounter(entityName) ?? new NumberCounter()
                    {
                        EntityName = entityName,
                        CounterType = RegNumberCounterType.Pattern,
                        Pattern = NumberCounterPattern.Undefined,
                        Value = "0"
                    };

                    if (!String.IsNullOrEmpty(counter?.Value))
                    {
                        var newValue = doc.GenerateRegNumber(counter?.Value);
                        if (!String.IsNullOrEmpty(newValue))
                        {
                            counter.Value = newValue;
                            SaveNumberCounter(counter);
                            return newValue;
                        }
                    }
                }
            }

            return null;
        }

        public void SetRegNumber(IDocument document, string entityName = null,
            RegNumberCounterType counterType = RegNumberCounterType.Increment, NumberCounterPattern pattern = NumberCounterPattern.Number)
        {
            if (document.RegNumber == null)
            {
                var _entityName = (document as IDocumentHelper_GenerateRegNumber)?.GetEntityNameKey?.Invoke() ?? entityName ?? document.GetType().Name;
                document.RegNumber = InvokeRegNumberGenerator(document, _entityName) ??
                                     GetRegNumber(_entityName, counterType, pattern);
            }
        }

        private NumberCounter GetLastNumberCounter(string entityName)
        {
            return _context.Set<NumberCounter>().AsNoTracking().FirstOrDefault(x => x.EntityName == entityName);
        }

        private void SaveNumberCounter(NumberCounter res)
        {
            if (res.Id != Guid.Empty)
                _context.Update(res);
            else
                _context.Attach(res);

            _context.SaveChanges();
        }

        private string GenerateNumber(string entityName, RegNumberCounterType counterType, NumberCounterPattern pattern)
        {
            var res = GetLastNumberCounter(entityName);

            if (res == null)
            {
                res = new NumberCounter()
                {
                    EntityName = entityName,
                    CounterType = counterType,
                    Pattern = pattern,
                    Value = "0"
                };
            }

            if (res.Pattern == NumberCounterPattern.Number)
            {
                var n = 0;
                try
                {
                    n = Convert.ToInt32(res.Value);
                }
                catch (Exception ex)
                {
                    Log.Error($"Get number for document {entityName} - {ex.Message} (Value = {res.Value})");
                }

                n += 1;
                res.Value = n.ToString();

                SaveNumberCounter(res);

                return res.Value;
            }
            else if (res.Pattern == NumberCounterPattern.LeadingZero)
            {
                var n = 0;
                try
                {
                    n = Convert.ToInt32(res.Value);
                }
                catch (Exception ex)
                {
                    Log.Error($"Get number for document {entityName} - {ex.Message} (Value = {res.Value})");
                }

                n += 1;
                res.Value = n.ToString().PadLeft(8, '0');

                SaveNumberCounter(res);

                return res.Value;
            }
            else
            {
                //TODO : add method for generating number by pattern
                return res.Value;
            }
        }
    }
}
