using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Core.Mvc.Controllers;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.DTO.DOS;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.ORG;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Message = App.Data.Models.MSG.Message;

namespace App.HostBack.Areas.DOC.Controllers
{
    [Authorize]
    [Area("DOC")]
    public class EDocumentController: BaseController<EDocumentListDTO, EDocumentDetailsDTO, EDocument>
    {
        private readonly ICommonDataService _dataService;
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly IObjectMapper _objectMapper;

        public EDocumentController(ICommonDataService dataService, IUserInfoService userInfoService, IEntityStateHelper entityStateHelper, IObjectMapper objectMapper)
            : base()
        {
            _dataService = dataService;
            _entityStateHelper = entityStateHelper;
            _objectMapper = objectMapper;
        }

        [NonAction]
        public override async Task<IActionResult> Details(Guid? id)
        {
            return await Task.Run(() => NotFound());
        }

        [BreadCrumb(Title = "Деталі досьє", Order = 3)]
        public async Task<IActionResult> Details(Guid? id, Guid appId)
        {
            EDocumentDetailsDTO model = null;

            if (id == null)
                return await Task.Run(() => NotFound());
            else
            {
                model = (await _dataService.GetDtoAsync<EDocumentDetailsDTO>(extraParameters: new object[] { $"\'{appId}\'" })).FirstOrDefault(x => x.Id == id.Value);
                if (model == null)
                {
                    return await Task.Run(() => NotFound());
                }
            }
            var branchContractors = _dataService.GetEntity<BranchEDocument>()
                .Where(x => x.EDocumentId == model.Id).ToList();
            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => branchContractors.Select(y => y.BranchId)
                .Contains(x.Id))).ToList();


