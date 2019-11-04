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
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Business.Services.TrlServices;
using App.Business.Services.UserServices;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Mvc.Controllers;
using App.Core.Mvc.Helpers;
using App.Data.DTO.APP;
using App.Data.DTO.ATU;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.DTO.MSG;
using App.Data.DTO.PRL;
using App.Data.DTO.RPT;
using App.Data.DTO.TRL;
using App.Data.Models.APP;
using App.Data.Models.Common;
using App.Data.Models.IML;
using App.Data.Models.PRL;
using App.Data.Models.TRL;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Message = App.Data.Models.MSG.Message;

namespace App.HostBack.Areas.MSG.Controllers
{
    [Authorize]
    [Area("Msg")]
    public class MessageTypesController: CommonController<MessageListDTO, MessageDetailDTO, Message>
    {
        private MessageService _messageService { get; }
        private IObjectMapper _objectMapper { get; }
        private IConverter _converter;
        private IMgsReportService _msgReportService { get; }
        private IAtuAddressService _atuAddressService { get; }
        private ICommonDataService _commonDataService { get; }
        private LicenseService _licenseService { get; }
        private IPrlLicenseService _prlLicenseService { get; }
        private IImlLicenseService _imlLicenseService { get; }
        private ITrlLicenseService _trlLicenseService { get; }
        private BackOfficeUserService _backOfficeUserService { get; }
        private AppAssigneeService _appAssigneeService { get; }

        public MessageTypesController(ICommonDataService DataService, IConfiguration configuration, ISearchFilterSettingsService filterSettingsService, IMgsReportService msgReportService, IConverter converter,
           IObjectMapper objectMapper, MessageService messageService, IAtuAddressService atuAddressService, ICommonDataService commonDataService, BackOfficeUserService backOfficeUserService, LicenseService licenseService, IPrlLicenseService prlLicenseService, IImlLicenseService imlLicenseService, ITrlLicenseService trlLicenseService, AppAssigneeService appAssigneeService)
            : base(DataService, configuration, filterSettingsService)
        {
            _objectMapper = objectMapper;
            _messageService = messageService;
            _converter = converter;
            _msgReportService = msgReportService;
            _atuAddressService = atuAddressService;
            _commonDataService = commonDataService;
            _backOfficeUserService = backOfficeUserService;
            _licenseService = licenseService;
            _prlLicenseService = prlLicenseService;
            _imlLicenseService = imlLicenseService;
            _trlLicenseService = trlLicenseService;
            _appAssigneeService = appAssigneeService;
        }

