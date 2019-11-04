using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Helpers;
using App.Business.Services.LimsService;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Data.Contexts;
using App.Data.DTO.APP;
using App.Data.DTO.IML;
using App.Data.Models.APP;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using Newtonsoft.Json;
using Serilog;

namespace App.Business.Services.ImlServices
{
    public class ImlApplicationProcessService
    {
        private readonly ICommonDataService _dataService;
        private readonly LimsExchangeService _limsExchangeService;
        private readonly IObjectMapper _objectMapper;
        private readonly IEntityStateHelper _entityStateHelper;
        private readonly MigrationDbContext _context;
        private readonly ImlMedicineService _imlMedicineService;

        public ImlApplicationProcessService(ICommonDataService dataService,
                                            LimsExchangeService limsExchangeService,
                                            IObjectMapper objectMapper,
                                            IEntityStateHelper entityStateHelper,
                                            MigrationDbContext context, ImlMedicineService imlMedicineService)
        {
            _dataService = dataService;
            _limsExchangeService = limsExchangeService;
            _objectMapper = objectMapper;
            _entityStateHelper = entityStateHelper;
            _context = context;
            _imlMedicineService = imlMedicineService;
        }

        public void SaveExpertise(ImlAppExpertiseDTO model)
        {
            var iml = _dataService.GetEntity<ImlApplication>(p => p.Id == model.Id).SingleOrDefault();
            if (iml == null)
                return;

            var orgUnitId = iml.OrgUnitId;
            _objectMapper.Map(model, iml);
            iml.ExpertiseResult = model.ExpertiseResultEnum;
            iml.PerformerOfExpertise = model.PerformerOfExpertiseId;
            iml.OrgUnitId = orgUnitId;

            _dataService.SaveChanges();
            _limsExchangeService.UpdateExpertiseIML(model); // сделал, но нет экспертизы в заяве , 
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

            var iml = _dataService.GetEntity<ImlApplication>(p => p.Id == decision.AppId).SingleOrDefault();
            if (iml == null || string.IsNullOrEmpty(iml.ExpertiseResult) || iml.AppState == "Reviewed")
            {
                return;
            }
            iml.AppDecision = decision;

            _dataService.Add(decision, isUpdate);
            _limsExchangeService.ExportDecisionIML(decision, isUpdate); // TODO: Сделать

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
                        .GetEntity<ImlApplication>(p => p.Id == decision.AppId).SingleOrDefault();
                    app.AppState = "Reviewed";
                    app.BackOfficeAppState = "Reviewed";
                    _limsExchangeService.DataService.SaveChanges();
                }
                _limsExchangeService.CloseApplicationIML(decision.AppId);
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


            var imlApplication = _dataService.GetEntity<ImlApplication>(x => x.Id == appId).Single();
            _limsExchangeService.RemoveDecisionIML(imlApplication.OldLimsId);
            _dataService.SaveChanges();
            return true;
        }

        #endregion

        #region PreLicense
        public bool RemovePreLicenseCheck(Guid appId)
        {
            var licCheck = _dataService.GetEntity<AppPreLicenseCheck>(p => p.AppId == appId).SingleOrDefault();
            if (licCheck == null)
            {
                return false;
            }

            _dataService.Remove(licCheck);

            if (licCheck.OldLimsId != null)
            {
                _limsExchangeService.DeletePreLicenseCheckIML(licCheck.OldLimsId.Value); // TODO: Сделать
            }
            _dataService.SaveChanges();
            return true;
        }

        public void SavePreLicenseCheck(AppPreLicenseCheckDTO model)
        {
            var iml = _dataService.GetEntity<ImlApplication>(p => p.Id == model.AppId).SingleOrDefault();
            if (iml == null || string.IsNullOrEmpty(iml.ExpertiseResult) || iml.ExpertiseResult == "Negative")
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

            _limsExchangeService.ExportPreLicenseCheckIML(preLicense); //TODO: Сделать спросить Юры
        }

        public async Task UpdatePreLicenseCheck()
        {
            // TODO: Сделать
            await _limsExchangeService.UpdatePreLicenseCheck();
        }

        #endregion

        #region LicenseMessage

        public void SaveLicenseMessage(AppLicenseMessageDTO model)
        {
            var iml = _dataService.GetEntity<ImlApplication>(p => p.Id == model.AppId).SingleOrDefault();
            if (iml == null || string.IsNullOrEmpty(iml.ExpertiseResult) || iml.AppDecisionId == null)
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
            licenseMsg.OldLimsId = _limsExchangeService.ExportLimsMessageIML(licenseMsg).Result;
            licenseMsg.State = "Підготовка";
            _dataService.Add(licenseMsg);
            _dataService.SaveChanges();
        }

        public async Task<List<AppLicenseMessage>> UpdateLicenseMessage()
        {
            // TODO: Сделать
            return await _limsExchangeService.UpdateLimsMessages(); //?? test this
        }

        #endregion