            model.ListOfBranchsNames = new List<string>();
            model.ListOfBranchsNames.AddRange(branchList.Select(st => st.Name + ',' + st.PhoneNumber));
            var app = (await _dataService.GetDtoAsync<AppStateDTO>(x => x.Id == appId))?.FirstOrDefault();
            ViewBag.appType = app.AppType;
            return View(model);
        }

        [BreadCrumb(Title = "Деталі досьє", Order = 3)]
        public async Task<IActionResult> DetailsMsg(Guid? id, Guid msgId)
        {
            EDocumentDetailsDTO dest = new EDocumentDetailsDTO();
            if (id == null)
                return await Task.Run(() => NotFound());
            
            var model = (await _dataService.GetDtoAsync<EDocumentDetailsMsgDTO>(extraParameters: new object[] { $"\'{msgId}\'" })).FirstOrDefault(x => x.Id == id.Value);
            if (model == null)
            {
                return await Task.Run(() => NotFound());
            }
            
            var branchContractors = _dataService.GetEntity<BranchEDocument>()
                .Where(x => x.EDocumentId == model.Id).ToList();
            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => branchContractors.Select(y => y.BranchId)
                .Contains(x.Id))).ToList();

            model.ListOfBranchsNames = new List<string>();
            model.ListOfBranchsNames.AddRange(branchList.Select(st => st.Name + ',' + st.PhoneNumber));

            _objectMapper.Map(model, dest);

            return View("Details",dest);
        }

        [HttpGet]
        public async Task<IActionResult> List(Guid? appId)
        {
            if (appId == null)
            {
                return NotFound();
            }
            var eDocumentList = (await _dataService.GetDtoAsync<EDocumentListDTO>(extraParameters: new object[] { $"\'{appId}\'" })).ToList();
            var eDocumentIds = eDocumentList.Select(x => x.Id);

            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId)).ToList();
            var branchContractors = _dataService.GetEntity<BranchEDocument>()
                .Where(x => eDocumentIds.Contains(x.EDocumentId)).ToList();

            foreach (var contr in eDocumentList)
            {
                contr.ListOfBranches = new List<string>();
                contr.ListOfBranches.AddRange(branchList.Where(br =>
                    branchContractors.Where(x => x.EDocumentId == contr.Id).Select(x => x.BranchId).Contains(br.Id)).Select(st => st.Name + ',' + st.PhoneNumber));
            }
            ViewBag.appId = appId;
            var app = (await _dataService.GetDtoAsync<AppStateDTO>(x => x.Id == appId))?.FirstOrDefault();
            if (app == null)
                return NotFound();
            ViewBag.appId = appId;
            ViewBag.AppSort = app.AppSort;
            string eDocType;
            switch (app.AppType)
            {
                case "PRL":
                    eDocType = "ManufactureDossier";
                    break;
                case "IML":
                    eDocType = "ImportDossier";
                    break;
                case "TRL":
                    eDocType = "";
                    break;
                default:
                    return NotFound();
            }

            ViewBag.eDocType = eDocType;
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(appId.Value).IsEdocument;
            ViewBag.appType = app.AppType;
            eDocumentList.ForEach(dto => dto.IsEditable = _entityStateHelper.IsEditableEdoc(dto.Id) ?? false);

            if (app.AppSort == "AddBranchApplication")
            {
                eDocumentList = eDocumentList.OrderBy(x => x.IsFromLicense).ToList();
                return PartialView("List_AddBranchApplication", eDocumentList);
            }
            if (app.AppSort == "AddBranchInfoApplication")
                return PartialView("List_AddBranchInfoApplication", eDocumentList);


            return PartialView(eDocumentList);
        }

        [HttpGet]
        public async Task<IActionResult> ListMsg(Guid appId, Guid msgId)
        {
            var msgModel = _dataService.GetEntity<Message>(p => p.Id == msgId).SingleOrDefault();

            ViewBag.appId = appId;
            ViewBag.msgId = msgId;
            ViewBag.IsAddable = msgModel.MessageState == "Project";
            // APP
            var eDocumentAppList = (await _dataService.GetDtoAsync<EDocumentListDTO>(extraParameters: new object[] { $"\'{appId}\'" })).ToList();
            var eDocumentAppIds = eDocumentAppList.Select(x => x.Id);

            var branchAppList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId)).ToList();
            var branchAppContractors = _dataService.GetEntity<BranchEDocument>()
                .Where(x => eDocumentAppIds.Contains(x.EDocumentId)).ToList();

            foreach (var contr in eDocumentAppList)
            {
                contr.ListOfBranches = new List<string>();
                contr.ListOfBranches.AddRange(branchAppList.Where(br =>
                    branchAppContractors.Where(x => x.EDocumentId == contr.Id).Select(x => x.BranchId).Contains(br.Id)).Select(st => st.Name + ',' + st.PhoneNumber));
            }

            eDocumentAppList.ForEach(dto =>
            {
                dto.IsEditable = _entityStateHelper.IsEditableEdoc(dto.Id) ?? false;
                dto.IsFromMessage = false;
            }); 

            // MSG
            var eDocumentMsgList = (await _dataService.GetDtoAsync<EDocumentListDTO>(extraParameters: new object[] { $"\'{msgId}\'" })).ToList();
            var eDocumentMsgIds = eDocumentMsgList.Select(x => x.Id);

            var branchMsgList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == msgId)).ToList();
            var branchMsgContractors = _dataService.GetEntity<BranchEDocument>()
                .Where(x => eDocumentMsgIds.Contains(x.EDocumentId)).ToList();

            foreach (var contr in eDocumentMsgList)
            {
                contr.ListOfBranches = new List<string>();
                contr.ListOfBranches.AddRange(branchMsgList.Where(br =>
                    branchMsgContractors.Where(x => x.EDocumentId == contr.Id).Select(x => x.BranchId).Contains(br.Id)).Select(st => st.Name + ',' + st.PhoneNumber));
            }

            eDocumentMsgList.ForEach(dto =>
            {
                dto.IsEditable = msgModel.MessageState == "Project";
                dto.IsFromMessage = true;
            });

            var eDocumentList = new List<EDocumentListDTO>();
            eDocumentList.AddRange(eDocumentAppList);
            eDocumentList.AddRange(eDocumentMsgList);

            return PartialView("ListMsg", eDocumentList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOnOpen(Guid appId, string documentType, string sort, string appType, Guid? msgId)
        {
            if (appId == Guid.Empty)
                return NotFound();
            var dto = new EDocumentDetailsDTO()
            {
                EDocumentType = documentType
            };
            var id = _dataService.Add<EDocument>(dto);
            await _dataService.SaveChangesAsync();

            return msgId == null 
                ? RedirectToAction("Edit", new { id, appId, sort, appType }) 
                : RedirectToAction("EditMsg", new {id, msgId});
        }

        [BreadCrumb(Title = "Редагування досьє", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id, Guid appId, string sort, string appType)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();
            var eDocument = (await _dataService.GetDtoAsync<EDocumentDetailsDTO>(extraParameters: new object[] { $"\'{appId}\'" })).FirstOrDefault(x => x.Id == id.Value);
            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);

            IEnumerable<BranchListDTO> branchList;
            switch (sort)
            {
                case "AddBranchInfoApplication":
                    branchList = (await _dataService.GetDtoAsync<BranchAltListDTO>(x => x.ApplicationId == appId && x.IsChangedOperationListForm == true))
                        .Select(x => new BranchListDTO() { Id = x.Id, Name = x.Name });
                    break;
                case "AddBranchApplication":
                    branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
                    break;
                default:
                    branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId)).ToList();
                    break;
            }
            
            if (appType == "TRL")
            {
                HttpContext.ModifyCurrentBreadCrumb(p => p.Name = "Редагування МТБ");
                eDocument.BranchId = branchIds.FirstOrDefault();
                ViewBag.selectBranchList = new SelectList(branchList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            }
            else
            {
                eDocument.ListOfBranches = branchIds.ToList();
                var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                ViewBag.multiSelectBranchList = multiselectlist;
            }

            eDocument.ApplicationId = appId;
            eDocument.AppSort = sort;
            eDocument.AppType = appType;

            return View(eDocument);
        }

        [BreadCrumb(Title = "Редагування досьє повідомлення", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> EditMsg(Guid id, Guid msgId)
        {
            if (id == Guid.Empty || msgId == Guid.Empty)
                return NotFound();

            var eDocument = (await _dataService.GetDtoAsync<EDocumentDetailsMsgDTO>(extraParameters: new object[] { $"\'{msgId}\'" })).FirstOrDefault(x => x.Id == id);
            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
            eDocument.ListOfBranches = branchIds.ToList();
            var branchList = (await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == msgId)).ToList();

            var multiSelectList = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            ViewBag.multiSelectBranchList = multiSelectList;
            eDocument.MessageId = msgId;

            return View("EditMsg", eDocument);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, EDocumentDetailsDTO model)
        {
            if (id != model.Id)
                return NotFound();
            
            var fileList = _dataService.GetEntity<FileStore>().Where(x => x.EntityId == id).Select(x => x.EntityId).ToList();
            IEnumerable<BranchListDTO> branchList;
            if (model.AppSort == "AddBranchApplication")
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.ApplicationId && x.IsFromLicense != true);
            else
                branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.ApplicationId);
            if (fileList.Count == 0)
            {
                if (model.AppType == "TRL")
                    ViewBag.selectBranchList = new SelectList(branchList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                else
                {
                    var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                    ViewBag.multiSelectBranchList = multiselectlist;
                }

                ModelState.AddModelError("", "Мае бути додано щонайменьше однин файл");
                //return await Task.FromResult(Json(new { success = false, ErrorMessage = "Мае бути додано щонайменьше однин файл" }));
                return View(model);
            }

            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
            
            if (model.AppSort != "AddBranchApplication" && (!model.ListOfBranches?.Any() ?? true) && model.AppType != "TRL")
            {
                model.ListOfBranches = branchIds.ToList();
                
                var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                ViewBag.multiSelectBranchList = multiselectlist;
                ModelState.Clear();
                ModelState.AddModelError("ListOfBranches", "Мае бути обрано щонайменьше одне МПД");
                    
                return View(model);
            }

            if (model.AppType == "TRL" && model.BranchId == null)
            {
                ViewBag.selectBranchList = new SelectList(branchList.Select(p => new { p.Id, p.Name }), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                ModelState.Clear();
                ModelState.AddModelError("BrancheId", "Оберіть МПД");

                return View(model);
            }
            try
            {
                model.Id = _dataService.Add<EDocument>(model);
                _dataService.SaveChanges();

                var branchContractors =
                    _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == model.Id).ToList();
                if (branchContractors.Count > 0)
                {
                    branchContractors.ForEach(x => _dataService.Remove(x));
                }

                if (model.AppType == "TRL")
                {
                    _dataService.Add(new BranchEDocument
                    {
                        EDocumentId = model.Id,
                        BranchId = model.BranchId.Value
                    });
                }
                else
                {
                    model.ListOfBranches?.ForEach(branchId => _dataService.Add(new BranchEDocument
                    {
                        EDocumentId = model.Id,
                        BranchId = branchId
                    }));
                }
                _dataService.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await Service.GetListDTO().SingleOrDefaultAsync(x => x.Id == id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var appType = (await _dataService.GetDtoAsync<AppStateDTO>(x => x.Id == model.ApplicationId)).FirstOrDefault()?.AppType;

            switch (appType)
            {
                case "PRL":
                    return RedirectToAction("Details", "Application", new
                    {
                        Area = "PRL",
                        Id = model.ApplicationId
                    }, "eDocument");
                case "IML":
                    return RedirectToAction("Details", "Application", new
                    {
                        Area = "IML",
                        Id = model.ApplicationId
                    }, "eDocument");
                case "TRL":
                    return RedirectToAction("Details", "Application", new
                    {
                        Area = "TRL",
                        Id = model.ApplicationId
                    }, "eDocument");
                default:
                    return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMsg(Guid id, EDocumentDetailsMsgDTO model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!model.ListOfBranches?.Any() ?? true)
            {
                var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
                model.ListOfBranches = branchIds.ToList();
                var branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.MessageId);
                var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
                ViewBag.multiSelectBranchList = multiselectlist;
                ModelState.Clear();
                ModelState.AddModelError("ListOfBranches", "Має бути обрано щонайменьше одне МПД");

                return View("EditMsg",model);
            }
            try
            {
                model.Id = _dataService.Add<EDocument>(model);
                _dataService.SaveChanges();

                var branchContractors =
                    _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == model.Id).ToList();
                if (branchContractors.Count > 0)
                {
                    branchContractors.ForEach(x => _dataService.Remove(x));
                }
                model.ListOfBranches.ForEach(branchId => _dataService.Add(new BranchEDocument()
                {
                    EDocumentId = model.Id,
                    BranchId = branchId
                }));
                _dataService.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await Service.GetListDTO().SingleOrDefaultAsync(x => x.Id == id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", "MessageTypes", new { Area = "MSG", Id = model.MessageId });
        }

        public override async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                var eDoc = _dataService.GetEntity<EDocument>(x => x.Id == id).SingleOrDefault();
                eDoc.RecordState = RecordState.D;
                var eDocBranches = _dataService.GetEntity<BranchEDocument>(x => x.EDocumentId == eDoc.Id)
                    .ToList();
                eDocBranches.ForEach(x => x.RecordState = RecordState.D);
                var files = _dataService.GetEntity<FileStore>(x => x.EntityId == id && x.EntityName == "EDocument")
                    .ToList();
                files.ForEach(x =>
                {
                    x.RecordState = RecordState.D;
                    FileStoreHelper.DeleteFileIfExist(x.FilePath);
                });
                await _dataService.SaveChangesAsync();
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception e)
            {
                return await Task.FromResult(Json(new { success = false, ErrorMessage = "Помилка видалення. " + (e.InnerException ?? e).Message }));
            }
        }

        [BreadCrumb(Title = "Редагування досьє", Order = 3)]
        [HttpGet]
        public async Task<IActionResult> EditLicense(Guid? id, Guid appId)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();
            var eDocument = (await _dataService.GetDtoAsync<EDocumentDetailsDTO>(extraParameters: new object[] { $"\'{appId}\'" })).FirstOrDefault(x => x.Id == id.Value);
            if (eDocument == null)
                return await Task.Run(() => NotFound());
            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
            eDocument.ListOfBranches = branchIds.ToList();
            IEnumerable<BranchListDTO> branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == appId && x.IsFromLicense != true);
            var multiselectlist = new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));
            ViewBag.multiSelectBranchList = multiselectlist;
            eDocument.ApplicationId = appId;

            return View(eDocument);
        }

        [HttpPost]
        public async Task<IActionResult> EditLicense(Guid id, EDocumentDetailsDTO model)
        {
            if (id != model.Id)
            {
                return await Task.Run(() => NotFound());
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var branchEDocuments = _dataService.GetEntity<BranchEDocument>(x => x.EDocumentId == model.Id).ToList();
                    var branches = _dataService.GetEntity<Branch>(x => branchEDocuments.Select(y => y.BranchId).Contains(x.Id)).ToList();
                    var branchesToDelete = branches.Where(x => x.IsFromLicense != true).ToList();
                    branchEDocuments.Where(x => branchesToDelete.Select(y => y.Id).Contains(x.BranchId)).ToList().ForEach(x => _dataService.Remove(x));
                    model.ListOfBranches?.ForEach(branchId => _dataService.Add(new BranchEDocument()
                    {
                        EDocumentId = model.Id,
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
                    id = model.ApplicationId,
                    sort = "AddBranchApplication"
                });
            }
            var branchIds = _dataService.GetEntity<BranchEDocument>().Where(x => x.EDocumentId == id).Select(x => x.BranchId);
            model.ListOfBranches = branchIds.ToList();

            IEnumerable<BranchListDTO> branchList = await _dataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == model.ApplicationId && x.IsFromLicense != true);
            ViewBag.multiSelectBranchList =
                new MultiSelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name));

            return View(model);
        }

        public async Task<IActionResult> ListPayment(Guid appId, string appType)
        {
            var eDocumentAppList = (await _dataService.GetDtoAsync<EDocumentPaymentListDTO>(
                p => p.EdocumentType == "PaymentDocument",
                extraParameters: new object[] { $"\'{appId}\'" })).ToList();

            string entityName = appType == "PRL" ? "PrlApplication" : appType == "IML" ? "ImlApplication" : "TrlApplication";


            var dto = new EDocumentDetailsDTO();
            if (eDocumentAppList.Count == 0)
            {
                dto = new EDocumentDetailsDTO()
                {
                    Id = Guid.NewGuid(),
                    EDocumentType = "PaymentDocument",
                    EDocumentStatus = "RequiresPayment",
                    EntityName = entityName,
                    EntityId = appId,
                    DateFrom = DateTime.Now
                };
                _dataService.Add<EDocument>(dto);
                await _dataService.SaveChangesAsync();
                return PartialView("ListPayment", dto);
            }

            var notVerified = eDocumentAppList.Where(p => p.EdocumentStatus == "PaymentNotVerified")
                .OrderByDescending(p => p.CreatedOn).FirstOrDefault();
            var docRequires = eDocumentAppList.FirstOrDefault(p => p.EdocumentStatus == "RequiresPayment");
            if (docRequires != null)
            {
                dto = (await _dataService.GetDtoAsync<EDocumentDetailsDTO>(p => p.Id == docRequires.Id, extraParameters: new object[] { $"\'{docRequires.EntityId}\'" })).SingleOrDefault();
                dto.Comment = notVerified?.Comment;
                return PartialView("ListPayment", dto);
            }

            docRequires = eDocumentAppList.FirstOrDefault(p => p.EdocumentStatus == "WaitingForConfirmation" || p.EdocumentStatus == "PaymentConfirmed");
            if (docRequires != null)
            {
                return PartialView("ListPayment", new EDocumentDetailsDTO{Id = docRequires.Id, EDocumentStatus = docRequires.EdocumentStatus, EntityId = docRequires.EntityId});
            }

            docRequires = eDocumentAppList.Where(p => p.EdocumentStatus == "PaymentNotVerified").OrderByDescending(p => p.CreatedOn).FirstOrDefault();
            if (docRequires != null)
            {
                dto = new EDocumentDetailsDTO()
                {
                    Id = Guid.NewGuid(),
                    EDocumentType = "PaymentDocument",
                    EDocumentStatus = "RequiresPayment",
                    EntityName = entityName,
                    EntityId = appId,
                    DateFrom = DateTime.Now
                };
                _dataService.Add<EDocument>(dto);
                await _dataService.SaveChangesAsync();

                dto.Comment = notVerified?.Comment;
            }

            return PartialView("ListPayment", dto);
        }

        public async Task<IActionResult> ListAdditionDocuments(Guid appId, string appType)
        {
            var eDocumentAppList = (await _dataService.GetDtoAsync<EDocumentPaymentListDTO>(
                p => p.EdocumentType == "AdditionDocument",
                extraParameters: new object[] { $"\'{appId}\'" })).FirstOrDefault();

            string entityName = appType == "PRL" ? "PrlApplication" : appType == "IML" ? "ImlApplication" : "TrlApplication";
            ViewBag.AppType = appType;
            var dto = new EDocumentDetailsDTO();
            if (eDocumentAppList == null)
            {
                dto = new EDocumentDetailsDTO()
                {
                    Id = Guid.NewGuid(),
                    EDocumentType = "AdditionDocument",
                    EntityName = entityName,
                    EntityId = appId,
                    DateFrom = DateTime.Now
                };
                _dataService.Add<EDocument>(dto);
                await _dataService.SaveChangesAsync();
                return PartialView("ListAnother", dto);
            }

            return PartialView("ListAnother", new EDocumentDetailsDTO
            {
                Id = eDocumentAppList.Id,
                EntityId = eDocumentAppList.EntityId
            });
        }
    }
}
