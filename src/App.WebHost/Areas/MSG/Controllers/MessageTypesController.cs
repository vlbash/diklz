using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Business.Services.AtuService;
using App.Business.Services.Common;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.ATU;
using App.Core.Mvc.Controllers;
using App.Data.DTO.ATU;
using App.Data.DTO.BRN;
using App.Data.DTO.MSG;
using App.Data.DTO.RPT;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using App.Business.ViewModels;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Helpers;
using App.Data.Models.APP;
using App.Data.Models.Common;
using App.Data.Models.IML;
using Message = App.Data.Models.MSG.Message;

namespace App.Host.Areas.MSG.Controllers
{
    [Area("Msg")]
    [Authorize(Policy = "Registered")]
    public class MessageTypesController: CommonController<MessageListDTO, MessageDetailDTO, Message>
    {
        private MessageService _messageService { get; }
        private IObjectMapper _objectMapper { get; }
        private IConverter _converter;
        private IMgsReportService _msgReportService;
        private LicenseService _licenseService { get; }
        private IAtuAddressService _atuAddressService { get; }
        private readonly IConfiguration _configuration;
        private readonly AppAssigneeService _appAssigneeService;

        public MessageTypesController(ICommonDataService DataService, IConfiguration configuration, ISearchFilterSettingsService filterSettingsService, IConverter converter, IMgsReportService msgReportService,
            MessageService messageService, IObjectMapper objectMapper, IAtuAddressService atuAddressService, LicenseService licenseService, AppAssigneeService appAssigneeService)
            : base(DataService, configuration, filterSettingsService)
        {
            _messageService = messageService;
            _objectMapper = objectMapper;
            _converter = converter;
            _msgReportService = msgReportService;
            _atuAddressService = atuAddressService;
            _licenseService = licenseService;
            _appAssigneeService = appAssigneeService;
            _configuration = configuration;
        }

