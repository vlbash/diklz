using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Core.Business.Services;
using App.Core.Common.Services;
using App.Core.Data;
using App.Core.Security;
using App.Data.DTO.SEC;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Business.Services.ControllerServices
{
    public class RightControllerService
    {
        private readonly ReflectionService _reflectionService;
        private readonly CoreDbContext _context;
        public ICommonDataService DataService { get; }

        public RightControllerService(ICommonDataService dataService, CoreDbContext context,
            ReflectionService reflectionService)
        {
            DataService = dataService;
            _context = context;
            _reflectionService = reflectionService;
        }

        public List<SelectListItem> GetEntityListNames (Guid? id)
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

            return entityListNames;
        }

        public IEnumerable<dynamic> GetAccesTypeList()
        {
            var accesTypeList = from EntityAccessLevel e in Enum.GetValues(typeof(EntityAccessLevel))
                select new
                {
                    Id = (int)e,
                    Name = e.ToString()
                };
            return accesTypeList;
        }

        public async Task<IEnumerable<dynamic>> GetModelProperties(Guid rightId)
        {
            var entityName = (await DataService.GetDtoAsync<RightDetailDTO>(r => r.Id == rightId)).FirstOrDefault()?.EntityName;
            var modelType = _context.GetApplicationModels()
                .FirstOrDefault(m => m.Name == entityName);

            var modelProperties = _reflectionService.GetTypeProperties(modelType)
                .Where(prop =>
                    (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                    && prop.GetCustomAttribute<NotMappedAttribute>(true) == null)
                .Select(x => new { Id = x.Name, Caption = x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? x.Name });
            return modelProperties;
        }
    }
}
