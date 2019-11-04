
using System;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Business.Services.EmailService;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.PRL;
using App.Data.Models.PRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DinkToPdf.Contracts;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.NotificationServices;
using App.Data.Models.APP;

namespace App.Host.Areas.PRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("PRL")]
    public class PrlAppAltController: CommonController<PrlAppDetailDTO, PrlAppDetailDTO, PrlApplication>
    {
        private ICommonDataService _commonDataService;
        private IPrlApplicationAltService _prlApplicationAltService;
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly INotificationService _notificationService;

        public PrlAppAltController(ICommonDataService commonDataService, IConfiguration configuration, ISearchFilterSettingsService filterSettingsService,
            IPrlApplicationAltService prlApplicationAltService, IEntityStateHelper entityStateHelper)
            : base(commonDataService, configuration, filterSettingsService)
        {
            _commonDataService = commonDataService;
            _prlApplicationAltService = prlApplicationAltService;
            _entityStateHelper = entityStateHelper;
        }

        public async Task<IActionResult> CreateOnOpen(string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return NotFound();
            Guid appId;
            try
            {
                appId = await _prlApplicationAltService.CreateOnOpen(sort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("AltAppDetails", new { id = appId, sort = sort });
        }

        public override async Task<IActionResult> Details(Guid id)
        {
            return await Task.Run(() => NotFound());
        }

        [BreadCrumb(Order = 2)]
        public async Task<IActionResult> AltAppDetails(Guid id, string sort)
        {
            var application = (await _commonDataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(id)[nameof(BaseApplication.AppState)] == "Project";
            switch (sort)
            {
                case "CancelLicenseApplication":
                case "DecreasePRLApplication":
                    if (sort == "CancelLicenseApplication")
                        HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про анулювання ліцензії(імпорт)"));
                    else
                        HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про звуження провадження виду господарської діяльності - Звуження виробництва лікарських засобів"));
                    return View("Details_CancelLicenseApplication", application);
                case "RemBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення МПД(виробництво)"));
                    return View("Details_RemBranchApplication", application);
                case "ChangeContrApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна контрактних контрагентів"));
                    return View("Details_ChangeContrApplication", application);
                case "ChangeAutPersonApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна уповноважених осіб"));
                    return View("Details_ChangeAutPersonApplication", application);
                case "AddBranchInfoApplication":
                    ViewBag.AppSort = sort;
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = sort == "AddBranchInfoApplication" ? "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання інформації про МПД(виробництво)" :
                        "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення інформації про МПД(виробництво)"));
                    return View("Details_ChangeBranchInfoApplication", application);
                case "RemBranchInfoApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = sort == "AddBranchInfoApplication" ? "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання інформації про МПД(виробництво)" :
                        "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення інформації про МПД(виробництво)"));
                    ViewBag.AppSort = sort;
                    return View("Details_ChangeBranchInfoApplication", application);
                case "AddBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання МПД(виробництво)"));
                    return View("Details_AddBranchApplication", application);
                //case "RenewLicenseApplication":
                //    return View("Details_RenewLicenseApplication", application);
                default:
                    return await Task.Run(() => NotFound());
            }
        }

        [HttpGet]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return await Task.Run(() => NotFound());
        }

        //public async Task<IActionResult> SubmitApplication(Guid id)
        //{
        //    if (id == Guid.Empty)
        //        return await Task.Run(() => NotFound());
        //    var app = _commonDataService.GetEntity<PrlApplication>(x => x.Id == id)?.FirstOrDefault();
        //    if (app == null)
        //        return await Task.Run(() => NotFound());
        //    app.AppState = "Submitted";
        //    await _commonDataService.SaveChangesAsync();

        //    await _notificationService.PrlCreateNotificationAppSend(id, app.AppSort, DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")), app.OrganizationInfoId);

        //    return RedirectToAction("Index", "Monitoring", new { Area = "MTR" });
        //}

        public async Task<IActionResult> ChangeLicenseDeleteCheck(Guid entityId, string entity)
        {
            bool success;
            if (entity == "branch")
                success = await _prlApplicationAltService.ChangeBranchDeleteCheck(entityId);
            if (entity == "contractor")
                success = false;//TODO
            if (entity == "assignee")
                success = false;//TODO
            else
                success = false;
            if (success)
                return Json(new { success = success });
            return StatusCode(500);
        }

        public override async Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return await Delete(id, softDeleting,_prlApplicationAltService.Delete);
        }
    }
}
