using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AppServices;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.Models.APP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.HostBack.Areas.APP.Controllers
{
    [Authorize]
    [Area("App")]
    public class AppAssigneeController: CommonController<AppAssigneeListDTO, AppAssigneeDetailDTO, AppAssignee>
    {
        private readonly AppAssigneeService _service;
        private readonly IEntityStateHelper _entityStateHelper;

        public AppAssigneeController(AppAssigneeService service, IConfiguration configuration,
            ISearchFilterSettingsService filterSettingsService, IEntityStateHelper entityStateHelper)
            : base(service.DataService, configuration, filterSettingsService)
        {
            _service = service;
            _entityStateHelper = entityStateHelper;
        }

        [BreadCrumb(Title = "Уповноважена особа", Order = 3)]
        public override async Task<IActionResult> Details(Guid id)
        {
            AppAssigneeDetailDTO model = null;

            if (id == Guid.Empty)
                return await Task.Run(() => NotFound());
            else
            {
                model = (await _service.DataService.GetDtoAsync<AppAssigneeDetailDTO>(x => x.Id == id)).FirstOrDefault();
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
            }
            var branchAssignee = _service.DataService.GetEntity<AppAssigneeBranch>()
                .Where(x => x.AssigneeId == model.Id).ToList();
            var branchList = (await _service.DataService.GetDtoAsync<BranchListDTO>(x => branchAssignee.Select(y => y.BranchId)
                .Contains(x.Id))).ToList();

            var appType = branchList.FirstOrDefault()?.AppType;
            model.AppType = appType;
            if (appType == "TRL")
            {
                HttpContext.ModifyCurrentBreadCrumb(p => p.Name = "Завідувачі/Уповноважені особи");

                if (model.OrgPositionType == "Manager")
                {
                    model.BranchName = branchList.Select(st => st.Name + ',' + st.PhoneNumber).FirstOrDefault();
                }
                else
                {
                    model.ListOfBranchsNames.AddRange(branchList.Select(st => st.Name + ',' + st.PhoneNumber));
                }
            }
            else
            {
                model.ListOfBranchsNames.AddRange(branchList.Select(st => st.Name + ',' + st.PhoneNumber));
            }

            return View(model);
        }

        [BreadCrumb(Title = "Редагування уповноваженої особи", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, Guid appId, string appType, string sort)
        {
            IEnumerable<BranchListDTO> branchList;
            if (sort == "AddBranchApplication")
                branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
            else
                branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            if (appType == "TRL")
            {
                HttpContext.ModifyCurrentBreadCrumb(p => p.Name = "Редагування завідувача/уповноваженої особи");
                ViewBag.selectBranchList = new SelectList(branchList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            }
                

            return await base.Edit(
                id: id,
                paramList: new Dictionary<string, string>
                {
                    {"appId", appId.ToString()}, {"AppSort", sort}, {"AppType", appType}
                },
                editFunction: _service.Edit);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, AppAssigneeDetailDTO model)
        {
            if (id != model.Id)
            {
                return await Task.Run(() => NotFound());
            }

            if (model.AppType == "TRL")
            {
                if (model.OrgPositionType == "Manager")
                {
                    model.ListOfBranches = null;
                    ModelState.Remove("ListOfBranches");
                    if (model.BranchId == null)
                    {
                        ModelState.AddModelError("BranchId", "Оберіть МПД");
                    }
                }
                else
                {
                    model.BranchId = Guid.Empty;
                    if (!model.ListOfBranches?.Any() ?? true)
                    {
                        ModelState.AddModelError("ListOfBranches", "Має бути обрано щонайменьше одне МПД");
                    }
                }
            }
            else
            {
                if (!model.ListOfBranches?.Any() ?? true)
                {
                    ModelState.AddModelError("ListOfBranches", "Має бути обрано щонайменьше одне МПД");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model = _service.Edit(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return await Task.Run(() => NotFound());
                }

                return Redirects(model.appId, model.AppSort);
            }

            model.ListOfBranches = _service.GetSelectedBranches(model.Id).ToList();
            model.BranchId = _service.GetSelectedBranches(model.Id).FirstOrDefault();
            IEnumerable<BranchListDTO> branchList;
            if (model.AppSort == "AddBranchApplication")
                branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId && x.IsFromLicense != true);
            else
                branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            ViewBag.selectBranchList = new SelectList(branchList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> List(Guid? appId, string sort, string appType)
        {
            if (appId == null)
            {
                return await Task.Run(() => NotFound());
            }

            var appAssigneeList = await _service.GetAssigneeList(appId);
            ViewBag.AppType = appType;
            ViewBag.appId = appId;
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(appId.Value).IsAssignee;
            foreach (var dto in appAssigneeList)
            {
                dto.IsEditable = _entityStateHelper.IsEditableAuth(dto.Id) ?? false;
            }
            if (sort == "AddBranchApplication")
            {
                appAssigneeList = appAssigneeList.OrderBy(x => x.IsFromLicense == true);
                return PartialView("List_AddBranchApplication", appAssigneeList);
            }
            

            ViewBag.Sort = sort;
            return PartialView(appAssigneeList);
        }

        public override async Task<IActionResult> Delete(Guid id, bool safeDeleted = false)
        {
            try
            {
                var assignee = _service.DataService.GetEntity<AppAssignee>(x => x.Id == id).SingleOrDefault();
                assignee.RecordState = RecordState.D;
                var assigneeBranches = _service.DataService
                    .GetEntity<AppAssigneeBranch>(x => x.AssigneeId == assignee.Id)
                    .ToList();
                assigneeBranches.ForEach(x => x.RecordState = RecordState.D);
                await _service.DataService.SaveChangesAsync();
                return await Task.FromResult(Json(new
                {
                    success = true
                }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new
                {
                    success = false,
                    ErrorMessage = "Помилка видалення. " + (e.InnerException ?? e).Message
                }));
            }
        }

        [BreadCrumb(Title = "Редагування уповноваженої особи", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> EditLicense(Guid? id, Guid appId, string sort)
        {
            IEnumerable<BranchListDTO> branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return await base.Edit(id, new Dictionary<string, string>
            {
                {"appId", appId.ToString()}, {"AppSort", sort}
            }, _service.Edit);
        }

        [HttpPost]
        public async Task<IActionResult> EditLicense(Guid id, AppAssigneeDetailDTO model)
        {
            if (id != model.Id)
            {
                return await Task.Run(() => NotFound());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model = await _service.EditLicense(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return await Task.Run(() => NotFound());
                }

                return Redirects(model.appId, model.AppSort);
            }

            model.ListOfBranches = _service.GetSelectedBranches(model.Id).ToList();
            var branchList = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId && x.IsFromLicense != true);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);
        }

        [NonAction]
        public ActionResult Redirects(Guid? appId, string sort)
        {
            switch (_service.GetAppType(appId).Result)
            {
                case "PRL":
                    if(sort == "GetLicenseApplication" || sort == "AdditionalInfoToLicense" || sort == "IncreaseToPRLApplication")
                    {
                        return RedirectToAction("Details", "Application", new {Area = "PRL", id = appId}, "appAssignee");
                    }
                    return RedirectToAction("AltAppDetails", "PrlAppAlt", new
                    {
                        Area = "PRL",
                        id = appId,
                        sort
                    }, "appAssignee");
                case "IML":
                    if (sort == "GetLicenseApplication" || sort == "AdditionalInfoToLicense" || sort == "IncreaseToIMLApplication")
                    {
                        return RedirectToAction("Details", "Application", new { Area = "IML", id = appId }, "appAssignee");
                    }
                    return RedirectToAction("AltAppDetails", "ImlAppAlt", new
                    {
                        Area = "IML",
                        id = appId,
                        sort
                    }, "appAssignee");
                case "TRL":
                    if (sort == "GetLicenseApplication" || sort == "AdditionalInfoToLicense" || sort == "IncreaseToTRLApplication")
                    {
                        return RedirectToAction("Details", "Application", new { Area = "TRL", id = appId }, "appAssignee");
                    }
                    return RedirectToAction("AltAppDetails", "TrlAppAlt", new
                    {
                        Area = "TRL",
                        id = appId,
                        sort
                    }, "appAssignee");
                default:
                    return NotFound();
            }
        }
    }
}