        [BreadCrumb(Order = 2, Title = "Створення повідомлень")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetMsgPDF(Guid id)
        {
            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] file;

            var newMsgType = DataService.GetDto<MessageTypeDTO>(x => x.Id == id).FirstOrDefault();

            try
            {
                switch (newMsgType.MessType)
                {
                    case "SgdChiefNameChange":
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_PIB_kerivnyka_SHD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFSgdChiefNameChange(id));
                        break;
                    case "OrgFopLocationChange":
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_mistseznakhozhdennia_SHD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFOrgFopLocationChange(id));
                        break;
                    case "SgdNameChange":
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_nazvy_SHD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFSgdNameChange(id));
                        break;
                    case "AnotherEvent":
                        file = await rep.CreatePDF($"Povidomlennia_pro_inshu_podiiu_SHD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFAnotherEvent(id));
                        break;
                    case "MPDActivitySuspension":
                        file = await rep.CreatePDF($"Povidomlennia_pro_pryzupynennia_provadzhennia_diialnosti_MPD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFMPDActivitySuspension(id));
                        break;
                    case "MPDActivityRestoration":
                        // тестировать
                        file = await rep.CreatePDF($"Povidomlennia_pro_vidnovlennia_provadzhennia_diialnosti_MPD_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFMPDActivityRestoration(id));
                        break;
                    case "MPDClosingForSomeActivity":
                        file = await rep.CreatePDF($"Povidomlennia_pro_zakryttia_MPD_dlia_provedennia_remontnykh_robit_tekhnichnoho_pereobladnannia_chy_inshykh_robit_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFMPDClosingForSomeActivity(id));
                        break;
                    case "MPDRestorationAfterSomeActivity":
                        // тестировать
                        file = await rep.CreatePDF($"Povidomlennia_pro_vidnovlennia_roboty_MPD_pislia_provedennia_remontnykh_robit_tekhnichnoho_pereobladnannia_chy_inshykh_robit_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFMPDRestorationAfterSomeActivity(id));
                        break;
                    case "MPDLocationRatification":
                        file = await rep.CreatePDF($"Povidomlennia_pro_utochnennia_adresy_mistsia_provadzhennia_diialnosti_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFMPDLocationRatification(id));
                        break;
                    case "PharmacyHeadReplacement":
                        // TODO тестировать - нет лицензии
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_zaviduiuchoho_aptechnoho_punktu_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFPharmacyHeadReplacement(id));
                        break;
                    case "PharmacyAreaChange":
                        // не сделано - нет лицензии
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_ploshchi_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFPharmacyAreaChange(id));
                        break;
                    case "PharmacyNameChange":
                        // не сделано - нет лицензии
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_nazvy_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFPharmacyNameChange(id));
                        break;
                    case "LeaseAgreementChange":
                        // не сделано - нет лицензии
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_dohovoru_orendy_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFLeaseAgreementChange(id));
                        break;
                    case "ProductionDossierChange":
                        file = await rep.CreatePDF($"Povidomlennia_pro_zaminu_abo_novu_redaktsiiu_dosie_z_vyrobnytstva_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFProductionDossierChange(id));
                        break;
                    case "SupplierChange":
                        // не сделано - нет лицензии
                        file = await rep.CreatePDF($"Povidomlennia_pro_zminu_postachalnyka_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFSupplierChange(id));
                        break;

                    default:
                        return await Task.Run(() => NotFound());
                }
            }
            catch (Exception)
            {
                return await Task.Run(() => NotFound());
            }

            return File(file, "application/pdf");
        }

        [BreadCrumb(Order = 2)]
        [HttpGet]
        public async Task<IActionResult> CreateMessageType(string msgType, Guid? id)
        {
            SetTitleBreadCrumb(msgType);

            switch (msgType)
            {
                case "SgdChiefNameChange": return await Edit<SgdChiefNameChangeMessageDTO>(id, msgType);

                case "SgdNameChange": return await Edit<SgdNameChangeMessageDTO>(id, msgType);

                case "OrgFopLocationChange": return await Edit<OrgFopLocationChangeMessageDTO>(id, msgType);

                case "MPDActivitySuspension": return await Edit<MPDActivitySuspensionMessageDTO>(id, msgType);

                case "MPDActivityRestoration": return await Edit<MPDActivityRestorationMessageDTO>(id, msgType);

                case "MPDClosingForSomeActivity": return await Edit<MPDClosingForSomeActivityMessageDTO>(id, msgType);

                case "MPDRestorationAfterSomeActivity": return await Edit<MPDRestorationAfterSomeActivityMessageDTO>(id, msgType);

                case "MPDLocationRatification": return await Edit<MPDLocationRatificationMessageDTO>(id, msgType);

                case "PharmacyHeadReplacement": return await Edit<PharmacyHeadReplacementMessageDTO>(id, msgType);

                case "PharmacyAreaChange": return await Edit<PharmacyAreaChangeMessageDTO>(id, msgType);

                case "PharmacyNameChange": return await Edit<PharmacyNameChangeMessageDTO>(id, msgType);

                case "LeaseAgreementChange": return await Edit<LeaseAgreementChangeMessageDTO>(id, msgType);

                case "ProductionDossierChange": return await Edit<ProductionDossierChangeMessageDTO>(id, msgType);

                case "SupplierChange": return await Edit<SupplierChangeMessageDTO>(id, msgType);

                case "AnotherEvent": return await Edit<AnotherEventMessageDTO>(id, msgType);

                default: return NotFound();
            }
        }

        #region EditMessage

        [BreadCrumb(Order = 2)]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMessage(Guid? id)
        {
            // To convert from the dictionary to the desired model
            var message = HttpContext.Request.Form["MessageType"].ToString();
            var dtoType = Assembly.GetAssembly(typeof(MessageDetailDTO)).GetTypes().FirstOrDefault(p => p.Name == $"{message}MessageDTO");
            var dtoModel = Activator.CreateInstance(dtoType);

            foreach (var propertyInfo in dtoType.GetProperties())
            {
                var myKey = propertyInfo.Name;
                if (!propertyInfo.CanRead || !HttpContext.Request.Form.Keys.Contains(myKey))
                {
                    continue;
                }

                try
                {
                    var value = Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Boolean ? HttpContext.Request.Form[myKey][0] : HttpContext.Request.Form[myKey].ToString();
                    var convertValue = TypeDescriptor.GetConverter(propertyInfo.PropertyType).ConvertFromInvariantString(value);
                    propertyInfo.SetValue(dtoModel, convertValue, null);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            SetTitleBreadCrumb(message);

            switch (message)
            {
                case "SgdChiefNameChange": return await Edit<SgdChiefNameChangeMessageDTO>(id, dtoModel);
                case "SgdNameChange": return await Edit<SgdNameChangeMessageDTO>(id, dtoModel);
                case "OrgFopLocationChange": return await Edit<OrgFopLocationChangeMessageDTO>(id, dtoModel);
                case "MPDActivitySuspension": return await Edit<MPDActivitySuspensionMessageDTO>(id, dtoModel);
                case "MPDActivityRestoration": return await Edit<MPDActivityRestorationMessageDTO>(id, dtoModel);
                case "MPDClosingForSomeActivity": return await Edit<MPDClosingForSomeActivityMessageDTO>(id, dtoModel);
                case "MPDRestorationAfterSomeActivity": return await Edit<MPDRestorationAfterSomeActivityMessageDTO>(id, dtoModel);
                case "MPDLocationRatification": return await Edit<MPDLocationRatificationMessageDTO>(id, dtoModel);
                case "PharmacyHeadReplacement": return await Edit<PharmacyHeadReplacementMessageDTO>(id, dtoModel);
                case "PharmacyAreaChange": return await Edit<PharmacyAreaChangeMessageDTO>(id, dtoModel);
                case "PharmacyNameChange": return await Edit<PharmacyNameChangeMessageDTO>(id, dtoModel);
                case "LeaseAgreementChange": return await Edit<LeaseAgreementChangeMessageDTO>(id, dtoModel);
                case "ProductionDossierChange": return await Edit<ProductionDossierChangeMessageDTO>(id, dtoModel);
                case "SupplierChange": return await Edit<SupplierChangeMessageDTO>(id, dtoModel);
                case "AnotherEvent": return await Edit<AnotherEventMessageDTO>(id, dtoModel);
                default: return NotFound();
            }
        }

        [NonAction]
        private async Task<IActionResult> Edit<TDetailDTO>(Guid? id, object model) where TDetailDTO : MessageDetailDTO
        {
            var currentModel = (TDetailDTO)model;
            if (id != null)
            {
                currentModel.Id = (Guid)id;
            }

            // Check License for MPD (Only 1)
            if (model.TryGetPropValue("LicenseType", out var licenseType))
            {
                if (!string.IsNullOrEmpty(licenseType as string))
                {
                    var licenseList = await _licenseService.GetActiveLicenses();
                    switch (licenseType)
                    {
                        case "PRL":
                            {
                                if (!licenseList.FirstOrDefault(p => p.type == "PRL").isActive)
                                {
                                    ModelState.AddModelError("LicenseType", "У вас немає цієї ліцензії");
                                }
                                currentModel.IsPrlLicense = true; break;
                            }
                        case "IML":
                            {
                                if (!licenseList.FirstOrDefault(p => p.type == "IML").isActive)
                                {
                                    ModelState.AddModelError("LicenseType", "У вас немає цієї ліцензії");
                                }
                                currentModel.IsImlLicense = true; break;
                            }
                        case "TRL":
                            {
                                if (!licenseList.FirstOrDefault(p => p.type == "TRL").isActive)
                                {
                                    ModelState.AddModelError("LicenseType", "У вас немає цієї ліцензії");
                                }
                                currentModel.IsTrlLicense = true; break;
                            }
                    }
                }
            }

            if (!currentModel.IsPrlLicense && !currentModel.IsImlLicense && !currentModel.IsTrlLicense)
            {
                ModelState.AddModelError("", "Хоча б одна ліцензія повинна бути обрана");
            }

            switch (currentModel)
            {
                case MPDActivitySuspensionMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }
                case MPDActivityRestorationMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }
                case MPDClosingForSomeActivityMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }
                case PharmacyNameChangeMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }
                case PharmacyHeadReplacementMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }
                case MPDLocationRatificationMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        if (ModelState.IsValid)
                        {
                            // ATU
                            var newSubjAddress = new SubjectAddress
                            {
                                StreetId = mod.StreetId,
                                PostIndex = mod.PostIndex,
                                Building = mod.Building,
                            };
                            if (!_atuAddressService.SaveAddress(newSubjAddress))
                            {
                                ModelState.AddModelError("", "Вулиця не знайдена у довіднику");
                            }
                            else
                            {
                                mod.AddressBusinessActivityId = newSubjAddress.Id;
                            }
                        }
                        break;
                    }
                case MPDRestorationAfterSomeActivityMessageDTO mod:
                    {
                        mod.MpdSelectedId = new Guid(mod.MPDGuidEnum);
                        break;
                    }

