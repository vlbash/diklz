using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Services.PrlServices;
using App.Business.Services.RptServices;
using App.Business.ViewModels;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
// using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.Common;
using App.Core.Data.Enums;
using App.Core.Data.Helpers;
using App.Data.DTO.MSG;
using App.Data.DTO.RPT;
using App.Data.DTO.Common;
using App.Data.Models.APP;
using App.Data.Models.Common;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using DinkToPdf.Contracts;
using Serilog;
using Microsoft.Extensions.Configuration;
using Message = App.Data.Models.MSG.Message;
using App.Business.Services.NotificationServices;
using System.Globalization;
using App.Business.Services.ImlServices;
using App.Business.Services.TrlServices;
using App.Data.Models;
using App.Data.Models.IML;
using App.Data.Models.TRL;

namespace App.Business.Services.Common
{
    public enum LicenseType
    {
        ePRL_LICENSE,
        eIML_LICENSE,
        eTRL_LICENSE
    }

    public class MessageService
    {
        private readonly ICommonDataService _dataService;
        private readonly IUserInfoService _infoService;
        private readonly IObjectMapper _objectMapper;
        private readonly IMgsReportService _msgReportService;
        private readonly INotificationService _notificationService;
        private readonly IPrlLicenseService _prlLicenseService;
        private readonly IImlLicenseService _imlLicenseService;
        private readonly ITrlLicenseService _trlLicenseService;

        public MessageService(ICommonDataService dataService, IPrlLicenseService prlLicenseService,
            IUserInfoService infoService, IObjectMapper objectMapper, IMgsReportService msgReportService, INotificationService notificationService, IImlLicenseService imlLicenseService, ITrlLicenseService trlLicenseService)
        {
            _dataService = dataService;
            _prlLicenseService = prlLicenseService;
            _msgReportService = msgReportService;
            _infoService = infoService;
            _notificationService = notificationService;
            _imlLicenseService = imlLicenseService;
            _trlLicenseService = trlLicenseService;
            _objectMapper = objectMapper;
        }

        public async Task<Guid> Save<TDetailDTO>(TDetailDTO model) where TDetailDTO : CoreDTO
        {
            var baseClass = model as MessageDetailDTO;
            if (baseClass == null)
            {
                throw new Exception("Виникла помилка при збережені");
            }

            Guid orgId;

            try
            {
                orgId = baseClass.OrgUnitId == Guid.Empty ? new Guid(_infoService.GetCurrentUserInfo().OrganizationId()) : baseClass.OrgUnitId;
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при обробці данних користувача", e);
            }

            Message message;
            if (model.Id == Guid.Empty)
            {
                message = new Message();
                _objectMapper.Map(model, message);
                message.Id = Guid.NewGuid();
                message.OrgUnitId = orgId;
                message.MessageState = "Project";
                message.ModifiedOn = DateTime.Now;

                if ((message.IsPrlLicense && !message.IsImlLicense && !message.IsTrlLicense) ||
                    (message.IsImlLicense && !message.IsPrlLicense && !message.IsTrlLicense) ||
                    (message.IsTrlLicense && !message.IsPrlLicense && !message.IsImlLicense))
                {
                    message.MessageHierarchyType = "Single";
                }
                else
                {
                    message.MessageHierarchyType = "Parent";
                }

                _dataService.Add(message);
                _dataService.SaveChanges();

                if (message.IsPrlLicense)
                {
                    CreateSubMessage(LicenseType.ePRL_LICENSE, orgId, message);
                }

                if (message.IsImlLicense)
                {
                    CreateSubMessage(LicenseType.eIML_LICENSE, orgId, message);
                }

                if (message.IsTrlLicense)
                {
                    CreateSubMessage(LicenseType.eTRL_LICENSE, orgId, message);
                }
                _dataService.SaveChanges();

                return message.Id;
            }

            message = _dataService.GetEntity<Message>(p => p.Id == model.Id).SingleOrDefault();
            if (message == null)
            {
                throw new Exception("Виникла помилка при збережені");
            }
            _objectMapper.Map(model, message);
            message.OrgUnitId = orgId;
            message.ModifiedOn = DateTime.Now;

            // Update data for all tyoe (children)
            if (message.MessageHierarchyType == "Parent")
            {
                var childrenMsg = _dataService.GetEntity<Message>(p => p.MessageParentId == message.Id).ToList();
                foreach (var msgChild in childrenMsg)
                {
                    UpdateMessageFromLicense(msgChild);
                }
            }
            else
            {
                UpdateMessageFromLicense(message);
            }
            

            await _dataService.SaveChangesAsync();

            return message.Id;
        }

