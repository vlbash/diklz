using System;
using System.Collections.Generic;
using System.Linq;
using App.Business.Exceptions;
using App.Core.Business.Services;
using App.Core.Business.Services.DistributedCacheService;
using App.Data.DTO.APP;
using App.Data.DTO.IML;
using App.Data.Models.APP;
using App.Data.Models.DOC;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using App.Data.Models.PRL;

namespace App.Business.Helpers
{
    public interface IEntityStateHelper
    {
        /// <summary>
        ///     Checks application states with given appId.
        ///     Returns <c>null</c> if there is no such app
        /// </summary>
        /// <param name="appId">Id of the app.</param>
        /// <returns>Dictionary with three keys: AppSort, AppState and BackOfficeAppState.</returns>
        /// <exception cref="System.Exception">Throws if could not find an application with such id, or could not read the database</exception>
        Dictionary<string, string> GetAppStates(Guid? appId);

        /// <summary>
        ///     Check if application status could be changed, throws <c>App.Business.Exceptions.BusinessRulesException</c> error if
        ///     not.
        /// </summary>
        /// <param name="appId">Application Id</param>
        /// <param name="targetState">Target state</param>
        void CheckChangeAppStatus(Guid? appId, string targetState);

        /// <summary>
        ///     Checks if application with appId can be edited.
        ///     Returns <c>null</c> if there is no such app
        /// </summary>
        /// <param name="appId">Id of the app.</param>
        /// <returns>true/false - result, null if id is empty or there is no such app.</returns>
        bool? IsEditableApp(Guid? appId, bool raiseError = false);

        /// <summary>
        ///     Checks if branch could be edited. Returns true if yes, null if not found. Returns false if raiseError false, throws
        ///     error if not
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        bool? IsEditableBranch(Guid? branchId, bool raiseError = false);

        /// <summary>
        ///     Checks if contractor could be edited. Returns true if yes, null if not found. Returns false if raiseError false,
        ///     throws error if not
        /// </summary>
        /// <param name="contractorId"></param>
        /// <returns></returns>
        bool? IsEditableContractor(Guid? contractorId, bool raiseError = false);

        /// <summary>
        ///     Checks if contractor could be edited. Returns true if yes, null if not found. Returns false if raiseError false,
        ///     throws error if not
        /// </summary>
        /// <param name="assigneeId"></param>
        /// <returns></returns>
        bool? IsEditableAuth(Guid? assigneeId, bool raiseError = false);

        bool? IsEditableEdoc(Guid? edocId, bool raiseError = false);

        bool? IsEditableMedicine(Guid? medicineId, bool raiseError = false);

        AccessStruct ApplicationAddability(Guid appId);
    }

    public class EntityStateHelper: IEntityStateHelper
    { 
        public EntityStateHelper(ICommonDataService dataService, IDistributedCacheService cacheService)
        {
            DataService = dataService;
            CacheService = cacheService;
        }

        private ICommonDataService DataService { get; }
        private IDistributedCacheService CacheService { get; }