                case OrgFopLocationChangeMessageDTO mod:
                    {
                        if (ModelState.IsValid)
                        {
                            // ATU
                            var newSubjAddress = new SubjectAddress
                            {
                                StreetId = mod.StreetId,
                                PostIndex = mod.PostIndex,
                                Building = mod.Building,
                            };
                            if (!_atuAddressService.SaveAddress(newSubjAddress))
                            {
                                ModelState.AddModelError("", "Вулиця не знайдена у довіднику");
                            }
                            else
                            {
                                mod.NewLocationId = newSubjAddress.Id;
                            }
                        }
                        break;
                    }
            }


            if (ModelState.IsValid)
            {
                var newId = await _messageService.Save(currentModel);
                return RedirectToAction("Details", "MessageTypes", new { id = newId });
            }

            await SetInformLicense(currentModel.MessageType, new Message{Id = currentModel.Id}, true);

            var messageType = typeof(TDetailDTO);
            var viewName = _messageService.GetViewName(messageType, "Edit");

            return View(viewName, model);
        }

        [NonAction]
        public async Task<IActionResult> Edit<TDetailDTO>(Guid? id, string msgType) where TDetailDTO : MessageDetailDTO
        {
            TDetailDTO model;

            if (id == null)
            {
                model = Activator.CreateInstance<TDetailDTO>();
                model.IsCreatedOnPortal = true;
            }
            else
            {
                model = (await DataService.GetDtoAsync<TDetailDTO>(x => x.Id == id.Value)).SingleOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
            }

            await SetInformLicense(msgType, new Message { Id = model.Id }, true);

            var messageType = typeof(TDetailDTO);
            var viewName = _messageService.GetViewName(messageType, "Edit");

            string mpd = "";
            if (model.IsPrlLicense)
            {
                mpd = "PRL";
            }
            else if (model.IsImlLicense)
            {
                mpd = "IML";
            }
            else if (model.IsTrlLicense)
            {
                mpd = "TRL";
            }

            GetMpdByLicense(mpd, viewName.Substring(0, viewName.Length - 5), model.Id);
            if (msgType == "PharmacyHeadReplacement")
                GetAssigneeByLicense(model.Id, null);

            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> EditMessage(Guid id)
        {
            var msgType = DataService.GetEntity<Message>(x => x.Id == id).SingleOrDefault()?.MessageType;

            return await CreateMessageType(msgType, id);
        }

        private async Task SetInformLicense(string msgType, Message msg, bool isActiveLicense)
        {
            var activeLicenses = await _licenseService.GetActiveLicenses();
            ViewBag.isPRL = activeLicenses.FirstOrDefault(p => p.type == "PRL").isActive;
            ViewBag.isIML = activeLicenses.FirstOrDefault(p => p.type == "IML").isActive;
            ViewBag.isTRL = activeLicenses.FirstOrDefault(p => p.type == "TRL").isActive;

            var prlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE) : _messageService.GetOrgInfoByMsgId(LicenseType.ePRL_LICENSE, msg.Id);
            var imlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.eIML_LICENSE) : _messageService.GetOrgInfoByMsgId(LicenseType.eIML_LICENSE, msg.Id);
            var trlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE) : _messageService.GetOrgInfoByMsgId(LicenseType.eTRL_LICENSE, msg.Id);

            switch (msgType)
            {
                case "SgdChiefNameChange":
                    {
                        ViewBag.PrlLicenseDirector = !string.IsNullOrEmpty(prlInfo.EDRPOU) ? prlInfo.OrgDirector : prlInfo.Name;
                        ViewBag.ImlLicenseDirector = !string.IsNullOrEmpty(imlInfo.EDRPOU) ? imlInfo.OrgDirector : imlInfo.Name;
                        ViewBag.TrlLicenseDirector = !string.IsNullOrEmpty(trlInfo.EDRPOU) ? trlInfo.OrgDirector : trlInfo.Name;
                    }
                    break;
                case "SgdNameChange":
                    {
                        ViewBag.PrlLicenseOrgName = prlInfo.Name;
                        ViewBag.ImlLicenseOrgName = imlInfo.Name;
                        ViewBag.TrlLicenseOrgName = trlInfo.Name;
                    }
                    break;
                case "OrgFopLocationChange":
                    {
                        // PRL
                        var address = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == prlInfo.AddressId).SingleOrDefault();
                        ViewBag.PrlLicenseOrgAddress = address?.Address;
                        // IML
                        address = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == imlInfo.AddressId).SingleOrDefault();
                        ViewBag.ImlLicenseOrgAddress = address?.Address;
                        // TRL
                        address = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == trlInfo.AddressId).SingleOrDefault();
                        ViewBag.TrlLicenseOrgAddress = address?.Address;
                    }
                    break;
                case "ProductionDossierChange":
                    {
                        var selectListHelper = new SelectListHelper(DataService);
                        ViewBag.LicenseList = selectListHelper.Enum("ActivityType", 
                            dto => (activeLicenses.FirstOrDefault(p => p.type == "PRL").isActive && dto.Code == "PRL") ||
                                   (activeLicenses.FirstOrDefault(p => p.type == "IML").isActive && dto.Code == "IML"));
                    }
                    break;
                case "PharmacyHeadReplacement":
                {
                    var assignee = DataService.GetEntity<AppAssignee>(p => p.Id == msg.MpdSelectedId).SingleOrDefault();
                    if (assignee != null)
                    {
                        var AssgneeTypeName = DataService.GetEntity<EnumRecord>(x =>
                                x.Code == assignee.OrgPositionType && x.EnumType == "OrgPositionType")
                            .SingleOrDefault();
                        ViewBag.AssgneeTypeName = AssgneeTypeName?.Name;
                    }

                    ViewBag.AssigneeLicenseFIO = assignee?.FIO;

                }
                    break;
            }
        }
        #endregion

        [BreadCrumb(Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                NotFound();
            }

            var model = DataService.GetEntity<Message>(x => x.Id == id).SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }

            if (model.NewLocationId != Guid.Empty)
            {
                var subAddress = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == model.NewLocationId).SingleOrDefault();
                if (subAddress != null)
                {
                    ViewBag.Address = subAddress.Address;
                }
            }
            else if (model.AddressBusinessActivityId != Guid.Empty)
            {
                var subAddress = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == model.AddressBusinessActivityId).SingleOrDefault();
                if (subAddress != null)
                {
                    ViewBag.Address = subAddress.Address;
                }
            }

            SetTitleBreadCrumb(model.MessageType);
            await SetInformLicense(model.MessageType, model, false);
            {
                MessageOrgInfoModel org;
                if (model.IsPrlLicense)
                {
                    org = _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE);
                }
                else if (model.IsImlLicense)
                {
                    org = _messageService.GetOrgInfo(LicenseType.eIML_LICENSE);
                }
                else
                {
                    org = _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE);
                }
                ViewBag.AppId = org.ParentId;
                if (model.MessageType != "PharmacyHeadReplacement")
                {
                    var branchId = DataService.GetEntity<ApplicationBranch>(p => p.LimsDocumentId == model.Id).SingleOrDefault();
                    if (branchId != null)
                    {
                        var branchName = DataService.GetDto<BranchDetailsDTO>(x => x.Id == branchId.BranchId).SingleOrDefault();
                        ViewBag.MpdSelected = $"{branchName?.Name} - {branchName?.Address}";
                    }
                }

                ViewBag.IsEditableFile = model.MessageState == "Project";

                // for msg supplier
                ViewBag.SearchFormUrl = Url.Action("ListMessage", "ImlMedicine", new { Area = "IML" }) + $"?msgId={model.Id}&messageState={model.MessageState}";

                return View(model.MessageType + "_Details", model);
            }
        }

        public override async Task<IActionResult> Delete(Guid id, bool isSoftDeleting = false)
        {
            return await Delete(id, isSoftDeleting, _messageService.Delete);
        }

        private void SetTitleBreadCrumb(string messageType)
        {
            switch (messageType)
            {
                case "SgdChiefNameChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна ПІБ керівника Суб'єкту господарювання")); break;
                case "SgdNameChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна найменування Суб'єкту господарювання")); break;
                case "OrgFopLocationChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна місця знаходження юридичної особи / фізичної особи підприємця")); break;
                case "MPDActivitySuspension":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Призупинення провадження діяльності МПД")); break;
                case "MPDActivityRestoration":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Відновлення провадження діяльності МПД")); break;
                case "MPDClosingForSomeActivity":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Закриття місця провадження діяльності для проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності")); break;
                case "MPDRestorationAfterSomeActivity":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Відновлення роботи місця провадження діяльності після проведення ремонтних робіт, технічного переобладнання чи інших робіт, пов'язаних з веденням певного виду господарської діяльності")); break;
                case "MPDLocationRatification":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Уточнення адреси місця провадження діяльності")); break;
                case "PharmacyHeadReplacement":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Заміна завідуючого аптечного пункту")); break;
                case "PharmacyAreaChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна площі аптечного закладу")); break;
                case "PharmacyNameChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Заміна назви аптечного закладу")); break;
                case "LeaseAgreementChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна договору оренди")); break;
                case "ProductionDossierChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Заміна або нова редакція Досьє з виробництва")); break;
                case "SupplierChange":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Зміна постачальника")); break;
                case "AnotherEvent":
                    HttpContext.ModifyCurrentBreadCrumb((crumb =>
                        crumb.Name = "Повідомити про іншу подію")); break;
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMsgFilesForSign(Guid id)
        {
            if (id == Guid.Empty)
                return new JsonResult(StatusCode(404));
            var filesModel = await _messageService.GetMsgFilesForSign(id, _converter);

            return new JsonResult(filesModel);
        }

        [HttpPost]
        public async Task<JsonResult> SaveSignedMsgFiles(FilesSignViewModel model)
        {
            if (model.id == Guid.Empty)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Index", "Home", new { Area = "" }) });
            if (model.files.Count == 0)
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Details", "MessageTypes", new { Area = "MSG", model.id }) });
            try
            {
                var tempPath = _configuration["FileStorePath"];
                await _messageService.SubmitMessage(model, tempPath);
                // await _messageService.SubmitApplication(Configuration, model);
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false, errorMessage = "Виникла помилка, спробуйте пізніше", returnUrl = Url.Action("Details", "MessageTypes", new { Area = "MSG", model.id }) });
            }
            return new JsonResult(new { success = true, returnUrl = Url.Action("Index", "Message", new { Area = "MSG" }) });
        }

        [HttpGet]
        public JsonResult CheckSupplierChange(Guid msgId)
        {
            var msg = DataService.GetEntity<ImlMedicine>(p => p.ApplicationId == msgId).Select(p => p.ParentId).ToList();
            if (msg.Count == 0)
            {
                return Json(new {success = false, message = "Дія не може бути виповнена. Ви не змінили жодного постачальника."});
            }

            return Json(new {success = true});
        }

        [HttpGet]
        public JsonResult GetMpdByLicense(string mpd, string msgType, Guid? msgId)
        {
            var orgInfo = new MessageOrgInfoModel();
            switch (mpd)
            {
                case "PRL":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE);
                    break;
                case "IML":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.eIML_LICENSE);
                    break;
                case "TRL":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE);
                    break;
            }
            var branchList = DataService.GetDto<BranchListDTO>().Where(x => x.ApplicationId == orgInfo.ParentId).Select(p => new
            {
                p.Id,
                Name = $"{p.Name} - {p.Address}",
                p.BranchActivity
            }).ToList();

            var selectedMpd = Guid.Empty;
            if (msgId != null)
            {
                var model = DataService.GetEntity<Message>(p => p.Id == msgId.Value).SingleOrDefault();
                if (model != null)
                {
                    selectedMpd = model.MpdSelectedId;
                }
            }


            SelectList selectList;
            switch (msgType)
            {
                case "MPDActivitySuspension":
                    selectList = new SelectList(branchList.Where(p => p.BranchActivity == "Active"), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
                case "MPDActivityRestoration":
                    selectList = new SelectList(branchList.Where(p => p.BranchActivity == "Suspended"), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
                case "MPDClosingForSomeActivity":
                    selectList = new SelectList(branchList.Where(p => p.BranchActivity == "Active"), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
                case "MPDRestorationAfterSomeActivity":
                    selectList = new SelectList(branchList.Where(p => p.BranchActivity == "Closed"), nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
                //case "OrgFopLocationChange":
                case "MPDLocationRatification":
                    selectList = new SelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
                default:
                    selectList = new SelectList(branchList, nameof(BranchListDTO.Id), nameof(BranchListDTO.Name), selectedMpd);
                    break;
            }
            ViewBag.BranchList = selectList;
            return Json(selectList);
        }

        [HttpGet]
        public JsonResult GetAssigneeByLicense(Guid? msgId, string orgPosType)
        {
            var orgInfo = _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE);
            if (orgInfo.ParentId == null)
                return null;
            var assigneeList = _appAssigneeService.GetAssigneeList(orgInfo.ParentId).Result;

            SelectList selectList;
            var message = DataService.GetDto<PharmacyHeadReplacementMessageDTO>(p => p.Id == msgId).SingleOrDefault();
            if (message == null)
            {
                var assignee = DataService.GetEntity<AppAssignee>(p => p.OrgPositionType == (!string.IsNullOrEmpty(orgPosType) ? orgPosType : "Authorized") && assigneeList.Select(z => z.Id).Contains(p.Id));
                selectList = new SelectList(assignee, nameof(AppAssignee.Id), nameof(AppAssignee.FIO));

                ViewBag.BranchList = selectList;
                ViewBag.OrgPosType = (!string.IsNullOrEmpty(orgPosType) ? orgPosType : "Authorized");
                return Json(selectList);
            }

            var assigneeMsg = DataService.GetEntity<AppAssignee>(p => assigneeList.Select(z => z.Id).Contains(p.Id));
            var assigneeType = assigneeMsg.SingleOrDefault(p => p.Id == message.MpdSelectedId);

            if (assigneeType == null)
                return null;

            selectList = new SelectList(assigneeMsg.Where(p => p.OrgPositionType == (!string.IsNullOrEmpty(orgPosType) ? orgPosType : assigneeType?.OrgPositionType)), nameof(AppAssignee.Id), nameof(AppAssignee.FIO), message.MpdSelectedId);

            ViewBag.BranchList = selectList;
            ViewBag.OrgPosType = (!string.IsNullOrEmpty(orgPosType) ? orgPosType : assigneeType?.OrgPositionType);

            return Json(selectList);
        }
    }
}
