using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.ControllerServices;
using App.Core.Business.Services;
using App.Core.Data;
using App.Core.Mvc.Controllers;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Host.Areas.SEC.Controllers
{
    [Area("Sec")]
    [Authorize]
    public class RightController: CommonController<RightListDTO, RightDetailDTO, Core.Security.Entities.Right>
    {
        private readonly RightControllerService _service;
        public RightController(RightControllerService controllerService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService) : base(controllerService.DataService, configuration, searchFilterSettingsService)
        {
            _service = controllerService;
        }

        [HttpGet]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            var entityListNames = _service.GetEntityListNames(id);
            ViewBag.EntityListNames = new SelectList(entityListNames, "Value", "Text");

            return await base.Edit(id);
        }

        #region FieldRights
        public async Task<IActionResult> FieldRights(Guid rightId, [FromServices] CoreDbContext context)
        {
            var modelProperties = await _service.GetModelProperties(rightId);
            var accesTypeList = _service.GetAccesTypeList();
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            ViewBag.AccesList = JsonConvert.SerializeObject(accesTypeList, serializerSettings);
            ViewBag.FieldList = JsonConvert.SerializeObject(modelProperties, serializerSettings);
            ViewBag.RightId = rightId;

            return PartialView("FieldRightsPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<FieldRightListDTO>> GetFieldRights(Guid rightId)
        {
            var data = await DataService.GetDtoAsync<FieldRightListDTO>(fr => fr.RightId == rightId);
            return data;
        }

        [HttpPut]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateFieldRight([FromBody] FieldRightListDTO item)
        {
            return await InsertFieldRight(item);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertFieldRight([FromBody] FieldRightListDTO item)
        {
            if (!ModelState.IsValid) {
                var errorsQuery = (from modelStateItem in ModelState
                                   where modelStateItem.Value.Errors.Any()
                                   select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery) {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            item.Caption = item.FieldName + ": " + item.AccessLevel.ToString();

            try {
                item.Id = DataService.Add<FieldRight>(item);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            return Ok(item);
        }

        [HttpDelete]
        public void DeleteFieldRight(Guid id)
        {
            if (id != Guid.Empty) {
                DataService.Remove<FieldRight>(id);
                DataService.SaveChangesAsync();
            }
        }
        #endregion
    }
}
