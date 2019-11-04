using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Common.Services;
using App.Core.Data;
using App.Core.Data.CustomAudit;
using App.Data.DTO.LOG;
using Microsoft.AspNetCore.Mvc.Rendering;
using Z.EntityFramework.Plus;

namespace App.Business.Services.ControllerServices
{
    public class LogAuditListControllerService
    {
        private readonly ReflectionService _reflectionService;
        private readonly CoreDbContext _context;
        public ICommonDataService DataService { get; }

        public LogAuditListControllerService(ICommonDataService dataService, CoreDbContext context,
            ReflectionService reflectionService)
        {
            DataService = dataService;
            _context = context;
            _reflectionService = reflectionService;
        }

        public Tuple<List<SelectListItem>, List<SelectListItem>> GetNamesStatesList()
        {
            var models = _context.GetApplicationModels()
                .ToDictionary(x => x.Name, x => x.GetCustomAttribute<DisplayAttribute>()?.Name);
            var entityListNames = new List<SelectListItem>();
            foreach (var item in models)
            {
                if (item.Key == "AuditEntry" || item.Key == "AuditEntryProperty")
                {
                    if (item.Key == "AuditEntry")
                    {
                        entityListNames.Add(new SelectListItem
                        {
                            Value = item.Key,
                            Text = $"Реєстр аудиту ({item.Key})"
                        });
                    }
                    else
                    {
                        entityListNames.Add(new SelectListItem
                        {
                            Value = item.Key,
                            Text = $"Реєстр полей аудиту ({item.Key})"
                        });

                    }
                }
                else
                {
                    entityListNames.Add(new SelectListItem
                    {
                        Value = item.Key,
                        Text = item.Value == null ? $"{item.Key}" : $"{item.Value} ({item.Key})"
                    });
                }
            }
            var states = new List<SelectListItem>();
            foreach (var item in AuditHelper.AuditEntryStateUa)
            {
                states.Add(new SelectListItem() { Value = item.Key.ToString(), Text = item.Value });
            }

            return new Tuple<List<SelectListItem>, List<SelectListItem>>(entityListNames, states);
        }

        public Dictionary<string, string> GetModels()
        {
            var models = GetContexModelsWithAuditDisplayName();
            return models;
        }

        public async Task<LogAuditEntryListDTO> GetModel(int? id)
        {
            var model = (await DataService.GetDtoAsync<LogAuditEntryListDTO>(x => x.AuditEntryId == id.Value))
                .FirstOrDefault();
            return model;
        }

        private Dictionary<string, string> GetContexModelsWithAuditDisplayName()
        {
            return _context.GetApplicationModels()
                .ToDictionary(x => x.Name, x => x.GetCustomAttribute<AuditDisplayAttribute>()?.Name);
        }
    }
}
