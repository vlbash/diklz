using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.ImlServices;
using App.Business.Services.NotificationServices;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.IML;
using App.Data.Enums;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace App.Host.Areas.IML.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Iml")]
    public class ImlAppController: CommonController<ImlAppDetailDTO, ImlApplication>
    {
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly ImlApplicationService _service;
        private readonly IAtuAddressService _atuAddressService;
        private readonly ApplicationService<ImlApplication> _applicationService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificaticationService;

        public ImlAppController(IConfiguration configuration, ISearchFilterSettingsService filterSettingsService,
            IEntityStateHelper entityStateHelper, ImlApplicationService service, IAtuAddressService atuAddressService,
            ApplicationService<ImlApplication> applicationService, INotificationService notificaticationService)
            : base(service.DataService, configuration,
            filterSettingsService)
        {
            _configuration = configuration;
            _entityStateHelper = entityStateHelper;
            _service = service;
            _atuAddressService = atuAddressService;
            _applicationService = applicationService;
            _notificaticationService = notificaticationService;
        }

        [NonAction]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        [BreadCrumb(Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            var appSort = (await _service.DataService.GetDtoAsync<AppShortDTO>(x => x.Id == id)).Select(x => x.AppSort).FirstOrDefault();
            if (string.IsNullOrEmpty(appSort))
                return await Task.Run(() => NotFound());
            if (appSort != "GetLicenseApplication" && appSort != "AdditionalInfoToLicense" && appSort != "IncreaseToIMLApplication")
                return RedirectToAction("AltAppDetails", "ImlAppAlt", new { Area = "IML", id = id, sort = appSort });
            if (appSort == "GetLicenseApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");
            if (appSort == "AdditionalInfoToLicense")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Доповнення інформації по наявній ліцензії");
            if (appSort == "IncreaseToIMLApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності" +
                                                                  " - Розширення до імпорту лікарських засобів");
            ViewBag.IsEditable = _entityStateHelper.IsEditableApp(id);

            return await base.Details(id);
        }

        public override async Task<IActionResult> Edit(Guid? id)
        {
            return NotFound();
        }

        [BreadCrumb(Title = "Заява на отримання ліцензії з імпорта", Order = 2)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, IDictionary<string, string> paramList)
        {
            return await base.Edit(id,
                paramList, _service.GetEditPortal);
        }

        public override async Task<IActionResult> Edit(Guid id, ImlAppDetailDTO model)
        {
            if (model.OrgType == OrgType.FOP)
            {
                if (ModelState.ContainsKey("EDRPOU"))
                    ModelState["EDRPOU"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("OrgDirector"))
                    ModelState["OrgDirector"].ValidationState = ModelValidationState.Valid;
                if (model.PassportNumber.Length != 6 && model.PassportNumber.Length != 9)
                {
                    ModelState.AddModelError("PassportNumber", "Довжина номеру паспорту має бути 6, або 9 (при викориристанні ID-картки)");
                }
                if (string.IsNullOrEmpty(model.PassportSerial))
                {
                    if (model.PassportNumber.Length != 9)
                    {
                        ModelState.AddModelError("PassportSerial", "Поле необхідне для заповнення (при використанні паспорту)");
                    }
                }
            }
            else
            {
                if (ModelState.ContainsKey("INN"))
                    ModelState["INN"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportSerial"))
                    ModelState["PassportSerial"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportNumber"))
                    ModelState["PassportNumber"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportDate"))
                    ModelState["PassportDate"].ValidationState = ModelValidationState.Valid;
                if (ModelState.ContainsKey("PassportIssueUnit"))
                    ModelState["PassportIssueUnit"].ValidationState = ModelValidationState.Valid;
            }
            if (ModelState.ContainsKey("EconomicClassificationType"))
                ModelState["EconomicClassificationType"].ValidationState = ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                // ATU
                var newSubjAddress = new SubjectAddress
                {
                    StreetId = model.StreetId,
                    PostIndex = model.PostIndex,
                    Building = model.Building,
                };
                if (!_atuAddressService.SaveAddress(newSubjAddress))
                {
                    ModelState.AddModelError("", "Вулиця не знайдена у довіднику");
                }
                else
                {
                    model.AddressId = newSubjAddress.Id;
                }
            }
            if (ModelState.IsValid)
            {
                var appId = await _service.SaveApplication(model);
                return RedirectToAction("Details", new { id = appId });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCheckBox(string checkBoxId, Guid appId)
        {
            var success = await _applicationService.ChangeCheckBox(checkBoxId, appId);
            if (success)
                return Json(new { success = success });
            return StatusCode(500);

        }

        [HttpPost]
        public async Task<IActionResult> SaveComment(string text, Guid appId)
        {
            var success = await _applicationService.SaveComment(text, appId);
            if (success)
                return Json(new { success = success });
            return StatusCode(500);
        }
        [HttpGet]
        public async Task<JsonResult> GetFilesForSign(Guid id)
        {
            if (id == Guid.Empty)
                return new JsonResult(StatusCode(404));
            var filesModel = await _service.GetFilesForSign(id);

            return new JsonResult(filesModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSignedFiles(FilesSignViewModel model)
        {
            if (model.id == Guid.Empty)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Index", "Home", new { Area = "" }) });
            if (model.files.Count == 0)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Details", "ImlApp", new { Area = "APP" }) });

            try
            {
                await _service.SubmitApplication(_configuration, model);
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Details", "ImlApp", new { Area = "IML", id = model.id }) });
            }

            try
            {
                var app = DataService.GetEntity<ImlApplication>(x => x.Id == model.id)?.FirstOrDefault();
                await _notificaticationService.PrlCreateNotificationAppSend(app.Id, app.AppSort, DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")), app.OrgUnitId);
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка з відправкою сповіщення на електронну пошту", returnUrl = Url.Action("Index", "Monitoring", new { Area = "MTR" }) });
            }

            return new JsonResult(new { success = true, returnUrl = Url.Action("Index", "Monitoring", new { Area = "MTR" }) });
        }
        public IActionResult SendPayment(Guid appId)
        {
            var result = _applicationService.ChangePaymentStatus(appId, "WaitingForConfirmation");
            if (!result)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> GetImlApplicationReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            byte[] file;

            var application = (await DataService.GetDtoAsync<AppStateDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }

            var sort = application.AppSort;

            try
            {
                file = await _service.GetApplicationFile(id, sort);
            }

            catch (Exception e)
            {
                Log.Error(e.Message);
                return await Task.Run(() => NotFound());
            }

            return File(file, "application/pdf");
        }
    }
}