        public bool ReturnApplication(AppShortDTO model)
        {
            var prlApplication = _dataService.GetEntity<ImlApplication>(app => app.Id == model.Id).Single();

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
            var imlApplication = _dataService.GetEntity<ImlApplication>(app => app.Id == model.Id).Single();

            _objectMapper.Map(model, imlApplication);
            _dataService.SaveChanges();
            var appDetailDto = _dataService.GetDto<ImlAppDetailDTO>(application => application.Id == imlApplication.Id).Single();

            var DublicateRegNum = _dataService.GetDto<ImlAppDetailDTO>(x =>
                x.RegNumber == appDetailDto.RegNumber && x.RegDate == appDetailDto.RegDate).ToList();

            if (DublicateRegNum.Count <= 1)
            {
                var backOfficeState = imlApplication.BackOfficeAppState;
                var appState = imlApplication.AppState;

                imlApplication.BackOfficeAppState = "Registered";
                imlApplication.AppState = "InReview";
                _dataService.SaveChanges();

                try
                {
                    if (imlApplication.AppSort != "AdditionalInfoToLicense")
                        _limsExchangeService.InsertApplication(imlApplication);
                }
                catch (Exception e)
                {
                    imlApplication.BackOfficeAppState = backOfficeState;
                    imlApplication.AppState = appState;
                    imlApplication.RegDate = null;
                    imlApplication.RegNumber = null;
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

        public void CreateLicenseFromApplication(AppDecision decision, AppProtocol protocol)
        {
            var application = _dataService.GetEntity<ImlApplication>(imlApplication => imlApplication.Id == decision.AppId)
                .FirstOrDefault();
            long id = 0;

            if (application == null)
            {
                Log.Error("[IML]CreateLicenseFromApplication - Заяву не знайдено");
                return;
            }

            var oldLicense = _dataService
                .GetEntity<ImlLicense>(x => x.OrgUnitId == application.OrgUnitId && x.LicState == "Active" && x.IsRelevant)
                .FirstOrDefault();
            try
            {
                if (decision == null)
                {
                    Log.Error("[IML]CreateLicenseFromApplication - Рішення не знайдено");
                    application.ErrorProcessingLicense = "Рішення не знайдено";
                    throw new Exception();
                }

                decision.IsClosed = true;
                _dataService.SaveChanges();

                if (protocol == null)
                {
                    Log.Error("[IML]CreateLicenseFromApplication - Протокол не знайдено");
                    application.ErrorProcessingLicense = "Протокол не знайдено";
                    throw new Exception();
                }

                if (application.AppSort == "GetLicenseApplication" || application.AppSort == "IncreaseToIMLApplication")
                {
                    if (oldLicense != null)
                    {
                        Log.Error("[IML]CreateLicenseFromApplication - У даного СГД вже є активна ліцензія");
                        application.ErrorProcessingLicense = "У даного СГД вже є активна ліцензія";
                        throw new Exception();
                    }
                }
                else
                {
                    if (oldLicense == null)
                    {
                        Log.Error("[IML]CreateLicenseFromApplication - У даного СГД немає активної ліцензії");
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

            long limsId = -1;
            if (application.AppSort == "GetLicenseApplication"
                || application.AppSort == "IncreaseToIMLApplication"
                || application.AppSort == "CancelLicenseApplication"
                || application.AppSort == "DecreaseIMLApplication"
                || application.AppSort == "ChangeAutPersonApplication"
                || application.AppSort == "AddBranchApplication"
                || application.AppSort == "RemBranchApplication"
                || application.AppSort == "ChangeDrugList"
                || application.AppSort == "ReplacementDrugList")
            {
                try
                {
                    limsId = _limsExchangeService.InsertLicenseIML(decision.AppId).Result;
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

            var newLicense = new ImlLicense
            {
                OldLimsId = limsId,
                OrgUnitId = application.OrgUnitId,
                Id = Guid.NewGuid(),
                ParentId = application.Id,
                LicType = "IML",
                LicState = "Active",
                IsRelevant = true,
                LicenseDate = decision.DateOfStart,
                OrderNumber = protocol.OrderNumber,
                OrderDate = protocol.OrderDate.Value
            };

            switch (application.AppSort)
            {
                case "ChangeDrugList":
                    {
                        _imlMedicineService.UpdateStatus(application.Id);
                        break;
                    }
                case "ReplacementDrugList":
                    {
                        Task.WaitAll(_imlMedicineService.DeleteAll(application.Id, true));
                        _imlMedicineService.UpdateStatus(application.Id);
                        break;
                    }
                case "RemBranchApplication":
                    {
                        var appBranches = _dataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == application.Id).Select(x => x.BranchId).ToList();
                        var branches = _dataService.GetEntity<Branch>(x => appBranches.Contains(x.Id) && x.LicenseDeleteCheck == true).ToList();
                        branches.ForEach(x => x.RecordState = RecordState.D);
                        branches.ForEach(x => x.BranchActivity = "Closed");
                        break;
                    }
                case "CancelLicenseApplication":
                case "DecreaseIMLApplication":
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
            }

            _dataService.Add(newLicense);

            application.AppState = "Reviewed";
            application.BackOfficeAppState = "Reviewed";

            _dataService.SaveChanges();
        }
    }
}
