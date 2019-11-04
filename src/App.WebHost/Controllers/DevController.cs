using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business.Attributes;
using App.Business.Extensions;
using App.Business.Services.AppServices;
using App.Business.Services.BranchService;
using App.Business.Services.ImlServices;
using App.Business.Services.LimsService;
using App.Business.Services.PrlServices;
using App.Business.Services.TrlServices;
using App.Core.Business.Services;
using App.Core.Data.Entities.ATU;
using App.Data.Contexts;
using App.Data.DTO.APP;
using App.Data.DTO.BRN;
using App.Data.DTO.IML;
using App.Data.DTO.PRL;
using App.Data.DTO.TRL;
using App.Data.Models.PRL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Host.Controllers
{
    [Authorize(Policy = "Registered")]
    [BreadCrumb(Title = "Develobster page", UseDefaultRouteUrl = true, ClearStack = true, Order = 0)]
    public class DevController: Controller
    {
        private static readonly Random _rand = new Random();
        private readonly IPrlApplicationService _prlApplicationService;
        private readonly ImlApplicationService _imlApplicationService;
        private readonly TrlApplicationService _trlApplicationService;
        private readonly LimsExchangeService _limsExchangeService;
        private readonly ICommonDataService _dataService;
        private readonly IBranchService _branchService;
        private readonly AppAssigneeService _assigneeService;
        private readonly IUserInfoService _userInfoService;
        private readonly ApplicationDbContext _context;

        public DevController(LimsExchangeService limsExchangeService, ICommonDataService dataService,
            IPrlApplicationService prlApplicationService, IBranchService branchService,
            AppAssigneeService assigneeService, IUserInfoService userInfoService, ApplicationDbContext context, ImlApplicationService imlApplicationService,
            TrlApplicationService trlApplicationService)
        {
            _limsExchangeService = limsExchangeService;
            _dataService = dataService;
            _prlApplicationService = prlApplicationService;
            _branchService = branchService;
            _assigneeService = assigneeService;
            _userInfoService = userInfoService;
            _context = context;
            _imlApplicationService = imlApplicationService;
            _trlApplicationService = trlApplicationService;
        }

        [Authorize]
        [BreadCrumb(Title = "Develobster", Order = 1)]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ImportProtocols()
        {
            await _limsExchangeService.ImportLimsProtocols();
            return new OkResult();
        }

        public string UpdateLimsProtocols()
        {
            var protocols = _limsExchangeService.UpdateLimsProtocols().Result;
            return protocols.ToString();
        }

        public IActionResult CreateNewPRLLicenseApplication()
        {
            var abjAdd = _dataService.GetEntity<SubjectAddress>().FirstOrDefault();
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var appDTO = new PrlAppDetailDTO()
            {
                Id = Guid.Empty,
                Caption = null,
                OrgUnitId = Guid.Empty,
                OrganizationInfoId = Guid.Empty,
                OrgName = userInfo.OrganizationName(),
                EDRPOU = userInfo.EDRPOU(),
                INN = userInfo.INN(),
                LegalFormType = "470",
                PassportSerial = null,
                PassportNumber = null,
                OwnershipType = "32",
                PassportDate = null,
                PassportIssueUnit = null,
                OrgDirector = null,
                EMail = $"{CreateString(5)}@mail.com",
                PhoneNumber = CreateNumericString(10),
                FaxNumber = CreateNumericString(10),
                StreetId = Guid.Empty,
                StreetName = null,
                CityId = Guid.Empty,
                CityName = null,
                CityEnum = null,
                PostIndex = null,
                RegionId = Guid.Empty,
                RegionName = null,
                DistrictName = null,
                Building = null,
                AddressType = null,
                AddressId = abjAdd.Id,
                NationalAccount = $"NationalAccount:{CreateString(10)}",
                NationalBankRequisites = $"NationalBankRequisites:{CreateString(10)}",
                InternationalAccount = $"InternationalAccount:{CreateString(10)}",
                InternationalBankRequisites = $"InternationalBankRequisites:{CreateString(10)}",
                Duns = $"Duns:{CreateString(10)}",
                AppType = "PRL",
                AppSort = "GetLicenseApplication",
                LegalFormName = null,
                OwnershipTypeName = null,
                IsCheckMpd = true,
                IsPaperLicense = true,
                IsCourierDelivery = true,
                IsPostDelivery = true,
                IsAgreeLicenseTerms = true,
                IsAgreeProcessingData = true,
                IsProtectionFromAggressors = true,
                IsCourierResults = true,
                IsPostResults = true,
                IsElectricFormResults = true,
                AppState = "Project",
                BackOfficeAppState = null,
                Comment = $"comment{CreateString(20)}",
                DecisionType = null,
                PerformerId = null,
                RegNumber = null,
                RegDate = null,
                ExpertiseResultEnum = null,
                ExpertiseResult = null,
                ExpertiseDate = null,
                PerformerOfExpertise = null,
                IsCreatedOnPortal = true
            };
            if (string.IsNullOrEmpty(userInfo?.EDRPOU()))
            {
                appDTO.OrgName = userInfo.FullName();
            }

            var appId = _prlApplicationService.SaveApplication(appDTO).Result;

            var branchCount = _rand.Next(1, 3);
            var branchIds = new List<Guid>();
            while (branchCount > 0)
            {
                branchCount--;
                var branch = new BranchDetailsDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    ApplicationId = appId,
                    OrganizationId = Guid.Parse(userInfo.OrganizationId()),
                    AppType = "PRL",
                    ApplicationBranchId = default,
                    Name = $"Name:{CreateString(10)}",
                    PhoneNumber = CreateNumericString(10),
                    FaxNumber = CreateNumericString(10),
                    EMail = $"{CreateString(5)}@mail.com",
                    AdressEng = null,
                    StreetId = default,
                    StreetName = null,
                    CityId = default,
                    CityName = null,
                    CityEnum = null,
                    PostIndex = null,
                    RegionId = default,
                    RegionName = null,
                    DistrictName = null,
                    Building = CreateNumericString(2),
                    AddressType = null,
                    AddressId = abjAdd.Id,
                    PRLIsAvailiableProdSites = true,
                    PRLIsAvailiableQualityZone = true,
                    PRLIsAvailiableStorageZone = true,
                    PRLIsAvailiablePickupZone = true,
                    OperationListForm = string.Empty,
                    IMLIsAvailiableStorageZone = true,
                    IMLIsAvailiablePermitIssueZone = true,
                    IMLIsAvailiableQuality = true,
                    TRLIsManufacture = true,
                    TRLIsWholesale = true,
                    TRLIsRetail = true,
                    OperationListDTO = default,
                    IsFromLicense = false
                };
                branchIds.Add(_branchService.Save(branch).Result);
            }

            var contractorCount = _rand.Next(1, 3);
            while (contractorCount > 0)
            {
                contractorCount--;
                var contractor = new PrlContractorDetailDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    OrgUnitId = default,
                    Name = CreateString(_rand.Next(5, 20)),
                    ContractorType = "Manufacturer",
                    ContractorTypeName = null,
                    Edrpou = CreateNumericString(9),
                    Address = CreateString(_rand.Next(10, 20)),
                    LicenseContractorId = Guid.Empty,
                    LicenseDeleteCheck = false,
                    ListOfBranches = null,
                    ListOfBranchsNames = null,
                    appId = appId,
                    IsFromLicense = false,
                    Sort = null
                };
                contractor.Id = _dataService.Add<PrlContractor>(contractor);
                _dataService.SaveChanges();
                branchIds.ForEach(brId =>
                    _dataService.Add(new PrlBranchContractor {ContractorId = contractor.Id, BranchId = brId}));
                _dataService.SaveChanges();
            }

            var assigneeCount = _rand.Next(1, 3);
            while (assigneeCount > 0)
            {
                assigneeCount--;
                var assignee = new AppAssigneeDetailDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    OrgUnitId = default,
                    appId = appId,
                    Name = $"Name:{CreateString(_rand.Next(5,10))}",
                    MiddleName = $"MiddleName:{CreateString(_rand.Next(5, 10))}",
                    LastName = $"LastName:{CreateString(_rand.Next(5, 10))}",
                    IPN = $"{CreateNumericString(10)}",
                    Birthday = DateTime.Now,
                    OrgPositionType = "Authorized",
                    EducationInstitution = $"{CreateString(_rand.Next(5, 10))}",
                    YearOfGraduation = $"{CreateNumericString(4)}",
                    NumberOfDiploma = $"{CreateNumericString(3)}",
                    DateOfGraduation = DateTime.Now,
                    Speciality = $"{CreateString(7)}",
                    WorkExperience = $"{CreateNumericString(2)}",
                    NumberOfContract = $"{CreateNumericString(6)}",
                    OrderNumber = $"{CreateNumericString(4)}",
                    DateOfContract = DateTime.Now,
                    DateOfAppointment = DateTime.Now,
                    NameOfPosition = $"{CreateString(10)}",
                    ContactInformation = $"{CreateNumericString(10)}",
                    Comment = $"{CreateString(20)}",
                    LicenseAssigneeId = null,
                    LicenseAssignee = null,
                    LicenseDeleteCheck = null,
                    ListOfBranches = branchIds,
                    ListOfBranchsNames = null,
                    IsFromLicense = null,
                    AppSort = null
                };
                var result = _assigneeService.Edit(assignee);
            }

            return RedirectToAction("Details", "PrlApp", new{Area = "PRL", id = appId});
        }

        public IActionResult CreateNewIMLLicenseApplication()
        {
            var abjAdd = _dataService.GetEntity<SubjectAddress>().FirstOrDefault();
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var appDTO = new ImlAppDetailDTO()
            {
                Id = Guid.Empty,
                Caption = null,
                OrgUnitId = Guid.Empty,
                OrganizationInfoId = Guid.Empty,
                OrgName = userInfo.OrganizationName(),
                EDRPOU = userInfo.EDRPOU(),
                INN = userInfo.INN(),
                LegalFormType = "470",
                PassportSerial = null,
                PassportNumber = null,
                OwnershipType = "32",
                PassportDate = null,
                PassportIssueUnit = null,
                OrgDirector = null,
                EMail = $"{CreateString(5)}@mail.com",
                PhoneNumber = CreateNumericString(10),
                FaxNumber = CreateNumericString(10),
                StreetId = Guid.Empty,
                StreetName = null,
                CityId = Guid.Empty,
                CityName = null,
                CityEnum = null,
                PostIndex = null,
                RegionId = Guid.Empty,
                RegionName = null,
                DistrictName = null,
                Building = null,
                AddressType = null,
                AddressId = abjAdd.Id,
                NationalAccount = $"NationalAccount:{CreateString(10)}",
                NationalBankRequisites = $"NationalBankRequisites:{CreateString(10)}",
                InternationalAccount = $"InternationalAccount:{CreateString(10)}",
                InternationalBankRequisites = $"InternationalBankRequisites:{CreateString(10)}",
                Duns = $"Duns:{CreateString(10)}",
                AppType = "IML",
                AppSort = "GetLicenseApplication",
                LegalFormName = null,
                OwnershipTypeName = null,
                IsCheckMpd = true,
                IsPaperLicense = true,
                IsCourierDelivery = true,
                IsPostDelivery = true,
                IsAgreeLicenseTerms = true,
                IsAgreeProcessingData = true,
                IsProtectionFromAggressors = true,
                IsCourierResults = true,
                IsPostResults = true,
                IsElectricFormResults = true,
                AppState = "Project",
                BackOfficeAppState = null,
                Comment = $"comment{CreateString(20)}",
                DecisionType = null,
                PerformerId = null,
                RegNumber = null,
                RegDate = null,
                ExpertiseResultEnum = null,
                ExpertiseResult = null,
                ExpertiseDate = null,
                PerformerOfExpertise = null,
                IsCreatedOnPortal = true
            };
            if (string.IsNullOrEmpty(userInfo?.EDRPOU()))
            {
                appDTO.OrgName = userInfo.FullName();
            }

            var appId = _imlApplicationService.SaveApplication(appDTO).Result;

            var branchCount = _rand.Next(1, 3);
            var branchIds = new List<Guid>();
            while (branchCount > 0)
            {
                branchCount--;
                var branch = new BranchDetailsDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    ApplicationId = appId,
                    OrganizationId = Guid.Parse(userInfo.OrganizationId()),
                    AppType = "IML",
                    ApplicationBranchId = default,
                    Name = $"Name:{CreateString(10)}",
                    PhoneNumber = CreateNumericString(10),
                    FaxNumber = CreateNumericString(10),
                    EMail = $"{CreateString(5)}@mail.com",
                    AdressEng = null,
                    StreetId = default,
                    StreetName = null,
                    CityId = default,
                    CityName = null,
                    CityEnum = null,
                    PostIndex = null,
                    RegionId = default,
                    RegionName = null,
                    DistrictName = null,
                    Building = CreateNumericString(2),
                    AddressType = null,
                    AddressId = abjAdd.Id,
                    PRLIsAvailiableProdSites = true,
                    PRLIsAvailiableQualityZone = true,
                    PRLIsAvailiableStorageZone = true,
                    PRLIsAvailiablePickupZone = true,
                    OperationListForm = string.Empty,
                    IMLIsAvailiableStorageZone = true,
                    IMLIsAvailiablePermitIssueZone = true,
                    IMLIsAvailiableQuality = true,
                    TRLIsManufacture = true,
                    TRLIsWholesale = true,
                    TRLIsRetail = true,
                    OperationListDTO = default,
                    IsFromLicense = false
                };
                branchIds.Add(_branchService.Save(branch).Result);
            }

            var assigneeCount = _rand.Next(1, 3);
            while (assigneeCount > 0)
            {
                assigneeCount--;
                var assignee = new AppAssigneeDetailDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    OrgUnitId = default,
                    appId = appId,
                    Name = $"Name:{CreateString(_rand.Next(5, 10))}",
                    MiddleName = $"MiddleName:{CreateString(_rand.Next(5, 10))}",
                    LastName = $"LastName:{CreateString(_rand.Next(5, 10))}",
                    IPN = $"{CreateNumericString(10)}",
                    Birthday = DateTime.Now,
                    OrgPositionType = "Authorized",
                    EducationInstitution = $"{CreateString(_rand.Next(5, 10))}",
                    YearOfGraduation = $"{CreateNumericString(4)}",
                    NumberOfDiploma = $"{CreateNumericString(3)}",
                    DateOfGraduation = DateTime.Now,
                    Speciality = $"{CreateString(7)}",
                    WorkExperience = $"{CreateNumericString(2)}",
                    NumberOfContract = $"{CreateNumericString(6)}",
                    OrderNumber = $"{CreateNumericString(4)}",
                    DateOfContract = DateTime.Now,
                    DateOfAppointment = DateTime.Now,
                    NameOfPosition = $"{CreateString(10)}",
                    ContactInformation = $"{CreateNumericString(10)}",
                    Comment = $"{CreateString(20)}",
                    LicenseAssigneeId = null,
                    LicenseAssignee = null,
                    LicenseDeleteCheck = null,
                    ListOfBranches = branchIds,
                    ListOfBranchsNames = null,
                    IsFromLicense = null,
                    AppSort = null
                };
                var result = _assigneeService.Edit(assignee);
            }

            return RedirectToAction("Details", "ImlApp", new { Area = "IML", id = appId });
        }

        public IActionResult CreateNewTRLLicenseApplication()
        {
            var abjAdd = _dataService.GetEntity<SubjectAddress>().FirstOrDefault();
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var appDTO = new TrlAppDetailDTO()
            {
                Id = Guid.Empty,
                Caption = null,
                OrgUnitId = Guid.Empty,
                OrganizationInfoId = Guid.Empty,
                OrgName = userInfo.OrganizationName(),
                EDRPOU = userInfo.EDRPOU(),
                INN = userInfo.INN(),
                LegalFormType = "470",
                PassportSerial = null,
                PassportNumber = null,
                OwnershipType = "32",
                PassportDate = null,
                PassportIssueUnit = null,
                OrgDirector = null,
                EMail = $"{CreateString(5)}@mail.com",
                PhoneNumber = CreateNumericString(10),
                FaxNumber = CreateNumericString(10),
                StreetId = Guid.Empty,
                StreetName = null,
                CityId = Guid.Empty,
                CityName = null,
                CityEnum = null,
                PostIndex = null,
                RegionId = Guid.Empty,
                RegionName = null,
                DistrictName = null,
                Building = null,
                AddressType = null,
                AddressId = abjAdd.Id,
                NationalAccount = $"NationalAccount:{CreateString(10)}",
                NationalBankRequisites = $"NationalBankRequisites:{CreateString(10)}",
                InternationalAccount = $"InternationalAccount:{CreateString(10)}",
                InternationalBankRequisites = $"InternationalBankRequisites:{CreateString(10)}",
                Duns = $"Duns:{CreateString(10)}",
                AppType = "TRL",
                AppSort = "GetLicenseApplication",
                LegalFormName = null,
                OwnershipTypeName = null,
                EconomicClassificationTypeName = null,
                IsCheckMpd = true,
                IsPaperLicense = true,
                IsCourierDelivery = true,
                IsPostDelivery = true,
                IsAgreeLicenseTerms = true,
                IsAgreeProcessingData = true,
                IsProtectionFromAggressors = true,
                IsCourierResults = true,
                IsPostResults = true,
                IsElectricFormResults = true,
                AppState = "Project",
                BackOfficeAppState = null,
                Comment = $"comment{CreateString(20)}",
                DecisionType = null,
                PerformerId = null,
                RegNumber = null,
                RegDate = null,
                ExpertiseResultEnum = null,
                ExpertiseResult = null,
                ExpertiseDate = null,
                PerformerOfExpertise = null,
                IsCreatedOnPortal = true
            };
            if (string.IsNullOrEmpty(userInfo?.EDRPOU()))
            {
                appDTO.OrgName = userInfo.FullName();
            }

            var appId = _trlApplicationService.SaveApplication(appDTO).Result;

            var branchCount = _rand.Next(1, 3);
            var branchIds = new List<Guid>();
            while (branchCount > 0)
            {
                branchCount--;
                var branch = new BranchDetailsDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    ApplicationId = appId,
                    OrganizationId = Guid.Parse(userInfo.OrganizationId()),
                    AppType = "TRL",
                    ApplicationBranchId = default,
                    Name = $"Name:{CreateString(10)}",
                    PhoneNumber = CreateNumericString(10),
                    FaxNumber = CreateNumericString(10),
                    EMail = $"{CreateString(5)}@mail.com",
                    AdressEng = null,
                    StreetId = default,
                    StreetName = null,
                    CityId = default,
                    CityName = null,
                    CityEnum = null,
                    PostIndex = null,
                    RegionId = default,
                    RegionName = null,
                    DistrictName = null,
                    Building = CreateNumericString(2),
                    AddressType = null,
                    AddressId = abjAdd.Id,
                    PRLIsAvailiableProdSites = true,
                    PRLIsAvailiableQualityZone = true,
                    PRLIsAvailiableStorageZone = true,
                    PRLIsAvailiablePickupZone = true,
                    OperationListForm = string.Empty,
                    IMLIsAvailiableStorageZone = true,
                    IMLIsAvailiablePermitIssueZone = true,
                    IMLIsAvailiableQuality = true,
                    TRLIsManufacture = true,
                    TRLIsWholesale = true,
                    TRLIsRetail = true,
                    OperationListDTO = default,
                    IsFromLicense = false
                };
                branchIds.Add(_branchService.Save(branch).Result);
            }

            var assigneeCount = _rand.Next(1, 3);
            while (assigneeCount > 0)
            {
                assigneeCount--;
                var assignee = new AppAssigneeDetailDTO
                {
                    Id = Guid.Empty,
                    Caption = null,
                    OrgUnitId = default,
                    appId = appId,
                    Name = $"Name:{CreateString(_rand.Next(5, 10))}",
                    MiddleName = $"MiddleName:{CreateString(_rand.Next(5, 10))}",
                    LastName = $"LastName:{CreateString(_rand.Next(5, 10))}",
                    IPN = $"{CreateNumericString(10)}",
                    Birthday = DateTime.Now,
                    OrgPositionType = "Authorized",
                    EducationInstitution = $"{CreateString(_rand.Next(5, 10))}",
                    YearOfGraduation = $"{CreateNumericString(4)}",
                    NumberOfDiploma = $"{CreateNumericString(3)}",
                    DateOfGraduation = DateTime.Now,
                    Speciality = $"{CreateString(7)}",
                    WorkExperience = $"{CreateNumericString(2)}",
                    NumberOfContract = $"{CreateNumericString(6)}",
                    OrderNumber = $"{CreateNumericString(4)}",
                    DateOfContract = DateTime.Now,
                    DateOfAppointment = DateTime.Now,
                    NameOfPosition = $"{CreateString(10)}",
                    ContactInformation = $"{CreateNumericString(10)}",
                    Comment = $"{CreateString(20)}",
                    LicenseAssigneeId = null,
                    LicenseAssignee = null,
                    LicenseDeleteCheck = null,
                    ListOfBranches = branchIds,
                    ListOfBranchsNames = null,
                    IsFromLicense = null,
                    AppSort = null
                };
                var result = _assigneeService.Edit(assignee);
            }

            return RedirectToAction("Details", "TrlApp", new { Area = "TRL", id = appId });
        }

        public async Task<IActionResult> RefreshEnums()
        {
            _context.EnumRecord.RemoveRange(_context.EnumRecord);
            _context.SaveChanges();
            DbInitializer.InsertEnums(_context);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SeedAdmin()
        {
            DbInitializer.SeedAdmin(_context);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ImportRP()
        {
            await _limsExchangeService.ImportLimsRP();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RefreshRoles()
        {
            DbInitializer.RefreshRights(_context);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddRoles()
        {
            DbInitializer.AddRoles(_context);
            return RedirectToAction("Index");
        }


        private static string CreateString(int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            var chars = new char[stringLength];

            for (var i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[_rand.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        private static string CreateNumericString(int stringLength)
        {
            const string allowedChars = "0123456789";
            var chars = new char[stringLength];

            for (var i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[_rand.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

    }
}