        private void CreateSubMessage(LicenseType licType, Guid orgId, Message message)
        {
            Guid? licGuid = Guid.Empty;
            var appOrgInfoId = Guid.Empty; ;
            switch (licType)
            {
                case LicenseType.ePRL_LICENSE:
                    {
                        licGuid = orgId == Guid.Empty ? _prlLicenseService.GetLicenseGuid() : _prlLicenseService.GetLicenseGuid(orgId);
                        var orgInfo = _dataService.GetEntity<PrlLicense>(p => p.Id == licGuid.Value).SingleOrDefault();
                        appOrgInfoId = _dataService.GetEntity<PrlApplication>(p => p.Id == orgInfo.ParentId)
                            .SingleOrDefault().OrganizationInfoId;
                    }
                    break;
                case LicenseType.eIML_LICENSE:
                    {
                        licGuid = orgId == Guid.Empty ? _imlLicenseService.GetLicenseGuid() : _imlLicenseService.GetLicenseGuid(orgId);
                        var orgInfo = _dataService.GetEntity<ImlLicense>(p => p.Id == licGuid.Value).SingleOrDefault();
                        appOrgInfoId = _dataService.GetEntity<ImlApplication>(p => p.Id == orgInfo.ParentId)
                            .SingleOrDefault().OrganizationInfoId;
                    }
                    break;
                case LicenseType.eTRL_LICENSE:
                    {
                        licGuid = orgId == Guid.Empty ? _trlLicenseService.GetLicenseGuid() : _trlLicenseService.GetLicenseGuid(orgId);
                        var orgInfo = _dataService.GetEntity<TrlLicense>(p => p.Id == licGuid.Value).SingleOrDefault();
                        appOrgInfoId = _dataService.GetEntity<TrlApplication>(p => p.Id == orgInfo.ParentId)
                            .SingleOrDefault().OrganizationInfoId;
                    }
                    break;
            }

            if (message.MessageHierarchyType == "Single")
            {
                message.ParentId = licGuid;
                message.OrganizationInfoId = appOrgInfoId;
                CopyBranchToMessage(message, licGuid.Value, licType);
                UpdateMessageFromLicense(message);
                return;
            }

            var newAppMessage = new Message();
            _objectMapper.Map(message, newAppMessage);
            newAppMessage.Id = Guid.NewGuid();
            newAppMessage.MessageParentId = message.Id;
            newAppMessage.ParentId = licGuid;
            newAppMessage.OrganizationInfoId = appOrgInfoId;
            newAppMessage.IsPrlLicense = licType == LicenseType.ePRL_LICENSE;
            newAppMessage.IsImlLicense = licType == LicenseType.eIML_LICENSE;
            newAppMessage.IsTrlLicense = licType == LicenseType.eTRL_LICENSE;
            newAppMessage.MessageHierarchyType = "Child";
            newAppMessage.ModifiedOn = DateTime.Now;
            UpdateMessageFromLicense(newAppMessage);

            CopyBranchToMessage(newAppMessage, licGuid.Value, licType);
        }

        public void UpdateMessageFromLicense(Message msg)
        {
            var orgInfo = _dataService.GetEntity<OrganizationInfo>(p => p.Id == msg.OrganizationInfoId).SingleOrDefault();
            if (orgInfo == null)
            {
                throw new NullReferenceException("Не знайдено інформацію по організації");
            }
            var org = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfo.OrganizationId).SingleOrDefault();
            switch (msg.MessageType)
            {
                case "SgdChiefNameChange":
                    {
                        if (org == null)
                        {
                            throw new NullReferenceException("Не знайдено організацію");
                        }
                        msg.SgdShiefOldFullName = string.IsNullOrEmpty(org?.EDRPOU) ? orgInfo.Name : orgInfo.OrgDirector;
                        break;
                    }
                case "SgdNameChange":
                    {
                        if (org == null)
                        {
                            throw new NullReferenceException("Не знайдено організацію");
                        }
                        msg.SgdOldFullName = orgInfo.Name;
                        break;
                    }
                case "OrgFopLocationChange":
                    {
                        msg.OldLocationId = orgInfo.AddressId;
                        break;
                    }
            }
        }