        [BreadCrumb(Order = 2, Title = "Створення повідомлень")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [BreadCrumb(Order = 2)]
        [HttpGet]
        public async Task<IActionResult> CreateMessageType(string msgType, Guid? id, Guid? orgId = default)
        {
            SetTitleBreadCrumb(msgType);
            switch (msgType)
            {
                case "SgdChiefNameChange": return await Edit<SgdChiefNameChangeMessageDTO>(id, orgId, msgType);

                case "SgdNameChange": return await Edit<SgdNameChangeMessageDTO>(id, orgId, msgType);

                case "OrgFopLocationChange": return await Edit<OrgFopLocationChangeMessageDTO>(id, orgId, msgType);

                case "MPDActivitySuspension": return await Edit<MPDActivitySuspensionMessageDTO>(id, orgId, msgType);

                case "MPDActivityRestoration": return await Edit<MPDActivityRestorationMessageDTO>(id, orgId, msgType);

                case "MPDClosingForSomeActivity": return await Edit<MPDClosingForSomeActivityMessageDTO>(id, orgId, msgType);

                case "MPDRestorationAfterSomeActivity": return await Edit<MPDRestorationAfterSomeActivityMessageDTO>(id, orgId, msgType);

                case "MPDLocationRatification": return await Edit<MPDLocationRatificationMessageDTO>(id, orgId, msgType);

                case "PharmacyHeadReplacement": return await Edit<PharmacyHeadReplacementMessageDTO>(id, orgId, msgType);

                case "PharmacyAreaChange": return await Edit<PharmacyAreaChangeMessageDTO>(id, orgId, msgType);

                case "PharmacyNameChange": return await Edit<PharmacyNameChangeMessageDTO>(id, orgId, msgType);

                case "LeaseAgreementChange": return await Edit<LeaseAgreementChangeMessageDTO>(id, orgId, msgType);

                case "ProductionDossierChange": return await Edit<ProductionDossierChangeMessageDTO>(id, orgId, msgType);

                case "SupplierChange": return await Edit<SupplierChangeMessageDTO>(id, orgId, msgType);

                case "AnotherEvent": return await Edit<AnotherEventMessageDTO>(id, orgId, msgType);
            }
            return NotFound();
        }

        #region EditMessage

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMessage(Guid? id, Guid orgId)
        {
            // To convert from the dictionary to the desired model
            var message = HttpContext.Request.Form["MessageType"].ToString();
            var dtoType = Assembly.GetAssembly(typeof(MessageDetailDTO)).GetTypes().FirstOrDefault(p => p.Name == $"{message}MessageDTO");
            var dtoModel = Activator.CreateInstance(dtoType);

            if (dtoType == null)
            {
                return NotFound();
            }

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
                    return NotFound(); // TODO better to show the message
                }
            }

            switch (message)
            {
                case "SgdChiefNameChange": return await Edit<SgdChiefNameChangeMessageDTO>(id, orgId, dtoModel);
                case "SgdNameChange": return await Edit<SgdNameChangeMessageDTO>(id, orgId, dtoModel);
                case "OrgFopLocationChange": return await Edit<OrgFopLocationChangeMessageDTO>(id, orgId, dtoModel);
                case "MPDActivitySuspension": return await Edit<MPDActivitySuspensionMessageDTO>(id, orgId, dtoModel);
                case "MPDActivityRestoration": return await Edit<MPDActivityRestorationMessageDTO>(id, orgId, dtoModel);
                case "MPDClosingForSomeActivity": return await Edit<MPDClosingForSomeActivityMessageDTO>(id, orgId, dtoModel);
                case "MPDRestorationAfterSomeActivity": return await Edit<MPDRestorationAfterSomeActivityMessageDTO>(id, orgId, dtoModel);
                case "MPDLocationRatification": return await Edit<MPDLocationRatificationMessageDTO>(id, orgId, dtoModel);
                case "PharmacyHeadReplacement": return await Edit<PharmacyHeadReplacementMessageDTO>(id, orgId, dtoModel);
                case "PharmacyAreaChange": return await Edit<PharmacyAreaChangeMessageDTO>(id, orgId, dtoModel);
                case "PharmacyNameChange": return await Edit<PharmacyNameChangeMessageDTO>(id, orgId, dtoModel);
                case "LeaseAgreementChange": return await Edit<LeaseAgreementChangeMessageDTO>(id, orgId, dtoModel);
                case "ProductionDossierChange": return await Edit<ProductionDossierChangeMessageDTO>(id, orgId, dtoModel);
                case "SupplierChange": return await Edit<SupplierChangeMessageDTO>(id, orgId, dtoModel);
                case "AnotherEvent": return await Edit<AnotherEventMessageDTO>(id, orgId, dtoModel);
                default: return NotFound();
            }
        }