        public bool? IsEditableApp(Guid? appId, bool raiseError = false)
        {
            if (appId == null || appId == Guid.Empty)
            {
                return null;
            }

            var appStates = GetAppStates(appId);
            if (appStates == null)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var BackOfficeAppState = appStates["BackOfficeAppState"];
                var AppState = appStates["AppState"];

                if (string.IsNullOrEmpty(BackOfficeAppState) && AppState == "Project" ||
                    BackOfficeAppState == "Project" && string.IsNullOrEmpty(AppState))
                {
                    var AppSort = appStates["AppSort"];
                    switch (AppSort)
                    {
                        case "IncreaseToPRLApplication":
                        case "IncreaseToIMLApplication":
                        case "IncreaseToTRLApplication":
                        case "RenewLicenseApplication":
                        case "GetLicenseApplication":
                        case "AdditionalInfoToLicense":
                            return true;
                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Ця заява не може бути редагована!");
        }


        public bool? IsEditableBranch(Guid? branchId, bool raiseError = false)
        {
            if (branchId == null || branchId == Guid.Empty)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var appId = DataService.GetEntity<ApplicationBranch>(apBrn => apBrn.BranchId == branchId).Single()
                    .LimsDocumentId;
                var appStates = GetAppStates(appId);
                if (appStates[nameof(BaseApplication.AppState)] == "Project" ||
                    appStates[nameof(BaseApplication.BackOfficeAppState)] == "Project")
                {
                    var appSort = appStates[nameof(BaseApplication.AppSort)];
                    switch (appSort)
                    {
                        case "GetLicenseApplication":
                        case "RenewLicenseApplication":
                        case "AddBranchInfoApplication":
                        case "RemBranchInfoApplication":
                        case "AdditionalInfoToLicense":
                            return true;

                        case "AddBranchApplication":
                            var branch = DataService.GetEntity<Branch>(br => br.Id == branchId).Single();
                            return branch.IsFromLicense == null || !branch.IsFromLicense.Value;
                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Це МПД не може бути редаговане!");
        }

        public bool? IsEditableContractor(Guid? contractorId, bool raiseError = false)
        {
            if (contractorId == null || contractorId == Guid.Empty)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var branchId = DataService
                    .GetEntity<PrlBranchContractor>(contractor => contractor.ContractorId == contractorId).First()
                    .BranchId;
                var appId = DataService.GetEntity<ApplicationBranch>(apBrn => apBrn.BranchId == branchId).Single()
                    .LimsDocumentId;
                var appStates = GetAppStates(appId);

                if (appStates[nameof(BaseApplication.AppState)] == "Project" ||
                    appStates[nameof(BaseApplication.BackOfficeAppState)] == "Project")
                {
                    var appSort = appStates[nameof(BaseApplication.AppSort)];
                    switch (appSort)
                    {
                        case "GetLicenseApplication":
                        case "RenewLicenseApplication":
                        case "ChangeContrApplication":
                        case "IncreaseToPRLApplication":
                        case "AddBranchApplication":
                        case "AdditionalInfoToLicense":
                            return true;

                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Цей контрактний контрагент не може бути редагований!");
        }

        public bool? IsEditableAuth(Guid? assigneeId, bool raiseError = false)

        {
            if (assigneeId == null || assigneeId == Guid.Empty)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var branchId = DataService
                    .GetEntity<AppAssigneeBranch>(assigneeBranch => assigneeBranch.AssigneeId == assigneeId).First()
                    .BranchId;
                var appId = DataService.GetEntity<ApplicationBranch>(apBrn => apBrn.BranchId == branchId).Single()
                    .LimsDocumentId;
                var appStates = GetAppStates(appId);

                if (appStates[nameof(BaseApplication.AppState)] == "Project" ||
                    appStates[nameof(BaseApplication.BackOfficeAppState)] == "Project")
                {
                    var appSort = appStates[nameof(BaseApplication.AppSort)];
                    switch (appSort)
                    {
                        case "AdditionalInfoToLicense":
                        case "GetLicenseApplication":
                        case "RenewLicenseApplication":
                        case "IncreaseToIMLApplication":
                        case "IncreaseToTRLApplication":
                        case "IncreaseToPRLApplication":
                        case "AddBranchApplication":
                        case "ChangeAutPersonApplication":
                            return true;

                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Ця відповідальна одиниця не може бути редагована!");
        }

        public bool? IsEditableEdoc(Guid? edocId, bool raiseError = false)

        {
            if (edocId == null || edocId == Guid.Empty)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var branchId = DataService
                    .GetEntity<BranchEDocument>(x => x.EDocumentId == edocId).First()
                    .BranchId;
                var appId = DataService.GetEntity<ApplicationBranch>(apBrn => apBrn.BranchId == branchId).Single()
                    .LimsDocumentId;
                var appStates = GetAppStates(appId);

                if (appStates[nameof(BaseApplication.AppState)] == "Project" ||
                    appStates[nameof(BaseApplication.BackOfficeAppState)] == "Project")
                {
                    var appSort = appStates[nameof(BaseApplication.AppSort)];
                    switch (appSort)
                    {
                        case "GetLicenseApplication":
                        case "RenewLicenseApplication":
                        case "IncreaseToPRLApplication":
                        case "IncreaseToIMLApplication":
                        case "IncreaseToTRLApplication":
                        case "AddBranchInfoApplication":
                        case "AddBranchApplication":
                        case "AdditionalInfoToLicense":
                            return true;
                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Ця відповідальна одиниця не може бути редагована!");
        }

        public bool? IsEditableMedicine(Guid? medicineId, bool raiseError = false)

        {
            if (medicineId == null || medicineId == Guid.Empty)
            {
                return null;
            }

            var func = new Func<bool>(delegate
            {
                var medicine = DataService.GetEntity<ImlMedicine>(x => x.Id == medicineId).FirstOrDefault();
                var appStates = GetAppStates(medicine.ApplicationId);

                if (appStates[nameof(BaseApplication.AppState)] == "Project" ||
                    appStates[nameof(BaseApplication.BackOfficeAppState)] == "Project")
                {
                    var appSort = appStates[nameof(BaseApplication.AppSort)];
                    switch (appSort)
                    {
                        case "AdditionalInfoToLicense":
                        case "GetLicenseApplication":
                        case "RenewLicenseApplication":
                        case "IncreaseToIMLApplication":
                        case "AddBranchApplication":
                            return true;

                        default:
                            return false;
                    }
                }

                return false;
            });

            if (func.Invoke())
            {
                return true;
            }

            if (!raiseError)
            {
                return false;
            }

            throw BusinessRulesException.CreateNew("Ця відповідальна одиниця не може бути редагована!");
        }

        public Dictionary<string, string> GetAppStates(Guid? appId)
        {
            Dictionary<string, string> states = null;

            if (appId == null || appId == Guid.Empty)
            {
                return null;
            }

            var appEntity = DataService.GetDto<AppStateDTO>(app => app.Id == appId).Single();
            states = new Dictionary<string, string>
            {
                {nameof(appEntity.BackOfficeAppState), appEntity.BackOfficeAppState},
                {nameof(appEntity.AppState), appEntity.AppState},
                {nameof(appEntity.AppSort), appEntity.AppSort},
                {nameof(appEntity.PerformerOfExpertise), appEntity.PerformerOfExpertise }
            };

            //var cachedValue = CacheService.GetValue<Dictionary<string, string>>($"AppStateById:{appId}");

            //if (cachedValue == null)
            //{
            //    T appEntity;
            //    try
            //    {
            //        appEntity = DataService.GetEntity<T>(app => app.Id == appId).Single();
            //    }
            //    catch (InvalidOperationException e)
            //    {
            //        throw new Exception("Виникла помилка при зчитуванні данних. Невірний Id", e);
            //    }

            //    catch (Exception e)
            //    {
            //        throw new Exception("Виникла помилка при спробі зчитати дані.", e);
            //    }

            //    states = new Dictionary<string, string>
            //    {
            //        {nameof(appEntity.BackOfficeAppState), appEntity.BackOfficeAppState},
            //        {nameof(appEntity.AppState), appEntity.AppState},
            //        {nameof(appEntity.AppSort), appEntity.AppSort}
            //    };

            //    CacheService.SetValueAsync($"AppStateById:{appId}", states, TimeSpan.FromMinutes(30));
            //}
            //else
            //{
            //    states = cachedValue;
            //}

            return states;
        }

        public void CheckChangeAppStatus(Guid? appId, string targetState)
        {
            var states = GetAppStates(appId);
            if (states == null)
            {
                throw new ArgumentNullException(nameof(appId), "Невірний id заяви");
            }

            if (string.IsNullOrWhiteSpace(targetState))
            {
                throw new ArgumentNullException(nameof(appId), "Невірна дія");
            }

            var backOfficeAppState = states[nameof(BaseApplication.BackOfficeAppState)];

            if (!IsChangeStatusAllowed(
                backOfficeAppState,
                states[nameof(BaseApplication.AppState)],
                targetState))
            {
                throw BusinessRulesException.CreateUpdateViolationException("Заява", $"{backOfficeAppState}",
                    targetState);
            }
        }

        private bool IsChangeStatusAllowed(string BackOfficeAppState, string AppState, string targetStatus)
        {
            if ((BackOfficeAppState == "Project" || AppState == "Project") &&
                targetStatus == "Submitted")
            {
                return true;
            }

            if (BackOfficeAppState == "Submitted" && AppState == "Submitted" &&
                (targetStatus == "InReview" || targetStatus == "Registered"))
            {
                return true;
            }

            if (BackOfficeAppState == "Registered" && AppState == "InReview" &&
                targetStatus == "InReview")
            {
                return true;
            }

            if (BackOfficeAppState == "InReview" && AppState == "InReview" &&
                targetStatus == "Reviewed")
            {
                return true;
            }

            return false;
        }

        public AccessStruct ApplicationAddability(Guid appId)
        {
            var addableStruct = new AccessStruct();
            var appStates = GetAppStates(appId);

            var backOfficeAppState = appStates["BackOfficeAppState"];
            var appState = appStates["AppState"];
            if (string.IsNullOrWhiteSpace(backOfficeAppState) && appState == "Project" ||
                backOfficeAppState == "Project" && string.IsNullOrWhiteSpace(appState))
            {
                var appSort = appStates["AppSort"];
                switch (appSort)
                {
                    case "CancelLicenseApplication":
                    case "RemBranchInfoApplication":
                    case "RemBranchApplication":
                    case "DecreaseActTypeApplication":
                        break;

                    case "AddBranchInfoApplication":
                        addableStruct.IsEdocument = true;
                        break;

                    case "ChangeContrApplication":
                        addableStruct.IsContractor = true;
                        break;
                    case "ChangeAutPersonApplication":
                        addableStruct.IsAssignee = true;
                        break;
                    case "AdditionalInfoToLicense":
                        addableStruct.IsAssignee = true;
                        addableStruct.IsContractor = true;
                        addableStruct.IsEdocument = true;
                        addableStruct.IsMedicine = true;
                        addableStruct.IsBranch = true;
                        break;

                    case "AddBranchApplication":
                    case "RenewLicenseApplication":
                    case "IncreaseToTRLApplication":
                    case "IncreaseToIMLApplication":
                    case "IncreaseToPRLApplication":
                    case "GetLicenseApplication":
                        addableStruct.IsAssignee = true;
                        addableStruct.IsBranch = true;
                        addableStruct.IsContractor = true;
                        addableStruct.IsEdocument = true;
                        addableStruct.IsMedicine = true;
                        break;

                    case "ChangeDrugList":
                    case "ReplacementDrugList":
                        addableStruct.IsMedicine = true;
                        break;
                }
            }

            return addableStruct;
        }
    }

    public struct AccessStruct
    {
        public bool IsBranch;
        public bool IsAssignee;
        public bool IsContractor;
        public bool IsEdocument;
        public bool IsMedicine;
    }
}
