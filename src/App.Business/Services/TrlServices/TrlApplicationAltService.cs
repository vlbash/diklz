using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Base;
using App.Core.Business.Services;
using App.Core.Business.Services.ObjectMapper;
using App.Core.Data.Entities.Common;
using App.Core.Data.Helpers;
using App.Data.Contexts;
using App.Data.DTO.TRL;
using App.Data.Models.APP;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.ORG;
using App.Data.Models.TRL;
using Serilog;

namespace App.Business.Services.TrlServices
{
    public class TrlApplicationAltService
    {
        private readonly ICommonDataService _commonDataService;
        private readonly ITrlLicenseService _trlLicenseService;
        private readonly IObjectMapper _objectMapper;
        private readonly MigrationDbContext _context;

        public TrlApplicationAltService(ICommonDataService commonDataService, ITrlLicenseService trlLicenseService,
            IObjectMapper objectMapper, MigrationDbContext context)
        {
            _commonDataService = commonDataService;
            _trlLicenseService = trlLicenseService;
            _objectMapper = objectMapper;
            _context = context;
        }

        public async Task<Guid> CreateOnOpen(string sort, Guid? id = null, bool isBackOffice = false)
        {
            Guid licenseId;
            try
            {
                if (id == null)
                    licenseId = _trlLicenseService.GetLicenseGuid().Value;
                else
                {
                    licenseId = _trlLicenseService.GetLicenseGuid(id).Value;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            Guid applicationId;
            try
            {
                applicationId = await ProcessLicenseToApplication(licenseId, sort, isBackOffice);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return applicationId;
        }

        private async Task<Guid> ProcessLicenseToApplication(Guid licenseId, string sort, bool isBackOffice)
        {
            var license = _commonDataService.GetEntity<TrlLicense>(lic => lic.Id == licenseId).FirstOrDefault();

            if (license == null || license.ParentId == null)
            {
                Log.Error("license == null");   
                throw new NullReferenceException("Виникла помилка!");
            }

            return await ProcessApplicationToApplication(license.ParentId.Value, licenseId, sort, isBackOffice);
        }

        public async Task<Guid> ProcessApplicationToApplication(Guid appId, Guid? licenseId = null, string sort = "", bool isBackOffice = false)
        {

            var licenseApplication = _commonDataService.GetEntity<TrlApplication>(x => x.Id == appId)
                .FirstOrDefault();
            if (licenseApplication == null)
            {
                Log.Error("licenseApplication == null");
                throw new NullReferenceException("Виникла помилка");
            }

            var newApplication = new TrlApplication();
            _objectMapper.Map(licenseApplication, newApplication);
            newApplication.Id = Guid.NewGuid();

            //ссылка на дочернюю заяву, из которой создана лицензия
            if (licenseId == null || licenseId == Guid.Empty)
                newApplication.ParentId = licenseApplication.ParentId;
            else
                newApplication.ParentId = licenseId;

            if (!string.IsNullOrEmpty(sort))
                newApplication.AppSort = sort;
            else
                newApplication.AppSort = licenseApplication.AppSort;

            if (isBackOffice)
            {
                newApplication.IsCreatedOnPortal = false;
                newApplication.AppState = null;
                newApplication.BackOfficeAppState = "Project";
            }
            else
            {
                newApplication.AppState = "Project";
                newApplication.BackOfficeAppState = null;
                newApplication.IsCreatedOnPortal = true;
            }

            newApplication.AppType = "TRL";
            newApplication.ModifiedOn = DateTime.Now;
            newApplication.IsCheckMpd = false;
            newApplication.IsPaperLicense = false;
            newApplication.IsCourierDelivery = false;
            newApplication.IsPostDelivery = false;
            newApplication.IsAgreeLicenseTerms = false;
            newApplication.IsAgreeProcessingData = false;
            newApplication.IsCourierResults = false;
            newApplication.IsPostResults = false;
            newApplication.IsElectricFormResults = false;
            newApplication.IsProtectionFromAggressors = false;
            newApplication.Comment = "";

            newApplication.ErrorProcessingLicense = null;
            newApplication.PerformerOfExpertise = null;
            newApplication.ExpertiseDate = null;
            newApplication.ExpertiseResult = null;
            newApplication.AppPreLicenseCheckId = null;
            newApplication.AppLicenseMessageId = null;
            newApplication.AppDecisionId = null;
            newApplication.PerformerId = null;
            newApplication.RegDate = null;
            newApplication.RegNumber = null;
            newApplication.Description = null;
            newApplication.OldLimsId = 0;

            var organizationInfo = _commonDataService.GetEntity<OrganizationInfo>(x => x.Id == licenseApplication.OrganizationInfoId)
                .FirstOrDefault();
            if (organizationInfo == null)
            {
                Log.Error("organizationInfo == null");
                throw new NullReferenceException("Виникла помилка");
            }

            var newOrganizationInfo = new OrganizationInfo();
            _objectMapper.Map(organizationInfo, newOrganizationInfo);
            newOrganizationInfo.Id = Guid.NewGuid();
            if (licenseId != null)
                newOrganizationInfo.IsActualInfo = false;
            else
            {
                newOrganizationInfo.IsActualInfo = true;
                _commonDataService.GetEntity<OrganizationInfo>(
                        x => x.OrganizationId == licenseApplication.OrgUnitId).ToList().ForEach(x=>x.IsActualInfo = false);

            }

            newApplication.OrganizationInfoId = newOrganizationInfo.Id;
            _commonDataService.Add(newOrganizationInfo);

            var licenseBranches = new List<Branch>();
            var licenseBranch = new List<ApplicationBranch>();

            var applicationBranchesIds = _commonDataService
                .GetEntity<ApplicationBranch>(x => x.LimsDocumentId == licenseApplication.Id)
                .Select(x => x.BranchId)
                .Distinct()
                .ToList();
            var applicationBranches = _commonDataService.GetEntity<Branch>(br => applicationBranchesIds.Contains(br.Id))
                .ToList();

            foreach (var branch in applicationBranches)
            {
                var licBr = _objectMapper.Map<Branch>(branch);
                licBr.Id = Guid.NewGuid();
                //ссылка на дочерний обьект мпд
                licBr.ParentId = branch.Id;
                licBr.IsFromLicense = true;
                licenseBranches.Add(licBr);

                licenseBranch.Add(new ApplicationBranch
                {
                    BranchId = licBr.Id,
                    LimsDocumentId = newApplication.Id
                });
            }

            var applicationAssigneeBranches =
                _commonDataService.GetEntity<AppAssigneeBranch>(x => applicationBranchesIds.Contains(x.BranchId));
            var applicationAssigneeIds = applicationAssigneeBranches.Select(x => x.AssigneeId).Distinct().ToList();
            var applicationAssignee =
                _commonDataService.GetEntity<AppAssignee>(x => applicationAssigneeIds.Contains(x.Id));
            var licenseAssigneeBranches = new List<AppAssigneeBranch>();
            var licenseAssignees = new List<AppAssignee>();

            foreach (var assignee in applicationAssignee)
            {
                var licAssignee = _objectMapper.Map<AppAssignee>(assignee);
                licAssignee.Id = Guid.NewGuid();
                licAssignee.IsFromLicense = true;
                licenseAssignees.Add(licAssignee);

                var assBranches = applicationAssigneeBranches.Where(x => x.AssigneeId == assignee.Id);
                foreach (var appAssigneeBranch in assBranches)
                {
                    licenseAssigneeBranches.Add(new AppAssigneeBranch
                    {
                        AssigneeId = licAssignee.Id,
                        BranchId = licenseBranches.FirstOrDefault(br => br.ParentId == appAssigneeBranch.BranchId).Id
                    });
                }
            }

            
            var applicationEdocumentBranches =
                _commonDataService.GetEntity<BranchEDocument>(x => applicationBranchesIds.Contains(x.BranchId));
            var applicationEdocumentIds = applicationEdocumentBranches.Select(x => x.EDocumentId).Distinct().ToList();
            var applicationEdocuments = _commonDataService
                .GetEntity<EDocument>(x => applicationEdocumentIds.Contains(x.Id)).ToList();
            var licenseEdocumentBranches = new List<BranchEDocument>();
            var licenseEdocuments = new List<EDocument>();
            var licenseFiles = new List<FileStore>();

            foreach (var applicationEDocument in applicationEdocuments)
            {
                var licEDocument = _objectMapper.Map<EDocument>(applicationEDocument);
                licEDocument.Id = Guid.NewGuid();
                licEDocument.IsFromLicense = true;
                licenseEdocuments.Add(licEDocument);

                var edocBranches = applicationEdocumentBranches.Where(x => x.EDocumentId == applicationEDocument.Id);
                foreach (var branchEdocument in edocBranches)
                {
                    licenseEdocumentBranches.Add(new BranchEDocument()
                    {
                        EDocumentId = licEDocument.Id,
                        BranchId = licenseBranches.FirstOrDefault(br => br.ParentId == branchEdocument.BranchId).Id
                    });
                }
                var fileStore = _commonDataService
                    .GetEntity<FileStore>(x => x.EntityName == "EDocument" && x.EntityId == applicationEDocument.Id).ToList();
                fileStore.ForEach(x =>
                {
                    x.EntityId = licEDocument.Id;
                    x.Id = Guid.NewGuid();
                    licenseFiles.Add(x);
                });
            }

            //var imlMedicine = (await _commonDataService.GetDtoAsync<ImlMedicineDetailDTO>(x => x.ApplicationId == appId)).ToList();
            //var imlMedicines = new List<ImlMedicine>();
            //foreach (var med in imlMedicine)
            //{
            //    var imlMed = _objectMapper.Map<ImlMedicine>(med);
            //    imlMed.Id = Guid.NewGuid();
            //    imlMed.ApplicationId = newApplication.Id;
            //    imlMed.IsFromLicense = true;
            //    imlMedicines.Add(imlMed);
            //}

            _commonDataService.Add(newApplication, false);
            licenseBranches.ForEach(branch => _commonDataService.Add(branch, false));
            licenseBranch.ForEach(appBranch => _commonDataService.Add(appBranch, false));
            licenseAssignees.ForEach(assignee => _commonDataService.Add(assignee, false));
            licenseAssigneeBranches.ForEach(assigneeBr => _commonDataService.Add(assigneeBr, false));
            licenseEdocuments.ForEach(eDoc => _commonDataService.Add(eDoc, false));
            licenseEdocumentBranches.ForEach(eDocBra => _commonDataService.Add(eDocBra, false));
            licenseFiles.ForEach(x => _commonDataService.Add(x, false));
            
            await _context.SaveChangesAsync();

            return newApplication.Id;
        }

        public async Task<bool> ChangeBranchDeleteCheck(Guid branchId)
        {
            var branch = _commonDataService.GetEntity<Branch>(x => x.Id == branchId).FirstOrDefault();
            if (branch == null)
            {
                return false;
            }

            //TODO check if correct
            //branch.LicenseDeleteCheck = !branch.LicenseDeleteCheck ?? true;

            if (branch.LicenseDeleteCheck == null)
            {
                branch.LicenseDeleteCheck = true;
            }
            else
            {
                branch.LicenseDeleteCheck = !branch.LicenseDeleteCheck;
            }

            try
            {
                await _commonDataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task Delete(Guid id, bool softDeleting)
        {
            try
            {
                var application = _commonDataService.GetEntity<TrlApplication>(x => x.Id == id).SingleOrDefault();
                application.RecordState = RecordState.D;
                var applicationBranches =
                    _commonDataService.GetEntity<ApplicationBranch>(x => x.LimsDocumentId == id).ToList();
                applicationBranches.ForEach(x => x.RecordState = RecordState.D);
                var branches = _commonDataService.GetEntity<Branch>(x => applicationBranches.Select(y => y.BranchId).Contains(x.Id)).ToList();
                branches.ForEach(x => x.RecordState = RecordState.D);
                var branchAssignees =
                    _commonDataService.GetEntity<AppAssigneeBranch>(x => branches.Select(y => y.Id).Contains(x.BranchId)).ToList();
                branchAssignees.ForEach(x => x.RecordState = RecordState.D);
                var assignees = _commonDataService
                    .GetEntity<AppAssignee>(x => branchAssignees.Select(y => y.AssigneeId).Contains(x.Id)).ToList();
                assignees.ForEach(x => x.RecordState = RecordState.D);
                var eDocumentBranches = _commonDataService
                    .GetEntity<BranchEDocument>(x => branches.Select(y => y.Id).Contains(x.BranchId)).ToList();
                eDocumentBranches.ForEach(x => x.RecordState = RecordState.D);
                var eDocument =
                    _commonDataService.GetEntity<EDocument>(x =>
                        eDocumentBranches.Select(y => y.EDocumentId).Contains(x.Id)).ToList();
                var licFileName = new List<string>();
                eDocument.ForEach(x =>
                {
                    if (x.IsFromLicense == true)
                    {
                        var files = _commonDataService
                            .GetEntity<FileStore>(y => y.EntityId == x.Id && y.EntityName == "EDocument").ToList();
                        licFileName.AddRange(files.Select(p => p.FileName));
                    }
                });

                eDocument.ForEach(x =>
                {
                    if (x.IsFromLicense == true)
                        return;
                    x.RecordState = RecordState.D;
                    var files = _commonDataService
                        .GetEntity<FileStore>(y => y.EntityId == x.Id && y.EntityName == "EDocument").ToList();
                    files.ForEach(y =>
                    {
                        y.RecordState = RecordState.D;
                        // Удалить файлы которые не принадлежат лицензии
                        if (!licFileName.Contains(y.FileName))
                        {
                            FileStoreHelper.DeleteFileIfExist(y.FilePath);
                        }
                    });
                });

                await _commonDataService.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

    }
}