        [NonAction]
        private async Task<IActionResult> Edit<TDetailDTO>(Guid? id, Guid orgId, object model) where TDetailDTO : MessageDetailDTO
        {
            var currentModel = (TDetailDTO)model;
            if (id != null)
            {
                currentModel.Id = (Guid)id;
            }

            if (model.TryGetPropValue("LicenseType", out var licenseType))
            {
                if (!string.IsNullOrEmpty(licenseType as string))
                {
                    switch (licenseType)
                    {
                        case "PRL":
                            {
                                currentModel.IsPrlLicense = true;
                                break;
                            }

                        case "IML":
                            {
                                currentModel.IsImlLicense = true;
                                break;
                            }

                        case "TRL":
                            {
                                currentModel.IsTrlLicense = true;
                                break;
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
                await _messageService.Save(currentModel);
                return RedirectToAction("Index", "Message", new { ActivityType = currentModel.License });
            }

            await SetInformLicenses(orgId == Guid.Empty ? currentModel.OrgUnitId : orgId, currentModel.MessageType, new Message { Id = currentModel.Id }, true);

            var messageType = typeof(TDetailDTO);
            var viewName = _messageService.GetViewName(messageType, "Edit");

            return View(viewName, model);
        }

        [NonAction]
        public async Task<IActionResult> Edit<TDetailDTO>(Guid? id, Guid? orgId, string msgType) where TDetailDTO : MessageDetailDTO
        {
            TDetailDTO model;

            if (orgId == null && id == null)
            {
                return RedirectToAction("ChooseOrganization", "AppOrganization",
                    new { area = "App", returnUrl = Url.Action("CreateMessageType", "MessageTypes", new { msgType }) });
            }

            if (id == null)
            {
                model = Activator.CreateInstance<TDetailDTO>();
                model.IsCreatedOnPortal = false;
            }
            else
            {
                model = (await DataService.GetDtoAsync<TDetailDTO>(x => x.Id == id.Value)).SingleOrDefault();
                if (model == null)
                {
                    return NotFound();
                }
            }

            await SetInformLicenses(orgId ?? model.OrgUnitId, msgType, new Message { Id = model.Id }, true);

            var messageType = typeof(TDetailDTO);
            var viewName = _messageService.GetViewName(messageType, "Edit");

            GetMpdByLicense(model.License, viewName.Substring(0, viewName.Length - 5), model.Id, orgId ?? model.OrgUnitId);
            if (msgType == "PharmacyHeadReplacement")
                GetAssigneeByLicense(model.Id, null, orgId ?? model.OrgUnitId);

            return View(viewName, model);
        }

        [HttpGet]
        public async Task<IActionResult> EditMessage(Guid id)
        {
            var msgType = DataService.GetEntity<Message>(x => x.Id == id).SingleOrDefault()?.MessageType;

            return await CreateMessageType(msgType, id);
        }

        private async Task SetInformLicenses(Guid orgId, string msgType, Message msg, bool isActiveLicense)
        {
            ViewBag.OrgId = orgId;
            var prlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE, orgId) : _messageService.GetOrgInfoByMsgId(LicenseType.ePRL_LICENSE, msg.Id);
            var imlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.eIML_LICENSE, orgId) : _messageService.GetOrgInfoByMsgId(LicenseType.eIML_LICENSE, msg.Id);
            var trlInfo = isActiveLicense ? _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE, orgId) : _messageService.GetOrgInfoByMsgId(LicenseType.eTRL_LICENSE, msg.Id);
            var activeLicense = await _licenseService.GetActiveLicenses(orgId);

            ViewBag.isPRL = activeLicense.FirstOrDefault(p => p.type == "PRL").isActive;
            ViewBag.isIML = activeLicense.FirstOrDefault(p => p.type == "IML").isActive;
            ViewBag.isTRL = activeLicense.FirstOrDefault(p => p.type == "TRL").isActive;

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
                            dto => (activeLicense.FirstOrDefault(p => p.type == "PRL").isActive && dto.Code == "PRL") ||
                                   (activeLicense.FirstOrDefault(p => p.type == "IML").isActive && dto.Code == "IML"));
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

