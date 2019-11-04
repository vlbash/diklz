using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Services.ImlServices;
using App.Business.Services.PrlServices;
using App.Business.Services.TrlServices;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Data.Contexts;
using App.Data.DTO.APP;
using App.Data.DTO.ATU;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.DTO.IML;
using App.Data.DTO.LIMS;
using App.Data.DTO.ORG;
using App.Data.DTO.P902;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.CRV;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using App.Data.Models.P902;
using App.Data.Models.PRL;
using App.Data.Models.TRL;
using App.Data.Repositories;
using App.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using FileStoreDTO = App.Core.Data.DTO.Common.FileStoreDTO;

namespace App.Business.Services.LimsService
{
    public class LimsExchangeService
    {
        public readonly ICommonDataService DataService;
        private readonly MigrationDbContext _dbContext;
        private readonly LimsRepository _limsRepository;
        private readonly IUserInfoService _userInfoService;
        private readonly IObjectMapper _objectMapper;
        private readonly IPrlLicenseService _prlLicenseService;
        private readonly IImlLicenseService _imlLicenseService;
        private readonly ITrlLicenseService _trlLicenseService;

        public LimsExchangeService(LimsRepository limsRepository, ICommonDataService dataService,
            IUserInfoService userInfoService, IObjectMapper objectMapper, MigrationDbContext dbContext, IPrlLicenseService prlLicenseService,
            IImlLicenseService imlLicenseService, ITrlLicenseService trlLicenseService)
        {
            _limsRepository = limsRepository;
            DataService = dataService;
            _userInfoService = userInfoService;
            _objectMapper = objectMapper;
            _dbContext = dbContext;
            _prlLicenseService = prlLicenseService;
            _imlLicenseService = imlLicenseService;
            _trlLicenseService = trlLicenseService;
        }

        public void InsertApplication(PrlApplication prlApplication)
        {
            if (prlApplication.AppSort == "GetLicenseApplication" || prlApplication.AppSort == "IncreaseToPRLApplication")
                InsertGetLicenseApplication(prlApplication.Id);

            if (prlApplication.AppSort == "CancelLicenseApplication" || prlApplication.AppSort == "DecreasePRLApplication")
                InsertLicenseCancelApplicationAsync(prlApplication);

            if (prlApplication.AppSort == "AddBranchApplication")
                InsertAddBranchApplicationAsync(prlApplication);

            if (prlApplication.AppSort == "RemBranchApplication")
                InsertRemBranchApplication(prlApplication);

            if (prlApplication.AppSort == "ChangeAutPersonApplication")
                InsertChangeAutPersonApplication(prlApplication);

            if (prlApplication.AppSort == "AddBranchInfoApplication" || prlApplication.AppSort == "RemBranchInfoApplication")
                InsertGetLicenseApplicationReason(prlApplication.Id);

            if (prlApplication.AppSort == "ChangeContrApplication")
                InsertGetLicenseApplicationReason(prlApplication.Id);
        }

        public void InsertApplication(ImlApplication imlApplication)
        {
            if (imlApplication.AppSort == "GetLicenseApplication" || imlApplication.AppSort == "IncreaseToIMLApplication")
                InsertGetLicenseApplicationIML(imlApplication.Id);

            if (imlApplication.AppSort == "CancelLicenseApplication" || imlApplication.AppSort == "DecreaseIMLApplication")
                InsertLicenseCancelApplicationAsyncIML(imlApplication);

            if (imlApplication.AppSort == "AddBranchApplication")
                InsertAddBranchApplicationAsyncIML(imlApplication);

            if (imlApplication.AppSort == "RemBranchApplication")
                InsertRemBranchApplicationIML(imlApplication);

            if (imlApplication.AppSort == "ChangeAutPersonApplication")
                InsertChangeAutPersonApplicationIML(imlApplication);

            if (imlApplication.AppSort == "ChangeDrugList" || imlApplication.AppSort == "ReplacementDrugList")
                InsertAppDrug(imlApplication);

            //if (imlApplication.AppSort == "ReplacementDrugList")
            //    ReplacementDrugList(imlApplication);
        }

        public void InsertApplication(TrlApplication trlApplication)
        {
            if (trlApplication.AppSort == "GetLicenseApplication" || trlApplication.AppSort == "IncreaseToTRLApplication")
                InsertGetLicenseApplicationTRL(trlApplication.Id);

            if (trlApplication.AppSort == "CancelLicenseApplication" || trlApplication.AppSort == "DecreaseTRLApplication")
                InsertLicenseCancelApplicationAsyncTRL(trlApplication);

            if (trlApplication.AppSort == "AddBranchApplication")
                InsertAddBranchApplicationAsyncTRL(trlApplication);

            if (trlApplication.AppSort == "RemBranchApplication")
                InsertRemBranchApplicationTRL(trlApplication);

            //if (imlApplication.AppSort == "ChangeAutPersonApplication")
            //    InsertChangeAutPersonApplicationIML(imlApplication);

            //if (imlApplication.AppSort == "ChangeDrugList" || imlApplication.AppSort == "ReplacementDrugList")
            //    InsertAppDrug(imlApplication);

            //if (imlApplication.AppSort == "ReplacementDrugList")
            //    ReplacementDrugList(imlApplication);
        }

        //public void InsertApplication(ResultInputControl resultInputControl)
        //{
        //    //if resultInputControl == 
        //}

        #region P902

        public async Task InsertSgdRepDrugList(ResultInputControl resultInputControl)
        {
            var resultDto =
                (await DataService.GetDtoAsync<ResultInputControlDetailsDTO>(x => x.Id == resultInputControl.Id))
                .FirstOrDefault();
            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_RepDrugId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_StatusId", SqlDbType.Int){Value = Convert.ToInt32(resultDto.StateLimsId)},
                new SqlParameter("@p_OwnerOrgId", SqlDbType.Int){Value = Convert.ToInt32(resultDto.TeritorialService)}, 
                new SqlParameter("@p_LicenseId", SqlDbType.Int){Value = CheckForDbNull(resultDto.OldLimsId)}, 
                new SqlParameter("@p_RpNumber", SqlDbType.VarChar){Value = CheckForDbNull(resultInputControl.RegisterNumber)},
                new SqlParameter("@p_RpTermDate", SqlDbType.Date) {Value = CheckForDbNull(resultInputControl.EndDate)},
                new SqlParameter("@p_DrugName", SqlDbType.VarChar){Value = CheckForDbNull(resultInputControl.DrugName)}, 
                new SqlParameter("@p_FormTypeDesc", SqlDbType.VarChar) {Value = CheckForDbNull(resultInputControl.DrugForm)}, 
                new SqlParameter("@p_ProducerName", SqlDbType.VarChar){Value = CheckForDbNull(resultInputControl.ProducerName)}, 
                new SqlParameter("@p_CountryName", SqlDbType.VarChar) {Value = CheckForDbNull(resultInputControl.ProducerCountry)}, 
                new SqlParameter("@p_RpId", SqlDbType.Int) 
                {
                    Value = CheckForDbNull(resultDto.DocId)
                }, 
                new SqlParameter("@p_SerialNum", SqlDbType.VarChar) {Value =CheckForDbNull(resultInputControl.MedicineSeries)},
                new SqlParameter("@p_TermDate", SqlDbType.Date) {Value = CheckForDbNull(resultInputControl.MedicineExpirationDate)},
                new SqlParameter("@p_SerialCount", SqlDbType.Int) {Value = CheckForDbNull(resultInputControl.SizeOfSeries)},
                new SqlParameter("@p_DrugUnitId", SqlDbType.VarChar){Value = Convert.ToInt32(resultDto.UnitOfMeasurementLimsId)}, 
                new SqlParameter("@p_ImportCount", SqlDbType.Int) {Value = CheckForDbNull(resultInputControl.AmountOfImportedMedicine)},
                new SqlParameter("@p_CustomsNum", SqlDbType.VarChar) {Value =CheckForDbNull(resultInputControl.WinNumber)}, 
                new SqlParameter("@p_CustomsDate", SqlDbType.Date) {Value = CheckForDbNull(resultInputControl.DateWin)},
                new SqlParameter("@p_ExamResultId", SqlDbType.Int) {Value = Convert.ToInt32(resultDto.InputControlResultLimsId)},
                new SqlParameter("@p_FactorInfo", SqlDbType.VarChar) {Value = CheckForDbNull(resultInputControl.NameOfMismatch)},
                new SqlParameter("@p_Comment", SqlDbType.VarChar){Value = CheckForDbNull(resultInputControl.Comment)}
            };

            resultInputControl.OldLimsId = await _limsRepository.InsertSgdRepDrugListP902(appParams);
            DataService.SaveChanges();
        }