        public async Task Delete(Guid id, bool isSoftDeleting)
        {
            try
            {
                var message = _dataService.GetEntity<Message>(p => p.Id == id).SingleOrDefault();
                if (message.MessageState != "Project")
                {
                    return;
                }
                message.RecordState = RecordState.D;
                var applicationBranches =
                    _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == id).ToList();
                applicationBranches.ForEach(x => x.RecordState = RecordState.D);
                var branches = _dataService.GetEntity<Branch>(x => applicationBranches.Select(y => y.BranchId).Contains(x.Id)).ToList();
                branches.ForEach(x => x.RecordState = RecordState.D);
                var branchAssignees =
                    _dataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).ToList();
                branchAssignees.ForEach(x => x.RecordState = RecordState.D);
                var assignees = _dataService
                    .GetEntity<AppAssignee>(x => branchAssignees.Select(y => y.AssigneeId).Contains(x.Id)).ToList();
                assignees.ForEach(x => x.RecordState = RecordState.D);
                var contractorBranches = _dataService
                    .GetEntity<PrlBranchContractor>(x => branches.Select(y => y.Id).Contains(x.BranchId)).ToList();
                contractorBranches.ForEach(x => x.RecordState = RecordState.D);
                var contractor = _dataService
                    .GetEntity<PrlContractor>(x => contractorBranches.Select(y => y.ContractorId).Contains(x.Id)).ToList();
                contractor.ForEach(x => x.RecordState = RecordState.D);
                await _dataService.SaveChangesAsync();
                var eDocumentBranches = _dataService
                    .GetEntity<BranchEDocument>(x => branches.Select(y => y.Id).Contains(x.BranchId)).ToList();
                eDocumentBranches.ForEach(x => x.RecordState = RecordState.D);
                var eDocument =
                    _dataService.GetEntity<EDocument>(x =>
                        eDocumentBranches.Select(y => y.EDocumentId).Contains(x.Id)).ToList();
                eDocument.ForEach(x =>
                {
                    x.RecordState = RecordState.D;
                    var files = _dataService
                        .GetEntity<FileStore>(y => y.EntityId == x.Id && y.EntityName == "EDocument").ToList();
                    files.ForEach(y =>
                    {
                        y.RecordState = RecordState.D;
                        FileStoreHelper.DeleteFileIfExist(y.FilePath);
                    });
                });
            }
            catch (Exception e)
            {
                throw new Exception("Виникла помилка при виделені повідомлення", e);
            }

        }

        #region Msg processing       


        public async Task SubmitMessage(Guid id, Guid? orgId = null)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException("Невідомий guid");
            }

            var message = _dataService.GetEntity<Message>(p => p.Id == id).SingleOrDefault();
            if (message == null)
            {
                throw new NullReferenceException("Повідомлення не знайдено");
            }

            if (message.MessageHierarchyType == "Parent")
            {
                var subMessages = _dataService.GetEntity<Message>(p => p.MessageParentId == message.Id).ToList();
                subMessages.ForEach(p =>
                {
                    p.MessageState = "Submitted";
                    p.ModifiedOn = DateTime.Now;
                });
            }

            message.MessageState = "Submitted";
            message.ModifiedOn = DateTime.Now;

            await _dataService.SaveChangesAsync();
            await _notificationService.PrlCreateNotificationMsgSend(id, message.MessageType, DateTime.Now.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")), message.OrgUnitId);
        }

        public async Task SubmitMessage(FilesSignViewModel model, string config)
        {
            foreach (var file in model.files)
            {
                file.name = file.name + ".p7s";
                var fileStore = new FileStoreDTO
                {
                    FileType = FileType.Unknown,
                    OrigFileName = file.name + ".p7s",
                    FileSize = GetOriginalLengthInBytes(file.file),
                    Ock = file.isSystemFile
                };
                if (file.id != Guid.Empty)
                {
                    fileStore.EntityId = file.idFileStore;
                    fileStore.EntityName = nameof(FileStore);
                    fileStore.ContentType = ".p7s";
                    fileStore.Description = "Підписаний PDF повідомлення";
                    await SaveFile(config, file, fileStore);
                }
                else
                {
                    fileStore.EntityId = model.id;
                    fileStore.EntityName = nameof(PrlApplication);
                    fileStore.ContentType = ".p7s";
                    fileStore.Description = "Підписаний PDF повідомлення";
                    await SaveFile(config, file, fileStore);
                }
            }

            await SubmitMessage(model.id);
        }

        public void RegistrationMessage(MsgShortDTO model)
        {
            var message = _dataService.GetEntity<Message>(p => p.Id == model.Id).SingleOrDefault();
            if (message == null)
            {
                throw new NullReferenceException("Повідомлення не знайдено");
            }

            _objectMapper.Map(model, message);
            message.MessageState = "Registered";
            message.ModifiedOn = DateTime.Now;
            _dataService.SaveChanges();
        }

        public async Task AcceptMessage(Guid id, Guid orgId)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException("Невідомий guid");
            }

            var message = _dataService.GetEntity<Message>(p => p.Id == id).SingleOrDefault();
            if (message == null)
            {
                throw new NullReferenceException("Повідомлення не знайдено");
            }

            var orgInfo = _dataService.GetEntity<OrganizationInfo>(p => p.Id == message.OrganizationInfoId).SingleOrDefault();
            if (orgInfo == null)
            {
                throw new NullReferenceException("Не знайдено інформацію по організації");
            }

            message.MessageState = "Accepted";
            message.ModifiedOn = DateTime.Now;

            switch (message.MessageType)
            {
                case "SgdChiefNameChange":
                    {
                        var org = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfo.OrganizationId).SingleOrDefault();
                        if (string.IsNullOrEmpty(org?.EDRPOU))
                        {
                            orgInfo.Name = message.SgdShiefFullName;
                        }
                        else
                        {
                            orgInfo.OrgDirector = message.SgdShiefFullName;
                        }
                        break;
                    }
                case "SgdNameChange":
                    {
                        var org = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfo.OrganizationId).SingleOrDefault();
                        if (org == null)
                        {
                            throw new NullReferenceException("Не знайдено організацію");
                        }
                        orgInfo.Name = message.SgdNewFullName;
                        break;
                    }
                case "OrgFopLocationChange":
                    {
                        orgInfo.AddressId = message.NewLocationId;
                        break;
                    }
                case "MPDActivitySuspension":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.BranchActivity = "Suspended";
                        break;
                    }
                case "MPDActivityRestoration":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.BranchActivity = "Active";
                        break;
                    }
                case "MPDClosingForSomeActivity":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.BranchActivity = "Closed";
                        break;
                    }
                case "MPDRestorationAfterSomeActivity":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.BranchActivity = "Active";
                        break;
                    }
                case "MPDLocationRatification":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.AddressId = message.AddressBusinessActivityId;
                        break;
                    }
                case "PharmacyHeadReplacement":
                    {
                        var assignee = _dataService.GetEntity<AppAssignee>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        assignee.Name = message.PharmacyHeadName;
                        assignee.MiddleName = message.PharmacyHeadMiddleName;
                        assignee.LastName = message.PharmacyHeadLastName;
                        break;
                    }
                case "PharmacyAreaChange": { break; }
                case "PharmacyNameChange":
                    {
                        var branch = _dataService.GetEntity<Branch>(p => p.Id == message.MpdSelectedId).SingleOrDefault();
                        branch.Name = message.NewPharmacyName;
                        break;
                    }
                case "LeaseAgreementChange": { break; }

                case "ProductionDossierChange":
                    {
                        if (message.IsPrlLicense)
                        {
                            var licenseGuid = _prlLicenseService.GetLicenseGuid(orgId);
                            if (licenseGuid == null)
                            {
                                throw new NullReferenceException("Не знайдена ліцензія");
                            }

                            var prlLicense = _dataService.GetEntity<PrlLicense>(p => p.Id == licenseGuid).SingleOrDefault();
                            if (prlLicense == null)
                            {
                                throw new NullReferenceException("Не знайдена ліцензія");
                            }
                            // TODO: Узнать как происходит создание досье для сразу 2 мпд (сколько добавляется записей в applicant_branch)
                            // В общем для каждого МПД создается своя запись applicant_branch(мпд это org_branch), которая может иметь edocumentBranch
                            var appBranchMsg = _dataService.GetEntity<ApplicationBranch>(p => p.LimsDocumentId == message.Id).ToList();
                            var appBranchApp = _dataService.GetEntity<ApplicationBranch>(p => p.LimsDocumentId == prlLicense.ParentId).ToList();

                            foreach (var applicationBranch in appBranchMsg)
                            {
                                var brnOrg = _dataService.GetEntity<Branch>(p => p.Id == applicationBranch.BranchId).SingleOrDefault();
                                var brnApp = appBranchApp.SingleOrDefault(p => p.BranchId == brnOrg.ParentId);
                                var branchEdoc = _dataService.GetEntity<BranchEDocument>(p => p.BranchId == applicationBranch.BranchId).ToList();
                                branchEdoc.ForEach(doc => doc.BranchId = brnApp.BranchId.Value);
                            }
                        }
                        else if (message.IsImlLicense)
                        {
                            var licenseGuid = _imlLicenseService.GetLicenseGuid(orgId);
                            if (licenseGuid == null)
                            {
                                throw new NullReferenceException("Не знайдена ліцензія");
                            }

                            var imlLicense = _dataService.GetEntity<ImlLicense>(p => p.Id == licenseGuid)
                                .SingleOrDefault();
                            if (imlLicense == null)
                            {
                                throw new NullReferenceException("Не знайдена ліцензія");
                            }

                            // TODO: Узнать как происходит создание досье для сразу 2 мпд (сколько добавляется записей в applicant_branch)
                            // В общем для каждого МПД создается своя запись applicant_branch(мпд это org_branch), которая может иметь edocumentBranch
                            var appBranchMsg = _dataService
                                .GetEntity<ApplicationBranch>(p => p.LimsDocumentId == message.Id).ToList();
                            var appBranchApp = _dataService
                                .GetEntity<ApplicationBranch>(p => p.LimsDocumentId == imlLicense.ParentId).ToList();

                            foreach (var applicationBranch in appBranchMsg)
                            {
                                var brnOrg = _dataService.GetEntity<Branch>(p => p.Id == applicationBranch.BranchId)
                                    .SingleOrDefault();
                                var brnApp = appBranchApp.SingleOrDefault(p => p.BranchId == brnOrg.ParentId);
                                var branchEdoc = _dataService
                                    .GetEntity<BranchEDocument>(p => p.BranchId == applicationBranch.BranchId).ToList();
                                branchEdoc.ForEach(doc => doc.BranchId = brnApp.BranchId.Value);
                            }
                        }

                        break;
                    }
                case "SupplierChange":
                    {
                        var medicineMsg = _dataService.GetEntity<ImlMedicine>(p => p.ApplicationId == message.Id).ToList();
                        if (medicineMsg.Count == 0)
                            return;

                        var medicineLic = _dataService.GetEntity<ImlMedicine>(p => medicineMsg.Select(z => z.ParentId).Contains(p.Id)).ToList();
                        foreach (var t in medicineLic)
                        {
                            var medMsg = medicineMsg.SingleOrDefault(p => p.ParentId == t.Id);
                            var tempName = t.SupplierName;
                            t.SupplierName = medMsg.SupplierName;
                            medMsg.SupplierName = tempName;
                        }
                        break;
                    }
                case "AnotherEvent": { break; }
            }

            await _dataService.SaveChangesAsync();
        }

        public async Task RejectMessage(Guid id, Guid orgId)
        {
            if (id == Guid.Empty)
            {
                throw new NullReferenceException("Невідомий guid");
            }

            var message = _dataService.GetEntity<Message>(p => p.Id == id).SingleOrDefault();
            if (message == null)
            {
                throw new NullReferenceException("Повідомлення не знайдео");
            }

            message.MessageState = "Rejected";
            await _dataService.SaveChangesAsync();
        }

        #endregion


        private void CopyBranchToMessage(Message message, Guid idLicense, LicenseType licenseType)
        {
            switch (licenseType)
            {
                case LicenseType.ePRL_LICENSE:
                    {
                        var license = _dataService.GetEntity<PrlLicense>(lic => lic.Id == idLicense).FirstOrDefault();

                        if (license == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseApplication = _dataService.GetEntity<PrlApplication>(x => x.Id == license.ParentId)
                            .FirstOrDefault();
                        if (licenseApplication == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseBranches = new List<Branch>();
                        var licenseBranch = new List<ApplicationBranch>();

                        var applicationBranchesIds = _dataService
                            .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == licenseApplication.Id && x.BranchId == message.MpdSelectedId)
                            .Select(x => x.BranchId)
                            .Distinct()
                            .ToList();
                        var applicationBranches = _dataService.GetEntity<Branch>(br => applicationBranchesIds.Contains(br.Id))
                            .ToList();

                        foreach (var branch in applicationBranches)
                        {
                            var licBr = _objectMapper.Map<Branch>(branch);
                            licBr.Id = Guid.NewGuid();
                            //ссылка на дочерний обьект мпд
                            licBr.ParentId = branch.Id;
                            licenseBranches.Add(licBr);

                            licenseBranch.Add(new ApplicationBranch
                            {
                                BranchId = licBr.Id,
                                LimsDocumentId = message.Id
                            });
                        }

                        var applicationContractorBranches =
                            _dataService.GetEntity<PrlBranchContractor>(x => applicationBranchesIds.Contains(x.BranchId));
                        var applicaitonContractorIds =
                            applicationContractorBranches.Select(x => x.ContractorId).Distinct().ToList();
                        var applicationContractors = _dataService
                            .GetEntity<PrlContractor>(x => applicaitonContractorIds.Contains(x.Id))
                            .ToList();
                        var licenseContractorBranches = new List<PrlBranchContractor>();
                        var licenseContractors = new List<PrlContractor>();

                        foreach (var applicationContractor in applicationContractors)
                        {
                            var licContractor = _objectMapper.Map<PrlContractor>(applicationContractor);
                            licContractor.Id = Guid.NewGuid();
                            licenseContractors.Add(licContractor);

                            var cntBranches = applicationContractorBranches.Where(x => x.ContractorId == applicationContractor.Id);
                            licenseContractorBranches.AddRange(cntBranches.Select(prlBranchContractor => new PrlBranchContractor
                            {
                                ContractorId = licContractor.Id,
                                BranchId = licenseBranches.FirstOrDefault(br => br.ParentId == prlBranchContractor.BranchId).Id
                            }));
                        }

                        if (message.MessageHierarchyType == "Child")
                        {
                            _dataService.Add(message);
                        }
                        licenseBranches.ForEach(branch => _dataService.Add(branch));
                        licenseBranch.ForEach(appBranch => _dataService.Add(appBranch));
                        licenseContractors.ForEach(contractor => _dataService.Add(contractor));
                        licenseContractorBranches.ForEach(contractorBr => _dataService.Add(contractorBr));
                        break;
                    }
                case LicenseType.eIML_LICENSE:
                    {
                        var license = _dataService.GetEntity<ImlLicense>(lic => lic.Id == idLicense).FirstOrDefault();

                        if (license == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseApplication = _dataService.GetEntity<ImlApplication>(x => x.Id == license.ParentId)
                            .FirstOrDefault();
                        if (licenseApplication == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseBranches = new List<Branch>();
                        var licenseBranch = new List<ApplicationBranch>();

                        var applicationBranchesIds = _dataService
                            .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == licenseApplication.Id && x.BranchId == message.MpdSelectedId)
                            .Select(x => x.BranchId)
                            .Distinct()
                            .ToList();
                        var applicationBranches = _dataService.GetEntity<Branch>(br => applicationBranchesIds.Contains(br.Id))
                            .ToList();

                        foreach (var branch in applicationBranches)
                        {
                            var licBr = _objectMapper.Map<Branch>(branch);
                            licBr.Id = Guid.NewGuid();
                            //ссылка на дочерний обьект мпд
                            licBr.ParentId = branch.Id;
                            licenseBranches.Add(licBr);

                            licenseBranch.Add(new ApplicationBranch
                            {
                                BranchId = licBr.Id,
                                LimsDocumentId = message.Id
                            });
                        }

                        if (message.MessageHierarchyType == "Child")
                        {
                            _dataService.Add(message);
                        }
                        licenseBranches.ForEach(branch => _dataService.Add(branch));
                        licenseBranch.ForEach(appBranch => _dataService.Add(appBranch));
                        break;
                    }
                case LicenseType.eTRL_LICENSE:
                    {
                        var license = _dataService.GetEntity<TrlLicense>(lic => lic.Id == idLicense).FirstOrDefault();

                        if (license == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseApplication = _dataService.GetEntity<TrlApplication>(x => x.Id == license.ParentId)
                            .FirstOrDefault();
                        if (licenseApplication == null)
                        {
                            throw new NullReferenceException("Виникла помилка");
                        }
                        var licenseBranches = new List<Branch>();
                        var licenseBranch = new List<ApplicationBranch>();

                        List<Guid?> applicationBranchesIds = null;
                        if(message.MessageType == "PharmacyHeadReplacement")
                        {
                            applicationBranchesIds = _dataService
                                .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == licenseApplication.Id)
                                .Select(x => x.BranchId)
                                .Distinct()
                                .ToList();
                        }
                        else
                        {
                            applicationBranchesIds = _dataService
                                .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == licenseApplication.Id && x.BranchId == message.MpdSelectedId)
                                .Select(x => x.BranchId)
                                .Distinct()
                                .ToList();
                        }
                        
                        var applicationBranches = _dataService.GetEntity<Branch>(br => applicationBranchesIds.Contains(br.Id))
                            .ToList();

                        foreach (var branch in applicationBranches)
                        {
                            var licBr = _objectMapper.Map<Branch>(branch);
                            licBr.Id = Guid.NewGuid();
                            //ссылка на дочерний обьект мпд
                            licBr.ParentId = branch.Id;
                            licenseBranches.Add(licBr);

                            licenseBranch.Add(new ApplicationBranch
                            {
                                BranchId = licBr.Id,
                                LimsDocumentId = message.Id
                            });
                        }

                        if (message.MessageType == "PharmacyHeadReplacement")
                        {
                            var applicationAssigneeBranches =
                                                        _dataService.GetEntity<AppAssigneeBranch>(x => applicationBranchesIds.Contains(x.BranchId));
                            var applicationAssigneeIds = applicationAssigneeBranches.Select(x => x.AssigneeId).Distinct().ToList();
                            var applicationAssignee =
                                _dataService.GetEntity<AppAssignee>(x => applicationAssigneeIds.Contains(x.Id));
                            var licenseAssigneeBranches = new List<AppAssigneeBranch>();
                            var licenseAssignees = new List<AppAssignee>();

                            foreach (var assignee in applicationAssignee)
                            {
                                var licAssignee = _objectMapper.Map<AppAssignee>(assignee);
                                licAssignee.Id = Guid.NewGuid();
                                licenseAssignees.Add(licAssignee);

                                var assBranches = applicationAssigneeBranches.Where(x => x.AssigneeId == assignee.Id);
                                licenseAssigneeBranches.AddRange(assBranches.Select(appAssigneeBranch => new AppAssigneeBranch
                                {
                                    AssigneeId = licAssignee.Id,
                                    BranchId = licenseBranches.FirstOrDefault(br => br.ParentId == appAssigneeBranch.BranchId).Id
                                }));
                            }

                            licenseAssignees.ForEach(assignee => _dataService.Add(assignee));
                            licenseAssigneeBranches.ForEach(assigneeBr => _dataService.Add(assigneeBr));
                        }


                        if (message.MessageHierarchyType == "Child")
                        {
                            _dataService.Add(message);
                        }
                        licenseBranches.ForEach(branch => _dataService.Add(branch));
                        licenseBranch.ForEach(appBranch => _dataService.Add(appBranch));
                        break;
                    }
            }
        }

        public MessageOrgInfoModel GetOrgInfo(LicenseType licenseType, Guid? orgId = null)
        {
            var result = new MessageOrgInfoModel();
            switch (licenseType)
            {
                case LicenseType.ePRL_LICENSE:
                    {
                        var licenseGuid = orgId == null ? _prlLicenseService.GetLicenseGuid() : _prlLicenseService.GetLicenseGuid(orgId);
                        if (licenseGuid == null)
                        {
                            return result;
                        }

                        var prlOrgInfo = _dataService.GetEntity<PrlLicense>(p => p.Id == licenseGuid).Select(p => new { p.ParentId }).SingleOrDefault();
                        var prl = _dataService.GetEntity<PrlApplication>(p => p.Id == prlOrgInfo.ParentId)
                            .SingleOrDefault();
                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == prl.OrganizationInfoId).Select(p => new { p.OrgDirector, p.Name, p.OrganizationId, p.AddressId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        result.OrgDirector = orgInfoModel?.OrgDirector;
                        result.ParentId = prlOrgInfo?.ParentId;
                        result.Name = orgInfoModel?.Name;
                        result.EDRPOU = orgModel?.EDRPOU;
                        result.AddressId = orgInfoModel.AddressId;
                        return result;
                    }
                case LicenseType.eTRL_LICENSE:
                    {
                        var licenseGuid = orgId == null ? _trlLicenseService.GetLicenseGuid() : _trlLicenseService.GetLicenseGuid(orgId);
                        if (licenseGuid == null)
                        {
                            return result;
                        }

                        var trlOrgInfo = _dataService.GetEntity<TrlLicense>(p => p.Id == licenseGuid).Select(p => new { p.ParentId }).SingleOrDefault();
                        var trl = _dataService.GetEntity<TrlApplication>(p => p.Id == trlOrgInfo.ParentId)
                            .SingleOrDefault();
                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == trl.OrganizationInfoId).Select(p => new { p.OrgDirector, p.Name, p.OrganizationId, p.AddressId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        result.OrgDirector = orgInfoModel?.OrgDirector;
                        result.ParentId = trlOrgInfo?.ParentId;
                        result.Name = orgInfoModel?.Name;
                        result.EDRPOU = orgModel?.EDRPOU;
                        result.AddressId = orgInfoModel.AddressId;
                        return result;
                    }

                case LicenseType.eIML_LICENSE:
                    {
                        var licenseGuid = orgId == null ? _imlLicenseService.GetLicenseGuid() : _imlLicenseService.GetLicenseGuid(orgId);
                        if (licenseGuid == null)
                        {
                            return result;
                        }

                        var imlOrgInfo = _dataService.GetEntity<ImlLicense>(p => p.Id == licenseGuid).Select(p => new { p.ParentId }).SingleOrDefault();
                        var iml = _dataService.GetEntity<ImlApplication>(p => p.Id == imlOrgInfo.ParentId)
                            .SingleOrDefault();
                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == iml.OrganizationInfoId).Select(p => new { p.OrgDirector, p.Name, p.OrganizationId, p.AddressId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        result.OrgDirector = orgInfoModel?.OrgDirector;
                        result.ParentId = imlOrgInfo?.ParentId;
                        result.Name = orgInfoModel?.Name;
                        result.EDRPOU = orgModel?.EDRPOU;
                        result.AddressId = orgInfoModel.AddressId;
                        return result;
                    }
                default: return result;
            }
        }

        public MessageOrgInfoModel GetOrgInfoByMsgId(LicenseType licenseType, Guid msgId)
        {
            var result = new MessageOrgInfoModel();
            var msgModel = _dataService.GetEntity<Message>(p => p.Id == msgId).SingleOrDefault();
            if (msgModel == null)
                return result;

            switch (licenseType)
            {
                case LicenseType.ePRL_LICENSE:
                    {
                        Guid? parentId = msgModel.ParentId;
                        if (msgModel.MessageHierarchyType == "Parent")
                        {
                            var childrenMsg = _dataService.GetEntity<Message>(p => p.MessageParentId == msgModel.Id && p.IsPrlLicense).SingleOrDefault();
                            if (childrenMsg == null)
                                return result;
                            result.OrgDirector = childrenMsg?.SgdShiefOldFullName;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? childrenMsg?.SgdShiefOldFullName : childrenMsg?.SgdOldFullName;
                            result.AddressId = childrenMsg.OldLocationId;
                            parentId = childrenMsg.ParentId;
                        }
                        var licenseModel = _dataService.GetEntity<PrlLicense>(p => p.Id == parentId).SingleOrDefault();
                        if (licenseModel == null)
                            return result;

                        var prlAppModel = _dataService.GetEntity<PrlApplication>(p => p.Id == licenseModel.ParentId).SingleOrDefault();
                        if (prlAppModel == null)
                            return result;

                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == prlAppModel.OrganizationInfoId).Select(p => new { p.OrganizationId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        if (msgModel.MessageHierarchyType != "Parent")
                        {
                            result.AddressId = msgModel.OldLocationId;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? msgModel.SgdShiefOldFullName : msgModel.SgdOldFullName;
                            result.OrgDirector = msgModel.SgdShiefOldFullName;
                        }

                        result.ParentId = prlAppModel?.ParentId;
                        result.EDRPOU = orgModel?.EDRPOU;
                        return result;
                    }
                case LicenseType.eIML_LICENSE:
                    {
                        Guid? parentId = msgModel.ParentId;
                        if (msgModel.MessageHierarchyType == "Parent")
                        {
                            var childrenMsg = _dataService.GetEntity<Message>(p => p.MessageParentId == msgModel.Id && p.IsImlLicense).SingleOrDefault();
                            if (childrenMsg == null)
                                return result;
                            result.OrgDirector = childrenMsg.SgdShiefOldFullName;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? childrenMsg.SgdShiefOldFullName : childrenMsg.SgdOldFullName;
                            result.AddressId = childrenMsg.OldLocationId;
                            parentId = childrenMsg.ParentId;
                        }
                        var licenseModel = _dataService.GetEntity<ImlLicense>(p => p.Id == parentId).SingleOrDefault();
                        if (licenseModel == null)
                            return result;

                        var imlAppModel = _dataService.GetEntity<ImlApplication>(p => p.Id == licenseModel.ParentId).SingleOrDefault();
                        if (imlAppModel == null)
                            return result;

                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == imlAppModel.OrganizationInfoId).Select(p => new { p.OrganizationId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        if (msgModel.MessageHierarchyType != "Parent")
                        {
                            result.AddressId = msgModel.OldLocationId;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? msgModel.SgdShiefOldFullName : msgModel.SgdOldFullName;
                            result.OrgDirector = msgModel.SgdShiefOldFullName;
                        }
                       
                        result.ParentId = imlAppModel?.ParentId;
                        result.EDRPOU = orgModel?.EDRPOU;
                        return result;
                    }
                case LicenseType.eTRL_LICENSE:
                    {
                        Guid? parentId = msgModel.ParentId;
                        if (msgModel.MessageHierarchyType == "Parent")
                        {
                            var childrenMsg = _dataService.GetEntity<Message>(p => p.MessageParentId == msgModel.Id && p.IsTrlLicense).SingleOrDefault();
                            if (childrenMsg == null)
                                return result;
                            result.OrgDirector = childrenMsg.SgdShiefOldFullName;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? childrenMsg.SgdShiefOldFullName : childrenMsg.SgdOldFullName;
                            result.AddressId = childrenMsg.OldLocationId;
                            parentId = childrenMsg.ParentId;
                        }
                        var licenseModel = _dataService.GetEntity<TrlLicense>(p => p.Id == parentId).SingleOrDefault();
                        if (licenseModel == null)
                            return result;

                        var trlAppModel = _dataService.GetEntity<TrlApplication>(p => p.Id == licenseModel.ParentId).SingleOrDefault();
                        if (trlAppModel == null)
                            return result;

                        var orgInfoModel = _dataService.GetEntity<OrganizationInfo>(p => p.Id == trlAppModel.OrganizationInfoId).Select(p => new { p.OrganizationId }).SingleOrDefault();
                        var orgModel = _dataService.GetEntity<OrganizationExt>(p => p.Id == orgInfoModel.OrganizationId).SingleOrDefault();

                        if (msgModel.MessageHierarchyType != "Parent")
                        {
                            result.AddressId = msgModel.OldLocationId;
                            result.Name = msgModel.MessageType == "SgdChiefNameChange" ? msgModel.SgdShiefOldFullName : msgModel.SgdOldFullName;
                            result.OrgDirector = msgModel.SgdShiefOldFullName;
                        }

                        result.ParentId = trlAppModel?.ParentId;
                        result.EDRPOU = orgModel?.EDRPOU;
                        return result;
                    }
                default: return result;
            }
        }

        public string GetViewName(Type messageType, string viewType)
        {
            if (messageType == typeof(SgdChiefNameChangeMessageDTO))
            {
                return "SgdChiefNameChange_" + viewType;
            }
            if (messageType == typeof(SgdNameChangeMessageDTO))
            {
                return "SgdNameChange_" + viewType;
            }
            if (messageType == typeof(OrgFopLocationChangeMessageDTO))
            {
                return "OrgFopLocationChange_" + viewType;
            }
            if (messageType == typeof(MPDActivitySuspensionMessageDTO))
            {
                return "MPDActivitySuspension_" + viewType;
            }
            if (messageType == typeof(MPDActivityRestorationMessageDTO))
            {
                return "MPDActivityRestoration_" + viewType;
            }
            if (messageType == typeof(MPDClosingForSomeActivityMessageDTO))
            {
                return "MPDClosingForSomeActivity_" + viewType;
            }
            if (messageType == typeof(MPDRestorationAfterSomeActivityMessageDTO))
            {
                return "MPDRestorationAfterSomeActivity_" + viewType;
            }
            if (messageType == typeof(MPDLocationRatificationMessageDTO))
            {
                return "MPDLocationRatification_" + viewType;
            }
            if (messageType == typeof(PharmacyHeadReplacementMessageDTO))
            {
                return "PharmacyHeadReplacement_" + viewType;
            }
            if (messageType == typeof(PharmacyAreaChangeMessageDTO))
            {
                return "PharmacyAreaChange_" + viewType;
            }
            if (messageType == typeof(PharmacyNameChangeMessageDTO))
            {
                return "PharmacyNameChange_" + viewType;
            }
            if (messageType == typeof(LeaseAgreementChangeMessageDTO))
            {
                return "LeaseAgreementChange_" + viewType;
            }
            if (messageType == typeof(ProductionDossierChangeMessageDTO))
            {
                return "ProductionDossierChange_" + viewType;
            }
            if (messageType == typeof(SupplierChangeMessageDTO))
            {
                return "SupplierChange_" + viewType;
            }

            if (messageType == typeof(AnotherEventMessageDTO))
            {
                return "AnotherEvent_" + viewType;
            }
            return "";
        }

        public async Task<FilesSignViewModel> GetMsgFilesForSign(Guid id, IConverter converter)
        {
            var fileModel = new FilesSignViewModel() { id = id, files = new List<FilesViewModel>() };
            var rep = new PdfFromHtmlOwnConverter(converter);

            byte[] msgFile;
            var filename = "";

            try
            {
                var newMsgType = _dataService.GetDto<MessageTypeDTO>(x => x.Id == id).FirstOrDefault();

                switch (newMsgType.MessType)
                {
                    case "SgdChiefNameChange":
                        filename = $"Povidomlennia_pro_zminu_PIB_kerivnyka_SHD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFSgdChiefNameChange(id));
                        break;

                    case "OrgFopLocationChange":
                        filename = $"Povidomlennia_pro_zminu_mistseznakhozhdennia_SHD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFOrgFopLocationChange(id));
                        break;

                    case "SgdNameChange":
                        filename = $"Povidomlennia_pro_zminu_nazvy_SHD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFSgdNameChange(id));
                        break;

                    case "AnotherEvent":
                        filename = $"Povidomlennia_pro_inshu_podiiu_SHD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF($"Повідомлення_про_іншу_подію_СГД_{DateTime.Now.ToString("Hmmss")}.pdf", await _msgReportService.MsgToPDFAnotherEvent(id));
                        break;

                    case "MPDActivitySuspension":
                        filename = $"Povidomlennia_pro_pryzupynennia_provadzhennia_diialnosti_MPD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFMPDActivitySuspension(id));
                        break;

                    case "MPDActivityRestoration":
                        filename = $"Povidomlennia_pro_vidnovlennia_provadzhennia_diialnosti_MPD_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFMPDActivityRestoration(id));
                        break;

                    case "MPDClosingForSomeActivity":
                        filename = $"Povidomlennia_pro_zakryttia_MPD_dlia_provedennia_remontnykh_robit_tekhnichnoho_pereobladnannia_chy_inshykh_robit_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFMPDClosingForSomeActivity(id));
                        break;

                    case "MPDRestorationAfterSomeActivity":
                        filename = $"Povidomlennia_pro_vidnovlennia_roboty_MPD_pislia_provedennia_remontnykh_robit_tekhnichnoho_pereobladnannia_chy_inshykh_robit_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFMPDRestorationAfterSomeActivity(id));
                        break;

                    case "MPDLocationRatification":
                        filename = $"Povidomlennia_pro_utochnennia_adresy_mistsia_provadzhennia_diialnosti_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFMPDLocationRatification(id));
                        break;

                    case "PharmacyHeadReplacement":
                        filename = $"Povidomlennia_pro_zminu_zaviduiuchoho_aptechnoho_punktu_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFPharmacyHeadReplacement(id));
                        break;

                    case "PharmacyAreaChange":
                        filename = $"Povidomlennia_pro_zminu_ploshchi_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFPharmacyAreaChange(id));
                        break;

                    case "PharmacyNameChange":
                        filename = $"Povidomlennia_pro_zminu_nazvy_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFPharmacyNameChange(id));
                        break;

                    case "LeaseAgreementChange":
                        filename = $"Povidomlennia_pro_zminu_dohovoru_orendy_aptechnoho_zakladu_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFLeaseAgreementChange(id));
                        break;

                    case "ProductionDossierChange":
                        filename = $"Povidomlennia_pro_zaminu_abo_novu_redaktsiiu_dosie_z_vyrobnytstva_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFProductionDossierChange(id));
                        break;

                    case "SupplierChange":
                        filename = $"Povidomlennia_pro_zminu_postachalnyka_{DateTime.Now.ToString("Hmmss")}.pdf";
                        msgFile = await rep.CreatePDF(filename, await _msgReportService.MsgToPDFSupplierChange(id));
                        break;

                    default:
                        msgFile = null;
                        break;
                }

                fileModel.files.Add(new FilesViewModel()
                {
                    id = Guid.Empty,
                    idFileStore = Guid.Empty,
                    name = filename,
                    file = Convert.ToBase64String(msgFile)
                });
            }
            catch (Exception e)
            {
                Log.Error("Maksim error");
                throw e;
            }
            return fileModel;
        }

        private double GetOriginalLengthInBytes(string base64string)
        {
            if (string.IsNullOrEmpty(base64string)) { return 0; }

            var characterCount = base64string.Length;
            var paddingCount = base64string.Substring(characterCount - 2, 2)
                .Count(c => c == '=');
            return (3 * (characterCount / 4)) - paddingCount;
        }

        private async Task SaveFile(string config, FilesViewModel model, FileStoreDTO fileStoreDto)
        {
            var folderForSave = config + DateTime.Now.ToString("ddMMyyyy") + "/";
            var filePath = Path.GetFullPath(folderForSave);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var newName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".zip";
            var tempfolder = Path.GetTempPath();
            var fullOrigPath = tempfolder + model.name;
            string fullZipPath = filePath + newName;
            var directoryInfo = (new FileInfo(fullOrigPath)).Directory;
            directoryInfo?.Create();

            await File.WriteAllBytesAsync(fullOrigPath, Convert.FromBase64String(model.file));

            CreateZip(fullZipPath, fullOrigPath);

            FileStoreHelper.DeleteFileIfExist(fullOrigPath);
            fileStoreDto.FileName = newName;
            fileStoreDto.FilePath = fullZipPath;
            _dataService.Add<FileStore>(fileStoreDto);
        }

        private static bool CreateZip(string zipFileName, string fileToZip)
        {
            FileInfo zipFile = new FileInfo(zipFileName);
            FileStream fs = zipFile.Create();
            using (ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(fileToZip, Path.GetFileName(fileToZip), CompressionLevel.Optimal);
            }
            return true;
        }
    }
}