        public async Task<IActionResult> ChangeMessageState(Guid id, Guid orgId, string type)
        {
            var msgDto = _commonDataService.GetDto<MsgShortDTO>(p => p.Id == id).SingleOrDefault();
            try
            {
                switch (type)
                {
                    case "Submit":
                        {
                            await _messageService.SubmitMessage(id, orgId);
                            break;
                        }
                    case "Accept":
                        {
                            await _messageService.AcceptMessage(id, orgId);
                            break;
                        }
                    case "Reject":
                        {
                            await _messageService.RejectMessage(id, orgId);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                // TODO: Message error or log ?
            }

            return RedirectToAction("Index", "Message", new { ActivityType = msgDto.License });
        }

        [BreadCrumb(Order = 2)]
        public override async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                NotFound();
            }

            var message = DataService.GetEntity<Message>(p => p.Id == id).SingleOrDefault();
            if (message == null)
            {
                return NotFound();
            }

            if (message.NewLocationId != Guid.Empty)
            {
                var subAddress = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == message.NewLocationId).SingleOrDefault();
                if (subAddress != null)
                {
                    ViewBag.Address = subAddress.Address;
                }
            }
            else if (message.AddressBusinessActivityId != Guid.Empty)
            {
                var subAddress = DataService.GetDto<AtuSubjectAddressDTO>(p => p.Id == message.AddressBusinessActivityId).SingleOrDefault();
                if (subAddress != null)
                {
                    ViewBag.Address = subAddress.Address;
                }
            }

            SetTitleBreadCrumb(message.MessageType);
            await SetInformLicenses(message.OrgUnitId, message.MessageType, message, false);

