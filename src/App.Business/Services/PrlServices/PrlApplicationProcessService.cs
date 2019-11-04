using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Helpers;
using App.Business.Services.LimsService;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Data.DTO.APP;
using App.Data.DTO.PRL;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using Newtonsoft.Json;
using Serilog;

namespace App.Business.Services.PrlServices
{
    public class PrlApplicationProcessService
    {
        private readonly ICommonDataService _dataService;
        private readonly LimsExchangeService _limsExchangeService;
        private readonly IObjectMapper _objectMapper;
        private readonly IEntityStateHelper _entityStateHelper;

        public PrlApplicationProcessService(ICommonDataService dataService, IObjectMapper objectMapper,
            IEntityStateHelper entityStateHelper, LimsExchangeService limsExchangeService)
        {
            _dataService = dataService;
            _objectMapper = objectMapper;
            _entityStateHelper = entityStateHelper;
            _limsExchangeService = limsExchangeService;
        }

        #region Decision

        public void SaveDecision(AppDecisionDTO model)
        {
            var decision = _dataService.GetEntity<AppDecision>(p => p.Id == model.Id).SingleOrDefault();
            var isUpdate = false;
            if (decision == null)
            {
                decision = new AppDecision();
            }
            else
            {
                _dataService.GetEntity<AppDecisionReason>(p => p.AppDecisionId == decision.Id).ToList()
                    .ForEach(x => _dataService.Remove(x));
                isUpdate = true;
            }
            _objectMapper.Map(model, decision);

            foreach (var reason in model.ListOfDecisionReason)
            {
                decision.AppDecisionReasons.Add(new AppDecisionReason { ReasonType = reason, AppDecisionId = decision.Id });
            }

            var prl = _dataService.GetEntity<PrlApplication>(p => p.Id == decision.AppId).SingleOrDefault();
            if (prl == null || string.IsNullOrEmpty(prl.ExpertiseResult) || prl.AppState == "Reviewed")
            {
                return;
            }
            prl.AppDecision = decision;

            _dataService.Add(decision, isUpdate);
            _limsExchangeService.ExportDecision(decision, isUpdate);

            _dataService.SaveChanges();
        }

        public async Task UpdateDecision(AppProtocol closedProtocol)
        {
            var decisions = _limsExchangeService.DataService.GetEntity<AppDecision>(p =>
                p.ProtocolId == closedProtocol.Id).ToList();
            foreach (var decision in decisions)
            {
                if (decision.DecisionType == "Accepted")
                {
                    CreateLicenseFromApplication(decision, closedProtocol);
                }
                else
                {
                    decision.IsClosed = true;
                    var app = _limsExchangeService.DataService
                        .GetEntity<PrlApplication>(p => p.Id == decision.AppId).SingleOrDefault();
                    app.AppState = "Reviewed";
                    app.BackOfficeAppState = "Reviewed";
                    _limsExchangeService.DataService.SaveChanges();
                }
                _limsExchangeService.CloseApplication(decision.AppId);
            }
        }

        public bool RemoveDecision(Guid appId)
        {
            var decision = _dataService.GetEntity<AppDecision>(p => p.AppId == appId).SingleOrDefault();
            if (decision == null)
            {
                return false;
            }

            _dataService.Remove(decision);

            var prlApplication = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).Single();
            _limsExchangeService.RemoveDecision(prlApplication.OldLimsId);
            _dataService.SaveChanges();
            return true;
        }

        #endregion

        #region PreLicenseCheck
        public bool RemovePreLicenseCheck(Guid appId)
        {
            var licCheck = _dataService.GetEntity<AppPreLicenseCheck>(p => p.AppId == appId).SingleOrDefault();
            if (licCheck == null)
            {
                return false;
            }

            _dataService.Remove(licCheck);

            //var prlApplication = _dataService.GetEntity<PrlApplication>(x => x.Id == appId).Single();
            if (licCheck.OldLimsId != null)
            {
                _limsExchangeService.DeletePreLicenseCheck(licCheck.OldLimsId.Value);
            }
            _dataService.SaveChanges();
            return true;
        }
        #endregion

