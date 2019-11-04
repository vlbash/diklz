using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Core.Business.Services;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.IML;
using App.Data.DTO.MTR;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;
using App.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace App.Host.Areas.MTR.Controllers
{
    [Area("Mtr")]
    [Authorize(Policy = "Registered")]
    public class MonitoringController: CommonController<MtrAppListDTO, PrlAppDetailDTO, LimsDoc>
    {
        private readonly ICommonDataService _dataService;
        private readonly IPrlApplicationAltService _prlApplicationAltService;
        private readonly ImlApplicationAltService _imlApplicationAltService;

        public MonitoringController(ICommonDataService dataService, IConfiguration configuration,
            ISearchFilterSettingsService filterSettingsService, IPrlApplicationAltService prlApplicationAltService, ImlApplicationAltService imlApplicationAltService)
            : base(dataService, configuration, filterSettingsService)
        {
            _dataService = dataService;
            _prlApplicationAltService = prlApplicationAltService;
            _imlApplicationAltService = imlApplicationAltService;
        }

        [BreadCrumb(Title = "Моніторинг розгляду", Order = 1)]
        public override Task<IActionResult> Index()
        {
            return base.Index();
        }

        public override async Task<IActionResult> List(IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<MtrAppListDTO> actionListOption)
        {
            var reasons = _dataService.GetEntity<EnumRecord>(p => p.EnumType == "DecisionReason").ToList();
            actionListOption.pg_SortExpression = !string.IsNullOrEmpty(actionListOption.pg_SortExpression)
                ? actionListOption.pg_SortExpression
                : "-ModifiedOn";
            return await base.List(paramList, actionListOption, async (x, y) =>
            {
                var decisions = (await DataService.GetDtoAsync<MtrAppListDTO>(actionListOption.pg_SortExpression, parameters: x,
                    skip: (actionListOption.pg_Page - 1) * PageRowCount.Value, take: PageRowCount.Value)).ToList();
                decisions.ForEach(decision =>
                {
                    decision.DecisionReason = decision.DecisionReason?
                        .Split(',')
                        .Aggregate("", (current, s) => current + (reasons.FirstOrDefault(p => p.Code == s)?.Name + ", "));
                });
                return decisions;

            });
        }

        [NonAction]
        public override async Task<IActionResult> Details(Guid id)
        {
            return await Task.Run(() => NotFound());
        }

        [BreadCrumb(Order = 2)]
        public async Task<IActionResult> Details(Guid id, string sort, string appType)
        {
            switch (appType)
            {
                case "PRL":
                    return await Details<PrlAppDetailDTO>(id, sort, appType);
                case "IML":
                    return await Details<ImlAppDetailDTO>(id, sort, appType);
                case "TRL":
                    return await Details<TrlAppDetailDTO>(id, sort, appType);
                default: return await Task.Run(() => NotFound());
            }
        }

        [NonAction]
        private async Task<IActionResult> Details<T>(Guid id, string sort, string appType) where T : BaseAppDetailDTO
        {
            if (id == Guid.Empty)
                return await Task.Run(() => NotFound());

            var application = (await _dataService.GetDtoAsync<T>(x => x.Id == id)).FirstOrDefault();
            if (application == null)
                return await Task.Run(() => NotFound());

            var appTypeEnum = appType == "PRL" ? "(Виробництво)" : appType == "IML" ? "(Імпорт)" : "(Торгівля)";

            switch (sort)
            {
                case "GetLicenseApplication":
                case "IncreaseToPRLApplication":
                case "IncreaseToIMLApplication":
                case "IncreaseToTRLApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про отримання ліцензії на провадження діяльності " + appTypeEnum));
                    return View(application);
                case "AdditionalInfoToLicense":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Доповнення інформації по наявній ліцензії " + appTypeEnum));
                    return View(application);
                case "CancelLicenseApplication":
                case "DecreasePRLApplication":
                case "DecreaseIMLApplication":
                case "DecreaseTRLApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про анулювання ліцензії " + appTypeEnum));
                    return View("Details_CancelLicenseApplication", application);
                case "RemBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення МПД " + appTypeEnum));
                    return View("Details_RemBranchApplication", application);
                case "ChangeContrApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна контрактних контрагентів"));
                    return View("Details_ChangeContrApplication", application);
                case "ChangeAutPersonApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну інформації у додатку до ліцензії щодо особливих умов провадження діяльності - Зміна уповноважених осіб " + appTypeEnum));
                    return View("Details_ChangeAutPersonApplication", application);
                case "AddBranchInfoApplication":
                case "RemBranchInfoApplication":
                    ViewBag.AppSort = sort;
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = sort == "AddBranchInfoApplication" ? "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання інформації про МПД" + appTypeEnum :
                        "Заява про внесення змін до ЄДР у зв’язку з припиненням діяльності за певним місцем провадження - Видалення інформації про МПД " + appTypeEnum));
                    return View("Details_ChangeBranchInfoApplication", application);
                case "AddBranchApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про внесення до ЄДР відомостей про місце провадження господарської діяльності - Додавання МПД " + appTypeEnum));
                    return View("Details_AddBranchApplication", application);
                case "RenewLicenseApplication":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про отримання ліцензії на провадження діяльності " + appTypeEnum));
                    return View("Details_RenewLicenseApplication", application);
                case "ChangeDrugList":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про зміну (доповнення) переліку лікарських засобів, що імпортує ліцензіат" + appTypeEnum));
                    return View("Details_ChangeDrugList", application);
                case "ReplacementDrugList":
                    HttpContext.ModifyCurrentBreadCrumb((crumb => crumb.Name = "Заява про заміну переліку лікарських засобів, що імпортує ліцензіат" + appTypeEnum));
                    return View("Details_ReplacementDrugList", application);
                default:
                    return await Task.Run(() => NotFound());
            }
        }

        public async Task<IActionResult> CreateNewApplication(Guid id, string appType)
        {
            if (id == Guid.Empty)
                return await Task.Run(() => NotFound());
            Guid appId;
            try
            {
                switch (appType)
                {
                    case "PRL":
                        {
                            appId = await _prlApplicationAltService.ProcessApplicationToApplication(id);
                            var application = (await _dataService.GetDtoAsync<PrlAppDetailDTO>(x => x.Id == appId)).FirstOrDefault();

                            if (application == null)
                                return await Task.Run(() => NotFound());
                            return RedirectToAction("Details", "PrlApp", new { Area = "PRL", id = appId });
                        }

                    case "IML":
                        {
                            appId = await _imlApplicationAltService.ProcessApplicationToApplication(id);
                            var application = (await _dataService.GetDtoAsync<ImlAppDetailDTO>(x => x.Id == appId)).FirstOrDefault();

                            if (application == null)
                                return await Task.Run(() => NotFound());
                            return RedirectToAction("Details", "ImlApp", new { Area = "IML", id = appId });
                        }
                    default: return await Task.Run(() => NotFound());

                }

            }
            catch (Exception)
            {
                return await Task.Run(() => NotFound());
            }
        }
    }
}
