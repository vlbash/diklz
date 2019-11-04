using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.LimsService;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.P902;
using App.Data.Models.P902;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using App.Business.Services.P902Services;
using App.Core.Business.Services.ObjectMapper;
using App.Data.DTO.CRV;
using App.Data.Models.CRV;
using Serilog;


namespace App.Host.Areas.P902.Controllers
{
    [Authorize]
    [Area("P902")]
    public class ResultInputControlController : CommonController<ResultInputControlListDTO, ResultInputControlDetailsDTO, ResultInputControl>
    {
        private readonly ResultInputControlService _service;
        private readonly IObjectMapper _objectMapper;

        public ResultInputControlController(IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, ResultInputControlService service, IObjectMapper objectMapper)
            : base(service.DataService, configuration, searchFilterSettingsService)
        {
            _service = service;
            _objectMapper = objectMapper;
        }

        public override async Task<IActionResult> Edit(Guid? id)
        {
            return await base.Edit(id, new Dictionary<string, string>(), _service.GetEditFunction);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, ResultInputControlDetailsDTO model)
        {
            if (model.Id != id)
                return NotFound();
            if (ModelState.IsValid)
            {
                var entity = new ResultInputControl();
                _objectMapper.Map(model, entity);
                DataService.Add(entity);
                await DataService.SaveChangesAsync();
                try
                {
                    await _service.InsertSgdRepDrugList(entity);
                }
                catch (Exception e)
                {
                    DataService.Remove(entity);
                    await DataService.SaveChangesAsync();
                    Log.Error($"Lims import error, {e.Message}");
                    return NotFound();
                }
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeSendCheck(Guid entityId)
        {
            bool success;
            try
            {
                success = await _service.ChangeCheck(entityId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            if (success)
                return Json(new { success = success});
            return StatusCode(500);
        }

        public async Task<IActionResult> SendResult()
        {
            bool success;
            try
            {
                success = await _service.UpdateStatus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            if (success)
                return Json(new { success = success });
            return StatusCode(500);
        }

        [HttpPost]
        public JsonResult AutoCompleteRp(string term)
        {
            var result = DataService.GetDto<LimsRPMinDTO>(
                    p => !string.IsNullOrEmpty(term),
                    extraParameters: new object[] { $"where lower(lims_rp.reg_num) LIKE lower('%{term}%') LIMIT 10" })
                .Select(p => new { value = p.RegNum, option = p.Id }).ToList();
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetLimsRp(Guid id)
        {
            return Json((await DataService.GetDtoAsync<LimsDetailsRPDTO>(x => x.Id == id)).FirstOrDefault());
        }
    }
}