        public void SaveExpertise(PrlAppExpertiseDTO model)
        {
            var prl = _dataService.GetEntity<PrlApplication>(p => p.Id == model.Id).SingleOrDefault();
            if (prl == null)
                return;

            var orgUnitId = prl.OrgUnitId;
            _objectMapper.Map(model, prl);
            prl.ExpertiseResult = model.ExpertiseResultEnum;
            prl.PerformerOfExpertise = model.PerformerOfExpertiseId;
            prl.OrgUnitId = orgUnitId;

            _dataService.SaveChanges();
            _limsExchangeService.UpdateExpertise(model);
        }

        public bool ReturnApplication(AppShortDTO model)
        {
            var prlApplication = _dataService.GetEntity<PrlApplication>(app => app.Id == model.Id).Single();
          
            try
            {
                prlApplication.ReturnComment = model.ReturnComment;
                prlApplication.ReturnCheck = true;
                _dataService.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool RegisterApplication(AppShortDTO model)
        {
            _entityStateHelper.CheckChangeAppStatus(model.Id, "Registered");
            var prlApplication = _dataService.GetEntity<PrlApplication>(app => app.Id == model.Id).Single();
            //if (prlApplication.RegNumber == model.RegNumber)
            //{
            //    return false;
            //}
            _objectMapper.Map(model, prlApplication);
            _dataService.SaveChanges();
            var appDetailDto = _dataService.GetDto<PrlAppDetailDTO>(application => application.Id == prlApplication.Id).Single();

            var DublicateRegNum = _dataService.GetDto<PrlAppDetailDTO>(x =>
                x.RegNumber == appDetailDto.RegNumber && x.RegDate == appDetailDto.RegDate).ToList();

            if (DublicateRegNum.Count <= 1)
            {
                var backOfficeState = prlApplication.BackOfficeAppState;
                var appState = prlApplication.AppState;

                prlApplication.BackOfficeAppState = "Registered";
                prlApplication.AppState = "InReview";
                _dataService.SaveChanges();

                try
                {
                    if (prlApplication.AppSort != "AdditionalInfoToLicense")
                        _limsExchangeService.InsertApplication(prlApplication);
                }
                catch (Exception e)
                {
                    prlApplication.BackOfficeAppState = backOfficeState;
                    prlApplication.AppState = appState;
                    prlApplication.RegDate = null;
                    prlApplication.RegNumber = null;
                    _dataService.SaveChanges();
                    throw new Exception("Виникла помилка при регестрації заяви. Зверніться до адміністратора", e);
                }

                return true;
            }
            else
            {
                //_dataService.Remove(prlApplication);
                return false;
            }
        }

        public void SavePreLicenseCheck(AppPreLicenseCheckDTO model)
        {
            var prl = _dataService.GetEntity<PrlApplication>(p => p.Id == model.AppId).SingleOrDefault();
            if (prl == null || string.IsNullOrEmpty(prl.ExpertiseResult) || prl.ExpertiseResult == "Negative")
            {
                return;
            }

            var preLicense = _dataService.GetEntity<AppPreLicenseCheck>(p => p.Id == model.Id).SingleOrDefault();
            if (preLicense != null)
            {
                return;
            }

            preLicense = new AppPreLicenseCheck();
            _objectMapper.Map(model, preLicense);
            _dataService.Add(preLicense);
            _dataService.SaveChanges();

            _limsExchangeService.ExportPreLicenseCheck(preLicense);
        }

        public async Task UpdatePreLicenseCheck()
        {
            await _limsExchangeService.UpdatePreLicenseCheck();
        }

        public void SaveLicenseMessage(AppLicenseMessageDTO model)
        {
            var prl = _dataService.GetEntity<PrlApplication>(p => p.Id == model.AppId).SingleOrDefault();
            if (prl == null || string.IsNullOrEmpty(prl.ExpertiseResult) || prl.AppDecisionId == null)
            {
                return;
            }

            var licenseMsg = _dataService.GetEntity<AppLicenseMessage>(p => p.Id == model.Id).SingleOrDefault();
            if (licenseMsg != null)
            {
                return;
            }

            licenseMsg = new AppLicenseMessage();
            _objectMapper.Map(model, licenseMsg);
            licenseMsg.Performer = model.PerformerId;
            licenseMsg.OldLimsId = _limsExchangeService.ExportLimsMessage(licenseMsg).Result;
            licenseMsg.State = "Підготовка";
            _dataService.Add(licenseMsg);
            _dataService.SaveChanges();
        }

        public async Task<List<AppLicenseMessage>> UpdateLicenseMessage()
        {
            return await _limsExchangeService.UpdateLimsMessages();
        }

        public async Task UpdateEndLicCheck()
        {
            await _limsExchangeService.UpdateEndLicCheck();
        }

        public void CreateLicenseFromApplication(AppDecision decision, AppProtocol protocol)
        {
            var application = _dataService.GetEntity<PrlApplication>(prlApplication => prlApplication.Id == decision.AppId)
                .FirstOrDefault();
            long id = 0;

            if (application == null)
            {
                Log.Error("CreateLicenseFromApplication - Заяву не знайдено");
                return;
            }

            var oldLicense = _dataService
                .GetEntity<PrlLicense>(x => x.OrgUnitId == application.OrgUnitId && x.LicState == "Active" && x.IsRelevant)
                .FirstOrDefault();
            try
            {
                if (decision == null)
                {
                    Log.Error("CreateLicenseFromApplication - Рішення не знайдено");
                    application.ErrorProcessingLicense = "Рішення не знайдено";
                    throw new Exception();
                }

                decision.IsClosed = true;
                _dataService.SaveChanges();

                if (protocol == null)
                {
                    Log.Error("CreateLicenseFromApplication - Протокол не знайдено");
                    application.ErrorProcessingLicense = "Протокол не знайдено";
                    throw new Exception();
                }

                if (application.AppSort == "GetLicenseApplication" || application.AppSort == "IncreaseToPRLApplication")
                {
                    if (oldLicense != null)
                    {
                        Log.Error("CreateLicenseFromApplication - У даного СГД вже є активна ліцензія");
                        application.ErrorProcessingLicense = "У даного СГД вже є активна ліцензія";
                        throw new Exception();
                    }
                }
                else
                {
                    if (oldLicense == null)
                    {
                        Log.Error("CreateLicenseFromApplication - У даного СГД немає активної ліцензії");
                        application.ErrorProcessingLicense = "У даного СГД немає активної ліцензії";
                        throw new Exception();
                    }
                    oldLicense.IsRelevant = false;
                    id = oldLicense.OldLimsId;
                }
            }
            catch (Exception)
            {
                _dataService.SaveChanges();
                return;
            }

            long limsId;
            if (application.AppSort == "GetLicenseApplication"
                || application.AppSort == "IncreaseToPRLApplication"
                || application.AppSort == "CancelLicenseApplication"
                || application.AppSort == "DecreasePRLApplication"
                || application.AppSort == "AddBranchApplication"
                || application.AppSort == "RemBranchApplication"
                || application.AppSort == "ChangeAutPersonApplication"
                || application.AppSort == "AddBranchInfoApplication"
                || application.AppSort == "RemBranchInfoApplication"
                || application.AppSort == "ChangeContrApplication"
                )
            {
                try
                {
                    limsId = _limsExchangeService.InsertLicense(decision.AppId).Result;
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    return;
                }
            }
            else
            {
                //lims insert for other sorts of application
                limsId = id;//id of new limsDoc
            }

            var newLicense = new PrlLicense
            {
                OldLimsId = limsId,
                OrgUnitId = application.OrgUnitId,
                Id = Guid.NewGuid(),
                ParentId = application.Id,
                LicType = "PRL",
                LicState = "Active",
                IsRelevant = true,
                LicenseDate = decision.DateOfStart,
                OrderNumber = protocol.OrderNumber,
                OrderDate = protocol.OrderDate.Value
            };

            switch (application.AppSort)
            {
                case "RemBranchApplication":
                    {
                        var appBranches = _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == application.Id).Select(x => x.BranchId).ToList();
                        var branches = _dataService.GetEntity<Branch>(x => appBranches.Contains(x.Id) && x.LicenseDeleteCheck == true).ToList();
                        branches.ForEach(x => x.RecordState = RecordState.D);
                        branches.ForEach(x => x.BranchActivity = "Closed");
                        break;
                    }
                case "CancelLicenseApplication":
                case "DecreasePRLApplication":
                    newLicense.LicState = "Canceled";
                    break;
                case "AddBranchInfoApplication":
                    {
                        var appBranches = _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == application.Id)
                            .Select(x => x.BranchId).ToList();
                        var branches = _dataService
                            .GetEntity<Branch>(x => appBranches.Contains(x.Id)).ToList();
                        foreach (var branch in branches)
                        {
                            List<Dictionary<string, string>> operationListForm;
                            try
                            {
                                operationListForm =
                                    JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(
                                        branch.OperationListForm);
                            }
                            catch (Exception)
                            {
                                operationListForm = new List<Dictionary<string, string>>();
                            }
                            List<Dictionary<string, string>> operationListFormChanging;
                            try
                            {
                                operationListFormChanging =
                                    JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(
                                        branch.OperationListFormChanging);
                            }
                            catch (Exception)
                            {
                                operationListFormChanging = new List<Dictionary<string, string>>();
                            }

                            foreach (var itemChanging in operationListFormChanging)
                            {
                                var exists = false;
                                var itemValueChanging = itemChanging.FirstOrDefault().Value;
                                foreach (var item in operationListForm)
                                {
                                    var itemValue = item.FirstOrDefault().Value;
                                    if (itemValueChanging == itemValue)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (exists == false)
                                {
                                    operationListForm.Add(itemChanging);
                                }
                            }
                            branch.OperationListForm = JsonConvert.SerializeObject(operationListForm.OrderBy(x => x.FirstOrDefault().Value));
                            branch.OperationListFormChanging = null;
                        }
                        break;
                    }
                case "RemBranchInfoApplication":
                    {
                        var appBranches = _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == application.Id)
                            .Select(x => x.BranchId).ToList();
                        var branches = _dataService
                            .GetEntity<Branch>(x => appBranches.Contains(x.Id)).ToList();
                        foreach (var branch in branches)
                        {
                            List<Dictionary<string, string>> operationListForm;
                            try
                            {
                                operationListForm =
                                    JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(
                                        branch.OperationListForm);
                            }
                            catch (Exception)
                            {
                                operationListForm = new List<Dictionary<string, string>>();
                            }
                            List<Dictionary<string, string>> operationListFormChanging;
                            try
                            {
                                operationListFormChanging =
                                    JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(
                                        branch.OperationListFormChanging);
                            }
                            catch (Exception)
                            {
                                operationListFormChanging = new List<Dictionary<string, string>>();
                            }
                            foreach (var itemChanging in operationListFormChanging)
                            {
                                var itemValueChanging = itemChanging.FirstOrDefault().Value;
                                for (int i = operationListForm.Count - 1; i >= 0; i--)
                                {
                                    var itemValue = operationListForm[i].FirstOrDefault().Value;
                                    if (itemValue == itemValueChanging)
                                        operationListForm.RemoveAt(i);
                                }
                            }
                            branch.OperationListForm =
                                JsonConvert.SerializeObject(operationListForm);
                            branch.OperationListFormChanging = null;
                        }
                        break;
                    }
            }

            _dataService.Add(newLicense);

            application.AppState = "Reviewed";
            application.BackOfficeAppState = "Reviewed";

            _dataService.SaveChanges();
        }

