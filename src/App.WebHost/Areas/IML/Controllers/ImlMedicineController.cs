using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Helpers;
using App.Business.Services.Common;
using App.Business.Services.ImlServices;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Data.Interfaces;
using App.Core.Mvc.Controllers;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.Models.IML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ReflectionIT.Mvc.Paging;
using Serilog;
using Message = App.Data.Models.MSG.Message;

namespace App.Host.Areas.IML.Controllers
{
    [Authorize(Policy = "Registered")]
    [Area("Iml")]
    public class ImlMedicineController: CommonController<ImlMedicineListDTO, ImlMedicineDetailDTO, ImlMedicine>
    {
        private readonly ImlMedicineService _medicineService;
        private readonly IConfiguration _configuration;
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly IImlLicenseService _imlLicenseService;

        private readonly MessageService _messageService;
        private readonly IObjectMapper _mapper;

        public ImlMedicineController(IConfiguration configuration, ISearchFilterSettingsService filterSettingsService, ImlMedicineService medicineService, IEntityStateHelper entityStateHelper, IImlLicenseService imlLicenseService, MessageService messageService, IObjectMapper mapper)
            : base(medicineService.DataService, configuration, filterSettingsService)
        {
            _configuration = configuration;
            _medicineService = medicineService;
            _entityStateHelper = entityStateHelper;
            _imlLicenseService = imlLicenseService;
            _messageService = messageService;
            _mapper = mapper;
        }

        [NonAction]
        public override async Task<IActionResult> Edit(Guid? id)
        {
            return NotFound();
        }
        [BreadCrumb(Title = "Лікарський засіб", Order = 3)]
        public async Task<IActionResult> Edit(Guid? id, IDictionary<string, string> paramList)
        {
            ViewBag.Countries = new SelectList(DataService.GetEntity<Country>()
                .Select(p => new { p.Id, p.Name }), nameof(Country.Id), nameof(Country.Name));
            return await base.Edit(id, paramList, _medicineService.GetEditModel);
        }

