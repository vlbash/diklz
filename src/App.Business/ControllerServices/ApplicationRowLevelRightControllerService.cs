using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using App.Core.Business.Services;
using App.Core.Common.Services;
using App.Core.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Business.Services.ControllerServices
{
    public class ApplicationRowLevelRightControllerService
    {
        private readonly ReflectionService _reflectionService;
        private readonly CoreDbContext _context; 
        public ICommonDataService DataService { get; }

        public ApplicationRowLevelRightControllerService(ICommonDataService dataService, CoreDbContext context,
            ReflectionService reflectionService)
        {
            DataService = dataService;
            _context = context;
            _reflectionService = reflectionService;
        }

        public List<SelectListItem> GetEntityListNames()
        {
            var models = _context.GetApplicationModels()
                .ToDictionary(x => x.Name, x => x.GetCustomAttribute<DisplayAttribute>()?.Name);

            var entityListNames = new List<SelectListItem>();
            foreach (var item in models)
            {
                entityListNames.Add(new SelectListItem
                {
                    Value = item.Key,
                    Text = item.Value == null ? $"{item.Key}" : $"{item.Value} ({item.Key})"
                });
            }

            return entityListNames;
        }
    }
}