        public void ProcessApplicationToLicense(Guid appId)
        {
            var application = _dataService.GetEntity<PrlApplication>(prlApplication => prlApplication.Id == appId)
                .FirstOrDefault();

            if (application == null)
            {
                throw new NullReferenceException("Заяву не знайдено");
            }

            //создание обьекта лицензии
            var license = new PrlLicense();
            _objectMapper.Map(application, license);
            license.Id = Guid.NewGuid();
            //ссылка на дочернюю заяву, из которой создана лицензия
            license.ParentId = application.Id;

            var licenseBranches = new List<Branch>();
            var licenseBranch = new List<ApplicationBranch>();
            var applicationBranchesIds = _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId).Select(x => x.BranchId).Distinct();
            var applicationBranches = _dataService.GetEntity<Branch>(br => applicationBranchesIds.Contains(br.Id));

            foreach (var branch in applicationBranches)
            {
                var licBr = _objectMapper.Map<Branch>(branch);
                licBr.Id = Guid.NewGuid();
                //ссылка на дочерний обьект мпд
                licBr.ParentId = branch.Id;
                licenseBranches.Add(licBr);

                licenseBranch.Add(new ApplicationBranch { BranchId = licBr.Id, LimsDocumentId = license.Id });
            }

            var applicationAssigneeBranches =
                _dataService.GetEntity<AppAssigneeBranch>(x => applicationBranchesIds.Contains(x.BranchId));
            var applicationAssigneeIds = applicationAssigneeBranches.Select(x => x.AssigneeId).Distinct();
            var applicationAssignee = _dataService.GetEntity<AppAssignee>(x => applicationAssigneeIds.Contains(x.Id));
            var licenseAssigneeBranches = new List<AppAssigneeBranch>();
            var licenseAssignees = new List<AppAssignee>();