            switch (message.MessageType)
            {
                case "MPDActivitySuspension":
                case "MPDActivityRestoration":
                case "MPDClosingForSomeActivity":
                case "MPDRestorationAfterSomeActivity":
                case "MPDLocationRatification":
                case "PharmacyNameChange":
                    {
                        var branchId = DataService.GetEntity<ApplicationBranch>(p => p.LimsDocumentId == message.Id).SingleOrDefault();
                        var branchName = DataService.GetDto<BranchDetailsDTO>(x => x.Id == branchId.BranchId).SingleOrDefault();
                        ViewBag.BranchName = $"{branchName.Name} - {branchName.Address}";
                        break;
                    }

                case "ProductionDossierChange":
                    {
                        if (message.IsPrlLicense)
                        {
                            var org = _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE, message.OrgUnitId);
                            ViewBag.AppId = org.ParentId;
                        }
                        else if (message.IsImlLicense)
                        {
                            var org = _messageService.GetOrgInfo(LicenseType.eIML_LICENSE, message.OrgUnitId);
                            ViewBag.AppId = org.ParentId;
                        }

                        break;
                    }
                case "SupplierChange":
                    {
                        ViewBag.SearchFormUrl = Url.Action("ListMessage", "ImlMedicine", new { Area = "IML" }) + $"?msgId={message.Id}&orgId={message.OrgUnitId}&messageState={message.MessageState}";
                        break;
                    }
            }

            // For additional files
            ViewBag.IsEditableFile = message.MessageState == "Project";

            return View(message.MessageType + "_Details", message);

        }

        public IActionResult GetApplicationDetails(Guid? orgId, Guid performerId, Guid msgId)
        {
            var msg = _commonDataService.GetEntity<Message>(p => p.Id == msgId).SingleOrDefault();

            var baseApp = new BaseAppDetailDTO();
            if (msg.IsPrlLicense)
            {
                var prlLicenseGuid = orgId == null ? _prlLicenseService.GetLicenseGuid() : _prlLicenseService.GetLicenseGuid(orgId);
                var license = _commonDataService.GetEntity<PrlLicense>(lic => lic.Id == prlLicenseGuid).FirstOrDefault();
                baseApp = _commonDataService.GetDto<PrlAppDetailDTO>(p => p.Id == license.ParentId).SingleOrDefault();
            }
            else if (msg.IsImlLicense)
            {
                var imlLicenseGuid = orgId == null ? _imlLicenseService.GetLicenseGuid() : _imlLicenseService.GetLicenseGuid(orgId);
                var license = _commonDataService.GetEntity<ImlLicense>(lic => lic.Id == imlLicenseGuid).FirstOrDefault();
                baseApp = _commonDataService.GetDto<ImlAppDetailDTO>(p => p.Id == license.ParentId).SingleOrDefault();
            }
            else if (msg.IsTrlLicense)
            {
                var trlLicenseGuid = orgId == null ? _trlLicenseService.GetLicenseGuid() : _trlLicenseService.GetLicenseGuid(orgId);
                var license = _commonDataService.GetEntity<TrlLicense>(lic => lic.Id == trlLicenseGuid).FirstOrDefault();
                baseApp = _commonDataService.GetDto<TrlAppDetailDTO>(p => p.Id == license.ParentId).SingleOrDefault();
            }


            if (!string.IsNullOrEmpty(msg?.RegNumber))
            {
                ViewBag.RegNumber = msg.RegNumber;
                ViewBag.RegDate = msg.RegDate.Value.ToString("dd.MM.yyyy");
            }

            ViewBag.PerformerName = _commonDataService.GetDto<UserDetailsDTO>(p => p.Id == performerId).Select(p => p.FIO).SingleOrDefault();

            return PartialView("_ApplicationMessagePartial", baseApp);
        }

        public override async Task<IActionResult> Delete(Guid id, bool isSoftDeleting = false)
        {
            return await Delete(id, isSoftDeleting, _messageService.Delete);
        }

        public IActionResult ModalRegisterMessage(Guid msgId, Guid orgId)
        {
            if (msgId == Guid.Empty || orgId == Guid.Empty)
            {
                return NotFound();
            }

            var msgShortDto = _commonDataService.GetDto<MsgShortDTO>(p => p.Id == msgId).SingleOrDefault();

            if (msgShortDto == null)
            {
                return NotFound();
            }

            msgShortDto.RegDate = DateTime.Now;
            ViewBag.OrgId = orgId;

            return PartialView("_ModalMsgRegEdit", msgShortDto);
        }

        public IActionResult RegisterMessage(MsgShortDTO model)
        {
            var message = _commonDataService.GetEntity<Message>(p => p.RegNumber == model.RegNumber).SingleOrDefault();
            if (message != null)
            {
                return Json(new { success = false, errorMessage = "Такий номер реєстрації вже існує!" });
            }
            if (ModelState.IsValid)
            {
                _messageService.RegistrationMessage(model);
                return Json(new { success = true });
            }

            return PartialView("_ModalMsgRegEdit", model);
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

        [HttpGet]
        public JsonResult CheckSupplierChange(Guid msgId)
        {
            var msg = DataService.GetEntity<ImlMedicine>(p => p.ApplicationId == msgId).Select(p => p.ParentId).ToList();
            if (msg.Count == 0)
            {
                return Json(new { success = false, message = "Дія не може бути виповнена. Ви не змінили жодного постачальника." });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetMpdByLicense(string mpd, string msgType, Guid? msgId, Guid? orgId)
        {
            var orgInfo = new MessageOrgInfoModel();
            switch (mpd)
            {
                case "PRL":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.ePRL_LICENSE, orgId);
                    break;
                case "IML":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.eIML_LICENSE, orgId);
                    break;
                case "TRL":
                    orgInfo = _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE, orgId);
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
        public JsonResult GetAssigneeByLicense(Guid? msgId, string orgPosType, Guid? orgId)
        {
            var orgInfo = _messageService.GetOrgInfo(LicenseType.eTRL_LICENSE, orgId);
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

        #region AutoComplete

        [HttpPost]
        public JsonResult AutoCompletePerformer(string term)
        {
            var result = _backOfficeUserService.GetLimsEmployee().Where(p => p.FIO.Contains(term ?? "", StringComparison.InvariantCultureIgnoreCase)).Select(p => new
            {
                value = $"{p.FIO}",
                option = p.Id
            }).ToList();

            return Json(result);
        }

        #endregion
    }
}
