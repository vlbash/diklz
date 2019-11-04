using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AtuService;
using App.Business.Services.LimsService;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Business.Services.NotificationServices;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Data.DTO.APP;
using App.Data.DTO.PRL;
using App.Data.Enums;
using App.Data.Models.PRL;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using App.Business.Services.AppServices;

namespace App.Host.Areas.PRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Prl")]
    public class PrlAppController: BaseController<PrlAppDetailDTO, PrlApplication>
    {
        private readonly LimsExchangeService _limsExchangeService;
        private readonly ICommonDataService _dataService;
        private readonly UserInfo _userInfo;
        private readonly IPrlApplicationService _prlApplicationService;
        private readonly IConverter _converter;
        private readonly IPrlReportService _prlReportService;
        private readonly PrlApplicationProcessService _applicationProcessService;
        private readonly IAtuAddressService _atuAddressService;
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly INotificationService _notificationService;
        private readonly ApplicationService<PrlApplication> _applicationService;

        public PrlAppController(IUserInfoService userInfoService, ICommonDataService dataService,
            IPrlApplicationService prlApplicationService, IConverter converter, PrlApplicationProcessService applicationProcessService,
            IPrlReportService prlReportService, IAtuAddressService atuAddressService, IEntityStateHelper entityStateHelper,
            INotificationService notificationService, LimsExchangeService limsExchangeService, ApplicationService<PrlApplication> applicationService)
            : base()
        {
            _userInfo = userInfoService.GetCurrentUserInfo();
            _dataService = dataService;
            _prlApplicationService = prlApplicationService;
            _converter = converter;
            _applicationProcessService = applicationProcessService;
            _atuAddressService = atuAddressService;
            _entityStateHelper = entityStateHelper;
            _prlReportService = prlReportService;
            _notificationService = notificationService;
            _limsExchangeService = limsExchangeService;
            _applicationService = applicationService;
        }

        [NonAction]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        [BreadCrumb(Order = 2)]
        public override async Task<IActionResult> Details(Guid? id)
        {
            var appSort = (await _dataService.GetDtoAsync<AppShortDTO>(x => x.Id == id)).Select(x => x.AppSort).FirstOrDefault();
            if (string.IsNullOrEmpty(appSort))
                return await Task.Run(() => NotFound());
            if (appSort != "GetLicenseApplication" && appSort != "AdditionalInfoToLicense" && appSort != "IncreaseToPRLApplication")
            {
                return RedirectToAction("AltAppDetails", "PrlAppAlt", new { Area = "PRL", id = id, sort = appSort });
            }
            if (appSort == "GetLicenseApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");
            if (appSort == "AdditionalInfoToLicense")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Доповнення інформації по наявній ліцензії");
            if (appSort == "IncreaseToPRLApplication")
                HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності - Розширення до виробництва лікарських засобів");
            ViewBag.IsEditable = _entityStateHelper.IsEditableApp(id);

            return await base.Details(id);
        }

        public override async Task<IActionResult> Edit(Guid? id)
        {
            return NotFound();
        }

        [BreadCrumb(Order = 2)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, string sort = "GetLicenseApplication")
        {
            if (id == null || id == Guid.Empty)
            {
                base.OnGetEdit += model =>
                {
                    model.AppSort = sort;
                    if (!string.IsNullOrEmpty(_userInfo?.EDRPOU()))
                    {
                        model.EDRPOU = _userInfo?.EDRPOU();
                        model.OrgName = _userInfo?.OrganizationName();
                    }
                    else
                    {
                        model.INN = _userInfo?.INN();
                        model.OrgName = _userInfo?.FullName();
                    }
                };
                if (sort == "GetLicenseApplication")
                    HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");
                if (sort == "IncreaseToPRLApplication")
                    HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності - Розширення до виробництва лікарських засобів");
            }
            else
            {
                var appSort = (await _dataService.GetDtoAsync<AppShortDTO>(x => x.Id == id)).Select(x => x.AppSort).FirstOrDefault();

                if (appSort == "GetLicenseApplication")
                    HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про отримання ліцензії на провадження діяльності");

                if (appSort == "AdditionalInfoToLicense")
                    HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Доповнення інформації по наявній ліцензії");

                if (sort == "IncreaseToPRLApplication")
                    HttpContext.ModifyCurrentBreadCrumb(x => x.Name = "Заява про розширення провадження виду господарської діяльності - Розширення до виробництва лікарських засобів");

            }

            return await base.Edit(id);
        }

        [BreadCrumb(Order = 2)]
        [HttpGet]
        public async Task<IActionResult> EditAdditionForLicense(Guid? id)
        {
            PrlAppDetailDTO model = null;

            if (id == null || id == Guid.Empty)
            {
                model = new PrlAppDetailDTO();
                var license = (await _limsExchangeService.GetLicenses("PRL", string.IsNullOrEmpty(_userInfo?.EDRPOU()) ? _userInfo.INN() : _userInfo.EDRPOU())).FirstOrDefault();
                model.AppSort = "AdditionalInfoToLicense";
                model.OwnershipType = license.OwnershipType;
                model.LegalFormType = license.LegalFormType;
                if (!string.IsNullOrEmpty(_userInfo?.EDRPOU()))
                {
                    model.EDRPOU = _userInfo?.EDRPOU();
                    model.OrgDirector = license.OrgDirector;
                    model.OrgName = license.OrganizationName;
                }
                else
                {
                    model.INN = _userInfo?.INN();
                    model.OrgName = license.OrganizationName;
                }
            }
            else
            {
                model = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            }

            return View("Edit", model);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, PrlAppDetailDTO model)
        {
            if (id != model.Id)//check
            {
                return NotFound();
            }
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
                var appId = await _prlApplicationService.SaveApplication(model);
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

        public async Task<IActionResult> GetPrlApplicationReport(Guid id)
        {
            if (id == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] file;

            var application = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }

            var sort = application.AppSort;

            try
            {
                switch (sort)
                {
                    case "GetLicenseApplication":
                        file = await rep.CreatePDF($"(Dodatok_02)_Zaiava_pro_otrymannia_litsenzii_na_provadzhennia_diialnosti_z_vyrobnytstva_likarskykh_zasobiv_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(id));
                        break;
                    case "AdditionalInfoToLicense":
                        file = await rep.CreatePDF($"Dopovnennia Informatsii Po Naiavnii Litsenzii_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(id));
                        break;
                    case "CancelLicenseApplication":
                        file = await rep.CreatePDF($"(Dodatok_22)_Zaiava_pro_anuliuvannia_litsenzii_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCancelLicenseApp(id));
                        break;
                    case "RemBranchApplication":
                        file = await rep.CreatePDF($"(Dodatok_16)_Zaiava_pro_vnesennia_zmin_do_YeDR_u_zviazku_z_prypynenniam_diialnosti_MPD_(Zakryttia_MPD)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRemBranchApp(id));
                        break;
                    case "ChangeContrApplication":
                        file = await rep.CreatePDF($"(Dodatok_18)_Zaiava_pro_zminu_informatsii_u_dodatku_(Zmina_kontraktnykh_vyrobnykiv_ta_laboratorii)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlChangeContrApp(id));
                        break;
                    case "ChangeAutPersonApplication":
                        file = await rep.CreatePDF($"(Dodatok_18)_Zaiava_pro_zminu_informatsii_u_dodatku_(Zmina_upovnovazhenykh_osib)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlChangeAutPersonApp(id));
                        break;
                    case "AddBranchInfoApplication":
                        file = await rep.CreatePDF($"(Dodatok_12)_Zaiava_pro_vnesennia_do_YeDR_vidomostei_pro_MPD_(Rozshyrennia_operatsii)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlAddBranchInfoApp(id));
                        break;
                    case "RemBranchInfoApplication":
                        file = await rep.CreatePDF($"(Dodatok_16)_Zaiava_pro_vnesennia_zmin_do_YeDR_u_zviazku_z_prypynenniam_diialnosti_MPD_(Zvuzhennia_operatsii)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRemBranchInfoApp(id));
                        break;
                    case "AddBranchApplication":
                        file = await rep.CreatePDF($"(Dodatok_12)_Zaiava_pro_vnesennia_do_YeDR_vidomostei_pro_MPD_(Dodavannia_MPD)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlAddBranchApp(id));
                        break;
                    //case "RenewLicenseApplication":
                    //    file = await rep.CreatePDF($"(Додаток_14)_Заява_на_переоформлення_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRenewLicenseApp(id));
                    //    // ++
                    //    break;
                    default:
                        return await Task.Run(() => NotFound());
                }
            }

            catch (Exception)
            {
                return await Task.Run(() => NotFound());
            }

            return File(file, "application/pdf");
        }

        //public async Task<IActionResult> SubmitApplication(Guid id)
        //{
        //    if (id == Guid.Empty)
        //        return await Task.Run(() => NotFound());
        //    var app = _dataService.GetEntity<PrlApplication>(x => x.Id == id)?.FirstOrDefault();
        //    if (app == null)
        //        return await Task.Run(() => NotFound());
        //    app.AppState = "Submitted";
        //    app.BackOfficeAppState = "Submitted";
        //    await _dataService.SaveChangesAsync();            
            
        //    return RedirectToAction("Index", "Monitoring", new { Area = "MTR" });
        //}

        [HttpGet]
        public async Task<JsonResult> GetFilesForSign(Guid id)
        {
            if (id == Guid.Empty)
                return new JsonResult(StatusCode(404));
            var filesModel = await _prlApplicationService.GetFilesForSign(id);

            return new JsonResult(filesModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSignedFiles(FilesSignViewModel model)
        {
            if (model.id == Guid.Empty)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Index", "Home", new { Area = "" }) });
            if (model.files.Count == 0)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Details", "PrlApp", new { Area = "APP" }) });

            try
            {
                await _prlApplicationService.SubmitApplication(Configuration, model);
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Edit", "PrlApp", new { Area = "APP" }) });
            }

            try
            {
                var app = _dataService.GetEntity<PrlApplication>(x => x.Id == model.id)?.FirstOrDefault();
                await _notificationService.PrlCreateNotificationAppSend(app.Id, app.AppSort, DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")), app.OrgUnitId);
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
                return Json(new {success = false});
            }

            return Json(new { success = true });
        }
    }
}
