﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AtuService;
using App.Business.Services.BranchService;
using App.Business.Services.OperationFormList;
using App.Business.Services.TrlServices;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.TRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace App.HostBack.Areas.BRN.Controllers
{
    [Authorize]
    [Area("BRN")]
    public class BranchController: BaseController<BranchListDTO, BranchDetailsDTO, Branch>
    {
        private readonly ICommonDataService _dataService;
        private readonly IOperationFormListService _formListService;
        private readonly IBranchService _branchService;
        private readonly IAtuAddressService _atuAddressService;
        private readonly IEntityStateHelper _entityStateHelper;

        public BranchController(ICommonDataService dataService, IOperationFormListService formListService,
            IBranchService branchService, IAtuAddressService atuAddressService, IEntityStateHelper entityStateHelper)
        {
            _dataService = dataService;
            _formListService = formListService;
            _branchService = branchService;
            _atuAddressService = atuAddressService;
            _entityStateHelper = entityStateHelper;
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid? appId, string sort, string appType)
        {
            if (appId == null)
            {
                return NotFound();
            }
            List<BranchListDTO> branchList;

            if (sort == "AddBranchApplication")
                branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true)).ToList();
            else
                branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId)).ToList();

            if (appType == "TRL")
            {
                var branchDlsList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.CreateTds == false && x.CreateDls == false && x.IsFromLicense !=true)).ToList();
                ViewBag.multiSelectBranchList =
                    new MultiSelectList(branchDlsList, nameof(EnumRecord.Id), nameof(EnumRecord.Name));

                ViewBag.Expertise = false; // для появления кнопок доручень
                var expertise = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.PerformerOfExpertise)];
                if (!string.IsNullOrEmpty(expertise))
                {
                    ViewBag.Expertise = true;
                }

                ViewBag.Decision = false;  // для появления кнопок доручень
                var decision= _dataService.GetEntity<TrlApplication>(x => x.Id == appId)
                    .Select(x => x.AppDecisionId)?.SingleOrDefault().ToString();

                if (!string.IsNullOrEmpty(decision))
                {
                    ViewBag.Decision = true;
                }

            }
            ViewBag.ApplicationId = appId.Value;
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(appId.Value).IsBranch;
            ViewBag.sort = sort;
            ViewBag.appState = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)];
            ViewBag.appType = appType;
            branchList.ForEach(dto => dto.isEditable = _entityStateHelper.IsEditableBranch(dto.Id));

            switch (sort)
            {
                case "RemBranchApplication":
                    branchList.ForEach(x=>x.isEditable = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)] == "Project");
                    return PartialView("List_RemBranchApplication", branchList.Where(x=>x.RecordState != RecordState.D));
                case "ChangeInfoApplication":
                    return PartialView("List_ChangeInfo", branchList);
                default:
                    return PartialView(branchList);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListForLicense(Guid? appId, string sort, string appType)
        {
            if (appId == null)
            {
                return NotFound();
            }

            List<BranchListForLicenseDTO> branchListLicense;
           
            branchListLicense = (await _dataService.GetDtoAsync<BranchListForLicenseDTO>(x => x.ApplicationId == appId)).ToList();
            branchListLicense.ForEach(dto => dto.isEditable = _entityStateHelper.IsEditableBranch(dto.Id));

            ViewBag.ApplicationId = appId.Value;
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(appId.Value).IsBranch;
            ViewBag.sort = sort;
            ViewBag.appState = _entityStateHelper.GetAppStates(appId)[nameof(BaseApplication.BackOfficeAppState)];
            ViewBag.appType = appType;

            return PartialView(branchListLicense);
            
        }

        [BreadCrumb(Title = "Редагування МПД", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, Guid? appId, string sort)
        {
            BranchDetailsDTO model;
            IEnumerable<EnumRecord> appActivityTypeList = new List<EnumRecord>();

            if (id == null)
            {
                if (appId == null)
                {
                    return await Task.Run(() => NotFound());
                }

                model = new BranchDetailsDTO();
                model.ApplicationId = appId.Value;
                //TODO null reference exception
                var app = (await _dataService.GetDtoAsync<ApplicationListDTO>(x => x.Id == appId)).SingleOrDefault();
                if (app != null)
                {
                    model.AppType = app.AppTypeEnum;
                    model.OrganizationId = app.OrgUnitId;
                }
                else
                    throw new NullReferenceException("ApplicationListDTO is null");
                
                if ((sort == "AddBranchApplication" && model.AppType == "TRL") || model.AppType == "TRL")
                {
                    appActivityTypeList = _branchService.GetAppActivityTypeList(model.AppType, appId);
                }
            }
            else
            {
                model = (await _dataService.GetDtoAsync<BranchDetailsDTO>()).SingleOrDefault(x => x.Id == id.Value);
                if (model == null)
                {
                    return NotFound();
                }

                model.AppType = (await _dataService.GetDtoAsync<ApplicationListDTO>()).SingleOrDefault(x => x.Id == model.ApplicationId)?.AppTypeEnum;

                var branchAppTypeModel = _dataService.GetDto<EntityEnumDTO>(x => x.BranchId == model.Id && x.EntityType == "BranchApplication")?.FirstOrDefault();

                model.TrlActivityType = branchAppTypeModel?.EnumCode;

                if ((sort == "AddBranchApplication" && model.AppType == "TRL") || model.AppType == "TRL")
                {
                    appActivityTypeList = _branchService.GetAppActivityTypeList(sort, appId);
                }
            }

            if (model.AppType == "TRL")
            {
                var pharmacyList = _branchService.GetPharmacyList(id, appId);

                ViewBag.PharmacyList = new SelectList(pharmacyList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
               
                ViewBag.trlActivityTypeList =
                    new MultiSelectList(appActivityTypeList, nameof(EnumRecord.Code), nameof(EnumRecord.Name));
            }

            
            
            model.OperationListDTO = _formListService.GetOperationListDTO();
            ViewBag.OperationListJson = JsonConvert.SerializeObject(model.OperationListDTO);

            return View(model);
        }


        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, BranchDetailsDTO model)
        {
            if (id != model.Id)
            {
                return await Task.Run(() => NotFound());
            }

            if (string.IsNullOrEmpty(model.AdressEng))
            {
                ModelState.Remove("AdressEng");
            }

            if (model.StreetId != Guid.Empty && model.CityId != Guid.Empty)
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
                    return View(model);
                }
                else
                {
                    model.AddressId = newSubjAddress.Id;
                }
            }

            if (model.AppType == "TRL")
            {
                if (!model.TrlActivityType?.Any() ?? true)
                {
                    ModelState.AddModelError("TrlActivityType", "Мае бути обрано щонайменьше один вид діяльності!");
                }
                if (string.IsNullOrEmpty(model.BranchType))
                {
                    ModelState.AddModelError("BranchType", "Поле необхідне для заповнення!");
                }
                if (model.BranchType == "PharmacyItem" && model.PharmacyId == null)
                {
                    ModelState.AddModelError("PharmacyId", "Оберіть аптеку!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _branchService.Save(model);
                }
                catch (Exception)
                {
                    return await Task.Run(() => NotFound());
                }

                return Json(new { status = "success", tab = "#branch" });
            }

            return Json(new { status = "false", errors = ModelState.Values.Where(i => i.Errors.Count > 0) });


        }

        [BreadCrumb(Title = "Деталі по МПД", Order = 3)]
        public override async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return await Task.Run(() => NotFound());
            }

            var model =
                (await _dataService.GetDtoAsync<BranchDetailsDTO>(x => x.Id == id.Value)).FirstOrDefault();
            if (model == null)
            {
                return await Task.Run(() => NotFound());
            }

            if (model.ApplicationId == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            var appEntity =
                (await _dataService.GetDtoAsync<ApplicationListDTO>(x => x.Id == model.ApplicationId)).FirstOrDefault();
            if (appEntity == null)
            {
                return await Task.Run(() => NotFound());
            }

            model.AppType = appEntity.AppTypeEnum;
            ViewBag.AppId = appEntity.Id;
            model.OperationListDTO = _formListService.GetOperationListDTO();
            ViewBag.OperationListJson = JsonConvert.SerializeObject(model.OperationListDTO);
            ViewBag.IsEditable = _entityStateHelper.IsEditableBranch(id);

            return View(model);
        }

        public async Task<JsonResult> DeleteBranch(Guid id)
        {
            try
            {
                await _branchService.Delete(id);
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Помилка видалення. " + (e.InnerException ?? e).Message }));
            }
        }

        #region ChangeBranchInfo
        public async Task<IActionResult> ListChangeInfoBranch(Guid? appId, string sort)
        {
            if (appId == null)
            {
                return NotFound();
            }

            var branchList = (await _dataService.GetDtoAsync<BranchAltListDTO>(x => x.ApplicationId == appId)).ToList();
            ViewBag.ApplicationId = appId.Value;
            ViewBag.AppSort = sort;
            branchList.ForEach(x => x.isEditable = _entityStateHelper.IsEditableBranch(x.Id));
            return PartialView("List_AddBranchInfoApplication", branchList);
        }

        [BreadCrumb(Title = "Деталі по МПД", Order = 3)]
        public async Task<IActionResult> DetailsChangeInfoBranch(Guid? id, string sort)
        {
            if (id == null)
            {
                return await Task.Run(() => NotFound());
            }

            var model =
                (await _dataService.GetDtoAsync<BranchAltDetailsDTO>(x => x.Id == id.Value)).FirstOrDefault();
            

            if (model == null)
            {
                return await Task.Run(() => NotFound());
            }

            if (model.ApplicationId == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            var appEntity =
                (await _dataService.GetDtoAsync<ApplicationListDTO>(x => x.Id == model.ApplicationId)).FirstOrDefault();
            if (appEntity == null)
            {
                return await Task.Run(() => NotFound());
            }

            model.AppType = appEntity.AppTypeEnum;
            model.AppSort = sort;

            model.OperationListDTO = _formListService.GetOperationListDTO();
            ViewBag.OperationListJson = JsonConvert.SerializeObject(model.OperationListDTO);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveChangingInfoBranch(Guid id, string OperationListFormChanging)
        {
            if (id == null || id == Guid.Empty)
            {
                return await Task.Run(() => NotFound());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _branchService.SaveChangingInfoAsync(id, OperationListFormChanging);
                }
                catch (Exception)
                {
                    return await Task.Run(() => NotFound());
                }

                return Json(new { status = "success", tab = "#branch" });
            }

            return Json(new { status = "false", errors = ModelState.Values.Where(i => i.Errors.Count > 0) });
        }

        #endregion

        public async Task<IActionResult> CreateTds(Guid? appId)
        {
            if (appId == null)
            {
                return await Task.Run(() => NotFound());
            }

            try
            {
                await _branchService.UpdateBranch(appId.Value);
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Трапилась помилка. " + (e.InnerException ?? e).Message }));
            }

        }
        public async Task<IActionResult> CreateDls(string selectedItemId)
        {

            var selectedType = JsonConvert.DeserializeObject<List<string>>(selectedItemId);
            var branchIdList = new List<Guid>();

            foreach (var itemId in selectedType)
            {
                branchIdList.Add(Guid.Parse(itemId));
            }

            try
            {
                await _branchService.UpdateBranch(branchIdList);
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Трапилась помилка. " + (e.InnerException ?? e).Message }));
            }

        }

    }
}