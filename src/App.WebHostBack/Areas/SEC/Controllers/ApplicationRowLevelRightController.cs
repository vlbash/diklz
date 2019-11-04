using System;
using System.Threading.Tasks;
using App.Business.Services.ControllerServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.SEC.Controllers
{
    [Area("Sec")]
    [Authorize]
    public class ApplicationRowLevelRightController : 
        CommonController<ApplicationRowLevelRightDTO, ApplicationRowLevelRightDTO, ApplicationRowLevelRight>
    {
        private readonly ApplicationRowLevelRightControllerService _service;

        public ApplicationRowLevelRightController(ApplicationRowLevelRightControllerService controllerService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService) : base(controllerService.DataService, configuration, searchFilterSettingsService)
        {
            _service = controllerService;
        }

        [HttpGet]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            var entityListNames = _service.GetEntityListNames();
            ViewBag.EntityListNames = new SelectList(entityListNames, "Value", "Text");
            return await base.Edit(id);
        }
    }
}
