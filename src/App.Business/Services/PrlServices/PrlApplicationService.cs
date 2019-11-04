using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using App.Data.DTO.PRL;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace App.Business.Services.PrlServices
{
    public class PrlApplicationService: IPrlApplicationService
    {
        public ICommonDataService DataService { get; }
        private readonly IUserInfoService _userInfoService;
        private readonly IPrlReportService _prlReportService;
        private readonly IObjectMapper _objectMapper;
        private readonly IConverter _converter;
        private readonly IPrlLicenseService _prlLicenseService;
        private readonly LimsExchangeService _limsExchangeService;
        private readonly ApplicationService<PrlApplication> _applicationService;

        public PrlApplicationService(ICommonDataService dataService, IUserInfoService userInfoService,
            IPrlReportService prlReportService, IObjectMapper objectMapper, IConverter converter,
            IPrlLicenseService prlLicenseService, PrlApplicationProcessService applicationProcessService, LimsExchangeService limsExchangeService, ApplicationService<PrlApplication> applicationService)
        {
            DataService = dataService;
            _userInfoService = userInfoService;
            _objectMapper = objectMapper;
            _prlReportService = prlReportService;
            _converter = converter;
            _prlLicenseService = prlLicenseService;
            _limsExchangeService = limsExchangeService;
            _applicationService = applicationService;
        }

        public async Task<Guid> SaveApplication(PrlAppDetailDTO editModel, bool isBackOffice)
        {
            Guid appId;
            var userInfo = await _userInfoService?.GetCurrentUserInfoAsync();
            var isNew = false;
            if (editModel.Id == Guid.Empty)
            {
                isNew = true;
                var model = new PrlAppDetailDTO();
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
                model.AppType = "PRL";

                var organizationInfo = new OrganizationInfo();
                organizationInfo.OrganizationId = model.OrgUnitId == Guid.Empty ? new Guid(userInfo?.OrganizationId()) : model.OrgUnitId;
                organizationInfo.Type = "PRL";
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
                organizationInfo.EMail = model.EMail;
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


                var license = _prlLicenseService.GetLicenseGuid(model.OrgUnitId);
                if (license == null)
                {
                    if (editModel.AppSort == "AdditionalInfoToLicense")
                        organizationInfo.IsPendingLicenseUpdate = true;
                    organizationInfo.IsActualInfo = true;
                    DataService.GetEntity<OrganizationInfo>(x =>
                            x.IsActualInfo && x.OrganizationId == model.OrgUnitId && x.Type == "PRL").ToList()
                        .ForEach(x => x.IsActualInfo = false);
                }

                DataService.Add(organizationInfo);
                DataService.SaveChanges();
                model.OrganizationInfoId = organizationInfo.Id;

                var organization = DataService.GetEntity<OrganizationExt>()
                    .SingleOrDefault(x => x.Id == model.OrgUnitId);
                if (string.IsNullOrEmpty(model.EDRPOU))
                {
                    organization.INN = model.INN;
                }

                // Тут работал Макс
                // organization.EMail = model.EMail;
                appId = DataService.Add<PrlApplication>(model);

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
                organizationInfo.EMail = editModel.EMail;

                // Тут работал Макс
                // var organization = DataService.GetEntity<OrganizationExt>().SingleOrDefault(x => x.Id == editModel.OrgUnitId);
                // organization.EMail = editModel.EMail;

                appId = DataService.Add<PrlApplication>(editModel);
                await DataService.SaveChangesAsync();
            }

            if (editModel.AppSort == "AdditionalInfoToLicense" && isNew)
            {
                var license = (await _limsExchangeService
                    .GetLicenses("PRL", string.IsNullOrEmpty(editModel.EDRPOU) ? editModel.INN : editModel.EDRPOU)).FirstOrDefault();
                if (license == null) throw new Exception();
                foreach (var branchLicense in license.Branches)
                {
                    var branch = new Branch
                    {
                        Id = Guid.NewGuid(),
                        OrganizationId = editModel.OrgUnitId == Guid.Empty
                            ? new Guid(userInfo?.OrganizationId())
                            : editModel.OrgUnitId,
                        OldLimsId = branchLicense.Id,
                        Name = branchLicense.Name
                    };
                    var branchId = DataService.Add(branch, false);
                    var appBranch = new ApplicationBranch()
                    {
                        LimsDocumentId = appId,
                        BranchId = branchId
                    };
                    DataService.Add(appBranch, false);
                    await DataService.SaveChangesAsync();
                };
            }

            return appId;
        }

        public async Task<FilesSignViewModel> GetFilesForSign(Guid id)
        {
            var appSort = DataService.GetEntity<PrlApplication>(x => x.Id == id).Select(x => x.AppSort).FirstOrDefault();
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
                        applicationFile = await rep.CreatePDF($"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_виробництва_лікарських_засобів_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(id));
                        filename = $"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_виробництва_лікарських_засобів_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "AdditionalInfoToLicense":
                        applicationFile = await rep.CreatePDF($"Доповнення інформації по наявній ліцензії{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(id));
                        filename = $"Доповнення інформації по наявній ліцензії{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "IncreaseToPRLApplication":
                        applicationFile = await rep.CreatePDF($"Заява_про_розширення_провадження_виду_господарської_діяльності_(Розширення_до_виробництва_лікарських_засобів){DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(id));
                        filename = $"Доповнення інформації по наявній ліцензії{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "CancelLicenseApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCancelLicenseApp(id));
                        filename = $"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "DecreasePRLApplication":
                        applicationFile = await rep.CreatePDF($"Заява_про_звуження_провадження_виду_господарської_діяльності_-_Звуження_виробництва_лікарських_засобів{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCancelLicenseApp(id));
                        filename = $"Заява_про_звуження_провадження_виду_господарської_діяльності_-_Звуження_виробництва_лікарських_засобів{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "RemBranchApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRemBranchApp(id));
                        filename = $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "ChangeContrApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_контрактних_виробників_та_лабораторій)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlChangeContrApp(id));
                        filename = $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_контрактних_виробників_та_лабораторій)_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "ChangeAutPersonApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlChangeAutPersonApp(id));
                        filename = $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "AddBranchInfoApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(розширення_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlAddBranchInfoApp(id));
                        filename = $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(розширення_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "RemBranchInfoApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(звуження_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRemBranchInfoApp(id));
                        filename = $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(звуження_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    case "AddBranchApplication":
                        applicationFile = await rep.CreatePDF($"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlAddBranchApp(id));
                        filename = $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf";
                        break;
                    //case "RenewLicenseApplication":
                    //    applicationFile = await rep.CreatePDF($"(Додаток_14)_Заява_на_переоформлення_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlRenewLicenseApp(id));
                    //    filename = $"(Додаток_14)_Заява_на_переоформлення_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf";
                    //    break;
                    default:
                        applicationFile = null;
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
                        fileStore.EntityName = nameof(PrlApplication);
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
            var app = DataService.GetEntity<PrlApplication>(x => x.Id == model.id)?.FirstOrDefault();
            app.AppState = "Submitted";
            app.BackOfficeAppState = "Submitted";
            app.IsCreatedOnPortal = true;
            await DataService.SaveChangesAsync();
        }

        public async Task SubmitBackOfficeApplication(IConfiguration config, Guid appId)
        {
            var application = DataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null)
            {
                throw new Exception();
            }
            byte[] file;

            try
            {
                file = await GetApplicationFile(appId, application.AppSort);
            }
            catch (Exception e)
            {
                throw e;
            }
            var fileStore = new FileStoreDTO
            {
                FileType = FileType.Pdf,
                OrigFileName = "PDF заяви.pdf",
                FileSize = file.Length,
                EntityId = appId,
                EntityName = nameof(PrlApplication),
                ContentType = ".pdf",
                Description = "PDF заяви"
            };
            await FileSignHelper.SaveFile(config, new FilesViewModel() { name = "PDF заяви", file = Convert.ToBase64String(file) }, fileStore, DataService);
            application.AppState = "Submitted";
            application.BackOfficeAppState = "Submitted";
            await DataService.SaveChangesAsync();
        }

        public async Task SubmitAdditionalInfoToLicense(Guid appId, string text)
        {
            var application = DataService.GetEntity<PrlApplication>(x => x.Id == appId).FirstOrDefault();
            if (application == null) throw new Exception();
            var organization = DataService.GetEntity<OrganizationExt>(x => x.Id == application.OrgUnitId).FirstOrDefault();
            if (organization == null) throw new Exception();
            var orgInfo = DataService.GetEntity<OrganizationInfo>(x => x.Id == application.OrganizationInfoId)
                .FirstOrDefault();
            if (orgInfo == null) throw new Exception();
            var limsLicense = (await _limsExchangeService.GetLicenses("Prl",
                    string.IsNullOrEmpty(organization.EDRPOU) ?
                        organization.INN : organization.EDRPOU)).FirstOrDefault();
            var newLic = new PrlLicense
            {
                OldLimsId = limsLicense.Id,
                OrgUnitId = application.OrgUnitId,
                ParentId = application.Id,
                LicType = "PRL",
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

        public async Task BackCreateApplication(PrlAppDetailDTO model, Guid? orgId, string appSort)
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
            model.AppSort = appSort;
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

        public async Task<byte[]> GetApplicationFile(Guid appId, string sort)
        {
            var rep = new PdfFromHtmlOwnConverter(_converter);
            byte[] file;
            switch (sort)
            {
                case "GetLicenseApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_02)_Заява_про_отримання_ліцензії_на_провадження_діяльності_з_виробництва_лікарських_засобів_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlCreateLicenseApp(appId));
                    break;
                case "AdditionalInfoToLicense":
                    file = await rep.CreatePDF($"Доповнення інформації по наявній ліцензії{DateTime.Now.ToString("Hmmss")}.pdf", await _prlReportService.PrlCreateLicenseApp(appId));
                    break;
                case "CancelLicenseApplication":
                    file = await rep.CreatePDF($"(Додаток_22)_Заява_про_анулювання_ліцензії_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlCancelLicenseApp(appId));
                    break;
                case "RemBranchApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(закриття_МПД){DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlRemBranchApp(appId));
                    break;
                case "ChangeContrApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_контрактних_виробників_та_лабораторій)_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlChangeContrApp(appId));
                    break;
                case "ChangeAutPersonApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_18)_Заява_про_зміну_інформації_у_додатку_(зміна_уповноважених_осіб)_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlChangeAutPersonApp(appId));
                    break;
                case "AddBranchInfoApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(розширення_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlAddBranchInfoApp(appId));
                    break;
                case "RemBranchInfoApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_16)_Заява_про_внесення_змін_до_ЄДР_у_зв'язку_з_припиненням_діяльності_МПД_(звуження_операцій)_{DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlRemBranchInfoApp(appId));
                    break;
                case "AddBranchApplication":
                    file = await rep.CreatePDF(
                        $"(Додаток_12)_Заява_про_внесення_до_ЄДР_відомостей_про_МПД_(додавання_МПД){DateTime.Now.ToString("Hmmss")}.pdf",
                        await _prlReportService.PrlAddBranchApp(appId));
                    break;
                default:
                    throw new Exception();
            }

            return file;
        }
        
        public async Task<FilesSignViewModel> GetFilesForVerification(Guid appId)
        {
            var files = DataService.GetEntity<FileStore>(x => x.EntityId == appId && x.EntityName == "PrlApplication");
            if (!files.Any())
            {
                throw new Exception("Файли до заяви відсутні");
            }
            var systemFile = files.FirstOrDefault(x => x.Ock);
            if (systemFile == null)
            {
                throw new Exception("Файл підпису досьє відсутний");
            }

            var systemFileErrors = await VerificationEDocumentHex(systemFile.Id, appId);
            var fileModel = new FilesSignViewModel() { id = appId, files = new List<FilesViewModel>() };
            var streamSystemFile = await GetFile(systemFile.Id);
            fileModel.files.Add(new FilesViewModel
            {
                id = Guid.Empty,
                idFileStore = systemFile.Id,
                names = systemFileErrors.names,
                file = Convert.ToBase64String(streamSystemFile.ToArray()),
                isSystemFile = true,
                errors = systemFileErrors.errors
            });
            files = files.Where(x => x.Id != systemFile.Id);
            if (!files.Any())
            {
                throw new Exception("Файл підпису заяви відсутний");
            }


            fileModel.files.AddRange(files.Select(x =>
           {
               var fileStream = GetFile(x.Id).Result;
               return new FilesViewModel()
               {
                   id = Guid.Empty,
                   idFileStore = x.Id,
                   names = new List<string>() { x.OrigFileName },
                   file = Convert.ToBase64String(fileStream.ToArray()),
                   isSystemFile = false
               };
           }));
            return fileModel;
        }

        private async Task<MemoryStream> GetFile(Guid fileId)
        {
            var dto = (await DataService.GetDtoAsync<FileStoreDTO>(x => x.Id == fileId)).FirstOrDefault();
            // С новым кором переделать FileStoreDTO
            var dtoCore = new Core.Data.DTO.Common.FileStoreDTO();
            _objectMapper.Map(dto, dtoCore);
            if (FileStoreHelper.LoadFile(dtoCore, out var stream, out var contentType))
            {
                return stream;
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task<(Dictionary<string, string> errors, List<string> names)> VerificationEDocumentHex(Guid fileId, Guid appId)
        {
            var errors = new Dictionary<string, string>();
            var names = new List<string>();
            string file = "";
            try
            {
                var stream = await GetFile(fileId);
                TextReader reader = new StreamReader(stream);
                var stringFile = reader.ReadToEnd();
                stringFile = stringFile.Replace('\0'.ToString(), "");
                int indexFrom = stringFile.IndexOf("@@startFile@@") + "@@startFile@@".Length;
                int indexTo = stringFile.IndexOf("@@endFile@@");
                file = stringFile.Substring(indexFrom, indexTo - indexFrom);
            }
            catch (Exception e)
            {
                throw new Exception("Помилка серверу(відсутній файл)");
            }
            var jsonObjs = JsonConvert.DeserializeObject<List<EDocumentMD5Model>>(file);
            var newJsonObjs = JsonConvert.DeserializeObject<List<EDocumentMD5Model>>((await _applicationService.GetEDocumentJsonFile(appId)).file);
            foreach (var jsonObj in jsonObjs)
            {
                var base64EncodedBytes = Convert.FromBase64String(jsonObj.Name);
                var name =  Encoding.UTF8.GetString(base64EncodedBytes);
                names.Add(name);
                var newJsonObj = newJsonObjs.FirstOrDefault(x => x.Id == jsonObj.Id);
                if (newJsonObj == null)
                {
                    errors.Add(jsonObj.Name, "Підписаний файл відсутній");
                    continue;
                }

                if (jsonObj.File != newJsonObj.File)
                {
                    errors.Add(jsonObj.Name, "Підписаний файл був змінений");
                }
            }

            var redunantFiles = newJsonObjs.Where(x => !jsonObjs.Select(y => y.Id).Contains(x.Id));
            if (redunantFiles.Any())
            {
                foreach (var redunantFile in redunantFiles)
                {
                    errors.Add(redunantFile.Name, "Файл відсутній в підписаному досьє");
                }
            }
            return (errors, names);
        }
    }
}
