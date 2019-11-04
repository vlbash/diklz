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
using App.Data.DTO.IML;
using App.Data.Models.APP;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.IML.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("IML")]
    public class ImlAppAltController : CommonController<ImlAppDetailDTO, ImlAppDetailDTO, ImlApplication>
    {
        private ICommonDataService _commonDataService { get; }
        private ImlApplicationAltService _imlApplicationAltService { get; }
        private readonly IEntityStateHelper _entityStateHelper;

        public ImlAppAltController(ICommonDataService dataService, IConfiguration configuration, ISearchFilterSettingsService searchFilterSettingsService, ImlApplicationAltService imlApplicationAltService, ICommonDataService commonDataService, IEntityStateHelper entityStateHelper) : base(dataService, configuration, searchFilterSettingsService)
        {
            _imlApplicationAltService = imlApplicationAltService;
            _commonDataService = commonDataService;
            _entityStateHelper = entityStateHelper;
        }

        public async Task<IActionResult> CreateOnOpen(string sort)
        {
            if (string.IsNullOrEmpty(sort))
                return NotFound();
            Guid appId;
            try
            {
                appId = await _imlApplicationAltService.CreateOnOpen(sort);
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
            var application = (await _commonDataService.GetDtoAsync<ImlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
            {
                return await Task.Run(() => NotFound());
            }
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(id)[nameof(BaseApplication.AppState)] == "Project";
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

        public override async Task<IActionResult> Delete(Guid id, bool softDeleting = false)
        {
            return await Delete(id, softDeleting, _imlApplicationAltService.Delete);
        }
    }
}