        [HttpPost]
        public override async Task<IActionResult> Edit(Guid id, ImlMedicineDetailDTO model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                DataService.Add<ImlMedicine>(model);
                await DataService.SaveChangesAsync();
                return RedirectToAction("Details", "ImlApp", new { id = model.ApplicationId }, "imlMedicine");
            }
            ViewBag.Countries = new SelectList(DataService.GetEntity<Country>()
                .Select(p => new { p.Id, p.Name }), nameof(Country.Id), nameof(Country.Name));
            return View(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> List(Guid? ApplicationId, IDictionary<string, string> paramList,
        //    Core.Mvc.Controllers.ActionListOption<ImlMedicineListDTO> options)
        //{
        //    if (ApplicationId == null)
        //        return NotFound();
        //    ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(ApplicationId.Value).IsMedicine;
        //    ViewBag.ApplicationId = ApplicationId;
        //    var file = DataService.GetEntity<FileStore>(x =>
        //        x.EntityId == ApplicationId && x.EntityName == nameof(ImlApplication) && x.Description == "Medicines").FirstOrDefault();
        //    if (file == null)
        //        ViewBag.FileCheck = false;
        //    else
        //        ViewBag.FileCheck = true;
        //    return await base.PartialList(paramList, options);
        //}

        [HttpGet]
        public async Task<IActionResult> List(Guid? ApplicationId, IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<ImlMedicineListDTO> options)
        {
            if (ApplicationId == null)
                return NotFound();
            ViewBag.IsAddable = _entityStateHelper.ApplicationAddability(ApplicationId.Value).IsMedicine;
            ViewBag.ApplicationId = ApplicationId;
            var file = DataService.GetEntity<FileStore>(x =>
                x.EntityId == ApplicationId && x.EntityName == nameof(ImlApplication) && x.Description == "Medicines").FirstOrDefault();
            if (file == null)
                ViewBag.FileCheck = false;
            else
                ViewBag.FileCheck = true;
            return await base.PartialList(paramList, options, (dictionary, option) => DataService.GetDtoAsync<ImlMedicineListDTO>(
                orderBy: options.pg_SortExpression ?? "-MedicineName",
                predicate: p => p.IsFromLicense == false,
                parameters: dictionary,
                skip: (options.pg_Page - 1) * PageRowCount.Value,
                take: PageRowCount.Value));
        }




        [HttpPost]
        public async Task<IActionResult> UploadMedicine(Guid? appId)
        {
            if (appId == null)
                return Json(new { success = false, errorText = "Помилка серверу, спробуйте пізніше" });
            if (Request.Form != null && Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    return Json(new { success = false, errorText = "Тип файлу повинен бути .xlsx" });
                try
                {
                    var errors = await _medicineService.UploadMedicine(appId, file);
                    return Json(new { success = true, errors });
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return Json(new { success = false, errorText = "Помилка серверу, спробуйте пізніше" });
                }
            }
            return Json(new { success = false, errorText = "Не обрано жодного файлу, оберіть файл" });
        }

        [NonAction]
        public override async Task<IActionResult> List(IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<ImlMedicineListDTO> options)
        {
            return await Task.Run(() => NotFound());
        }

        public async Task<IActionResult> DeleteAll(Guid? appId)
        {
            if (appId == null)
            {
                Log.Error("NotFound appId");
                return Json(new { success = false });
            }

            await _medicineService.DeleteAll(appId.Value);
            var file = DataService.GetEntity<FileStore>(x =>
                x.EntityId == appId && x.EntityName == nameof(ImlApplication) && x.Description == "Medicines").FirstOrDefault();
            if (file == null)
            {
                Log.Error("Not exists file");
                return Json(new { success = false });
            }

            DataService.Remove(file);
            await DataService.SaveChangesAsync();
            return Json(new { success = true });
        }

        public async Task<IActionResult> GetExcellFile(Guid? appId)
        {
            if (appId == null)
            {
                Log.Error("NotFound appId");
                return NotFound();
            }

            var file = DataService.GetEntity<FileStore>(x =>
                x.EntityId == appId && x.EntityName == nameof(ImlApplication) && x.Description == "Medicines").FirstOrDefault();
            if (file == null)
            {
                Log.Error("Not exists file");
                return NotFound();
            }

            return RedirectToAction("Download", "FileStore", new { Area = "", fileId = file.Id });
        }

        #region Message block

        public async Task<IActionResult> ListMessage(Guid? msgId, string messageState, IDictionary<string, string> paramList,
            Core.Mvc.Controllers.ActionListOption<ImlMedicineListMsgDTO> options)
        {
            var licenseGuid = _imlLicenseService.GetLicenseGuid();
            var license = DataService.GetEntity<ImlLicense>(p => p.Id == licenseGuid).SingleOrDefault();
            options.pg_PartialViewName = "ListMessage";
            ViewBag.MsgId = msgId;
            ViewBag.AppId = license.ParentId;
            ViewBag.MessageState = messageState;

            paramList = paramList
                .Where(x => !string.IsNullOrEmpty(x.Value) &&
                            x.Key != "__RequestVerificationToken" &&
                            x.Key != "X-Requested-With" &&
                            !x.Key.StartsWith("pg_"))
                .ToDictionary(x => x.Key, x => x.Value);

            var medicineMsg = DataService.GetEntity<ImlMedicine>(p => p.ApplicationId == msgId).Select(p => p.ParentId).ToList();
            IEnumerable<ImlMedicineListMsgDTO> medList;
            if (messageState == "Project")
            {
                medList = await DataService.GetDtoAsync<ImlMedicineListMsgDTO>(
                    orderBy: options.pg_SortExpression ?? "ParentId",
                    parameters: paramList,
                    predicate: p => (p.ApplicationId == license.ParentId && !medicineMsg.Contains(p.Id) && (p.MessageState == "Project" || string.IsNullOrEmpty(p.MessageState))) || (p.ApplicationId == msgId),
                    skip: (options.pg_Page - 1) * PageRowCount.Value,
                    take: PageRowCount.Value);
            }
            else
            {
                medList = await DataService.GetDtoAsync<ImlMedicineListMsgDTO>(
                    orderBy: options.pg_SortExpression ?? "ParentId",
                    parameters: paramList,
                    predicate: p => (p.ApplicationId == license.ParentId && !medicineMsg.Contains(p.Id) && (p.MessageState == "Project")) || (p.ApplicationId == msgId),
                    skip: (options.pg_Page - 1) * PageRowCount.Value,
                    take: PageRowCount.Value);
            }

            medList.ToList().ForEach(p =>
            {
                p.NewName = DataService.GetEntity<ImlMedicine>(z => z.Id == p.ParentId).SingleOrDefault()?.SupplierName;
                p.OldName = DataService.GetEntity<ImlMedicine>(z => p.ParentId == z.Id).SingleOrDefault()?.SupplierName;
            });

            var pagingList = PagingList.Create(medList,
                PageRowCount.Value,
                options.pg_Page,
                "ParentId",
                "Id",
                x => (x as IPagingCounted)?.TotalRecordCount,
                "ListMessage",
                true);

            return PartialView(options.pg_PartialViewName, pagingList);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSupplierName(Guid? appId, Guid? msgId, Guid? medId, string newName)
        {
            if (medId == null || msgId == null || appId == null)
            {
                return Json(new { success = false });
            }

            var msg = DataService.GetEntity<Message>(p => p.Id == msgId).SingleOrDefault();
            if (msg == null || msg.MessageState != "Project")
            {
                return Json(new { success = false });
            }

            var medicineMsg = DataService.GetEntity<ImlMedicine>(p => p.Id == medId && p.ApplicationId == msgId).SingleOrDefault();
            if (medicineMsg == null)
            {
                var medicineLicense = DataService.GetEntity<ImlMedicine>(p => p.Id == medId && p.ApplicationId == appId)
                    .SingleOrDefault();
                if (medicineLicense == null)
                    return Json(new { success = false });

                var medClone = new ImlMedicine();
                _mapper.Map(medicineLicense, medClone);
                medClone.Id = Guid.NewGuid();
                medClone.ApplicationId = msgId.Value;
                medClone.ParentId = medicineLicense.Id;
                medClone.SupplierName = newName;
                DataService.Add(medClone);
                DataService.SaveChanges();

                return Json(new { success = true, newMedId = medClone.Id });
            }
            else
            {
                medicineMsg.SupplierName = newName;
                DataService.SaveChanges();
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> RemoveMedicineMsg(Guid? medId, Guid? msgId)
        {
            if (medId == null || msgId == null)
            {
                return Json(new { success = false });
            }

            var msg = DataService.GetEntity<Message>(p => p.Id == msgId).SingleOrDefault();
            if (msg == null || msg.MessageState != "Project")
            {
                return Json(new { success = false });
            }

            var medicineMsg = DataService.GetEntity<ImlMedicine>(p => p.Id == medId).SingleOrDefault();
            if (medicineMsg == null)
            {
                return Json(new { success = false });
            }

            var medicineLic = DataService.GetEntity<ImlMedicine>(p => p.Id == medicineMsg.ParentId).SingleOrDefault();
            if (medicineLic == null)
            {
                return Json(new { success = false });
            }

            DataService.Remove(medicineMsg);
            DataService.SaveChanges();

            return Json(new { success = true, oldId = medicineLic.Id, oldName = medicineLic.SupplierName });
        }

        #endregion
    }
}
