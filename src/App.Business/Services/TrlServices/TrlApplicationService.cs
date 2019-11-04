using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Extensions;
using App.Business.Helpers;
using App.Business.Services.AppServices;
using App.Business.Services.LimsService;
using App.Business.Services.RptServices;
using App.Business.ViewModels;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Enums;
using App.Core.Data.Helpers;
using App.Data.DTO.ATU;
using App.Data.DTO.Common;
using App.Data.DTO.TRL;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.TRL;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace App.Business.Services.TrlServices
{
    public class TrlApplicationService
    {
        public ICommonDataService DataService { get; }
        private readonly IObjectMapper _objectMapper;
        private readonly IUserInfoService _userInfoService;
        private readonly LimsExchangeService _limsExchangeService;
        private readonly ApplicationService<TrlApplication> _applicationService;
        private readonly IConverter _converter;
        private TrlReportService _trlReportService;

        public TrlApplicationService(ICommonDataService dataService, IObjectMapper objectMapper, IUserInfoService userInfoService, ApplicationService<TrlApplication> applicationService, LimsExchangeService limsExchangeService, IConverter converter, TrlReportService trlReportService)
        {
            DataService = dataService;
            _objectMapper = objectMapper;
            _userInfoService = userInfoService;
            _applicationService = applicationService;
            _limsExchangeService = limsExchangeService;
            _converter = converter;
            _trlReportService = trlReportService;
        }

        public async Task<TrlAppDetailDTO> GetEditPortal(Guid? id, IDictionary<string, string> paramList)
        {
            TrlAppDetailDTO model;
            if (id == null || id == Guid.Empty)
            {
                model = new TrlAppDetailDTO();
                var userInfo = await _userInfoService.GetCurrentUserInfoAsync();

                model.AppSort = "GetLicenseApplication";
                if (!string.IsNullOrEmpty(userInfo?.EDRPOU()))
                {
                    model.EDRPOU = userInfo?.EDRPOU();
                    model.OrgName = userInfo?.OrganizationName();
                }
                else
                {
                    model.INN = userInfo?.INN();
                    model.OrgName = userInfo?.FullName();
                }
            }
            else
            {
                model = (await DataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            }

            return model;
        }

        public async Task<TrlAppDetailDTO> GetEditPortal(Guid? id, string sort = "GetLicenseApplication")
        {
            TrlAppDetailDTO model;
            if (id == null || id == Guid.Empty)
            {
                model = new TrlAppDetailDTO();
                var userInfo = await _userInfoService.GetCurrentUserInfoAsync();
                model.AppSort = sort;
                if (!string.IsNullOrEmpty(userInfo?.EDRPOU()))
                {
                    model.EDRPOU = userInfo?.EDRPOU();
                    model.OrgName = userInfo?.OrganizationName();
                }
                else
                {
                    model.INN = userInfo?.INN();
                    model.OrgName = userInfo?.FullName();
                }
            }
            else
            {
                model = (await DataService.GetDtoAsync<TrlAppDetailDTO>(x => x.Id == id)).FirstOrDefault();
            }

            return model;
        }

        public async Task<Guid> SaveApplication(TrlAppDetailDTO editModel, bool isBackOffice = false)
        {
            Guid appId;
            var userInfo = await _userInfoService?.GetCurrentUserInfoAsync();
            var isNew = false;
            if (editModel.Id == Guid.Empty)
            {
                isNew = true;
                var model = new TrlAppDetailDTO();
                _objectMapper.Map(editModel, model);
                if (model.OrgUnitId == Guid.Empty)
                    model.OrgUnitId = new Guid(userInfo.OrganizationId());
                if (isBackOffice)
                {
                    model.IsCreatedOnPortal = false;
                    model.BackOfficeAppState = "Project";
                }
                else
                {
                    model.AppState = "Project";
                    model.IsCreatedOnPortal = true;
                }
                model.AppType = "TRL";

                var organizationInfo = new OrganizationInfo();
                organizationInfo.OrganizationId = model.OrgUnitId == Guid.Empty ? new Guid(userInfo?.OrganizationId()) : model.OrgUnitId;
                organizationInfo.Type = "TRL";
                if (string.IsNullOrEmpty(model.EDRPOU))
                {
                    if (isBackOffice == false)
                        organizationInfo.OrgDirector = userInfo.FullName();
                }
                else
                {
                    organizationInfo.OrgDirector = model.OrgDirector;
                }

                organizationInfo.Name = model.OrgName;
                organizationInfo.LegalFormType = model.LegalFormType;
                organizationInfo.OwnershipType = model.OwnershipType;
                organizationInfo.PhoneNumber = model.PhoneNumber;
                organizationInfo.FaxNumber = model.FaxNumber;
                organizationInfo.NationalAccount = model.NationalAccount;
                organizationInfo.InternationalAccount = model.InternationalAccount;
                organizationInfo.NationalBankRequisites = model.NationalBankRequisites;
                organizationInfo.InternationalBankRequisites = model.InternationalBankRequisites;
                organizationInfo.PassportSerial = model.PassportSerial;
                organizationInfo.PassportNumber = model.PassportNumber;
                organizationInfo.PassportDate = model.PassportDate;
                organizationInfo.PassportIssueUnit = model.PassportIssueUnit;
                organizationInfo.AddressId = model.AddressId;
                organizationInfo.EconomicClassificationType = model.EconomicClassificationType;

                //var license = _prlLicenseService.GetLicenseGuid(model.OrgUnitId);
                //if (license == null)
                //{
                //    if (editModel.AppSort == "AdditionalInfoToLicense")
                //        organizationInfo.IsPendingLicenseUpdate = true;
                //    organizationInfo.IsActualInfo = true;
                //    DataService.GetEntity<OrganizationInfo>(x =>
                //            x.IsActualInfo && x.OrganizationId == model.OrgUnitId && x.Type == "PRL").ToList()
                //        .ForEach(x => x.IsActualInfo = false);
                //}

                DataService.Add(organizationInfo);
                DataService.SaveChanges();
                model.OrganizationInfoId = organizationInfo.Id;

                var organization = DataService.GetEntity<OrganizationExt>()
                    .SingleOrDefault(x => x.Id == model.OrgUnitId);
                if (string.IsNullOrEmpty(model.EDRPOU))
                {
                    organization.INN = model.INN;
                }

                organization.EMail = model.EMail;
                appId = DataService.Add<TrlApplication>(model);

                await DataService.SaveChangesAsync();

                var enumRecordsId = model.ListOfTrlActivityType.ToList();
                var enumRecordList = DataService.GetEntity<EnumRecord>(x => enumRecordsId.Contains(x.Id)).ToList();
                enumRecordList?.ForEach(x => DataService.Add(new EntityEnumRecords()
                {
                    EntityId = appId,
                    EntityType = "LicenseApplication",
                    EnumRecordType = x.EnumType,
                    EnumRecordCode = x.Code
                }));

                await DataService.SaveChangesAsync();
            }
            else
            {
                if (isBackOffice)
                {
                    editModel.IsCreatedOnPortal = false;
                    editModel.BackOfficeAppState = "Project";
                }
                else
                {
                    editModel.AppState = "Project";
                    editModel.IsCreatedOnPortal = true;
                }
                var organizationInfo = DataService.GetEntity<OrganizationInfo>()
                    .SingleOrDefault(x => x.Id == editModel.OrganizationInfoId);

                organizationInfo.LegalFormType = editModel.LegalFormType;
                organizationInfo.OwnershipType = editModel.OwnershipType;
                organizationInfo.PhoneNumber = editModel.PhoneNumber;
                organizationInfo.FaxNumber = editModel.FaxNumber;
                organizationInfo.NationalAccount = editModel.NationalAccount;
                organizationInfo.InternationalAccount = editModel.InternationalAccount;
                organizationInfo.NationalBankRequisites = editModel.NationalBankRequisites;
                organizationInfo.InternationalBankRequisites = editModel.InternationalBankRequisites;
                organizationInfo.PassportSerial = editModel.PassportSerial;
                organizationInfo.PassportNumber = editModel.PassportNumber;
                organizationInfo.PassportDate = editModel.PassportDate;
                organizationInfo.PassportIssueUnit = editModel.PassportIssueUnit;
                organizationInfo.OrgDirector = editModel.OrgDirector;
                organizationInfo.AddressId = editModel.AddressId;
                organizationInfo.EconomicClassificationType = editModel.EconomicClassificationType;


                var organization = DataService.GetEntity<OrganizationExt>()
                    .SingleOrDefault(x => x.Id == editModel.OrgUnitId);
                organization.EMail = editModel.EMail;
                appId = DataService.Add<TrlApplication>(editModel);


                var entityEnumRecords =
                    DataService.GetEntity<EntityEnumRecords>().Where(x => x.EntityId == editModel.Id).ToList();
                if (entityEnumRecords.Count > 0)
                {
                    entityEnumRecords.ForEach(x => DataService.Remove(x));
                }

                var enumRecordsId = editModel.ListOfTrlActivityType.ToList();
                var enumRecordList = DataService.GetEntity<EnumRecord>(x => enumRecordsId.Contains(x.Id)).ToList();
                enumRecordList?.ForEach(x => DataService.Add(new EntityEnumRecords()
                {
                    EntityId = editModel.Id,
                    EntityType = "LicenseApplication",
                    EnumRecordType = x.EnumType,
                    EnumRecordCode = x.Code
                }));

                await DataService.SaveChangesAsync();
            }

            //if (editModel.AppSort == "AdditionalInfoToLicense" && isNew)
            //{
            //    var license = (await _limsExchangeService
            //        .GetLicenses("PRL", string.IsNullOrEmpty(editModel.EDRPOU) ? editModel.INN : editModel.EDRPOU)).FirstOrDefault();
            //    if (license == null) throw new Exception();
            //    foreach (var branchLicense in license.Branches)
            //    {
            //        var branch = new Branch
            //        {
            //            Id = Guid.NewGuid(),
            //            OrganizationId = editModel.OrgUnitId == Guid.Empty
            //                ? new Guid(userInfo?.OrganizationId())
            //                : editModel.OrgUnitId,
            //            OldLimsId = branchLicense.Id,
            //            Name = branchLicense.Name
            //        };
            //        var branchId = DataService.Add(branch, false);
            //        var appBranch = new ApplicationBranch()
            //        {
            //            LimsDocumentId = appId,
            //            BranchId = branchId
            //        };
            //        DataService.Add(appBranch, false);
            //        await DataService.SaveChangesAsync();
            //    }
            //}

            return appId;
        }

        public async Task SubmitAdditionalInfoToLicense(Guid appId, string text)
        {
            var application = DataService.GetEntity<TrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null) throw new Exception();
            var organization = DataService.GetEntity<OrganizationExt>(x => x.Id == application.OrgUnitId).FirstOrDefault();
            if (organization == null) throw new Exception();
            var orgInfo = DataService.GetEntity<OrganizationInfo>(x => x.Id == application.OrganizationInfoId)
                .FirstOrDefault();
            if (orgInfo == null) throw new Exception();
            var limsLicense = (await _limsExchangeService.GetLicenses("Trl",
                string.IsNullOrEmpty(organization.EDRPOU) ?
                    organization.INN : organization.EDRPOU)).FirstOrDefault();
            var newLic = new TrlLicense
            {
                OldLimsId = limsLicense.Id,
                OrgUnitId = application.OrgUnitId,
                ParentId = application.Id,
                LicType = "Trl",
                LicState = "Active",
                IsRelevant = true,
                LicenseDate = DateTime.Parse(limsLicense.RegistrationDate),
                OrderDate = limsLicense.OrderDate,
                OrderNumber = limsLicense.OrderNumber
            };
            DataService.Add(newLic);

            application.AppState = "Reviewed";
            application.BackOfficeAppState = "Reviewed";

            orgInfo.IsActualInfo = true;
            orgInfo.IsPendingLicenseUpdate = false;

            var decision = new AppDecision { Id = Guid.NewGuid(), AppId = appId, DecisionType = "Accepted", DecisionDescription = text };
            application.AppDecisionId = decision.Id;
            DataService.Add(decision);

            await DataService.SaveChangesAsync();
        }

        public async Task SubmitBackOfficeApplication(IConfiguration config, Guid appId)
        {
            var application = DataService.GetEntity<TrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
            {
                throw new Exception();
            }
            byte[] file;

            try
            {
                // TODO: Сделать
                file = await GetApplicationFile(appId, application.AppSort);
            }
            catch (Exception e)
            {
                throw e;
            }
            // TODO: Сделать
            //var fileStore = new FileStoreDTO
            //{
            //    FileType = FileType.Pdf,
            //    OrigFileName = "PDF заяви.pdf",
            //    FileSize = file.Length,
            //    EntityId = appId,
            //    EntityName = nameof(PrlApplication),
            //    ContentType = ".pdf",
            //    Description = "PDF заяви"
            //};
            //await SaveFile(config, new FilesViewModel() { name = "PDF заяви", file = Convert.ToBase64String(file) }, fileStore);
            application.AppState = "Submitted";
            application.BackOfficeAppState = "Submitted";
            await DataService.SaveChangesAsync();
        }

        public async Task<byte[]> GetApplicationFile(Guid appId, string sort)
        {
            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] file;
            switch (sort)
            {
                case "GetLicenseApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_торгівлі_лікарськими_засобами_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _trlReportService.TrlCreateLicenseApp(appId));
                    break;
                case "CancelLicenseApplication":
                    file = await rep.CreatePDF($"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _trlReportService.TrlCancelLicenseApp(appId));
                    break;
                case "RemBranchApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf",
                        await _trlReportService.TrlRemBranchApp(appId));
                    break;
                case "ChangeAutPersonApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _trlReportService.TrlChangeAutPersonApp(appId));
                    break;
                case "AddBranchApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf",
                        await _trlReportService.TrlAddBranchApp(appId));
                    break;
                default:
                    return null;
            }

            return file;
        }


        public async Task BackCreateApplication(TrlAppDetailDTO model, Guid? orgId, string sort)
        {
            if (orgId == null)
                throw new Exception();

            var organizationInfo = DataService.GetEntity<OrganizationInfo>(x => x.IsActualInfo && x.OrganizationId == orgId)
                .FirstOrDefault();

            _objectMapper.Map(organizationInfo, model);
            var address = DataService.GetDto<AtuSubjectAddressDTO>(x => x.Id == model.AddressId).FirstOrDefault();

            if (address != null)
                _objectMapper.Map(address, model);

            model.Id = Guid.Empty;
            model.AppSort = sort;
            var org = DataService.GetEntity<OrganizationExt>(x => x.Id == orgId).FirstOrDefault();

            if (org == null)
                throw new Exception();

            if (!string.IsNullOrEmpty(org.EDRPOU))
                model.EDRPOU = org.EDRPOU;
            else
                model.INN = org.INN;

            model.OrgUnitId = orgId.Value;

            // Тут работал Макс
            // model.EMail = org.EMail;
        }

        public async Task TrlActivityTypeList(TrlAppDetailDTO model)
        {
            if (model != null)
            {
                //var entityEnRec = DataService.GetEntity<EntityEnumRecords>(x => x.EntityId == model.Id).Select(x => x.EnumRecordCode).ToList();
                //var enRecordListModel = DataService.GetEntity<EnumRecord>(x => entityEnRec.Contains(x.Code)).ToList();

                var enRecordListModel = DataService.GetDto<EntityEnumDTO>(x => x.ApplicationId == model.Id).ToList();

                model.ListOfTrlActivityType = enRecordListModel.Select(x => x.EnumRecordId).ToList();
            }
            else
                throw new Exception();
        }

        public async Task<FilesSignViewModel> GetFilesForSign(Guid id)
        {
            var appSort = DataService.GetEntity<TrlApplication>(x => x.Id == id).Select(x => x.AppSort).FirstOrDefault();
            var fileModel = new FilesSignViewModel() { id = id, files = new List<FilesViewModel>() };
            fileModel.files.Add(await _applicationService.GetEDocumentJsonFile(id));
            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] applicationFile;
            string filename = "";

            try
            {   //TODO check on sort(other pdf html sorts) TODOED!
                switch (appSort)
                {
                    case "GetLicenseApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_торгівлі_лікарськими_засобами_{DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlCreateLicenseApp(id));
                        filename = $"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_торгівлі_лікарськими_засобами_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "IncreaseToTRLApplication":
                        applicationFile = await rep.CreatePDF($"Заява_про_розширення_провадження_виду_господарської_діяльності_(Розширення_до_торгівлі_лікарськими_засобами{DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlCreateLicenseApp(id));
                        filename = $"Заява_про_розширення_провадження_виду_господарської_діяльності_(Розширення_до_торгівлі_лікарськими_засобами){DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "CancelLicenseApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlCancelLicenseApp(id));
                        filename = $"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "DecreaseTRLApplication":
                        applicationFile = await rep.CreatePDF($"Заява_про_звуження_провадження_виду_господарської_діяльності_-_Звуження_торгівлі_лікарських_засобів{DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlCancelLicenseApp(id));
                        filename = $"Заява_про_звуження_провадження_виду_господарської_діяльності_-_Звуження_торгівлі_лікарських_засобів{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "RemBranchApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlRemBranchApp(id));
                        filename = $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "ChangeAutPersonApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlChangeAutPersonApp(id));
                        filename = $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "AddBranchApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf", await _trlReportService.TrlChangeAutPersonApp(id));
                        filename = $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    default:
                        applicationFile = await rep.CreatePDF("applicationPDF.pdf", "<h1>Hi</h1>");
                        filename = "applicationPDF.pdf";
                        break;
                }

                fileModel.files.Add(new FilesViewModel()
                {
                    id = Guid.Empty,
                    idFileStore = Guid.Empty,
                    name = filename,
                    file = Convert.ToBase64String(applicationFile)
                });
            }
            catch (Exception e)
            {
                Log.Error("Maksim error");
                throw e;
            }
            return fileModel;
        }

        public async Task SubmitApplication(IConfiguration config, FilesSignViewModel model)
        {
            try
            {
                foreach (var file in model.files)
                {
                    var fileStore = new FileStoreDTO
                    {
                        FileType = FileType.Unknown,
                        OrigFileName = file.name + ".p7s",
                        FileSize = FileSignHelper.GetOriginalLengthInBytes(file.file),
                        Ock = file.isSystemFile
                    };
                    if (file.id != Guid.Empty)
                    {
                        fileStore.EntityId = file.idFileStore;
                        fileStore.EntityName = nameof(FileStore);
                        fileStore.ContentType = ".p7s";
                        fileStore.Description = "Підписаний PDF заяви";
                        await FileSignHelper.SaveFile(config, file, fileStore, DataService);
                    }
                    else
                    {
                        fileStore.EntityId = model.id;
                        fileStore.EntityName = nameof(TrlApplication);
                        fileStore.ContentType = ".p7s";
                        fileStore.Description = "Підписаний PDF заяви";
                        await FileSignHelper.SaveFile(config, file, fileStore, DataService);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
            var app = DataService.GetEntity<TrlApplication>(x => x.Id == model.id)?.FirstOrDefault();
            app.AppState = "Submitted";
            app.BackOfficeAppState = "Submitted";
            app.IsCreatedOnPortal = true;
            await DataService.SaveChangesAsync();
        }
    }
}
