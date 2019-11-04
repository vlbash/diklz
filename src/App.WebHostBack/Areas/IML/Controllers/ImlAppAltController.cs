using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.ImlServices;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.ATU;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.Models.APP;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.IML.Controllers
{
    [Authorize]
    [Area("IML")]
    public class ImlAppAltController: CommonController<ImlAppDetailDTO, ImlAppDetailDTO, ImlApplication>
    {
        private ICommonDataService _commonDataService;
        private ImlApplicationAltService _imlApplicationAltService;
        private readonly IEntityStateHelper _entityStateHelper;
        public ImlAppAltController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, ICommonDataService commonDataService, IEntityStateHelper entityStateHelper, ImlApplicationAltService imlApplicationAltService) : base(dataService, configuration, searchFilterSettingsService)
        {
            _commonDataService = commonDataService;
            _entityStateHelper = entityStateHelper;
            _imlApplicationAltService = imlApplicationAltService;
        }

        public async Task<IActionResult> CreateOnOpen(string sort, Guid? orgId)
        {
            if (string.IsNullOrEmpty(sort))
                return NotFound();
            Guid appId;
            try
            {
                appId = await _imlApplicationAltService.CreateOnOpen(sort, orgId, true);
            }
            catch (Exception)
            {
                return await Task.Run(() => NotFound());
            }
            return RedirectToAction("AltAppDetails", new { id = appId, sort = sort });
        }

        [BreadCrumb(Order = 2)]
        public async Task<IActionResult> AltAppDetails(Guid id, string sort)
        {
            var application = (await _commonDataService.GetDtoAsync<ImlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }

            // ATU
            if (application.AddressId != Guid.Empty)
            {
                var subAddress = _commonDataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == application.AddressId).SingleOrDefault();
                if (subAddress != null)
                {
                    application.PostIndex = subAddress.PostIndex;
                    ViewBag.Address = subAddress.Address;
                }
            }
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(id)[nameof(BaseApplication.BackOfficeAppState)] == "Project";
            ViewBag.PerformerName = _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == application.PerformerId).Select(p => p.FIO).SingleOrDefault();
            switch (sort)
            {
                case "CancelLicenseApplication":
                case "DecreaseIMLApplication":
                    if (sort == "CancelLicenseApplication")
                        HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про анулювання ліцензії(імпорт)"));
                    else
                        HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про звуження провадження виду господарської діяльності - Звуження виробництва лікарських засобів"));
                    return View("Details_CancelLicenseApplication", application);
                case "ChangeAutPersonApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна уповноважених осіб"));
                    return View("Details_ChangeAutPersonApplication", application);
                case "AddBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання МПД(імпорт)"));
                    return View("Details_AddBranchApplication", application);
                case "RemBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення МПД(імпорт)"));
                    return View("Details_RemBranchApplication", application);
                case "ChangeDrugList":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну (доповнення) переліку лікарських засобів, що імпортує ліцензіат"));
                    return View("Details_ChangeDrugList", application);
                case "ReplacementDrugList":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про заміну переліку лікарських засобів, що імпортує ліцензіат"));
                    return View("Details_ReplacementDrugList", application);
                    
                default:
                    return await Task.Run(() => NotFound());
            }
        }

        [HttpGet]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return await Task.Run(() => NotFound());
        }

        public async Task<IActionResult> SubmitApplication(Guid id)
        {
            if (id == Guid.Empty)
                return await Task.Run(() => NotFound());
            var app = _commonDataService.GetEntity<ImlApplication>(x => x.Id == id)?.FirstOrDefault();
            if (app == null)
                return await Task.Run(() => NotFound());
            app.AppState = "Submitted";
            await _commonDataService.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public override async Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return await Delete(id, softDeleting, _imlApplicationAltService.Delete);
        }

    }
}
