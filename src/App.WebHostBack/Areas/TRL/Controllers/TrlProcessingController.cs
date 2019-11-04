using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Helpers;
using App.Business.Services.ImlServices;
using App.Business.Services.LimsService;
using App.Business.Services.TrlServices;
using App.Business.Services.UserServices;
using App.Core.Business.Services;
using App.Core.Data.Entities.Common;
using App.Data.DTO.APP;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.DTO.TRL;
using App.Data.Models.APP;
using App.Data.Models.TRL;
using App.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.HostBack.Areas.TRL.Controllers
{
    [Authorize]
    [Area("TRL")]
    public class TrlProcessingController: Controller
    {
        private readonly ICommonDataService _commonDataService;
        private readonly TrlApplicationService _trlApplicationService;
        private readonly BackOfficeUserService _backOfficeUserService;
        private readonly TrlApplicationProcessService _trlAppProcessService;
        private readonly IEntityStateHelper _entityStateHelper;

        public TrlProcessingController(ICommonDataService commonDataService,
                                       TrlApplicationService trlApplicationService,
                                       BackOfficeUserService backOfficeUserService,
                                       TrlApplicationProcessService trlAppProcessService,
                                       IEntityStateHelper entityStateHelper)
        {
            _commonDataService = commonDataService;
            _trlApplicationService = trlApplicationService;
            _backOfficeUserService = backOfficeUserService;
            _trlAppProcessService = trlAppProcessService;
            _entityStateHelper = entityStateHelper;
        }

        #region Expertise

        public IActionResult ExpertiseDetails(Guid appId)
        {
            var expertise = _commonDataService.GetDto<TrlAppExpertiseDTO>(p => p.Id == appId).SingleOrDefault() ??
                            new TrlAppExpertiseDTO { Id = appId };

            expertise.PerformerName = _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == expertise.PerformerOfExpertiseId).Select(p => p.FIO).SingleOrDefault();
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)] != "Reviewed";
            return PartialView("_trlExpertiseDetails", expertise);
        }

        public IActionResult ModalExpertise(Guid appId)
        {
            var expertise = _commonDataService.GetDto<TrlAppExpertiseDTO>(p => p.Id == appId).SingleOrDefault();
            if (expertise == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(expertise.ExpertiseResultEnum))
            {
                expertise.ExpertiseComment = null;
            }

            if (expertise.PerformerOfExpertiseId == Guid.Empty)
            {
                expertise.ExpertiseDate = DateTime.Now;
            }
            else
            {
                expertise.PerformerOfExpertiseName = _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == expertise.PerformerOfExpertiseId).Select(p => p.FIO).SingleOrDefault();
            }

            return PartialView("_ModalTrlExpertiseEdit", expertise);
        }

        [HttpPost]
        public IActionResult SaveExpertise(TrlAppExpertiseDTO model)
        {
            if (ModelState.IsValid)
            {
                _trlAppProcessService.SaveExpertise(model);
                return Json(new { success = true });
            }

            return PartialView("_ModalTrlExpertiseEdit", model);
        }

        #endregion

        #region Decision

        public IActionResult DecisionDetails(Guid appId)
        {
            var decision = _commonDataService.GetDto<AppDecisionDTO>(p => p.AppId == appId).SingleOrDefault() ??
                           new AppDecisionDTO { AppId = appId, Id = Guid.Empty };

            if (string.IsNullOrEmpty(decision.ExpertiseResultEnum))
            {
                var exp = _commonDataService.GetDto<TrlAppExpertiseDTO>(p => p.Id == appId).SingleOrDefault();
                decision.ExpertiseResultEnum = exp?.ExpertiseResultEnum;
            }

            var reasons = _commonDataService.GetEntity<EnumRecord>(p => p.EnumType == "DecisionReason").ToList();
            var decisions = decision.DecisionReasons?.Split(',').Aggregate("", (current, s) => current + (reasons.FirstOrDefault(p => p.Code == s)?.Name + ","));
            decision.DecisionReasons = decisions?.Remove(decisions.Length - 1, 1);
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)] != "Reviewed";

            return PartialView("_trlDecisionDetails", decision);
        }

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        public IActionResult ModalDecisionApplication(Guid id)
        {
            var decision = _commonDataService.GetDto<AppDecisionDTO>(p => p.AppId == id).SingleOrDefault() ??
                           new AppDecisionDTO { AppId = id, Id = Guid.Empty };

            if (!string.IsNullOrEmpty(decision.DecisionReasons))
            {
                decision.ListOfDecisionReason = new List<string>(decision.DecisionReasons.Split(','));
            }

            var regNumber = _commonDataService.GetEntity<TrlApplication>(p => p.Id == id).Select(p => p.RegNumber)
                .SingleOrDefault();
            ViewBag.RegNumber = regNumber;
            ViewBag.Protocols = new SelectList(_commonDataService.GetDto<ProtocolDTO>(p => p.StatusName == "В роботі" && p.Type == "TRL", take: 10)
                    .Select(p => new { ProtocolId = p.Id, DateAndNumber = $"№{p.ProtocolNumber} - {p.ProtocolDate:dd.MM.yyyy}" }), "ProtocolId",
                    "DateAndNumber");

            return PartialView("_ModalTrlDecisionEdit", decision);
        }

        public IActionResult RemoveDecision(Guid appId)
        {
            var success = _trlAppProcessService.RemoveDecision(appId);
            if (!success)
                return NotFound();
            return RedirectToAction("Details", "Application", new { id = appId });
        }

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppProtocol })]
        [HttpPost]
        public IActionResult SaveDecision(AppDecisionDTO model)
        {
            foreach (var reason in model.ListOfDecisionReason)
            {
                switch (model.DecisionType)
                {
                    case "Denied" when reason != "DiscrepancyLU" && reason != "Inaccuracy":
                    case "WithoutConsideration" when (reason == "DiscrepancyLU" || reason == "Inaccuracy"):
                        ModelState.AddModelError("ListOfDecisionReason", "Деякі підстави не відповідають рішенню");
                        break;
                }
            }

            if (ModelState.IsValid)
            {
                _trlAppProcessService.SaveDecision(model);
                return Json(new { success = true });
            }

            return PartialView("_ModalTrlDecisionEdit", model);

        }

        #endregion

        #region PreLicenseCheck

        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppCheck })]
        public IActionResult PreLicenseCheckDetails(Guid appId)
        {
            var preLicense = _commonDataService.GetDto<AppPreLicenseCheckDTO>(p => p.AppId == appId).SingleOrDefault() ??
                             new AppPreLicenseCheckDTO { AppId = appId, Id = Guid.Empty };
            preLicense.CheckCreatedName =
                _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == preLicense.CheckCreatedId).Select(p => p.FIO).SingleOrDefault();

            if (string.IsNullOrEmpty(preLicense.ExpertiseResultEnum))
            {
                var exp = _commonDataService.GetDto<TrlAppExpertiseDTO>(p => p.Id == appId).SingleOrDefault();
                preLicense.ExpertiseResultEnum = exp?.ExpertiseResultEnum;
            }
            ViewBag.IsEditable = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)] != "Reviewed";
            return PartialView("_trlPreLicenseCheck", preLicense);
        }

        public IActionResult ModalPreLicenseCheck(Guid appId)
        {
            var preLicense = _commonDataService.GetDto<AppPreLicenseCheckDTO>(p => p.AppId == appId).SingleOrDefault();

            if (preLicense != null)
            {
                return NotFound();
            }

            preLicense = new AppPreLicenseCheckDTO
            {
                AppId = appId,
                Id = Guid.Empty
            };

            if (preLicense.CheckCreatedId != Guid.Empty)
            {
                preLicense.CheckCreatedName = _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == preLicense.CheckCreatedId).Select(p => p.FIO).SingleOrDefault();
            }

            return PartialView("_ModalTrlPreLicenseCheckEdit", preLicense);
        }

        public IActionResult RemovePreLicenseCheck(Guid appId)
        {
            var success = _trlAppProcessService.RemovePreLicenseCheck(appId);
            if (!success)
                return NotFound();
            return RedirectToAction("Details", "Application", new { id = appId });
        }

        [HttpPost]
        public IActionResult SavePreLicenseCheck(AppPreLicenseCheckDTO model)
        {
            if (ModelState.IsValid)
            {
                _trlAppProcessService.SavePreLicenseCheck(model);
                return Json(new { success = true });
            }

            return PartialView("_ModalTrlPreLicenseCheckEdit", model);
        }

        #endregion

        #region LicenseMessage 
        [TypeFilter(typeof(LimsExchangeFilter), Arguments = new object[] { LimsRepository.ChangesTrackedEnum.AppNotice })]
        public IActionResult LicenseMessageDetails(Guid appId)
        {
            var licenseMsg = _commonDataService.GetDto<AppLicenseMessageDTO>(p => p.AppId == appId).SingleOrDefault() ??
                             new AppLicenseMessageDTO { AppId = appId, Id = Guid.Empty };

            if (string.IsNullOrEmpty(licenseMsg.ExpertiseResultEnum))
            {
                var exp = _commonDataService.GetDto<TrlAppExpertiseDTO>(p => p.Id == appId).SingleOrDefault();
                licenseMsg.ExpertiseResultEnum = exp?.ExpertiseResultEnum;
            }

            if (string.IsNullOrEmpty(licenseMsg.DecisionType))
            {
                var decision = _commonDataService.GetDto<AppDecisionDTO>(p => p.AppId == appId).SingleOrDefault();
                licenseMsg.DecisionType = decision?.DecisionType;
            }

            licenseMsg.PerformerName =
                _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == licenseMsg.PerformerId).Select(p => p.FIO).SingleOrDefault();

            return PartialView("_trlLicenseMessageDetails", licenseMsg);
        }

        public IActionResult ModalLicenseMessage(Guid appId)
        {
            var licenseMsg = _commonDataService.GetDto<AppLicenseMessageDTO>(p => p.AppId == appId).SingleOrDefault();
            if (licenseMsg != null)
            {
                return NotFound();
            }

            licenseMsg = new AppLicenseMessageDTO { AppId = appId, Id = Guid.Empty };

            return PartialView("_ModalTrlLicenseMessageEdit", licenseMsg);
        }

        public IActionResult SaveLicenseMessage(AppLicenseMessageDTO model)
        {
            if (ModelState.IsValid)
            {
                _trlAppProcessService.SaveLicenseMessage(model);
                return Json(new { success = true });
            }

            return PartialView("_ModalTrlLicenseMessageEdit", model);
        }

        #endregion

    }
}
