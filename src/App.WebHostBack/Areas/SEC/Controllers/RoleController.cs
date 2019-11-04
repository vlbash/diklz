using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.ControllerServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Core.Security.Entities;
using App.Data.DTO.SEC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace App.Host.Areas.SEC.Controllers
{
    [Area("Sec")]
    [Authorize]
    public class RoleController: CommonController<RoleListDTO, RoleListDTO, Role>
    {
        private readonly RoleControllerService _service;

        public RoleController(RoleControllerService controllerService,
            IConfiguration configuration,
            ISearchFilterSettingsService searchFilterSettingsService) : base(controllerService.DataService, configuration, searchFilterSettingsService)
        {
            _service = controllerService;
        }

        [HttpGet]
        public async Task<IActionResult> MakeCopy(Guid id)
        {
            var model = (await DataService.GetDtoAsync<RoleListDTO>(x => x.Id == id)).FirstOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            model.Caption = $"{model.Caption} (Copy)";

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MakeCopy(RoleListDTO model)
        {
            if (model.Id == Guid.Empty)
            {
                return NotFound();
            }

            var savedModel = await _service.GetSavedModelAsync(model);

            return View("Details", savedModel);
        }

        #region RoleRights

        public async Task<IActionResult> RoleRights(Guid roleId)
        {
            ViewBag.RoleId = roleId;

            var rightList = (await DataService.GetDtoAsync<RightListDTO>())
                .Select(x => new {rightId = x.Id, rightName = x.Caption, isActive = x.IsActive});
            var serializerSettings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };
            ViewBag.RightList = JsonConvert.SerializeObject(rightList, serializerSettings);
            
            return PartialView("RoleRightsPartial");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IEnumerable<RoleRightListDTO>> GetRoleRights(Guid roleId)
        {
            var data = await DataService.GetDtoAsync<RoleRightListDTO>(fr => fr.RoleId == roleId);
            return data;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> InsertRoleRight([FromBody] RoleRightListDTO item)
        {
            if (!ModelState.IsValid)
            {
                var errorsQuery = (from modelStateItem in ModelState
                    where modelStateItem.Value.Errors.Any()
                    select modelStateItem.Value.Errors[0].ErrorMessage);

                var errorMessage = "Виникла помилка під час перевірки данних:";
                foreach (var error in errorsQuery)
                {
                    errorMessage += "\n" + error;
                }
                return BadRequest(errorMessage);
            }

            var right = (await DataService.GetDtoAsync<RoleRightListDTO>(x => x.Id == item.RightId)).FirstOrDefault();

            try
            {
                item.Id = DataService.Add<RoleRight>(item);
                await DataService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }

            if (right!=null)
            {
                item.IsActive = right.IsActive;
            }
            
            return Ok(item);
        }

        [HttpDelete]
        public async Task DeleteRoleRight(Guid id)
        {
            if (id != Guid.Empty)
            {
                DataService.Remove<RoleRight>(id);
                await DataService.SaveChangesAsync();
            }
        }

        #endregion
    }
}
