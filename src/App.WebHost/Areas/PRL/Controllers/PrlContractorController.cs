using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Helpers;
using App.Core.Base;
using App.Core.Business.Services;
using App.Data.DTO.BRN;
using App.Data.DTO.PRL;
using App.Data.Models.DOC;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Host.Areas.PRL.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Prl")]
    public class PrlContractorController: BaseController<PrlContractorListDTO, PrlContractorDetailDTO, PrlContractor>
    {
        private readonly ICommonDataService _dataService;
        private readonly IEntityStateHelper _entityStateHelper;


        public PrlContractorController(ICommonDataService dataService, IEntityStateHelper entityStateHelper)
            : base()
        {
            _dataService = dataService;
            _entityStateHelper = entityStateHelper;
        }

        [BreadCrumb(Title = "Контрактний контрагент", Order = 3)]
        public override async Task<IActionResult> Details(Guid? id)
        {
            PrlContractorDetailDTO model = null;

            if (id == null)
                return await Task.Run(() => NotFound());
            else
            {
                model = (await _dataService.GetDtoAsync<PrlContractorDetailDTO>(x => x.Id == id.Value)).FirstOrDefault();
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
            }
            var branchContractors = _dataService.GetEntity<PrlBranchContractor>()
                .Where(x => x.ContractorId == model.Id).ToList();
            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => branchContractors.Select(y=>y.BranchId)
                .Contains(x.Id))).ToList();
            

            model.ListOfBranchsNames = new List<string>();
            model.ListOfBranchsNames.AddRange(branchList.Select(st => st.Name + ',' + st.PhoneNumber));
            return View(model);
        }

        [BreadCrumb(Title = "Контрактний контрагент", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, Guid appId, string sort)
        {
            PrlContractorDetailDTO model = null;

            if (id == null)
                model = new PrlContractorDetailDTO();
            else
            {
                model = (await _dataService.GetDtoAsync<PrlContractorDetailDTO>(x => x.Id == id.Value)).FirstOrDefault();
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
            }

            model.appId = appId;
            model.Sort = sort;
            var branchIds = _dataService.GetEntity<PrlBranchContractor>().Where(x => x.ContractorId == model.Id).Select(x => x.BranchId);
            model.ListOfBranches = branchIds.ToList();

            IEnumerable<BranchListDTO> branchList;
            if (sort == "AddBranchApplication")
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
            else
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, PrlContractorDetailDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!model.ListOfBranches?.Any() ?? true)
            {
                ModelState.AddModelError("ListOfBranches", "Мае бути обрано щонайменьше одне МПД");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.Id = _dataService.Add<PrlContractor>(model);
                    _dataService.SaveChanges();

                    var branchContractors =
                        _dataService.GetEntity<PrlBranchContractor>().Where(x => x.ContractorId == model.Id).ToList();
                    if (branchContractors.Count > 0)
                    {
                        branchContractors.ForEach(x => _dataService.Remove(x));
                    }
                    model.ListOfBranches?.ForEach(branchId => _dataService.Add(new PrlBranchContractor()
                    {
                        ContractorId = model.Id,
                        BranchId = branchId
                    }));
                    _dataService.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await Service.GetListDTO().SingleOrDefaultAsync(x => x.Id == id) == null)
                    {
                        return await Task.Run(() => NotFound());
                    }
                    else
                    {
                        throw;
                    }
                }

               
                return RedirectToAction("Details", "PrlApp", new { Area = "PRL", Id = model.appId}, "prlContractor");
            }
            var branchIds = _dataService.GetEntity<PrlBranchContractor>().Where(x => x.ContractorId == model.Id).Select(x => x.BranchId);
            model.ListOfBranches = branchIds.ToList();
            IEnumerable<BranchListDTO> branchList;
            if (model.Sort == "AddBranchApplication")
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId && x.IsFromLicense != true);
            else
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> List(Guid? appId, string sort)
        {
            if (appId == null)
            {
                return NotFound();
            }
            var contractorList = (await _dataService.GetDtoAsync<PrlContractorListDTO>(extraParameters: new object[] { $"\'{appId}\'" })).ToList();
            var contractorIds = contractorList.Select(x => x.Id);

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId)).ToList();
            var branchContractors = _dataService.GetEntity<PrlBranchContractor>()
                .Where(x => contractorIds.Contains(x.ContractorId)).ToList();

            foreach (var contr in contractorList)
            {
                contr.ListOfBranchsNames = new List<string>();
                contr.ListOfBranchsNames.AddRange(branchList.Where(br =>
                    branchContractors.Where(x => x.ContractorId == contr.Id).Select(x => x.BranchId).Contains(br.Id)).Select(st => st.Name + ',' + st.PhoneNumber));
            }

            ViewBag.appId = appId;
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(appId.Value).IsContractor;
            ViewBag.AppSort = sort;
            contractorList.ForEach(x => x.IsEditable = _entityStateHelper.IsEditableContractor(x.Id) ?? false);

            if (sort == "AddBranchApplication")
            {
                contractorList = contractorList.OrderBy(x => x.IsFromLicense == true).ToList();
                return PartialView("List_AddBranchApplication", contractorList);
            }

            if (sort == "ChangeContrApplication")
            {
                contractorList = contractorList.OrderBy(x => x.IsFromLicense == true).ToList();
                return PartialView("List_ChangeContrApplication", contractorList);
            }

            return PartialView(contractorList);
        }

        internal async void GetSelectedBranches(PrlContractorDetailDTO m)
        {
            var branchIds = _dataService.GetEntity<PrlBranchContractor>().Where(x => x.ContractorId == m.Id).Select(x => x.BranchId);
            m.ListOfBranches = branchIds.ToList();

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == m.appId)).ToList();
            var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            ViewBag.multiSelectBranchList = multiselectlist;
        }

        public async Task<JsonResult> DeleteContractor(Guid id)
        {
            try
            {
                var contractor = _dataService.GetEntity<PrlContractor>(x => x.Id == id).SingleOrDefault();
                contractor.RecordState = RecordState.D;
                var contrBranches = _dataService.GetEntity<PrlBranchContractor>(x => x.ContractorId == contractor.Id)
                    .ToList();
                contrBranches.ForEach(x => x.RecordState = RecordState.D);
                await _dataService.SaveChangesAsync();
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Помилка видалення. " + (e.InnerException ?? e).Message }));
            }
        }

        [BreadCrumb(Title = "Контрактний контрагент", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> EditLicense(Guid? id, Guid appId)
        {

            PrlContractorDetailDTO model = null;

            if (id == null)
                model = new PrlContractorDetailDTO();
            else
            {
                model = (await _dataService.GetDtoAsync<PrlContractorDetailDTO>(x => x.Id == id.Value)).FirstOrDefault();
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
            }
            var branchIds = _dataService.GetEntity<PrlBranchContractor>().Where(x => x.ContractorId == model.Id).Select(x => x.BranchId);
            model.ListOfBranches = branchIds.ToList();

            model.appId = appId;
            IEnumerable<BranchListDTO> branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLicense(Guid id, PrlContractorDetailDTO model)
        {
            if (id != model.Id)
            {
                return await Task.Run(() => NotFound());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contractorBranches = _dataService.GetEntity<PrlBranchContractor>(x => x.ContractorId == model.Id).ToList();
                    var branches = _dataService.GetEntity<Branch>(x => contractorBranches.Select(y => y.BranchId).Contains(x.Id)).ToList();
                    var branchesToDelete = branches.Where(x => x.IsFromLicense != true).ToList();
                    contractorBranches.Where(x => branchesToDelete.Select(y => y.Id).Contains(x.BranchId)).ToList().ForEach(x => _dataService.Remove(x));
                    model.ListOfBranches?.ForEach(branchId => _dataService.Add(new PrlBranchContractor()
                    {
                        ContractorId = model.Id,
                        BranchId = branchId
                    }));

                    await _dataService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return await Task.Run(() => NotFound());
                }
                return RedirectToAction("AltAppDetails", "PrlAppAlt", new
                {
                    Area = "PRL",
                    id = model.appId,
                    sort = "AddBranchApplication"
                });
            }
            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
            model.ListOfBranches = branchIds.ToList();
            IEnumerable<BranchListDTO> branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.appId && x.IsFromLicense != true);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);
        }

    }
}