        public async Task UpdateRepDrugStatus()
        {
            var drugSend = DataService.GetEntity<ResultInputControl>(x => x.SendCheck == true && x.State == "Project" /*&& x.OldLimsId != 0*/).ToList();
            foreach (var item in drugSend)
            {
                var appParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_RepDrugId", SqlDbType.Int) { Value = item.OldLimsId},
                    new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 2} //відправлено
                };
                await _limsRepository.UpdateRepDrugStatusP902(appParams);
            }
        }

        #endregion
        public static object CheckForDbNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }

        public async void InsertAppDrug(ImlApplication imlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var imlMedicineList =
                DataService.GetEntity<ImlMedicine>(x => x.ApplicationId == imlApplication.Id && x.IsFromLicense == false).ToList();
            var license = DataService.GetEntity<ImlLicense>(prlLicense => prlLicense.Id == imlApplication.ParentId)
                .FirstOrDefault();
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == imlApplication.Id)
                .Single();
            var applicationBranches =
                DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == imlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            //var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == imlApplication.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value =Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date)
                {
                    Value = appDetailDto.RegDate ?? DateTime.Now
                }, //regdate 
                new SqlParameter("@p_PersonId", SqlDbType.Int)
                {
                    Value = performerId ?? 461 //TODO implement later
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                //new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                //{
                //    Value = DBNull.Value, IsNullable = true
                //}, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = imlApplication.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = imlApplication.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(imlApplication.ImlAnotherActivity)},

                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            var appEntity = DataService.GetEntity<ImlApplication>(app => app.Id == imlApplication.Id).Single();

            appEntity.OldLimsId = await _limsRepository.InsertImlApplication(appParams);
            DataService.SaveChanges();
            //var counter = 1;
            //foreach (var imlMedicine in imlMedicineList)
            //{

            //    var appParamsList = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_DrugId", SqlDbType.Int) {Direction = ParameterDirection.Output},
            //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = CheckForDbNull(Convert.ToInt32(appEntity.OldLimsId))},
            //        new SqlParameter("@p_PosNumber", SqlDbType.Int) {Value = counter++},
            //        new SqlParameter("@p_RpNumber", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.RegisterNumber)},
            //        new SqlParameter("@p_DrugName", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.MedicineName)},
            //        new SqlParameter("@p_FormtypeDesc", SqlDbType.VarChar){Value = CheckForDbNull(imlMedicine.FormName)},
            //        new SqlParameter("@p_ActiveDose", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.DoseInUnit)},
            //        new SqlParameter("@p_CountInPack", SqlDbType.VarChar) {Value = DBNull.Value},
            //        new SqlParameter("@p_DrugMnn", SqlDbType.VarChar) {Value = imlMedicine.MedicineNameEng},
            //        new SqlParameter("@p_AtcCode", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.AtcCode)},
            //        new SqlParameter("@p_ProducerName", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.ProducerName)
            //        },
            //        new SqlParameter("@p_ProducerCountry", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.ProducerCountry) //, IsNullable = true
            //        },
            //        new SqlParameter("@p_SupplierName", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.SupplierName)
            //        },
            //        new SqlParameter("@p_SupplierCountry", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.SupplierCountry)
            //        },
            //        new SqlParameter("@p_SupplierAddress", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.SupplierAddress)},
            //        new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.Notes)},
            //        new SqlParameter("@p_IsLicense", SqlDbType.Bit) {Value = 1},
            //        new SqlParameter("@p_IsProblem", SqlDbType.Bit) {Value = 0},
            //        new SqlParameter("@p_ProblemInfo", SqlDbType.VarChar) {Value = " "}
            //    };
            //    imlMedicine.OLdDRugId = await _limsRepository.InsertAppDrugIML(appParamsList);
            //}
            //DataService.SaveChanges();
        }

        private async void InsertChangeAutPersonApplication(PrlApplication prlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var license = DataService.GetEntity<PrlLicense>(prlLicense => prlLicense.Id == prlApplication.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == prlApplication.Id).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == prlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = prlApplication.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int){Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int){Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //dodelat
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            var appEntity = DataService.GetEntity<PrlApplication>(app => app.Id == prlApplication.Id).Single();

            appEntity.OldLimsId = await _limsRepository.InsertPrlApplication(appParams);


            var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchIds.Contains(x.BranchId)).ToList();
            var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId).Distinct().ToList();
            var assignees = DataService.GetEntity<AppAssignee>(x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);


            var personIds = "";

            foreach (var assignee in assignees)
            {
                string personPos =
                    string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                        ? "Не визначено"
                        : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                            + (!string.IsNullOrEmpty(assignee.WorkExperience) ? $" Cтаж роботи: {assignee.WorkExperience} місяців" : "");

                string education = $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                                   $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                               + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                               $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";



                var assigneeParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_PersonId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = license.OldLimsId},
                    new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                    new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250)) },
                    new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000)) },
                    new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000))}, //???
                    new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                    new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000)),IsNullable = true},
                    new SqlParameter("@p_Used", SqlDbType.Bit){Value = 1}
                };
                var getOldlLmsid = await _limsRepository.PrlUpdateAssignee(assigneeParamList);

                personIds += "," + getOldlLmsid;

                var assigneeParamListapplication = new List<SqlParameter>
                {
                    new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                    new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                    new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250))},
                    new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000))},
                    new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000))}, //???
                    new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                    new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000))}
                };

                _limsRepository.PrlUpdateAssigneePRL(assigneeParamListapplication);

            }

            if (!string.IsNullOrEmpty(personIds))
            {
                personIds = personIds.Substring(1);

                var assigneeParamListUpdate = new List<SqlParameter> //для оновлення поля діє в старому лімлі
                {
                    new SqlParameter("@p_PersonIds", SqlDbType.VarChar) {Value = personIds},
                    new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = license.OldLimsId}
                };
                _limsRepository.PrlUpdatePersonIds(assigneeParamListUpdate);
            }

            DataService.SaveChanges();
            await InsertAttach(prlApplication.Id, prlApplication.OldLimsId);
            DataService.SaveChanges();

        }

        private async void InsertChangeAutPersonApplicationIML(ImlApplication imlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var license = DataService.GetEntity<ImlLicense>(prlLicense => prlLicense.Id == imlApplication.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == imlApplication.Id).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == imlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == imlApplication.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_PersonId", SqlDbType.Int)
                {
                   Value = performerId ?? 461 
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                //new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                //{
                //    Value = DBNull.Value, IsNullable = true
                //}, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},

                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = imlApplication.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = imlApplication.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(imlApplication.ImlAnotherActivity)},

                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            var appEntity = DataService.GetEntity<ImlApplication>(app => app.Id == imlApplication.Id).Single();

            appEntity.OldLimsId = await _limsRepository.InsertImlApplication(appParams);
            var personIds = "";
            foreach (var branchEntityOldLimsId in branchEntityList)
            {
                var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchEntityOldLimsId.Id == x.BranchId)// .Contains(x.BranchId))
                    .ToList();
                var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId)
                    .Distinct().ToList();
                var assignees =
                    DataService.GetEntity<AppAssignee>(
                        x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);

                //branchAssignee

                foreach (var assignee in assignees)
                {
                    string personPos =
                        string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                            ? "Не визначено"
                            : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                              + (!string.IsNullOrEmpty(assignee.WorkExperience)
                                  ? $" Cтаж роботи: {assignee.WorkExperience} місяців"
                                  : "");

                    string education =
                        $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                        $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                    string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                                   + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                                   $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";

                    var assigneeParamListLic = new List<SqlParameter>
                    {
                        new SqlParameter("@p_PersonId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                        new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = branchEntityOldLimsId.OldLimsId},
                        new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                        new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250)) },
                        new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000)) },
                        new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000))}, //???
                        new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                        new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000)),IsNullable = true},
                        new SqlParameter("@p_Used", SqlDbType.Bit){Value = 1}
                    };
                    var getOldlLmsid = await _limsRepository.PrlUpdateAssigneeIMLLicense(assigneeParamListLic);

                    personIds += "," + getOldlLmsid;

                    var assigneeParamList = new List<SqlParameter>
                    {
                        new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                        new SqlParameter("@p_BranchAppId", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(branchEntityOldLimsId.OldLimsId)
                        },
                        new SqlParameter("@p_PersonName", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_PersonPos", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(personPos.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_Education", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(education.SubstringFromStart(1000))
                        },
                        new SqlParameter("@p_Phone", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Fax", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Experience", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_ContractInfo", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000)),
                            IsNullable = true
                        }, //???
                        new SqlParameter("@p_Notes", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(notes.SubstringFromStart(1000))
                        }
                    };

                    _limsRepository.PrlUpdateAssigneeIMLPerson(assigneeParamList);
                }
                if (!string.IsNullOrEmpty(personIds))
                {
                    personIds = personIds.Substring(1);

                    var assigneeParamListUpdate = new List<SqlParameter> //для оновлення поля діє в старому лімлі
                    {
                        new SqlParameter("@p_PersonIds", SqlDbType.VarChar) {Value = personIds},
                        new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = branchEntityOldLimsId.OldLimsId}
                    };
                    _limsRepository.PrlUpdatePersonIdsIMLLicense(assigneeParamListUpdate);
                }
            }


            DataService.SaveChanges();
            //await InsertAttach(imlApplication.Id, imlApplication.OldLimsId);
            DataService.SaveChanges();

        }

        private async void InsertLicenseCancelApplicationAsync(PrlApplication prlApplication)
        {
            var appId = prlApplication.Id;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var appEntity = DataService.GetEntity<PrlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<PrlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value}, //not used
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertPrlApplication(appParams);
            DataService.Add(appEntity, true);
            await InsertAttach(prlApplication.Id, prlApplication.OldLimsId);
            DataService.SaveChanges();
        }

        private async void InsertLicenseCancelApplicationAsyncIML(ImlApplication imlApplication)
        {
            var appId = imlApplication.Id;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var appEntity = DataService.GetEntity<ImlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<ImlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == appEntity.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_PersonId", SqlDbType.Int) 
                {
                   Value = performerId ?? 461 
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                //new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                //{
                //    Value = DBNull.Value, IsNullable = true
                //}, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},

                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = imlApplication.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = imlApplication.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(imlApplication.ImlAnotherActivity)},

                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertImlApplication(appParams);
            DataService.Add(appEntity, true);
            await InsertAttach(imlApplication.Id, imlApplication.OldLimsId);
            DataService.SaveChanges();
        }

        private async void InsertLicenseCancelApplicationAsyncTRL(TrlApplication trlApplication)
        {
            var appId = trlApplication.Id;
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var appEntity = DataService.GetEntity<TrlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<TrlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<TrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var entityEnRec = DataService.GetEntity<EntityEnumRecords>(x => x.EntityId == appEntity.Id).Select(x => x.EnumRecordCode).ToList();
            var enRec = DataService.GetEntity<EnumRecord>(x => entityEnRec.Contains(x.Code));

            string p_LictypeIds = "";
            foreach (var ids in enRec)
            {
                p_LictypeIds += "," + ids.ExParam1;
            }
            if (!string.IsNullOrEmpty(p_LictypeIds))
            {
                p_LictypeIds = p_LictypeIds.Substring(1);
            }

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_LicAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppStatusId", SqlDbType.Int){Value =  DBNull.Value},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_LictypeIds", SqlDbType.VarChar)
                {
                    Value = CheckForDbNull(p_LictypeIds)
                },
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    //Value = DBNull.Value
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Passport", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertTrlApplication(appParams);
            DataService.Add(appEntity, true);
            //await InsertAttach(trlApplication.Id, trlApplication.OldLimsId);
            DataService.SaveChanges();
        }

        private async void InsertAddBranchApplicationAsync(PrlApplication prlApplication)
        {
            var appId = prlApplication.Id;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var appEntity = DataService.GetEntity<PrlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<PrlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },

                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value}, //not used
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertPrlApplication(appParams);
            int counter = 1;
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.IsFromLicense == false && branch.RecordState != RecordState.D);
            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .Single();

                int residenceTypeId;

                #region residence type

                //Lims [CDC_RESIDENCE_TYPE]
                //  RESIDENCE_TYPE_ID     RESIDENCE_TYPE_NAME
                //  1                     Місто
                //  2                     Село
                //  3                     Не визначено
                //  4                     Селище міського типу
                switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion

                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_IsAddressMatches", SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(0)
                    }, //default value 0
                    new SqlParameter("@p_RegionId", SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(1)
                    }, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int)
                    {
                        Value = residenceTypeId
                    }, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar)
                    {
                        Value = branchAddressDto.Address
                    }, //parse branch address
                    new SqlParameter("@p_OperationForm", SqlDbType.VarChar) {Value = string.Empty}, //default '-' value
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar)
                    {
                        Value = branchAddressDto.PostIndex
                    }, //branch post index
                    new SqlParameter("@p_Phone", SqlDbType.VarChar)
                    {
                        Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)
                    }, //branch phone
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = String.Empty},
                    new SqlParameter("@p_IsSaveConditions", SqlDbType.Int) {Value = Convert.ToInt32(0)} //default
                };

                branchEntity.OldLimsId = await _limsRepository.InsertPrlBranch(branchParamList);
            }
            DataService.Add(appEntity, true);
            //await InsertAttachMPD(prlApplication.Id, prlApplication.OldLimsId);
            DataService.SaveChanges();
            var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchIds.Contains(x.BranchId)).ToList();
            var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId).Distinct().ToList();
            var assignees = DataService.GetEntity<AppAssignee>(x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);


            var personIds = "";

            foreach (var assignee in assignees)
            {
                string personPos =
                    string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                        ? "Не визначено"
                        : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                            + (!string.IsNullOrEmpty(assignee.WorkExperience) ? $" Cтаж роботи: {assignee.WorkExperience} місяців" : "");

                string education = $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                                   $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                               + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                               $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";



                var assigneeParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_PersonId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = license.OldLimsId},
                    new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                    new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250)) },
                    new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000)) },
                    new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000))}, //???
                    new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                    new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000)),IsNullable = true},
                    new SqlParameter("@p_Used", SqlDbType.Bit){Value = 1}
                };
                var getOldlLmsid = await _limsRepository.PrlUpdateAssignee(assigneeParamList);

                personIds += "," + getOldlLmsid;

                var assigneeParamListapplication = new List<SqlParameter>
                {
                    new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                    new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                    new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250))},
                    new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000))},
                    new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000))}, //???
                    new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                    new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000))}
                };

                _limsRepository.PrlUpdateAssigneePRL(assigneeParamListapplication);

            }

            if (!string.IsNullOrEmpty(personIds))
            {
                personIds = personIds.Substring(1);

                var assigneeParamListUpdate = new List<SqlParameter> //для оновлення поля діє в старому лімлі
                {
                    new SqlParameter("@p_PersonIds", SqlDbType.VarChar) {Value = personIds},
                    new SqlParameter("@p_LicenseId", SqlDbType.Int) {Value = license.OldLimsId}
                };
                _limsRepository.PrlUpdatePersonIds(assigneeParamListUpdate);
            }

            DataService.SaveChanges();
            //await InsertAttach(prlApplication.Id, prlApplication.OldLimsId);
            //DataService.SaveChanges();
        }

        private async void InsertAddBranchApplicationAsyncIML(ImlApplication imlApplication)
        {
            var appId = imlApplication.Id;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var appEntity = DataService.GetEntity<ImlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<ImlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == appEntity.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_PersonId", SqlDbType.Int)
                {
                   Value = performerId?? 461 
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                //new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                //{
                //    Value = DBNull.Value, IsNullable = true
                //}, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},

                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = imlApplication.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = imlApplication.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(imlApplication.ImlAnotherActivity)},

                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertImlApplication(appParams);

            //var branchEntityOldLimsId = "";
            int counter = 1;
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.IsFromLicense == false && branch.RecordState != RecordState.D);
            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .Single();

                int residenceTypeId;

                #region residence type

                switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion
                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_IsAddressMatches", SqlDbType.Int){Value = Convert.ToInt32(0)}, //default value 0
                    new SqlParameter("@p_RegionId", SqlDbType.Int){Value = Convert.ToInt32(1)}, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int){Value = residenceTypeId}, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar){Value = branchAddressDto.Address}, //parse branch address
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar){Value = branchAddressDto.PostIndex},
                    new SqlParameter("@p_Phone", SqlDbType.VarChar){Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)},
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = String.Empty},
                    new SqlParameter("@p_ToCheck", SqlDbType.Bit) {Value = DBNull.Value}
                };

                //branchEntity.OldLimsId = await _limsRepository.InsertImlBranch(branchParamList);
                branchEntity.OldLimsId = await _limsRepository.InsertImlBranch(branchParamList);
                //branchEntityOldLimsId = branchEntity.OldLimsId.ToString();
            }
            DataService.Add(appEntity, true);
            //await InsertAttach(imlApplication.Id, appEntity.OldLimsId);
            DataService.SaveChanges();



            foreach (var branchEntityOldLimsId in branchEntityList)
            {
                var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchEntityOldLimsId.Id == x.BranchId)   // .Contains(x.BranchId))
                    .ToList();
                var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId)
                    .Distinct().ToList();
                var assignees =
                    DataService.GetEntity<AppAssignee>(
                        x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);

                //branchAssignee

                foreach (var assignee in assignees)
                {
                    string personPos =
                        string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                            ? "Не визначено"
                            : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                              + (!string.IsNullOrEmpty(assignee.WorkExperience)
                                  ? $" Cтаж роботи: {assignee.WorkExperience} місяців"
                                  : "");

                    string education =
                        $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                        $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                    string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                                   + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                                   $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";


                    var assigneeParamList = new List<SqlParameter>
                    {
                        new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                        new SqlParameter("@p_BranchAppId", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(branchEntityOldLimsId.OldLimsId)
                        },
                        new SqlParameter("@p_PersonName", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_PersonPos", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(personPos.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_Education", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(education.SubstringFromStart(1000))
                        },
                        new SqlParameter("@p_Phone", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Fax", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Experience", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_ContractInfo", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000)),
                            IsNullable = true
                        }, //???
                        new SqlParameter("@p_Notes", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(notes.SubstringFromStart(1000))
                        }
                    };

                    _limsRepository.PrlUpdateAssigneeIML(assigneeParamList);
                }
            }

            DataService.SaveChanges();

        }

        private async void InsertAddBranchApplicationAsyncTRL(TrlApplication trlApplication)
        {
            var appId = trlApplication.Id;

            var userInfo = _userInfoService.GetCurrentUserInfo();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var appEntity = DataService.GetEntity<TrlApplication>(application => application.Id == appId).FirstOrDefault();
            var license = DataService.GetEntity<TrlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<TrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var entityEnRec = DataService.GetEntity<EntityEnumRecords>(x => x.EntityId == appEntity.Id).Select(x => x.EnumRecordCode).ToList();
            var enRec = DataService.GetEntity<EnumRecord>(x => entityEnRec.Contains(x.Code));

            //var branchType = DataService.GetDto<EntityEnumDTO>(x =>
            //    branchIds.Contains(x.BranchId) && x.EntityType == "Branch").Single();
            //var branchTypeIds = DataService.GetDto<EntityEnumDTO>(x =>
            //    branchIds.Contains(x.BranchId) && x.EntityType == "BranchApplication").ToList();

            string p_LictypeIds = "";
            foreach (var ids in enRec)
            {
                p_LictypeIds += "," + ids.ExParam1;
            }
            if (!string.IsNullOrEmpty(p_LictypeIds))
            {
                p_LictypeIds = p_LictypeIds.Substring(1);
            }

            //string p_BranchTypeIds = "";
            //foreach (var ids in branchTypeIds)
            //{
            //    p_BranchTypeIds += "," + ids.ExParam1;
            //}
            //if (!string.IsNullOrEmpty(p_BranchTypeIds))
            //{
            //    p_BranchTypeIds = p_BranchTypeIds.Substring(1);
            //}

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_LicAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppStatusId", SqlDbType.Int){Value =  DBNull.Value},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int){Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))},
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_AppReasonId", SqlDbType.Int){Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value},

                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar){Value = appDetailDto.RegNumber},
                new SqlParameter("@p_RegAppDate", SqlDbType.Date){Value = appDetailDto.RegDate ?? DateTime.Now},
                //new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity ?? 461}, //?? dobavit
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value/*branchIds.Count*/, IsNullable = true}, //?? x3
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit) {Value = DBNull.Value},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId},
                new SqlParameter("@p_LictypeIds", SqlDbType.VarChar)
                {
                    Value = CheckForDbNull(p_LictypeIds)
                },
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    //Value =  DBNull.Value
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},

                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = appDetailDto.OrgDirector},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },

                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Passport", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) { Value = Convert.ToInt32(appDetailDto.LegalFormType) },
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(3)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            appEntity.OldLimsId = await _limsRepository.InsertTrlApplication(appParams);

            int counter = 1;
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.IsFromLicense == false && branch.RecordState != RecordState.D);
            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .FirstOrDefault();

                var acepticCondition = DataService.GetEntity<EnumRecord>(x => x.Code == branchEntity.AsepticConditions)
                    .Select(x => x.ExParam1).SingleOrDefault();

                var branchType = DataService.GetEntity<EnumRecord>(x => x.Code == branchEntity.BranchType
                                                                        && x.EnumType == "BranchType").SingleOrDefault();

                var branchTypeIdsBranch = DataService.GetDto<EntityEnumDTO>(x => x.BranchId == branchEntity.Id && x.EntityType == "BranchApplication")
                    .Select(x => x.ExParam1).Distinct().ToList(); //перелік видів робіт в МПД

                string p_branchTypeIdsBranch = "";
                foreach (var ids in branchTypeIdsBranch)
                {
                    p_branchTypeIdsBranch += "," + ids;
                }
                if (!string.IsNullOrEmpty(p_branchTypeIdsBranch))
                {
                    p_branchTypeIdsBranch = p_branchTypeIdsBranch.Substring(1);
                }

                int residenceTypeId;

                var coatuCode = branchAddressDto.Code.ToString().Substring(0, 5);
                var paramListCoutu = new List<SqlParameter>
                {
                    new SqlParameter("@p_RegionId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_RegionCode", SqlDbType.Int) {Value = coatuCode}
                };

                var pRegionId = await _limsRepository.GetId(paramListCoutu);


                #region residence type

                switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion

                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchTypeId", SqlDbType.Int) {Value = branchType.ExParam1},
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_RegionId", SqlDbType.Int){Value = Convert.ToInt32(pRegionId)}, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int){Value = residenceTypeId}, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar){Value = branchAddressDto.Address}, //parse branch address
                    new SqlParameter("@p_BranchTypeIds", SqlDbType.VarChar) {Value = CheckForDbNull(p_branchTypeIdsBranch)},
                    new SqlParameter("@p_AseptId", SqlDbType.Int) {Value = CheckForDbNull(acepticCondition)},
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar){Value = branchAddressDto.PostIndex},
                    new SqlParameter("@p_Phone", SqlDbType.VarChar){Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)},
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = CheckForDbNull(branchEntity.SpecialConditions)},
                    new SqlParameter("@p_Remarks", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_ToCheck", SqlDbType.Bit) {Value = DBNull.Value}
                };

                branchEntity.OldLimsId = await _limsRepository.InsertTrlBranch(branchParamList);

                //if (branchEntity.CreateTds)
                //{
                //    var paramLis = new List<SqlParameter>
                //    {
                //        new SqlParameter("@p_CheckId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                //        new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)},
                //        new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
                //        new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = " "},
                //        new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
                //    };
                //    await _limsRepository.TrlCheckCreate(paramLis);
                //}
            }

            //var branchEntityListDls = branchEntityList.Where(x => x.CreateDls == true).ToList();
            //string branchEntityListDlsToUpdate = "";
            //foreach (var ids in branchEntityListDls)
            //{
            //    branchEntityListDlsToUpdate += "," + ids.OldLimsId;
            //}
            //if (!string.IsNullOrEmpty(branchEntityListDlsToUpdate))
            //{
            //    branchEntityListDlsToUpdate = branchEntityListDlsToUpdate.Substring(1);
            //}

            //if (branchEntityListDls.Any())
            //{
            //    var paramLis = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
            //        new SqlParameter("@p_CheckId", SqlDbType.Int) {Value = 0},
            //        new SqlParameter("@p_CheckDivIds", SqlDbType.VarChar) {Value = branchEntityListDlsToUpdate},
            //        new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
            //        new SqlParameter("@p_TermDate", SqlDbType.DateTime) {Value = DateTime.Now.AddDays(6)},
            //        new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = ""},
            //        new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            //    };
            //    await _limsRepository.TrlCheckDivAdd(paramLis);
            //}
            //await InsertAttach(appId, limsAppId);
            //DataService.SaveChanges();
            DataService.Add(appEntity, true);
            //await InsertAttach(imlApplication.Id, appEntity.OldLimsId);
            DataService.SaveChanges();

        }

        private async void InsertRemBranchApplication(PrlApplication prlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var license = DataService.GetEntity<PrlLicense>(prlLicense => prlLicense.Id == prlApplication.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == prlApplication.Id).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == prlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = prlApplication.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int){Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int){Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            prlApplication.OldLimsId = await _limsRepository.InsertPrlApplication(appParams);

            var branchEntityList = DataService.GetEntity<Branch>(branch =>
                branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true && branch.RecordState != RecordState.D);

            foreach (var branch in branchEntityList)
            {
                var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                var branchParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_AppId", SqlDbType.Int){Value = prlApplication.OldLimsId},
                    new SqlParameter("@p_LicDocId", SqlDbType.Int){Value = licBranch.OldLimsId}
                };
                branch.OldLimsId = licBranch.OldLimsId;
                _limsRepository.PrlAppBranchAdd(branchParams);
            }
            await InsertAttach(prlApplication.Id, prlApplication.OldLimsId);
            DataService.SaveChanges();
        }

        private async void InsertRemBranchApplicationIML(ImlApplication imlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var license = DataService.GetEntity<ImlLicense>(prlLicense => prlLicense.Id == imlApplication.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == imlApplication.Id).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == imlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == imlApplication.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date) {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_PersonId", SqlDbType.Int)
                {
                   Value = performerId ?? 461 //TODO implement later
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                //new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                //{
                //    Value = DBNull.Value, IsNullable = true
                //}, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value

                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = DBNull.Value
                    //Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},

                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = imlApplication.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = imlApplication.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) },
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(imlApplication.ImlAnotherActivity)},

                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.OwnershipType)},
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int) {Value = Convert.ToInt32(1208)}, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            imlApplication.OldLimsId = await _limsRepository.InsertImlApplication(appParams);

            //var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true && branch.RecordState != RecordState.D);
            ////var branchEntityListTest = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.IsFromLicense == false && branch.RecordState != RecordState.D);
            //foreach (var branch in branchEntityList)
            //{
            //    //var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
            //    var branchParams = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_ID", SqlDbType.Int){ Value = Convert.ToInt32(branch.OldLimsId)},
            //        new SqlParameter("@err_code", SqlDbType.Int){Direction = ParameterDirection.Output},
            //        new SqlParameter("@err_msg", SqlDbType.VarChar,400){Direction = ParameterDirection.Output}
            //        //new SqlParameter("@p_AppId", SqlDbType.Int){Value = imlApplication.OldLimsId},
            //        //new SqlParameter("@p_LicDocId", SqlDbType.Int){Value = licBranch.OldLimsId},
            //        //new SqlParameter("@p_BranchAppId", SqlDbType.Int){ Value = Convert.ToInt32(branch.OldLimsId)}
            //    };
            //    //branch.OldLimsId = licBranch.OldLimsId;            // ??
            //    _limsRepository.ImlAppBranchRem(branchParams);
            //}
            //await InsertAttach(imlApplication.Id, imlApplication.OldLimsId);
            var branchEntityList = DataService.GetEntity<Branch>(branch => 
                branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true && branch.RecordState != RecordState.D);

            foreach (var branch in branchEntityList)
            {
                var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                var branchParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_AppId", SqlDbType.Int){Value = imlApplication.OldLimsId},
                    new SqlParameter("@p_LicDocId", SqlDbType.Int){Value = licBranch.OldLimsId},
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int){Direction = ParameterDirection.Output}
                };
                branch.OldLimsId = licBranch.OldLimsId;
                _limsRepository.ImlAppBranchAdd(branchParams);
            }
            DataService.SaveChanges();
        }

        private async void InsertRemBranchApplicationTRL(TrlApplication trlApplication)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var license = DataService.GetEntity<TrlLicense>(prlLicense => prlLicense.Id == trlApplication.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<TrlAppDetailDTO>(application => application.Id == trlApplication.Id).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == trlApplication.Id);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = trlApplication.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var entityEnRec = DataService.GetEntity<EntityEnumRecords>(x => x.EntityId == trlApplication.Id).Select(x => x.EnumRecordCode).ToList();
            var enRec = DataService.GetEntity<EnumRecord>(x => entityEnRec.Contains(x.Code));

            string p_LictypeIds = "";
            foreach (var ids in enRec)
            {
                p_LictypeIds += "," + ids.ExParam1;
            }
            if (!string.IsNullOrEmpty(p_LictypeIds))
            {
                p_LictypeIds = p_LictypeIds.Substring(1);
            }

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_LicAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppStatusId", SqlDbType.Int){Value =  DBNull.Value},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int){Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))},
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_AppReasonId", SqlDbType.Int){Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value},

                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar){Value = appDetailDto.RegNumber},
                new SqlParameter("@p_RegAppDate", SqlDbType.Date){Value = appDetailDto.RegDate ?? DateTime.Now},
                //new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity ?? 461}, //?? dobavit
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value/*branchIds.Count*/, IsNullable = true}, //?? x3
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit) {Value = DBNull.Value},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId},
                new SqlParameter("@p_LictypeIds", SqlDbType.VarChar)
                {
                    Value = CheckForDbNull(p_LictypeIds)
                },
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    //Value =  DBNull.Value
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},

                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = appDetailDto.OrgDirector},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },

                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Passport", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) { Value = Convert.ToInt32(appDetailDto.LegalFormType) },
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(3)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            trlApplication.OldLimsId = await _limsRepository.InsertTrlApplication(appParams);

            //var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true && branch.RecordState != RecordState.D);
            //foreach (var branch in branchEntityList)
            //{
            //    //var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
            //    var branchParams = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_ID", SqlDbType.Int){ Value = Convert.ToInt32(branch.OldLimsId)},
            //        new SqlParameter("@err_code", SqlDbType.Int){Direction = ParameterDirection.Output},
            //        new SqlParameter("@err_msg", SqlDbType.VarChar,400){Direction = ParameterDirection.Output}
            //    };
            //    _limsRepository.ImlAppBranchRem(branchParams);
            //}
            //await InsertAttach(trlApplication.Id, trlApplication.OldLimsId);
            var branchEntityList = DataService.GetEntity<Branch>(branch =>
                branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true && branch.RecordState != RecordState.D);
            foreach (var branch in branchEntityList)
            {
                var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                var branchParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_LicAppId", SqlDbType.Int){Value = trlApplication.OldLimsId},
                    new SqlParameter("@p_LicDocId", SqlDbType.Int){Value = licBranch.OldLimsId}
                };
                branch.OldLimsId = licBranch.OldLimsId;
                _limsRepository.TrlAppBranchAdd(branchParams);
            }
            DataService.SaveChanges();
        }

        public async void InsertGetLicenseApplication(Guid appId)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));
            var appEntity = DataService.GetEntity<PrlApplication>(app => app.Id == appId).Single();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value = appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date)  {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = branchIds.Count, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = DBNull.Value}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = appDetailDto.OrgDirector},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = appDetailDto.EMail, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = 193}, //not used
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            var insertAppTask = _limsRepository.InsertPrlApplication(appParams);
            var limsAppId = await insertAppTask;
            appEntity.OldLimsId = limsAppId;

            int counter = 1;

            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .Single();

                int residenceTypeId;

                #region residence type

                //Lims [CDC_RESIDENCE_TYPE]
                //  RESIDENCE_TYPE_ID     RESIDENCE_TYPE_NAME
                //  1                     Місто
                //  2                     Село
                //  3                     Не визначено
                //  4                     Селище міського типу
                switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion

                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(limsAppId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_IsAddressMatches", SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(0)
                    }, //default value 0
                    new SqlParameter("@p_RegionId", SqlDbType.Int)
                    {
                        Value = Convert.ToInt32(1)
                    }, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int)
                    {
                        Value = residenceTypeId
                    }, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar)
                    {
                        Value = branchAddressDto.Address
                    }, //parse branch address
                    new SqlParameter("@p_OperationForm", SqlDbType.VarChar) {Value = string.Empty}, //default '-' value
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar)
                    {
                        Value = branchAddressDto.PostIndex
                    }, //branch post index
                    new SqlParameter("@p_Phone", SqlDbType.VarChar)
                    {
                        Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)
                    }, //branch phone
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = String.Empty},
                    new SqlParameter("@p_IsSaveConditions", SqlDbType.Int) {Value = Convert.ToInt32(1)} //default
                };

                branchEntity.OldLimsId = await _limsRepository.InsertPrlBranch(branchParamList);
            }

            //try
            //{
                await InsertAttach(appId, limsAppId);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            DataService.SaveChanges();
            //var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchIds.Contains(x.BranchId)).ToList();
            ////var assigneeIds = branchAssignee.Select(x => x.AssigneeId).Distinct().ToList();
            //var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId).Distinct().ToList();
            //var assignees = DataService.GetEntity<AppAssignee>(x => assigneeIds.Contains(x.Id));
            var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchIds.Contains(x.BranchId)).ToList();
            var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId).Distinct().ToList();
            var assignees = DataService.GetEntity<AppAssignee>(x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);

            foreach (var assignee in assignees)
            {
                string personPos =
                    string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                        ? "Не визначено"
                        : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                            + (!string.IsNullOrEmpty(assignee.WorkExperience) ? $" Cтаж роботи: {assignee.WorkExperience} місяців" : "");

                string education = $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                                   $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                               + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                               $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";

                var assigneeParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = limsAppId},
                    new SqlParameter("@p_PersonName", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))},
                    new SqlParameter("@p_PersonPos", SqlDbType.VarChar) {Value = CheckForDbNull(personPos.SubstringFromStart(250))},
                    new SqlParameter("@p_Education", SqlDbType.VarChar) {Value = CheckForDbNull(education.SubstringFromStart(1000))},
                    new SqlParameter("@p_ContactInfo", SqlDbType.VarChar) {Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000)), IsNullable = true}, //???
                    new SqlParameter("@p_Email", SqlDbType.VarChar){Value = DBNull.Value},
                    new SqlParameter("@p_Notes", SqlDbType.VarChar){Value = CheckForDbNull(notes.SubstringFromStart(1000))}
                };

                _limsRepository.PrlUpdateAssigneePRL(assigneeParamList);
            }

            DataService.SaveChanges();

        }

        private async Task InsertAttach(Guid applicationId, long limsDocId)
        {
            var branches = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == applicationId);
            var edocumentBranchIds = DataService.GetEntity<BranchEDocument>(x => branches.Select(y => y.Id)
                                                                                     .Contains(x.BranchId) && x.RecordState != RecordState.D).Select(x => x.EDocumentId).ToList();
            var edocuments = DataService.GetEntity<EDocument>(x => edocumentBranchIds.Contains(x.Id) && x.RecordState != RecordState.D).ToList();
            var fileList = new List<FileStore>();
            var appFileStores = DataService
                .GetEntity<FileStore>(x => x.EntityId == applicationId && x.EntityName == "PrlApplication").ToList();
            fileList.AddRange(appFileStores);
            edocuments.ForEach(eDoc => fileList.AddRange(DataService.GetEntity<FileStore>(x => x.EntityId == eDoc.Id && x.EntityName == "EDocument")));

            foreach (var file in fileList)
            {
                var fileParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_FileId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_FileTypeId", SqlDbType.Int) {Value = 70},
                    new SqlParameter("@p_EntityId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_EntityTypeId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_DocId", SqlDbType.Int) {Value = limsDocId},
                    new SqlParameter("@p_FileName", SqlDbType.VarChar) {Value = file.OrigFileName},
                    new SqlParameter("@p_FileSize", SqlDbType.Int) {Value = file.FileSize},
                    new SqlParameter("@p_FileDate", SqlDbType.DateTime) {Value = file.CreatedOn},
                    new SqlParameter("@p_Description", SqlDbType.VarChar) {Value = !string.IsNullOrEmpty(file.Description) ? file.Description : ""},
                    new SqlParameter("@p_CreatorName", SqlDbType.VarChar)
                    {
                        Value = DataService.GetEntity<Person>(x => x.Id == file.CreatedBy).FirstOrDefault()?.FIO
                    },
                    new SqlParameter("@p_FileDocNum", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_FileDocDate", SqlDbType.DateTime) {Value = DBNull.Value}
                };
                var id = await _limsRepository.InsertAttachment(fileParamList);
                var dto = new FileStoreDTO();
                _objectMapper.Map(file, dto);
                FileStoreHelper.LoadFile(dto, out MemoryStream stream, out string contentType);
                var bytes = stream.ToArray();
                var attachParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_FileId", SqlDbType.Int) {Value = id},
                    new SqlParameter("@p_FileImage", SqlDbType.VarBinary) {Value = bytes}
                };
                _limsRepository.InsertAttaches(attachParamList);
            }
        }

        private async Task InsertAttachMPD(Guid applicationId, long limsDocId)
        {
            var branches = await DataService.GetDtoAsync<BranchListDTO>(x => x.ApplicationId == applicationId);
            var edocumentBranchIds = DataService.GetEntity<BranchEDocument>(x => branches.Select(y => y.Id)
                                                                                     .Contains(x.BranchId) && x.RecordState != RecordState.D).Select(x => x.EDocumentId).ToList();
            var edocuments = DataService.GetEntity<EDocument>(x => edocumentBranchIds.Contains(x.Id) && x.RecordState != RecordState.D).ToList();
            var fileList = new List<FileStore>();
            var appFileStores = DataService
                .GetEntity<FileStore>(x => x.EntityId == applicationId && x.EntityName == "PrlApplication").ToList();
            fileList.AddRange(appFileStores);
            edocuments.ForEach(eDoc => fileList.AddRange(DataService.GetEntity<FileStore>(x => x.EntityId == eDoc.Id && x.EntityName == "EDocument")));

            foreach (var file in fileList)
            {
                var fileParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_FileId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_FileTypeId", SqlDbType.Int) {Value = 70},
                    new SqlParameter("@p_EntityId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_EntityTypeId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_DocId", SqlDbType.Int) {Value = limsDocId},
                    new SqlParameter("@p_FileName", SqlDbType.VarChar) {Value = file.OrigFileName},
                    new SqlParameter("@p_FileSize", SqlDbType.Int) {Value = file.FileSize},
                    new SqlParameter("@p_FileDate", SqlDbType.DateTime) {Value = file.CreatedOn},
                    new SqlParameter("@p_Description", SqlDbType.VarChar) {Value = !string.IsNullOrEmpty(file.Description) ? file.Description : ""},
                    new SqlParameter("@p_CreatorName", SqlDbType.VarChar)
                    {
                        Value = DataService.GetEntity<Person>(x => x.Id == file.CreatedBy).FirstOrDefault()?.FIO
                    },
                    new SqlParameter("@p_FileDocNum", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_FileDocDate", SqlDbType.DateTime) {Value = DBNull.Value}
                };
                var id = await _limsRepository.InsertAttachmentMPD(fileParamList);
                var dto = new FileStoreDTO();
                _objectMapper.Map(file, dto);
                FileStoreHelper.LoadFile(dto, out MemoryStream stream, out string contentType);
                var bytes = stream.ToArray();
                var attachParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_FileId", SqlDbType.Int) {Value = id},
                    new SqlParameter("@p_FileImage", SqlDbType.VarBinary) {Value = bytes}
                };
                _limsRepository.InsertAttaches(attachParamList);
            }
        }

        public async void InsertGetLicenseApplicationReason(Guid appId)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var appEntity = DataService.GetEntity<PrlApplication>(app => app.Id == appId).Single();
            var license = DataService.GetEntity<PrlLicense>(prlLicense => prlLicense.Id == appEntity.ParentId).FirstOrDefault();
            var appDetailDto = DataService.GetDto<PrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();


            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int)
                {
                     Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))
                },
                new SqlParameter("@p_AppReasonId", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                },
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar)
                {
                    Value =  ResolveLimsReasonIds(appDetailDto.AppSort)
                }, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar)
                {
                    Value =appDetailDto.RegNumber
                }, //applicaiton number
                new SqlParameter("@p_RegAppDate", SqlDbType.Date)  {Value = appDetailDto.RegDate ?? DateTime.Now}, //regdate 
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_DrugGroupIds", SqlDbType.Int)
                {
                    Value = DBNull.Value, IsNullable = true
                }, //default value 11 - empty value
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = license.OldLimsId}, //lic number
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true/*= appDetailDto.OrgDirector*/},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },
                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = appDetailDto.EMail, IsNullable = true},
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) {Value = Convert.ToInt32(appDetailDto.LegalFormType)},
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_ExpResultId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };

            var insertAppTask = _limsRepository.InsertPrlApplication(appParams);
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));

            var limsAppId = await insertAppTask;
            appEntity.OldLimsId = limsAppId;

            DataService.SaveChanges();
        }

        public async Task<long> InsertLicense(Guid appId)
        {
            var appEntity = DataService.GetEntity<PrlApplication>(app => app.Id == appId).Single();
            var decision = DataService.GetDto<AppDecisionDTO>(dto => dto.AppId == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            if (appEntity.AppSort != "AddBranchInfoApplication"
                && appEntity.AppSort != "RemBranchInfoApplication"
                && appEntity.AppSort != "ChangeContrApplication")
            {
                var licParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_LicDocId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                    new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                    new SqlParameter("@p_LicRegNum", SqlDbType.VarChar) {Value = appId.ToString().Substring(0, 7)},
                    new SqlParameter("@p_NewLicRegNum", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                    new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                    new SqlParameter("@p_DecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_NewDecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = ""}, //TODO lims users
                    new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = ""},
                    new SqlParameter("@p_ModifyDate", SqlDbType.DateTime) {Value = DateTime.Now},
                    new SqlParameter("@p_SessionId", SqlDbType.Int) {Value = 228},
                    new SqlParameter("@p_OrgId", SqlDbType.Int) {Value = 1},
                    new SqlParameter("@p_UserId", SqlDbType.Int) {Value = 859},
                    new SqlParameter("@p_Domain", SqlDbType.VarChar) {Value = "CRV_License"},
                    new SqlParameter("@p_UserName", SqlDbType.VarChar) {Value = "adm_tadejeva"},
                    new SqlParameter("@p_UserFullName", SqlDbType.VarChar) {Value = "test user full name"}
                };
                //await InsertDelLicenseFile(appEntity);
                if (appEntity.AppSort != "ChangeAutPersonApplication")
                    _limsRepository.AppSetStatus(appEntity.OldLimsId);
                else
                    CloseApplication(appEntity.Id);

                //if (appEntity.AppSort == "RemBranchApplication")
                //{
                //    var branchEntityListRemove = DataService.GetEntity<Branch>(branch =>
                //        branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true &&
                //        branch.RecordState != RecordState.D);
                //    foreach (var branch in branchEntityListRemove)
                //    {
                //        //var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                //        var branchParams = new List<SqlParameter>
                //        {
                //            new SqlParameter("@p_ID", SqlDbType.Int) {Value = Convert.ToInt32(branch.OldLimsId)},
                //            new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                //            new SqlParameter("@err_msg", SqlDbType.VarChar, 400){Direction = ParameterDirection.Output}
                //        };
                //        _limsRepository.ImlAppBranchRem(branchParams);
                //    }
                //}

                var lic_id = await _limsRepository.PrlLicenseProcessRun(licParams, appEntity.OldLimsId);
                if (appEntity.AppSort == "AddBranchApplication")
                {
                    try
                    {
                        await InsertAttachMPD(appEntity.Id, appEntity.OldLimsId);
                    }
                    catch (Exception e)
                    {
                        return lic_id;
                        //throw;
                    }
                }

                return lic_id;
            }

            var limsId = DataService.GetEntity<PrlLicense>(x => x.Id == appEntity.ParentId).FirstOrDefault().OldLimsId;
            CloseApplication(appEntity.Id);
            _limsRepository.RemoveLicenseFile(limsId);
            await InsertAttach(appEntity.Id, limsId);

            return limsId;
        }

        public async Task<long> InsertLicenseIML(Guid appId)
        {
            var appEntity = DataService.GetEntity<ImlApplication>(app => app.Id == appId).Single();
            var decision = DataService.GetDto<AppDecisionDTO>(dto => dto.AppId == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));
            if (appEntity.AppSort != "AddBranchInfoApplication"
                && appEntity.AppSort != "RemBranchInfoApplication"
                && appEntity.AppSort != "ChangeContrApplication")
            {
                var licParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_LicDocId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                    new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                    new SqlParameter("@p_LicRegNum", SqlDbType.VarChar) {Value = appId.ToString().Substring(0, 7)},
                    new SqlParameter("@p_NewLicRegNum", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                    new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                    new SqlParameter("@p_DecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_NewDecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = ""}, //TODO lims users
                    new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = ""},
                    new SqlParameter("@p_ModifyDate", SqlDbType.DateTime) {Value = DateTime.Now},
                    new SqlParameter("@p_SessionId", SqlDbType.Int) {Value = 228},
                    new SqlParameter("@p_OrgId", SqlDbType.Int) {Value = 1},
                    new SqlParameter("@p_UserId", SqlDbType.Int) {Value = 859},
                    new SqlParameter("@p_Domain", SqlDbType.VarChar) {Value = "CRV_License"},
                    new SqlParameter("@p_UserName", SqlDbType.VarChar) {Value = "adm_tadejeva"},
                    new SqlParameter("@p_UserFullName", SqlDbType.VarChar) {Value = "test user full name"}
                };
                //await InsertDelLicenseFile(appEntity);
                if (appEntity.AppSort != "ChangeAutPersonApplication")
                    _limsRepository.AppSetStatusIML(appEntity.OldLimsId);
                else
                    CloseApplicationIML(appEntity.Id);
                //if (appEntity.AppSort == "RemBranchApplication")
                //{
                //    var branchEntityListRemove = DataService.GetEntity<Branch>(branch =>
                //        branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true &&
                //        branch.RecordState != RecordState.D);
                //    foreach (var branch in branchEntityListRemove)
                //    {
                //        //var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                //        var branchParams = new List<SqlParameter>
                //        {
                //            new SqlParameter("@p_ID", SqlDbType.Int) {Value = Convert.ToInt32(branch.OldLimsId)},
                //            new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                //            new SqlParameter("@err_msg", SqlDbType.VarChar, 400){Direction = ParameterDirection.Output}
                //        };
                //        _limsRepository.ImlAppBranchRem(branchParams);
                //    }
                //}

                return await _limsRepository.ImlLicenseProcessRun(licParams, appEntity.OldLimsId);
            }

            var limsId = DataService.GetEntity<ImlLicense>(x => x.Id == appEntity.ParentId).FirstOrDefault().OldLimsId;
            CloseApplicationIML(appEntity.Id);
            _limsRepository.RemoveLicenseFile(limsId);
            await InsertAttach(appEntity.Id, limsId);

            return limsId;
        }

        public async Task<long> InsertLicenseTRL(Guid appId)
        {
            var appEntity = DataService.GetEntity<TrlApplication>(app => app.Id == appId).Single();
            var decision = DataService.GetDto<AppDecisionDTO>(dto => dto.AppId == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id));
            //if (appEntity.AppSort != "AddBranchInfoApplication"
            //    && appEntity.AppSort != "RemBranchInfoApplication"
            //    && appEntity.AppSort != "ChangeContrApplication")
            //{
                var licParams = new List<SqlParameter>
                {
                    new SqlParameter("@p_LicDocId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                    //new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                    new SqlParameter("@p_LicRegNum", SqlDbType.VarChar) {Value = appId.ToString().Substring(0, 7)},
                    new SqlParameter("@p_NewLicRegNum", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                    new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                    new SqlParameter("@p_DecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_NewDecisionText", SqlDbType.VarChar)
                    {
                        Value = string.Empty
                    }, //this value is not used in the stored proc (!)
                    new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = ""}, //TODO lims users
                    new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = ""},
                    new SqlParameter("@p_ModifyDate", SqlDbType.DateTime) {Value = DateTime.Now},
                    new SqlParameter("@p_SessionId", SqlDbType.Int) {Value = 228},
                    new SqlParameter("@p_OrgId", SqlDbType.Int) {Value = 1},
                    new SqlParameter("@p_UserId", SqlDbType.Int) {Value = 859},
                    new SqlParameter("@p_Domain", SqlDbType.VarChar) {Value = "CRV_License"},
                    new SqlParameter("@p_UserName", SqlDbType.VarChar) {Value = "adm_tadejeva"},
                    new SqlParameter("@p_UserFullName", SqlDbType.VarChar) {Value = "test user full name"}
                };
                //await InsertDelLicenseFile(appEntity);
                if (appEntity.AppSort != "ChangeAutPersonApplication")
                    _limsRepository.AppSetStatusTRL(appEntity.OldLimsId);
                else
                    CloseApplicationTRL(appEntity.Id);

                //if (appEntity.AppSort == "RemBranchApplication")
                //{
                //    var branchEntityListRemove = DataService.GetEntity<Branch>(branch =>
                //        branchIds.Contains(branch.Id) && branch.LicenseDeleteCheck == true &&
                //        branch.RecordState != RecordState.D);
                //    foreach (var branch in branchEntityListRemove)
                //    {
                //        //var licBranch = DataService.GetEntity<Branch>(x => x.Id == branch.ParentId).Single();
                //        var branchParams = new List<SqlParameter>
                //            {
                //                new SqlParameter("@p_ID", SqlDbType.Int) {Value = Convert.ToInt32(branch.OldLimsId)},
                //                new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                //                new SqlParameter("@err_msg", SqlDbType.VarChar, 400)
                //                {
                //                    Direction = ParameterDirection.Output
                //                }
                //            };
                //        _limsRepository.ImlAppBranchRem(branchParams);
                //    }
                //}

            return await _limsRepository.TrlLicenseProcessRun(licParams, appEntity.OldLimsId);
           // }
           //var limsId = DataService.GetEntity<ImlLicense>(x => x.Id == appEntity.ParentId).FirstOrDefault().OldLimsId;
            //CloseApplicationIML(appEntity.Id);
            //_limsRepository.RemoveLicenseFile(limsId);
            //await InsertAttach(appEntity.Id, limsId);

            //return limsId;
        }

        public void ExportDecision(AppDecision decision, bool isUpdate = false)
        {
            var prlApplication = DataService.GetEntity<PrlApplication>(app => app.Id == decision.AppId).Single();
            var protocol = DataService.GetEntity<AppProtocol>(appProtocol => appProtocol.Id == decision.ProtocolId)
                .Single();

            int decisionTypeId;
            switch (decision.DecisionType)
            {
                case "Accepted":
                    decisionTypeId = 2;
                    break;
                case "Denied":
                    decisionTypeId = 3;
                    break;
                case "WithoutConsideration":
                    decisionTypeId = 4;
                    break;
                default:
                    decisionTypeId = 1;
                    break;
            }

            var decisionParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = prlApplication.OldLimsId},
                new SqlParameter("@p_DecisionTypeId", SqlDbType.Int) {Value = decisionTypeId},
                new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_ProtocolId", SqlDbType.Int) {Value = protocol.OldLimsId},
                new SqlParameter("@p_DecisionText", SqlDbType.VarChar) {Value = decision.DecisionDescription},
                new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = decision.Notes ?? string.Empty},
                new SqlParameter("@p_IsCheat", SqlDbType.Bit) {Value = 0}
            };
            _limsRepository.UpdatePrlDecision(decisionParams);
        }

        public void ExportDecisionIML(AppDecision decision, bool isUpdate = false)
        {
            var imlApplication = DataService.GetEntity<ImlApplication>(app => app.Id == decision.AppId).Single();
            var protocol = DataService.GetEntity<AppProtocol>(appProtocol => appProtocol.Id == decision.ProtocolId)
                .Single();

            int decisionTypeId;
            switch (decision.DecisionType)
            {
                case "Accepted":
                    decisionTypeId = 2;
                    break;
                case "Denied":
                    decisionTypeId = 3;
                    break;
                case "WithoutConsideration":
                    decisionTypeId = 4;
                    break;
                default:
                    decisionTypeId = 1;
                    break;
            }

            var decisionParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = imlApplication.OldLimsId},
                new SqlParameter("@p_DecisionTypeId", SqlDbType.Int) {Value = decisionTypeId},
                new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_ProtocolId", SqlDbType.Int) {Value = protocol.OldLimsId},
                new SqlParameter("@p_DecisionText", SqlDbType.VarChar) {Value = decision.DecisionDescription},
                new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = decision.Notes ?? string.Empty},
                new SqlParameter("@p_IsCheat", SqlDbType.Bit) {Value = 0}
            };
            _limsRepository.UpdateDecisionIML(decisionParams);
        }

        public void ExportDecisionTRL(AppDecision decision, bool isUpdate = false)
        {
            var trlApplication = DataService.GetEntity<TrlApplication>(app => app.Id == decision.AppId).Single();
            var protocol = DataService.GetEntity<AppProtocol>(appProtocol => appProtocol.Id == decision.ProtocolId)
                .Single();

            int decisionTypeId;
            switch (decision.DecisionType)
            {
                case "Accepted":
                    decisionTypeId = 2;
                    break;
                case "Denied":
                    decisionTypeId = 3;
                    break;
                case "WithoutConsideration":
                    decisionTypeId = 4;
                    break;
                default:
                    decisionTypeId = 1;
                    break;
            }

            var decisionParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = trlApplication.OldLimsId},
                new SqlParameter("@p_DecisionTypeId", SqlDbType.Int) {Value = decisionTypeId},
                new SqlParameter("@p_NewLicStartDate", SqlDbType.DateTime) {Value = decision.DateOfStart},
                new SqlParameter("@p_NewLicEndDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_ProtocolId", SqlDbType.Int) {Value = protocol.OldLimsId},
                new SqlParameter("@p_DecisionText", SqlDbType.VarChar) {Value = decision.DecisionDescription},
                new SqlParameter("@p_PaidSum", SqlDbType.Decimal) {Value = decision.PaidMoney},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = decision.Notes ?? string.Empty},
                new SqlParameter("@p_IsCheat", SqlDbType.Bit) {Value = 0}
            };
            _limsRepository.UpdateDecisionTRL(decisionParams);
        }

        public void RemoveDecision(long limsAppId)
        {
            var appId = new SqlParameter("@p_AppId", SqlDbType.Int) { Value = limsAppId };
            _limsRepository.RemoveDecision(appId);
        }

        public void RemoveDecisionIML(long limsAppId)
        {
            var appId = new SqlParameter("@p_AppId", SqlDbType.Int) { Value = limsAppId };
            _limsRepository.RemoveDecisionIML(appId);
        }

        public void RemoveDecisionTRL(long limsAppId)
        {
            var appId = new SqlParameter("@p_LicAppId", SqlDbType.Int) { Value = limsAppId };
            _limsRepository.RemoveDecisionTRL(appId);
        }

        public async void UpdateExpertise(PrlAppExpertiseDTO model)
        {
            var app = DataService.GetEntity<PrlApplication>(x => x.Id == model.Id).Single();
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == model.PerformerOfExpertiseId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_ExpResultId", SqlDbType.Int)
                {
                    Value = CheckForDbNull(ResolveLimsExpertiseId(model.ExpertiseResultEnum)/* == "Positive" ? 1 : 2*/)
                },
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = model.ExpertiseDate},
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = CheckForDbNull(Convert.ToInt32(performerId))}
            };

            await _limsRepository.UpdateExpertise(sqlParams);
        }

        public async void UpdateExpertiseIML(ImlAppExpertiseDTO model)
        {
            var app = DataService.GetEntity<ImlApplication>(x => x.Id == model.Id).Single();

            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_ExpResultId", SqlDbType.Int)
                {
                    Value = CheckForDbNull(ResolveLimsExpertiseId(model.ExpertiseResultEnum)/* == "Positive" ? 1 : 2*/)/*Convert.ToInt32(model.ExpertiseResultEnum == "Positive" ? 1 : 2)*/
                },
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = model.ExpertiseDate},
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value},
            };

            await _limsRepository.UpdateExpertise(sqlParams);
        }

        public async void UpdateExpertiseTRL(TrlAppExpertiseDTO model)
        {
            var app = DataService.GetEntity<TrlApplication>(x => x.Id == model.Id).Single();

            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_ExpResultId", SqlDbType.Int)
                {
                    Value = CheckForDbNull(ResolveLimsExpertiseId(model.ExpertiseResultEnum)/* == "Positive" ? 1 : 2*/)/*Convert.ToInt32(model.ExpertiseResultEnum == "Positive" ? 1 : 2)*/
                },
                new SqlParameter("@p_ExpDate", SqlDbType.Date) {Value = model.ExpertiseDate},
                new SqlParameter("@p_ExpPerformerId", SqlDbType.Int) {Value = DBNull.Value},
            };

            await _limsRepository.UpdateExpertise(sqlParams);
        }

        private static  int? ResolveLimsExpertiseId(string ExpertiseResultEnum)
        {
            switch (ExpertiseResultEnum)
            {
                case "Positive":
                    return 1;
                case "Negative":
                    return 2;
                default:
                    return null;
            }
        }

        public async Task<long> ExportLimsMessage(AppLicenseMessage model)
        {
            var app = DataService.GetEntity<PrlApplication>(x => x.Id == model.AppId).Single();
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_NoticeId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_OwnerId", SqlDbType.Int) {Value = 1},
                new SqlParameter("@p_RegNum", SqlDbType.VarChar) {Value = model.MessageNumber},
                new SqlParameter("@p_RegDate", SqlDbType.DateTime) {Value = model.DateOfMessage},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = model.SignedFullName},
                new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = model.SignedJobPosition},
                new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity.OldLimsId ?? 461}, //TODO implement later
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = CheckForDbNull(userFullName)},
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            return await _limsRepository.InsertLimsNotice(sqlParams);
        }

        public async Task<long> ExportLimsMessageIML(AppLicenseMessage model)
        {
            var app = DataService.GetEntity<ImlApplication>(x => x.Id == model.AppId).Single();
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_NoticeId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_OwnerId", SqlDbType.Int) {Value = 1},
                new SqlParameter("@p_RegNum", SqlDbType.VarChar) {Value = model.MessageNumber},
                new SqlParameter("@p_RegDate", SqlDbType.DateTime) {Value = model.DateOfMessage},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = model.SignedFullName},
                new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = model.SignedJobPosition},
                new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity.OldLimsId ?? 461}, //TODO implement later
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = CheckForDbNull(userFullName)},
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            return await _limsRepository.InsertLimsNoticeIML(sqlParams);
        }

        public async Task<long> ExportLimsMessageTRL(AppLicenseMessage model)
        {
            var app = DataService.GetEntity<TrlApplication>(x => x.Id == model.AppId).Single();
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault();
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p_NoticeId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_OwnerId", SqlDbType.Int) {Value = 1},
                new SqlParameter("@p_RegNum", SqlDbType.VarChar) {Value = model.MessageNumber},
                new SqlParameter("@p_RegDate", SqlDbType.DateTime) {Value = model.DateOfMessage},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SignerPib", SqlDbType.VarChar) {Value = model.SignedFullName},
                new SqlParameter("@p_SignerPos", SqlDbType.VarChar) {Value = model.SignedJobPosition},
                //new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity.OldLimsId ?? 461}, //TODO implement later
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = CheckForDbNull(userFullName)},
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            return await _limsRepository.InsertLimsNoticeTRL(sqlParams);
        }

        public async Task<List<AppLicenseMessage>> UpdateLimsMessages()
        {
            var pendingChanges =
                _limsRepository.GetPendingChanges(LimsRepository.ChangesTrackedEnum.AppNotice).ToList();
            var pendingIds = pendingChanges.Where(changes => changes.Action == "UPDATE" || changes.Action == "INSERT")
                .Select(x => x.EntityId).ToList();
            var pendingNotices = (await _limsRepository.GetLimsNotice())
                .Where(notice => pendingIds.Contains(notice.NoticeId));

            var updatedLimsMessages = new List<AppLicenseMessage>();
            if (pendingNotices.Any())
            {
                var pendingNoticesIds = pendingNotices.Select(notice => notice.NoticeId).ToList();
                var toUpdate = pendingNotices
                    .Where(protocol => pendingNoticesIds.Contains(protocol.NoticeId)).ToList();
                toUpdate.ForEach(notice =>
                {
                    AppLicenseMessage portalMessage;
                    try
                    {
                        portalMessage = DataService
                            .GetEntity<AppLicenseMessage>(licenseMessage => licenseMessage.OldLimsId == notice.NoticeId)
                            .FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            $"Error updating messages! There is more than one protocol with OldLImsId equals {notice.NoticeId}",
                            e);
                    }

                    if (portalMessage == null)
                    {
                        return;
                    }

                    MapOneLicenseMessage(notice, portalMessage);
                    DataService.Add(portalMessage, true);
                    updatedLimsMessages.Add(portalMessage);
                });
            }

            DataService.SaveChanges();
            _limsRepository.UpdatePendingChanges(pendingChanges.Select(x => x.Id).ToList());

            return updatedLimsMessages;
        }

        public async Task ImportLimsProtocols()
        {
            var limsProtocols = (await _limsRepository.GetLimsProtocols()).ToList();

            var appProtocols = MapProtocols(limsProtocols);
            var protocolsToAdd = new List<AppProtocol>();
            appProtocols.ForEach(newProtocol =>
            {
                if (!_dbContext.AppProtocols.Where(protocol => protocol.OldLimsId == newProtocol.OldLimsId)
                    .AsNoTracking().Any())
                {
                    protocolsToAdd.Add(newProtocol);
                }
            });
            _dbContext.AppProtocols.AddRange(protocolsToAdd);
            _dbContext.SaveChanges();
        }

        public async Task<List<AppProtocol>> UpdateLimsProtocols()
        {
            //List of all not processed pending changes about protocols
            var pendingChanges = _limsRepository.GetPendingChanges(LimsRepository.ChangesTrackedEnum.AppProtocol)
                .ToList();
            var pendingChangesToInsert = _limsRepository.GetPendingChangesToInsert(LimsRepository.ChangesTrackedEnum.AppProtocol)
                .ToList();
            var pendingIds = pendingChanges.Select(x => x.EntityId).ToList();
            var pendingProtocols = (await _limsRepository.GetLimsProtocols())
                .Where(protocol => pendingIds.Contains(protocol.OldLimsId));

            //insert new
            var insertedProtocolsIds = pendingChangesToInsert.Where(changes => changes.Action == "INSERT")
                .Select(x => x.EntityId).ToList();
            if (insertedProtocolsIds.Any())
            {
                var toInsert = pendingProtocols
                    .Where(protocol => insertedProtocolsIds.Contains(protocol.OldLimsId)).ToList();
                MapProtocols(toInsert).ForEach(protocol => DataService.Add(protocol));
                DataService.SaveChanges();
            }

            //update old
            var updatedProtocols = new List<AppProtocol>();
            var updatedProtocolsIds = pendingChanges.Where(changes => changes.Action == "UPDATE")
                .Select(x => x.EntityId).ToList();
            if (updatedProtocolsIds.Any())
            {
                var toUpdate = pendingProtocols
                    .Where(protocol => updatedProtocolsIds.Contains(protocol.OldLimsId)).ToList();
                foreach (var limsProtocol in toUpdate)
                {
                    AppProtocol portalProtocol;
                    try
                    {
                        portalProtocol = DataService
                            .GetEntity<AppProtocol>(pr => pr.OldLimsId == limsProtocol.OldLimsId).Single();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error updating protocols! There is more than one protocol with OldLImsId equals {limsProtocol.OldLimsId}", e);
                    }

                    MapOneProtocol(limsProtocol, portalProtocol);
                    DataService.Add(portalProtocol, true);
                    updatedProtocols.Add(portalProtocol);
                }
            }

            // delete
            var deleteProtocolsIds = pendingChanges.Where(changes => changes.Action == "DELETE")
                .Select(x => x.EntityId).ToList();
            if (deleteProtocolsIds.Any())
            {
                var toDelList = pendingChanges
                       .Where(protocol => deleteProtocolsIds.Contains(protocol.EntityId)).ToList();

                foreach (var limsProtocol in toDelList)
                {
                    AppProtocol portalProtocol;
                    try
                    {
                        portalProtocol = DataService
                            .GetEntity<AppProtocol>(pr => pr.OldLimsId == limsProtocol.EntityId).SingleOrDefault();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error deleting protocols! {limsProtocol.EntityId}", e);
                    }
                    if (portalProtocol != null)
                    {
                        DataService.Remove(portalProtocol);
                    }
                }
            };

            DataService.SaveChanges();
            _limsRepository.UpdatePendingChanges(pendingChanges.Select(x => x.EntityId).ToList());

            return updatedProtocols;
        }

        public async Task<List<LimsRP>> UpdateLimsRp()
        {
            //List of all not processed pending changes about protocols
            var pendingChanges = _limsRepository.GetPendingChanges(LimsRepository.ChangesTrackedEnum.LimsRp)
                    .ToList();

            var pendingChangesToInsert = _limsRepository.GetPendingChangesToInsert(LimsRepository.ChangesTrackedEnum.LimsRp)
                .ToList();
            var pendingIds = pendingChanges.Select(x => x.EntityId).ToList();
            var pendingLimsRp = (await _limsRepository.GetLimsRp())
                .Where(rp => pendingIds.Contains(rp.DocId));

            //insert new
            //var insertedRpIds = pendingChangesToInsert.Where(changes => changes.Action == "INSERT")
            //    .Select(x => x.EntityId).ToList();
            //if (insertedRpIds.Any())
            //{
            //    var toInsert = pendingLimsRp
            //        .Where(rp => insertedRpIds.Contains(rp.DocId)).ToList();
            //    MapOneRp(toInsert).ForEach(protocol => DataService.Add(protocol));  //??
            //    DataService.SaveChanges();
            //}

            //update old
            var updatedRps = new List<LimsRP>();
            var updatedRpIds = pendingChanges.Where(changes => changes.Action == "UPDATE")
                .Select(x => x.EntityId).ToList();
            if (updatedRpIds.Any())
            {
                var toUpdate = pendingLimsRp
                    .Where(rp => updatedRpIds.Contains(rp.DocId)).ToList();
                foreach (var oldLimsRp in toUpdate)
                {
                    LimsRP limsRp;
                    try
                    {
                        limsRp = DataService
                            .GetEntity<LimsRP>(rp => rp.DocId == oldLimsRp.DocId).Single();
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error updating protocols! There is more than one protocol with OldLImsId equals {oldLimsRp.DocId}", e);
                    }

                    MapOneRp(oldLimsRp, limsRp);
                    DataService.Add(limsRp, true);
                    updatedRps.Add(limsRp);
                }
            }

            // delete
            //var deleteProtocolsIds = pendingChanges.Where(changes => changes.Action == "DELETE")
            //    .Select(x => x.EntityId).ToList();
            //if (deleteProtocolsIds.Any())
            //{
            //    var toDelList = pendingChanges
            //           .Where(protocol => deleteProtocolsIds.Contains(protocol.EntityId)).ToList();

            //    foreach (var limsProtocol in toDelList)
            //    {
            //        LimsRP portalProtocol;
            //        try
            //        {
            //            portalProtocol = DataService
            //                .GetEntity<LimsRP>(pr => pr.DocId == limsProtocol.EntityId).SingleOrDefault();
            //        }
            //        catch (Exception e)
            //        {
            //            throw new Exception($"Error deleting protocols! {limsProtocol.EntityId}", e);
            //        }
            //        if (portalProtocol != null)
            //        {
            //            DataService.Remove(portalProtocol);
            //        }
            //    }
            //};

            DataService.SaveChanges();
            _limsRepository.UpdatePendingChanges(pendingChanges.Select(x => x.EntityId).ToList());

            return updatedRps;
        }

        public async void ExportPreLicenseCheck(AppPreLicenseCheck preLicense)
        {
            var app = DataService.GetEntity<PrlApplication>(application => application.Id == preLicense.AppId).Single();

            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_CheckId", SqlDbType.Decimal) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_BeginDate", SqlDbType.DateTime) {Value = preLicense.ScheduledStartDate},
                new SqlParameter("@p_EndDate", SqlDbType.DateTime) {Value = preLicense.ScheduledEndDate},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_OrderNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_OrderDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_OrderPerformer", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_FactDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_DefectCount", SqlDbType.Int) {Value = 0},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_ActTypeId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_ActNum", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = "Lims test"}, //TODO user
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            try
            {
                preLicense.OldLimsId = await _limsRepository.InsertPrlLicenseCheck(checkParameters);
                DataService.Add(preLicense);
                DataService.SaveChanges();
            }
            catch (Exception e)
            {
                DataService.Remove(preLicense);
                DataService.SaveChanges();
                throw;
            }

        }

        public async void ExportPreLicenseCheckIML(AppPreLicenseCheck preLicense)
        {
            var app = DataService.GetEntity<ImlApplication>(application => application.Id == preLicense.AppId).Single();

            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_CheckId", SqlDbType.Decimal) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_TaskDate",SqlDbType.DateTime) {Value = DateTime.Now},
                new SqlParameter("@p_PerformerId",SqlDbType.Int) {Value = 1},
                new SqlParameter("@p_TermDate",SqlDbType.DateTime) {Value = preLicense.ScheduledStartDate},
                new SqlParameter("@p_TaskNum",SqlDbType.VarChar) {Value = app.RegNumber},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_BeginDate", SqlDbType.DateTime) {Value = preLicense.ScheduledStartDate},
                new SqlParameter("@p_EndDate", SqlDbType.DateTime) {Value = preLicense.ScheduledEndDate},

                new SqlParameter("@p_OrderNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_OrderDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_CheckPersonIds", SqlDbType.VarChar) {Value = DBNull.Value},

                new SqlParameter("@p_OrderPerformer", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityDate", SqlDbType.DateTime) {Value = DBNull.Value},
                //new SqlParameter("@p_FactDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 1},

                new SqlParameter("@p_SendDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_SenderName", SqlDbType.VarChar) {Value = DBNull.Value},

                new SqlParameter("@p_ActReceiveNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_ActReceiveDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_IsSigned", SqlDbType.Bit) {Value = 1},
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = "Lims test"}, //TODO user
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            try
            {
                preLicense.OldLimsId = await _limsRepository.InsertPrlLicenseCheckIML(checkParameters);
                DataService.Add(preLicense);
                DataService.SaveChanges();
            }
            catch (Exception e)
            {
                DataService.Remove(preLicense);
                DataService.SaveChanges();
                throw;
            }

        }

        public async void ExportPreLicenseCheckTRL(AppPreLicenseCheck preLicense)
        {
            var app = DataService.GetEntity<TrlApplication>(application => application.Id == preLicense.AppId).Single();

            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_CheckId", SqlDbType.Decimal) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = app.OldLimsId},
                new SqlParameter("@p_TaskDate",SqlDbType.DateTime) {Value = DateTime.Now},
                new SqlParameter("@p_PerformerId",SqlDbType.Int) {Value = 1},
                new SqlParameter("@p_TermDate",SqlDbType.DateTime) {Value = preLicense.ScheduledStartDate},
                new SqlParameter("@p_TaskNum",SqlDbType.VarChar) {Value = app.RegNumber},
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_BeginDate", SqlDbType.DateTime) {Value = preLicense.ScheduledStartDate},
                new SqlParameter("@p_EndDate", SqlDbType.DateTime) {Value = preLicense.ScheduledEndDate},

                new SqlParameter("@p_OrderNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_OrderDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_CheckPersonIds", SqlDbType.VarChar) {Value = DBNull.Value},

                new SqlParameter("@p_OrderPerformer", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_IdentityDate", SqlDbType.DateTime) {Value = DBNull.Value},
                //new SqlParameter("@p_FactDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 1},

                new SqlParameter("@p_SendDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_SenderName", SqlDbType.VarChar) {Value = DBNull.Value},

                new SqlParameter("@p_ActReceiveNum", SqlDbType.VarChar) {Value = DBNull.Value},
                new SqlParameter("@p_ActReceiveDate", SqlDbType.DateTime) {Value = DBNull.Value},
                new SqlParameter("@p_IsSigned", SqlDbType.Bit) {Value = 1},
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = "Lims test"}, //TODO user
                new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            };

            try
            {
                preLicense.OldLimsId = await _limsRepository.InsertPrlLicenseCheckTRL(checkParameters);
                DataService.Add(preLicense);
                DataService.SaveChanges();
            }
            catch (Exception e)
            {
                DataService.Remove(preLicense);
                DataService.SaveChanges();
                throw;
            }

        }

        public async Task UpdatePreLicenseCheck()
        {
            var pendingChanges = _limsRepository.GetPendingChanges(LimsRepository.ChangesTrackedEnum.AppCheck).ToList();

            var pendingIds = pendingChanges.Where(changes => changes.Action == "UPDATE").Select(x => x.EntityId).ToList();
            var pendingChecks =
                (await _limsRepository.GetLimsCheck()).Where(check => pendingIds.Contains(check.CheckId)).ToList();

            var updatedLimsMessages = new List<AppPreLicenseCheck>();
            if (pendingChecks.Any())
            {
                pendingChecks.ForEach(check =>
                {
                    AppPreLicenseCheck portalCheck;
                    try
                    {
                        portalCheck = DataService
                            .GetEntity<AppPreLicenseCheck>(licenseCheck => licenseCheck.OldLimsId == check.CheckId)
                            .FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            $"Error updating messages! There is more than one protocol with OldLImsId equals {check.CheckId}",
                            e);
                    }

                    if (portalCheck == null)
                    {
                        return;
                    }

                    MapOneCheck(check, portalCheck);
                    DataService.Add(portalCheck, true);
                    updatedLimsMessages.Add(portalCheck);
                });
            }

            DataService.SaveChanges();
            _limsRepository.UpdatePendingChanges(pendingChanges.Select(x => x.Id).ToList());
        }

        public async Task UpdateEndLicCheck()
        {
            var pendingChanges = _limsRepository.GetPendingChanges(LimsRepository.ChangesTrackedEnum.EndLicCheck).ToList();
            var pendingIds = pendingChanges.Select(x => x.EntityId).ToList();

            var pendingChecks =
                (await _limsRepository.GetEndLicCheck()).Where(check => pendingIds.Contains(check.Id)).ToList();

            if (pendingChanges.Any())
            {
                foreach (var check in pendingChecks)
                {
                    var license = DataService.GetEntity<PrlLicense>(x => x.OldLimsId == check.Id && x.IsRelevant).FirstOrDefault();

                    if (license == null)
                    {
                        continue;
                    }

                    license.EndReasonText = check.EndReasonText;
                    license.EndOrderNumber = check.EndOrderNumber;
                    license.EndOrderDate = check.EndOrderDate;
                    license.EndOrderText = check.EndOrderText;
                    license.LicState = "Invalidated";
                }
            }

            await DataService.SaveChangesAsync();
            _limsRepository.UpdatePendingChanges(pendingIds);
        }

        public async void DeletePreLicenseCheck(long oldLimsId)
        {
            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_ID", SqlDbType.Int) {Value = oldLimsId},
                new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@err_msg", SqlDbType.VarChar) { Value = string.Empty, Direction = ParameterDirection.Output}
            };

            _limsRepository.DeletePrlLicenseCheck(checkParameters);
        }

        public async void DeletePreLicenseCheckIML(long oldLimsId)
        {
            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_ID", SqlDbType.Int) {Value = oldLimsId},
                new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@err_msg", SqlDbType.VarChar) { Value = string.Empty, Direction = ParameterDirection.Output}
            };

            _limsRepository.DeletePrlLicenseCheckIML(checkParameters);
        }

        public async void DeletePreLicenseCheckTRL(long oldLimsId)
        {
            var checkParameters = new List<SqlParameter>
            {
                new SqlParameter("@p_ID", SqlDbType.Int) {Value = oldLimsId},
                new SqlParameter("@err_code", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@err_msg", SqlDbType.VarChar) { Value = string.Empty, Direction = ParameterDirection.Output}
            };

            _limsRepository.DeletePrlLicenseCheckTRL(checkParameters);
        }

        public void CloseApplication(Guid appId)
        {
            var appEntity = DataService.GetEntity<PrlApplication>(application => application.Id == appId).Single();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 5} //CONST_LIC_APS5_FINISH
            };
            _limsRepository.CloseApplication(parameters);
        }

        public void CloseApplicationIML(Guid appId)
        {
            var appEntity = DataService.GetEntity<ImlApplication>(application => application.Id == appId).Single();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 5} //CONST_LIC_APS5_FINISH
            };
            _limsRepository.CloseApplication(parameters);
        }

        public void CloseApplicationTRL(Guid appId)
        {
            var appEntity = DataService.GetEntity<TrlApplication>(application => application.Id == appId).Single();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Value = appEntity.OldLimsId},
                new SqlParameter("@p_StatusId", SqlDbType.Int) {Value = 5} //CONST_LIC_APS5_FINISH
            };
            _limsRepository.CloseApplication(parameters);
        }

        public async Task<List<LicenseLIMS>> GetLicenses(string licType, string edrpou)
        {
            return await _limsRepository.GetLimsLicense(licType, edrpou);
        }

        public async Task ImportLimsRP()
        {
            var listRP = _limsRepository.GetLimsRP();

            var transCounter = 0;

            foreach (var limsOldRp in listRP)
            {
                var newModel = new LimsRP();
                _objectMapper.Map(limsOldRp, newModel);
                _dbContext.LimsRP.Add(newModel);
                transCounter++;
                if (transCounter % 500 == 0)
                    _dbContext.SaveChanges();
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task ImportLimsDepartmentalSubordination()
        {
            var spoduList = DataService.GetEntity<DepartmentalSubordination>().ToList();
            _dbContext.RemoveRange(spoduList);
            await _dbContext.SaveChangesAsync();

            var limsSpoduList = _limsRepository.GetLimsSpodu();
           
            foreach (var limsSpodu in limsSpoduList)
            {
                var newModel = new DepartmentalSubordination();
                _objectMapper.Map(limsSpodu, newModel);
                _dbContext.DepartmentalSubordinations.Add(newModel);
            }

            await _dbContext.SaveChangesAsync();
        }

        private static List<AppProtocol> MapProtocols(IEnumerable<LimsProtocol> protocols)
            => protocols.Select(limsProtocol => MapOneProtocol(limsProtocol)).ToList();

        private static AppProtocol MapOneProtocol(LimsProtocol limsProtocol, AppProtocol destinationProtocol = null)
        {
            var protocol = new AppProtocol();
            if (destinationProtocol != null)
            {
                protocol = destinationProtocol;
            }

            protocol.OldLimsId = limsProtocol.OldLimsId;
            protocol.StatusId = limsProtocol.StatusId;
            protocol.StatusName = limsProtocol.StatusName;
            protocol.ProtocolNumber = limsProtocol.ProtocolNumber;
            protocol.ProtocolDate = limsProtocol.ProtocolDate;
            protocol.OrderNumber = limsProtocol.OrderNumber;
            protocol.OrderDate = limsProtocol.OrderDate;
            protocol.Type = limsProtocol.Type;
            return protocol;

            //if (destinationProtocol == null)
            //{
            //    return new AppProtocol
            //    {
            //        OldLimsId = limsProtocol.OldLimsId,
            //        StatusName = limsProtocol.StatusName,
            //        ProtocolNumber = limsProtocol.ProtocolNumber,
            //        ProtocolDate = limsProtocol.ProtocolDate,
            //        OrderNumber = limsProtocol.OrderNumber,
            //        OrderDate = limsProtocol.OrderDate
            //    };
            //}
            //else
            //{
            //    destinationProtocol.OldLimsId = limsProtocol.OldLimsId;
            //    destinationProtocol.StatusName = limsProtocol.StatusName;
            //    destinationProtocol.ProtocolNumber = limsProtocol.ProtocolNumber;
            //    destinationProtocol.ProtocolDate = limsProtocol.ProtocolDate;
            //    destinationProtocol.OrderNumber = limsProtocol.OrderNumber;
            //    destinationProtocol.OrderDate = limsProtocol.OrderDate;
            //    return destinationProtocol;
            //}
        }

        private static void MapOneCheck(LimsCheck limsCheck, AppPreLicenseCheck destinationCheck)
        {
            destinationCheck.EndDateOfCheck = limsCheck.FactDate;
            destinationCheck.ResultOfCheck = limsCheck.DefectCount;
        }

        private static void MapOneRp(LimsOldRP limsOldRp, LimsRP destinationLimsRp)
        {
            destinationLimsRp.DrugNameUkr = limsOldRp.DrugNameUkr;
            //destinationLimsRp.DocId = limsOldRp.DocId;
            destinationLimsRp.ActiveSubstances = limsOldRp.ActiveSubstances;
            destinationLimsRp.AtcCode = limsOldRp.AtcCode;
            destinationLimsRp.CountryId = limsOldRp.CountryId;
            destinationLimsRp.CountryName = limsOldRp.CountryName;
            destinationLimsRp.DrugClassId = limsOldRp.DrugClassId;
            destinationLimsRp.DrugNameUkr = limsOldRp.DrugNameUkr;
            destinationLimsRp.DrugClassName = limsOldRp.DrugClassName;
            destinationLimsRp.DrugNameEng = limsOldRp.DrugNameEng;
            destinationLimsRp.DrugtypeId = limsOldRp.DrugtypeId;
            destinationLimsRp.DrugtypeName = limsOldRp.DrugtypeName;
            destinationLimsRp.EndDate = limsOldRp.EndDate;
            destinationLimsRp.FarmGroup = limsOldRp.FarmGroup;
            destinationLimsRp.FormName = limsOldRp.FormName;
            destinationLimsRp.FormTypeId = limsOldRp.FormTypeId;
            destinationLimsRp.FormtypeDesc = limsOldRp.FormtypeDesc;
            destinationLimsRp.IsResident = limsOldRp.IsResident;
            destinationLimsRp.Notes = limsOldRp.Notes;
            destinationLimsRp.OffOrderDate = limsOldRp.OffOrderDate;
            destinationLimsRp.OffOrderNum = limsOldRp.OffOrderNum;
            destinationLimsRp.OffReason = limsOldRp.OffReason;
            destinationLimsRp.OrdRegDate = limsOldRp.OrdRegDate;
            destinationLimsRp.OrdRegNum = limsOldRp.OrdRegNum;
            destinationLimsRp.ProdCountryName = limsOldRp.ProdCountryName;
            destinationLimsRp.ProducerName = limsOldRp.ProducerName;
            destinationLimsRp.PublicityInfo = limsOldRp.PublicityInfo;
            destinationLimsRp.RegDate = limsOldRp.OrdRegDate;
            destinationLimsRp.RegNum = limsOldRp.OrdRegNum;
            destinationLimsRp.RegProcCode = limsOldRp.RegProcCode;
            destinationLimsRp.RegProcedure = limsOldRp.RegProcedure;
            destinationLimsRp.RegprocCode = limsOldRp.RegprocCode;
            destinationLimsRp.RegprocId = limsOldRp.RegprocId;
            destinationLimsRp.RegprocName = limsOldRp.RegprocName;
            destinationLimsRp.RpOrderId = limsOldRp.RpOrderId;
            destinationLimsRp.SaleTerms = limsOldRp.SaleTerms;
            destinationLimsRp.SideName = limsOldRp.SideName;
            destinationLimsRp.StateId = limsOldRp.StateId;
        }

        //private static List<LimsRP> MapRp(IEnumerable<LimsOldRP> rp)
        //{
        //    var t =  rp.Select(LimsRP => MapOneRp(LimsRP)).ToList();
        //    return t;
        //}

        private static AppLicenseMessage MapOneLicenseMessage(LimsNotice limsNotice,
            AppLicenseMessage destinationLicenseMessage = null)
        {
            var licenseMessage = new AppLicenseMessage();
            if (destinationLicenseMessage != null)
            {
                licenseMessage = destinationLicenseMessage;
            }

            licenseMessage.OldLimsId = limsNotice.NoticeId;
            licenseMessage.SignedFullName = limsNotice.SignerPib;
            licenseMessage.SignedJobPosition = limsNotice.SignerPos;
            licenseMessage.MessageNumber = limsNotice.RegNum;
            licenseMessage.State = limsNotice.StatusName;
            if (limsNotice.RegDate != null)
            {
                licenseMessage.DateOfMessage = (DateTime)limsNotice.RegDate;
            }
            //licenseMessage.Performer = limsNotice.PersonId //TODO cogdato nado bydet

            return licenseMessage;
        }

        private static int ResolveLimsAppType(string appSort)
        {
            switch (appSort)
            {
                case "GetLicenseApplication":
                    return 1;
                case "AddBranchApplication":
                    return 2;
                case "RemBranchApplication":
                    return 6;
                case "CancelLicenseApplication":
                case "DecreaseIMLApplication":
                case "DecreaseTRLApplication":
                case "DecreasePRLApplication":
                    return 5;
                case "ChangeAutPersonApplication":
                    return 10;
                case "AddBranchInfoApplication":
                case "RemBranchInfoApplication":
                    return 3;
                case "ChangeContrApplication":
                    return 3;
                case "ChangeDrugList":
                    return 9;
                case "ReplacementDrugList":
                    return 8;
                default:
                    return 1;
            }
        }

        private static int? ResolveLimsAppReason(string appSort)
        {
            switch (appSort)
            {
                case "GetLicenseApplication":
                    return 1;
                case "CancelLicenseApplication":
                case "DecreaseIMLApplication":
                case "DecreaseTRLApplication":
                case "DecreasePRLApplication":
                    return 1;
                case "RemBranchApplication":
                    return 8;
                default:
                    return 1;
            }
        }

        private string ResolveLimsReasonIds(string appSort)
        {
            switch (appSort)
            {
                case "AddBranchInfoApplication":
                    return "16";
                case "RemBranchInfoApplication":
                    return "17";
                case "ChangeContrApplication":
                    return "11 , 12";
                default:
                    return null;
            }
        }

        public async void InsertGetLicenseApplicationIML(Guid appId)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;
            var userEntity = DataService.GetEntity<EmployeeExt>
                (ext => ext.PersonId == userInfo.PersonId).FirstOrDefault().OldLimsId;
            var appDetailDto = DataService.GetDto<ImlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.RecordState != RecordState.D);
            var appEntity = DataService.GetEntity<ImlApplication>(app => app.Id == appId).Single();
            var performer = DataService.GetDto<EmployeeExtDetailDTO>(x => x.PersonId == appEntity.PerformerId).FirstOrDefault();
            var performerId = performer?.OldLimsId;

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_AppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int){Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))},
                new SqlParameter("@p_AppReasonId", SqlDbType.Int){Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value},
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar){Value = appDetailDto.RegNumber},
                new SqlParameter("@p_RegAppDate", SqlDbType.Date){Value = appDetailDto.RegDate ?? DateTime.Now},
                new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = performerId ?? 461}, //?? dobavit
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value/*branchIds.Count*/, IsNullable = true}, //?? x3
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit) {Value = DBNull.Value},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = appDetailDto.OrgDirector},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },

                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Email", SqlDbType.VarChar) { Value = appDetailDto.EMail, IsNullable = true },
                new SqlParameter("@p_IsImportReady", SqlDbType.Int) {Value = appEntity.ImlIsImportingFinished ? Convert.ToInt32(1) : Convert.ToInt32(0) }, 
                new SqlParameter("@p_IsImportInbulk", SqlDbType.Int) {Value = appEntity.ImlIsImportingInBulk ? Convert.ToInt32(1) : Convert.ToInt32(0) }, 
                new SqlParameter("@p_ImportOther", SqlDbType.VarChar) {Value =CheckForDbNull(appEntity.ImlAnotherActivity)}, 
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) { Value = Convert.ToInt32(appDetailDto.LegalFormType) },
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(3)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };


            var insertAppTask = _limsRepository.InsertImlApplication(appParams);
            var limsAppId = await insertAppTask;
            appEntity.OldLimsId = limsAppId;

            int counter = 1;

            //var branchEntityOldLimsId = "";
            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .Single();

                int residenceTypeId;

                #region residence type

                //Lims [CDC_RESIDENCE_TYPE]
                //  RESIDENCE_TYPE_ID     RESIDENCE_TYPE_NAME
                //  1                     Місто
                //  2                     Село
                //  3                     Не визначено
                //  4                     Селище міського типу
                switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion

                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(limsAppId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_IsAddressMatches", SqlDbType.Int){Value = Convert.ToInt32(0)}, //default value 0
                    new SqlParameter("@p_RegionId", SqlDbType.Int){Value = Convert.ToInt32(1)}, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int){Value = residenceTypeId}, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar){Value = branchAddressDto.Address}, //parse branch address
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar){Value = branchAddressDto.PostIndex},
                    new SqlParameter("@p_Phone", SqlDbType.VarChar){Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)},
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = String.Empty},
                    new SqlParameter("@p_ToCheck", SqlDbType.Bit) {Value = DBNull.Value}
                };

                branchEntity.OldLimsId = await _limsRepository.InsertImlBranch(branchParamList);
                //branchEntityOldLimsId = branchEntity.OldLimsId.ToString();
            }

            try
            {
                await InsertAttach(appId, limsAppId);
            }
            catch (Exception e)
            {
                throw e;
            }
            DataService.SaveChanges();
  
            foreach (var branchEntityOldLimsId in branchEntityList)
            {
                var branchAssignee = DataService.GetEntity<AppAssigneeBranch>(x => branchEntityOldLimsId.Id == x.BranchId)   // .Contains(x.BranchId))
                    .ToList();
                var assigneeIds = branchAssignee.Where(x => x.RecordState != RecordState.D).Select(x => x.AssigneeId)
                    .Distinct().ToList();
                var assignees =
                    DataService.GetEntity<AppAssignee>(
                        x => assigneeIds.Contains(x.Id) && x.RecordState != RecordState.D);

                //branchAssignee

                foreach (var assignee in assignees)
                {
                    string personPos =
                        string.IsNullOrEmpty(assignee.NameOfPosition) && string.IsNullOrEmpty(assignee.WorkExperience)
                            ? "Не визначено"
                            : (!string.IsNullOrEmpty(assignee.NameOfPosition) ? $"{assignee.NameOfPosition};" : "")
                              + (!string.IsNullOrEmpty(assignee.WorkExperience)
                                  ? $" Cтаж роботи: {assignee.WorkExperience} місяців"
                                  : "");

                    string education =
                        $"{assignee.EducationInstitution}, закінчив у {assignee.YearOfGraduation}р. Спеціальність: {assignee.Speciality}." +
                        $" Диплом №{assignee.NumberOfDiploma}, виданий у {assignee.DateOfGraduation?.ToShortDateString()}р.";

                    string notes = (string.IsNullOrEmpty(assignee.IPN) ? "" : $"ІНН: {assignee.IPN}.")
                                   + $" Д/Н: {assignee.Birthday?.ToShortDateString()}. Трудовий договір №{assignee.NumberOfContract} від {assignee.DateOfContract?.ToShortDateString()}." +
                                   $" Призначено на посаду наказом №{assignee.OrderNumber} від {assignee.DateOfAppointment?.ToShortDateString()}.";


                    var assigneeParamList = new List<SqlParameter>
                    {
                        new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = DBNull.Value},
                        new SqlParameter("@p_BranchAppId", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(branchEntityOldLimsId.OldLimsId)
                        },
                        new SqlParameter("@p_PersonName", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.FIO.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_PersonPos", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(personPos.SubstringFromStart(250))
                        },
                        new SqlParameter("@p_Education", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(education.SubstringFromStart(1000))
                        },
                        new SqlParameter("@p_Phone", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Fax", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Email", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_Experience", SqlDbType.VarChar) {Value = DBNull.Value},
                        new SqlParameter("@p_ContractInfo", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(assignee.ContactInformation.SubstringFromStart(1000)),
                            IsNullable = true
                        }, //???
                        new SqlParameter("@p_Notes", SqlDbType.VarChar)
                        {
                            Value = CheckForDbNull(notes.SubstringFromStart(1000))
                        }
                    };

                    _limsRepository.PrlUpdateAssigneeIML(assigneeParamList);
                }
            }

            DataService.SaveChanges();

            //var imlMedicineList =
            //    DataService.GetEntity<ImlMedicine>(x => x.ApplicationId == appId).ToList();

            //var counterMedList = 1;
            //foreach (var imlMedicine in imlMedicineList)
            //{

            //    var appParamsList = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_DrugId", SqlDbType.Int) {Direction = ParameterDirection.Output},
            //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = CheckForDbNull(Convert.ToInt32(appEntity.OldLimsId))},
            //        new SqlParameter("@p_PosNumber", SqlDbType.Int) {Value = counterMedList++},
            //        new SqlParameter("@p_RpNumber", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.RegisterNumber)},
            //        new SqlParameter("@p_DrugName", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.MedicineName)},
            //        new SqlParameter("@p_FormtypeDesc", SqlDbType.VarChar){Value = CheckForDbNull(imlMedicine.FormName)},
            //        new SqlParameter("@p_ActiveDose", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.DoseInUnit)},
            //        new SqlParameter("@p_CountInPack", SqlDbType.VarChar) {Value = DBNull.Value},
            //        new SqlParameter("@p_DrugMnn", SqlDbType.VarChar) {Value = imlMedicine.MedicineNameEng},
            //        new SqlParameter("@p_AtcCode", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.AtcCode)},
            //        new SqlParameter("@p_ProducerName", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.ProducerName)
            //        },
            //        new SqlParameter("@p_ProducerCountry", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.ProducerCountry) //, IsNullable = true
            //        },
            //        new SqlParameter("@p_SupplierName", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.SupplierName)
            //        },
            //        new SqlParameter("@p_SupplierCountry", SqlDbType.VarChar)
            //        {
            //            Value = CheckForDbNull(imlMedicine.SupplierCountry)
            //        },
            //        new SqlParameter("@p_SupplierAddress", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.SupplierAddress)},
            //        new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = CheckForDbNull(imlMedicine.Notes)},
            //        new SqlParameter("@p_IsLicense", SqlDbType.Bit) {Value = 1},
            //        new SqlParameter("@p_IsProblem", SqlDbType.Bit) {Value = 0},
            //        new SqlParameter("@p_ProblemInfo", SqlDbType.VarChar) {Value = " "}
            //    };
            //    imlMedicine.OLdDRugId = await _limsRepository.InsertAppDrugIML(appParamsList);
            //}
            //DataService.SaveChanges();

        }

        public async void InsertGetLicenseApplicationTRL(Guid appId)
        {
            var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
            var userFullName = DataService.GetDto<UserDetailsDTO>(x => x.Id == userInfo.UserId).FirstOrDefault()?.FIO;

            var appDetailDto = DataService.GetDto<TrlAppDetailDTO>(application => application.Id == appId).Single();
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch => branchIds.Contains(branch.Id) && branch.RecordState != RecordState.D);
            var appEntity = DataService.GetEntity<TrlApplication>(app => app.Id == appId).Single();
            var appEntityPersonId = appEntity.PerformerId;
            var person = DataService.GetEntity<Person>(p => p.Id == appEntityPersonId);
            var performer = person.Select(p => p.FIO).SingleOrDefault();

            var entityEnRec = DataService.GetEntity<EntityEnumRecords>(x => x.EntityId == appEntity.Id).Select(x => x.EnumRecordCode).ToList();
            var enRec = DataService.GetEntity<EnumRecord>(x => entityEnRec.Contains(x.Code));

            string p_LictypeIds = "";
            foreach (var ids in enRec)
            {
                p_LictypeIds += ","+ ids.ExParam1;
            }
            if (!string.IsNullOrEmpty(p_LictypeIds))
            {
                p_LictypeIds = p_LictypeIds.Substring(1);
            }

            var appParams = new List<SqlParameter>
            {
                new SqlParameter("@p_LicAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@p_AppStatusId", SqlDbType.Int){Value =  DBNull.Value},
                new SqlParameter("@p_AppTypeId", SqlDbType.Int){Value = Convert.ToInt32(ResolveLimsAppType(appDetailDto.AppSort))},
                new SqlParameter("@p_AppReasonIds", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_AppReasonId", SqlDbType.Int){Value = (object)ResolveLimsAppReason(appDetailDto.AppSort) ?? DBNull.Value},
         
                new SqlParameter("@p_SgdAppNum", SqlDbType.VarChar){Value = DBNull.Value, IsNullable = true}, //not used
                new SqlParameter("@p_SgdAppDate", SqlDbType.Date) {Value = DBNull.Value, IsNullable = true}, //today
                new SqlParameter("@p_RegAppNum", SqlDbType.VarChar){Value = appDetailDto.RegNumber},
                new SqlParameter("@p_RegAppDate", SqlDbType.Date){Value = appDetailDto.RegDate ?? DateTime.Now},
                //new SqlParameter("@p_PersonId", SqlDbType.Int) {Value = userEntity ?? 461}, //?? dobavit
                new SqlParameter("@p_IsFree", SqlDbType.Int) {Value = Convert.ToInt32(0)}, //default 0
                new SqlParameter("@p_Performer", SqlDbType.VarChar)
                {
                    //Value = "Тадеєва Юлія Петрівна", IsNullable = true
                    Value = CheckForDbNull(performer)
                }, //null
                new SqlParameter("@p_Notes", SqlDbType.VarChar) {Value = DBNull.Value, IsNullable = true},
                new SqlParameter("@p_BranchCount", SqlDbType.Int) {Value = DBNull.Value/*branchIds.Count*/, IsNullable = true}, //?? x3
                new SqlParameter("@p_IsActsReceived", SqlDbType.Bit) {Value = DBNull.Value},
                new SqlParameter("@p_LicDocId", SqlDbType.Int) {Value = DBNull.Value},
                new SqlParameter("@p_LictypeIds", SqlDbType.VarChar)
                {
                    Value = CheckForDbNull(p_LictypeIds)
                },
                new SqlParameter("@p_SideEdrpou", SqlDbType.VarChar)
                {
                    //Value =  DBNull.Value
                    Value = string.IsNullOrEmpty(appDetailDto.EDRPOU) ? appDetailDto.INN : appDetailDto.EDRPOU
                },
                new SqlParameter("@p_OrgformId", SqlDbType.Int) {Value = (int)appDetailDto.OrgType},
                new SqlParameter("@p_SideName", SqlDbType.VarChar) {Value = appDetailDto.OrgName},
               
                new SqlParameter("@p_SideAddress", SqlDbType.VarChar) {Value = appDetailDto.Address, IsNullable = true},
                new SqlParameter("@p_SideIndex", SqlDbType.VarChar) {Value = appDetailDto.PostIndex, IsNullable = true},
                new SqlParameter("@p_SideDirectorPib", SqlDbType.VarChar) {Value = appDetailDto.OrgDirector},
                new SqlParameter("@p_SideContacts", SqlDbType.VarChar)
                {
                    Value =
                        $"{appDetailDto.PhoneNumber.Replace(" ", string.Empty)}, {appDetailDto.FaxNumber.Replace(" ", string.Empty)}"
                },

                new SqlParameter("@p_BankAccount", SqlDbType.VarChar)
                {
                    Value =
                        $"Рахунок в нац. валюті №{appDetailDto.NationalAccount}, реквізити банку: {appDetailDto.NationalBankRequisites}{Environment.NewLine}" +
                        $"Рахунок в іноземній валюті №{appDetailDto.InternationalAccount}, реквізити банку: {appDetailDto.InternationalBankRequisites}"
                },
                new SqlParameter("@p_Passport", SqlDbType.VarChar) { Value = DBNull.Value },
                new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)}, //1 for Ukraine
                new SqlParameter("@p_OpfgTypeId", SqlDbType.Int) { Value = Convert.ToInt32(appDetailDto.LegalFormType) },
                new SqlParameter("@p_OwnershipTypeId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(appDetailDto.OwnershipType)
                },
                new SqlParameter("@p_KvedId", SqlDbType.Int) {Value = Convert.ToInt32(3)}, //default value 24.42.0
                new SqlParameter("@p_SpoduId", SqlDbType.Int)
                {
                    Value = Convert.ToInt32(1208)
                }, //default value "відомча підпорядкованість відсутня"
                new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = userFullName ?? "Не знайдено"},
                new SqlParameter("@p_CreateDate", SqlDbType.Date) {Value = DateTime.Now.Date}
            };


            var insertAppTask = _limsRepository.InsertTrlApplication(appParams); //todo 
            var limsAppId = await insertAppTask;   
            appEntity.OldLimsId = limsAppId;

            int counter = 1;

           


            foreach (var branchEntity in branchEntityList)
            {
                var branchAddressDto = DataService.GetDto<AtuSubjectAddressDTO>(dto => dto.Id == branchEntity.AddressId)
                    .FirstOrDefault();

                var acepticCondition = DataService.GetEntity<EnumRecord>(x => x.Code == branchEntity.AsepticConditions)
                    .Select(x => x.ExParam1).FirstOrDefault();

                var branchType = DataService.GetEntity<EnumRecord>(x => x.Code == branchEntity.BranchType
                     && x.EnumType == "BranchType").SingleOrDefault();

                var branchTypeIdsBranch = DataService.GetDto<EntityEnumDTO>(x => x.BranchId == branchEntity.Id && x.EntityType == "BranchApplication")
                    .Select(x => x.ExParam1).Distinct().ToList(); //перелік видів робіт в МПД

                string p_branchTypeIdsBranch = "";
                foreach (var ids in branchTypeIdsBranch)
                {
                    p_branchTypeIdsBranch += "," + ids;
                }
                if (!string.IsNullOrEmpty(p_branchTypeIdsBranch))
                {
                    p_branchTypeIdsBranch = p_branchTypeIdsBranch.Substring(1);
                }

                int residenceTypeId;

                var coatuCode = branchAddressDto.Code.ToString().Substring(0, 5);
                var paramListCoutu = new List<SqlParameter>
                {
                    new SqlParameter("@p_RegionId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_RegionCode", SqlDbType.Int) {Value = coatuCode}
                };

                var pRegionId = await _limsRepository.GetId(paramListCoutu);

                //if (string.IsNullOrEmpty(pRegionId.ToString()))
                //{
                //    return 1;
                //}

                #region residence type

                    switch (branchAddressDto.CityEnum)
                {
                    case "TownsOfDistrictSubordination":
                    case "CitiesOfRegionalSubordination":
                        residenceTypeId = 1;
                        break;
                    case "UrbanTypeVillages":
                        residenceTypeId = 4;
                        break;
                    case "Hamlet":
                    case "Village":
                        residenceTypeId = 2;
                        break;
                    default:
                        residenceTypeId = 3;
                        break;
                }

                #endregion

                var branchParamList = new List<SqlParameter>
                {
                    new SqlParameter("@p_BranchAppId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                    new SqlParameter("@p_BranchLicId", SqlDbType.Int) {Value = DBNull.Value},
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(limsAppId)},
                    new SqlParameter("@p_BranchNumber", SqlDbType.Int) {Value = Convert.ToInt32(counter++)},
                    new SqlParameter("@p_BranchTypeId", SqlDbType.Int) {Value = CheckForDbNull(branchType.ExParam1)}, 
                    new SqlParameter("@p_BranchName", SqlDbType.VarChar) {Value = branchEntity.Name}, //branchName
                    new SqlParameter("@p_RegionId", SqlDbType.Int){Value = Convert.ToInt32(pRegionId)}, //default value 1 for 'Ukraine'
                    new SqlParameter("@p_ResidenceTypeId", SqlDbType.Int){Value = residenceTypeId}, //parse branch address LocalityType, compare to Residence_Type
                    new SqlParameter("@p_BranchAddress", SqlDbType.VarChar){Value = branchAddressDto.Address}, //parse branch address
                    new SqlParameter("@p_BranchTypeIds", SqlDbType.VarChar) {Value = CheckForDbNull(p_branchTypeIdsBranch)},
                    new SqlParameter("@p_AseptId", SqlDbType.Int) {Value = CheckForDbNull(acepticCondition)},
                    new SqlParameter("@p_BranchAddressIdx", SqlDbType.VarChar){Value = branchAddressDto.PostIndex},
                    new SqlParameter("@p_Phone", SqlDbType.VarChar){Value = branchEntity.PhoneNumber.Replace(" ", string.Empty)},
                    new SqlParameter("@p_SpecialConditions", SqlDbType.VarChar) {Value = CheckForDbNull(branchEntity.SpecialConditions)},
                    new SqlParameter("@p_Remarks", SqlDbType.VarChar) {Value = DBNull.Value},
                    new SqlParameter("@p_ToCheck", SqlDbType.Bit) {Value = DBNull.Value}
                };

                branchEntity.OldLimsId = await _limsRepository.InsertTrlBranch(branchParamList);

               

                //if (branchEntity.CreateTds)
                //{
                //    var paramLis = new List<SqlParameter>
                //    {
                //        new SqlParameter("@p_CheckId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(limsAppId)},
                //        new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)},
                //        new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
                //        new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = " "},
                //        new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
                //    };
                //    await _limsRepository.TrlCheckCreate(paramLis);
                //}
            }

            //var branchEntityListDls = branchEntityList.Where(x => x.CreateDls == true).ToList();
            //string branchEntityListDlsToUpdate = "";
            //foreach (var ids in branchEntityListDls)
            //{
            //    branchEntityListDlsToUpdate += "," + ids.OldLimsId;
            //}
            //if (!string.IsNullOrEmpty(branchEntityListDlsToUpdate))
            //{
            //    branchEntityListDlsToUpdate = branchEntityListDlsToUpdate.Substring(1);
            //}

            //if (branchEntityListDls.Any())
            //{
            //    var paramLis = new List<SqlParameter>
            //    {
            //        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(limsAppId)},
            //        new SqlParameter("@p_CheckId", SqlDbType.Int) {Value = 0},
            //        new SqlParameter("@p_CheckDivIds", SqlDbType.VarChar) {Value = branchEntityListDlsToUpdate},
            //        new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
            //        new SqlParameter("@p_TermDate", SqlDbType.DateTime) {Value = DateTime.Now.AddDays(6)},
            //        new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = ""},
            //        new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
            //    };
            //    await _limsRepository.TrlCheckDivAdd(paramLis);
            //}
            //await InsertAttach(appId, limsAppId);

            DataService.SaveChanges();

            //DataService.SaveChanges();
        }

        public async Task CreateTdsAsync(Guid appId)
        {
            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch =>
                branchIds.Contains(branch.Id) && branch.RecordState != RecordState.D);
            var appEntity = DataService.GetEntity<TrlApplication>(app => app.Id == appId).Single();
            foreach (var branchEntity in branchEntityList)
            {
                if (branchEntity.CreateTds)
                {
                    var paramLis = new List<SqlParameter>
                    {
                        new SqlParameter("@p_CheckId", SqlDbType.Int) {Direction = ParameterDirection.Output},
                        new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                        new SqlParameter("@p_RegionId", SqlDbType.Int) {Value = Convert.ToInt32(1)},
                        new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
                        new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = " "},
                        new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
                    };
                    await _limsRepository.TrlCheckCreate(paramLis);
                }
            }

            DataService.SaveChanges();
        }

        public async Task CreateDlsAsync(List<Guid> branchList)
        {
            var idBranch = branchList.FirstOrDefault();
            var branchId = DataService.GetEntity<Branch>(x => x.Id == idBranch).FirstOrDefault();
            var appId = DataService.GetEntity<ApplicationBranch>(x => x.BranchId == branchId.Id).Select(x=>x.LimsDocumentId).FirstOrDefault();

            var applicationBranches = DataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == appId);
            var branchIds = applicationBranches.Select(x => x.BranchId).ToList();
            var branchEntityList = DataService.GetEntity<Branch>(branch =>
                branchIds.Contains(branch.Id) && branch.RecordState != RecordState.D);

            var appEntity = DataService.GetEntity<TrlApplication>(app => app.Id == appId).Single();
            var branchEntityListDls = branchEntityList.Where(x => x.CreateDls == true).ToList();

            string branchEntityListDlsToUpdate = "";
            foreach (var ids in branchEntityListDls)
            {
                branchEntityListDlsToUpdate += "," + ids.OldLimsId;
            }
            if (!string.IsNullOrEmpty(branchEntityListDlsToUpdate))
            {
                branchEntityListDlsToUpdate = branchEntityListDlsToUpdate.Substring(1);
            }

            if (branchEntityListDls.Any())
            {
                var paramLis = new List<SqlParameter>
                {
                    new SqlParameter("@p_AppId", SqlDbType.Int) {Value = Convert.ToInt32(appEntity.OldLimsId)},
                    new SqlParameter("@p_CheckId", SqlDbType.Int) {Value = 0},
                    new SqlParameter("@p_CheckDivIds", SqlDbType.VarChar) {Value = branchEntityListDlsToUpdate},
                    new SqlParameter("@p_TaskDate", SqlDbType.DateTime) {Value = DateTime.Now},
                    new SqlParameter("@p_TermDate", SqlDbType.DateTime) {Value = DateTime.Now.AddDays(6)},
                    new SqlParameter("@p_CreatorName", SqlDbType.VarChar) {Value = ""},
                    new SqlParameter("@p_CreateDate", SqlDbType.DateTime) {Value = DateTime.Now}
                };
            await _limsRepository.TrlCheckDivAdd(paramLis);
            }

            DataService.SaveChanges();
        }
    }


    internal static class Extenstion
    {
        public static string SubstringFromStart(this string str, int size)
        {
            return string.IsNullOrEmpty(str) ? str : str.Substring(0, Math.Min(str.Length, size));
        }
    }
}