            foreach (var assignee in applicationAssignee)
            {
                var licAssignee = _objectMapper.Map<AppAssignee>(assignee);
                licAssignee.Id = Guid.NewGuid();
                licenseAssignees.Add(licAssignee);

                var assBranches = applicationAssigneeBranches.Where(x => x.AssigneeId == assignee.Id);
                foreach (var appAssigneeBranch in assBranches)
                {
                    licenseAssigneeBranches.Add(new AppAssigneeBranch
                    {
                        AssigneeId = licAssignee.Id,
                        BranchId = applicationBranches.FirstOrDefault(br => br.ParentId == appAssigneeBranch.BranchId)
                            .Id
                    });
                }
            }

            var applicationContractorBranches =
                _dataService.GetEntity<PrlBranchContractor>(x => applicationBranchesIds.Contains(x.BranchId));
            var applicaitonContractorIds = applicationContractorBranches.Select(x => x.ContractorId).Distinct();
            var applicationContractors =
                _dataService.GetEntity<PrlContractor>(x => applicaitonContractorIds.Contains(x.Id));
            var licenseContractorBranches = new List<PrlBranchContractor>();
            var licenseContractors = new List<PrlContractor>();

            foreach (var applicationContractor in applicationContractors)
            {
                var licContractor = _objectMapper.Map<PrlContractor>(applicationContractor);
                licContractor.Id = Guid.NewGuid();
                licenseContractors.Add(licContractor);

                var cntBranches = applicationContractorBranches.Where(x => x.ContractorId == applicationContractor.Id);
                foreach (var prlBranchContractor in cntBranches)
                {
                    licenseContractorBranches.Add(new PrlBranchContractor
                    {
                        ContractorId = licContractor.Id,
                        BranchId = applicationBranches.FirstOrDefault(br => br.ParentId == prlBranchContractor.BranchId)
                            .Id
                    });
                }
            }

            _dataService.Add(license);
            licenseBranches.ForEach(branch => _dataService.Add(branch));
            licenseContractors.ForEach(contractor => _dataService.Add(contractor));
            licenseContractorBranches.ForEach(contractorBr => _dataService.Add(contractorBr));
            licenseAssignees.ForEach(assignee => _dataService.Add(assignee));
            licenseAssigneeBranches.ForEach(assigneeBr => _dataService.Add(assigneeBr));
        }
    }
}
