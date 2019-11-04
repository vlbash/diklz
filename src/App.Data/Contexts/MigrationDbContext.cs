using System;
using System.Collections.Generic;
using App.Core.Data;
using Microsoft.EntityFrameworkCore;
using App.Core.Common.Extensions;
using App.Core.Data.DTO.ATU;
using App.Core.Data.DTO.Common;
using App.Core.Data.DTO.Org;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.CDN;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;
using App.Core.Security.Entities;
using App.Data.DTO.APP;
using App.Data.DTO.ATU;
using App.Data.DTO.BRN;
using App.Data.DTO.Common;
using App.Data.DTO.Common.Widget;
using App.Data.DTO.CRV;
using App.Data.DTO.USER;
using App.Data.DTO.DOS;
using App.Data.DTO.MTR;
using App.Data.DTO.ORG;
using App.Data.DTO.RPT;
using App.Data.Models;
using App.Data.Models.APP;
using App.Data.Models.IML;
using App.Data.Models.ORG;
using App.Data.Models.PRL;
using App.Data.Models.TRL;
using App.Data.DTO.FDB;
using App.Data.DTO.IML;
using App.Data.DTO.LOG;
using App.Data.DTO.NTF;
using App.Data.DTO.P902;
using App.Data.DTO.PRL;
using App.Data.DTO.SEC;
using App.Data.DTO.TRL;
using App.Data.Models.CRV;
using App.Data.Models.DOC;
using App.Data.Models.DOS;
using App.Data.Models.NTF;
using App.Data.Models.FDB;
using App.Data.Models.P902;
using FileStoreDTO = App.Data.DTO.Common.FileStoreDTO;
using Profile = App.Core.Security.Entities.Profile;

namespace App.Data.Contexts
{
    public class MigrationDbContext: CoreDbContext
    {
        private static IEnumerable<Type> _applicationModels;


        #region Security
        public DbSet<FieldRight> FieldRights { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileRole> ProfileRoles { get; set; }
        public DbSet<ProfileRight> ProfileRights { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleRight> RoleRights { get; set; }
        public DbSet<ApplicationRowLevelRight> ApplicationRowLevelRights { get; set; }
        public DbSet<RowLevelRight> RowLevelRights { get; set; }
        public DbSet<RowLevelSecurityObject> RowLevelSecurtityObjects { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserDefaultValue> UserDefaultValues { get; set; }

        public override IEnumerable<Type> GetApplicationModels()
        {
            if (_applicationModels == null)
            {
                _applicationModels = GetType().GetPropertyGenericArguments(typeof(DbSet<>));
            }

            return _applicationModels;
        }

        public DbQuery<ApplicationRowLevelRightDTO> ApplicationRowLevelRightDTO { get; set; }
        public DbQuery<FieldRightListDTO> FieldRightListDTO { get; set; }
        public DbQuery<ProfileListDTO> ProfileListDTO { get; set; }
        public DbQuery<ProfileDetailDTO> ProfileDetailDTO { get; set; }
        public DbQuery<ProfileEmployeeListDTO> ProfileEmployeeListDTO { get; set; }
        public DbQuery<ProfileEmployeeListMinDTO> ProfileEmployeeListMinDTO { get; set; }
        public DbQuery<ProfileRightListDTO> ProfileRightListDTO { get; set; }
        public DbQuery<ProfileRoleListDTO> ProfileRoleListDTO { get; set; }
        public DbQuery<ProfileRowLevelRightListDTO> ProfileRowLevelRightListDTO { get; set; }
        public DbQuery<ProfileRowLevelRightDTO> ProfileRowLevelRightDTO { get; set; }
        public DbQuery<RightListDTO> RightListDTO { get; set; }
        public DbQuery<RightDetailDTO> RightDetailDTO { get; set; }
        public DbQuery<RoleListDTO> RoleListDTO { get; set; }
        public DbQuery<RoleRightListDTO> RoleRightListDTO { get; set; }
        public DbQuery<OrgEmployeeMinDTO> OrgEmployeeMinDTO { get; set; }
        public DbQuery<PersonListDTO> PersonListDTO { get; set; }
        public DbQuery<PersonDetailDTO> PersonDetailDTO { get; set; }

        #endregion

        #region App

        public DbSet<ApplicationBranch> ApplicationBranches { get; set; }

        public DbSet<AppAssignee> AppAssignees { get; set; }
        public DbSet<AppAssigneeBranch> AppAssigneeBranches { get; set; }
        public DbQuery<AppAssigneeDetailDTO> AppAssigneeDetailDTO { get; set; }
        public DbQuery<AppAssigneeListDTO> AppAssigneeListDTO { get; set; }

        public DbQuery<ApplicationListDTO> ApplicationListDTO { get; set; }
        public DbQuery<AppShortDTO> AppShortDTO { get; set; }

        public DbSet<EDocument> EDocuments { get; set; }
        public DbSet<BranchEDocument> BranchEDocuments { get; set; }

        public DbQuery<EDocumentListDTO> EDocumentListDTO { get; set; }
        public DbQuery<EDocumentPaymentListDTO> EDocumentPaymentListDTO { get; set; }
        public DbQuery<EDocumentDetailsDTO> EDocumentDetailsDTO { get; set; }
        public DbQuery<EDocumentDetailsMsgDTO> EDocumentDetailsMsgDTO { get; set; }
        public DbQuery<RegisterEDocumentListDTO> RegisterEDocumentListDTO { get; set; }

        public DbQuery<MtrAppListDTO> MtrAppListDTO { get; set; }

        public DbSet<AppDecision> AppDecisions { get; set; }
        public DbSet<AppDecisionReason> AppDecisionReasons { get; set; }
        public DbQuery<AppDecisionDTO> AppDecisionDTO { get; set; }

        public DbSet<AppLicenseMessage> AppLicenseMessages { get; set; }
        public DbSet<AppPreLicenseCheck> AppPreLicenseChecks { get; set; }
        public DbSet<AppProtocol> AppProtocols { get; set; }
        public DbQuery<ProtocolDTO> ProtocolDto { get; set; }
        public DbQuery<ProtocolListDTO> ProtocolListDTO { get; set; }
        public DbQuery<AppPreLicenseCheckDTO> AppPreLicenseCheckDTO { get; set; }
        public DbQuery<AppLicenseMessageDTO> AppLicenseMessageDTO { get; set; }
        public DbQuery<AppStateDTO> AppStateDTO { get; set; }
        public DbQuery<AppOrganizationExtFullDTO> AppOrganizationExtFullDTO { get; set; }
        public DbQuery<AppOrganizationExtMediumDTO> AppOrganizationExtMediumDTO { get; set; }
        public DbQuery<AppOrganizationExtShortDTO> AppOrganizationExtShortDTO { get; set; }

        #endregion

        #region Org

        public DbSet<OrganizationExt> Organizations { get; set; }
        public DbSet<OrganizationInfo> OrganizationsInfo { get; set; }
        public DbSet<Branch> OrgBranches { get; set; }
        public DbSet<EmployeeExt> EmployeesExt { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<OrgUnit> OrgUnit { get; set; }
        public DbSet<OrgUnitPosition> OrgUnitPosition { get; set; }
        public DbSet<OrgUnitPositionEmployee> OrgUnitPositionEmployee { get; set; }
        public DbSet<PharmacyItemPharmacy> PharmacyItemPharmacies { get; set; }

        public DbSet<Position> Position { get; set; }

        public DbQuery<EmployeeExtDetailDTO> EmployeeExtDetailDTO { get; set; }
        public DbQuery<BranchListDTO> BranchListDTO { get; set; }
        public DbQuery<BranchListForLicenseDTO> BranchListForLicenseDTO { get; set; }
        public DbQuery<BranchListMsgDTO> BranchListMsgDTO { get; set; }
        public DbQuery<BranchAltListDTO> BranchAltListDTO { get; set; }
        public DbQuery<BranchDetailsDTO> BranchDetailsDTO { get; set; }
        public DbQuery<BranchAltDetailsDTO> BranchAltDetailsDTO { get; set; }
        public DbQuery<OrganizationExtDetailDTO> OrganizationExtDetailDTO { get; set; }
        public DbQuery<BranchRegisterListDTO> BranchRegisterListDTO { get; set; }

        #endregion

        #region Prl

        public DbSet<PrlLicense> PrlLicenses { get; set; }
        public DbQuery<PrlLicenseDetailDTO> PrlLicenseDetailDTO { get; set; }

        public DbQuery<PrlLicenseListDTO> PrlLicenseListDTO { get; set; }

        public DbSet<PrlApplication> PrlApplications { get; set; }
        public DbQuery<PrlAppDetailDTO> PrlAppDetailDTO { get; set; }
        public DbQuery<PrlAppListDTO> PrlAppListDTO { get; set; }

        public DbSet<PrlContractor> PrlContractors { get; set; }
        public DbSet<PrlBranchContractor> PrlBranchContractors { get; set; }
        public DbQuery<PrlContractorListDTO> PrlContractorListDTO { get; set; }
        public DbQuery<PrlContractorDetailDTO> PrlContractorDetailDTO { get; set; }

        public DbQuery<PrlOrganizationExtShortDTO> PrlOrganizationExtShortDTO { get; set; }
        public DbQuery<PrlOrganizationExtMediumDTO> PrlOrganizationExtMediumDTO { get; set; }
        public DbQuery<PrlOrganizationExtFullDTO> PrlOrganizationExtFullDTO { get; set; }
        public DbQuery<PrlAppExpertiseDTO> PrlAppExpertiseDTO { get; set; }
        public DbQuery<PrlLicenseRegisterListDTO> PrlLicenseRegisterListDTO { get; set; }
        public DbQuery<PrlLicenseRegisterDetailDTO> PrlLicenseRegisterDetailDTO { get; set; }
        #endregion

        #region Trl

        public DbSet<TrlApplication> TrlApplications { get; set; }
        public DbSet<TrlLicense> TrlLicenses { get; set; }
        public DbQuery<TrlAppDetailDTO> TrlAppDetailDTO { get; set; }
        public DbQuery<TrlAppListDTO> TrlAppListDTO { get; set; }
        public DbQuery<TrlOrganizationExtShortDTO> TrlOrganizationExtShortDTO { get; set; }
        public DbQuery<TrlOrganizationExtMediumDTO> TrlOrganizationExtMediumDTO { get; set; }
        public DbQuery<TrlOrganizationExtFullDTO> TrlOrganizationExtFullDTO { get; set; }
        public DbQuery<TrlOrganizationExtListDTO> TrlOrganizationExtListDTO { get; set; }
        public DbQuery<TrlAppExpertiseDTO> TrlAppExpertiseDTO { get; set; }
        public DbQuery<TrlLicenseDetailDTO> TrlLicenseDetailDTO { get; set; }
        public DbQuery<TrlLicenseListDTO> TrlLicenseListDTO { get; set; }
        public DbQuery<TrlLicenseRegisterDetailDTO> TrlLicenseRegisterDetailDTO { get; set; }
        public DbQuery<TrlLicenseRegisterListDTO> TrlLicenseRegisterListDTO { get; set; }

        #endregion

        #region Iml

        public DbSet<ImlApplication> ImlApplications { get; set; }
        public DbSet<ImlLicense> ImlLicenses { get; set; }
        public DbSet<ImlMedicine> ImlMedicines { get; set; }

        public DbQuery<ImlAppDetailDTO> ImlAppDetailDTO { get; set; }
        public DbQuery<ImlAppListDTO> ImlAppListDTO { get; set; }
        public DbQuery<ImlAppExpertiseDTO> ImlAppExpertiseDTO { get; set; }
        public DbQuery<ImlLicenseDetailDTO> ImlLicenseDetailDTO { get; set; }
        public DbQuery<ImlLicenseListDTO> ImlLicenseListDTO { get; set; }
        public DbQuery<ImlOrganizationExtShortDTO> ImlOrganizationExtShortDTO { get; set; }
        public DbQuery<ImlOrganizationExtMediumDTO> ImlOrganizationExtMediumDTO { get; set; }
        public DbQuery<ImlOrganizationExtFullDTO> ImlOrganizationExtFullDTO { get; set; }
        public DbQuery<ImlOrganizationExtListDTO> ImlOrganizationExtListDTO { get; set; }
        public DbQuery<ImlMedicineDetailDTO> ImlMedicineDetailDTO { get; set; }
        public DbQuery<ImlMedicineListDTO> ImlMedicineListDTO { get; set; }
        public DbQuery<ImlMedicineListMsgDTO> ImlMedicineListMsgDTO { get; set; }
        public DbQuery<ImlMedicineMinDTO> ImlMedicineMinDTO { get; set; }
        public DbQuery<ImlMedicineRegisterListDTO> ImlMedicineRegisterListDTO { get; set; }
        public DbQuery<ImlLicenseRegisterDetailDTO> ImlLicenseRegisterDetailDTO { get; set; }
        public DbQuery<ImlLicenseRegisterListDTO> ImlLicenseRegisterListDTO { get; set; }
        #endregion

        #region Msg

        public DbSet<Models.MSG.Message> Messages { get; set; }

        public DbQuery<DTO.MSG.MessageListDTO> MessageListDTO { get; set; }
        public DbQuery<DTO.MSG.SgdChiefNameChangeMessageDTO> SgdChiefNameChangeMessageDTO { get; set; }

        public DbQuery<DTO.MSG.SgdNameChangeMessageDTO> SgdNameChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.OrgFopLocationChangeMessageDTO> OrgFopLocationChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MPDActivitySuspensionMessageDTO> MPDActivitySuspensionMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MPDActivityRestorationMessageDTO> MPDActivityRestorationMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MPDClosingForSomeActivityMessageDTO> MPDClosingForSomeActivityMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MPDRestorationAfterSomeActivityMessageDTO> MPDRestorationAfterSomeActivityMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MPDLocationRatificationMessageDTO> MPDLocationRatificationMessageDTO { get; set; }
        public DbQuery<DTO.MSG.PharmacyHeadReplacementMessageDTO> PharmacyHeadReplacementMessageDTO { get; set; }
        public DbQuery<DTO.MSG.PharmacyNameChangeMessageDTO> PharmacyNameChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.PharmacyAreaChangeMessageDTO> PharmacyAreaChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.LeaseAgreementChangeMessageDTO> LeaseAgreementChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.ProductionDossierChangeMessageDTO> ProductionDossierChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.SupplierChangeMessageDTO> SupplierChangeMessageDTO { get; set; }
        public DbQuery<DTO.MSG.AnotherEventMessageDTO> AnotherEventMessageDTO { get; set; }
        public DbQuery<DTO.MSG.MsgShortDTO> MsgShortDTO { get; set; }

        #endregion

        #region Common

        public DbSet<LimsDoc> LimsDocs { get; set; }

        public DbSet<FileStore> FileStore { get; set; }
        public DbQuery<FileStoreDTO> FileStoreDTO { get; set; }

        public DbQuery<UserDetailsDTO> UserDetailsDTO { get; set; }

        public DbQuery<EntityEnumDTO> EntityEnumDTO { get; set; }


        public DbQuery<WidgetBackDTO> WidgetBackDTO { get; set; }
        public DbQuery<WidgetPaymentDTO> WidgetPaymentDTO { get; set; }

        public DbQuery<CurrentLicenseDTO> CurrentLicenseDTO { get; set; }
        #endregion

        #region Atu

        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityDistricts> CityDistricts { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<SubjectAddress> SubjectAddresses { get; set; }

        public DbQuery<AtuCityDTO> AtuCityDTO { get; set; }
        public DbQuery<AtuStreetDTO> AtuStreetDTO { get; set; }

        public DbQuery<AtuCityListDTO> AtuCityListDTO { get; set; }
        public DbQuery<AtuCitySelectDTO> AtuCitySelectDTO { get; set; }
        public DbQuery<AtuRegionListDTO> AtuRegionListDTO { get; set; }
        public DbQuery<AtuRegionSelectDTO> AtuRegionSelectDTO { get; set; }
        public DbQuery<AtuSubjectAddressDTO> AtuSubjectAddressDTO { get; set; }

        #endregion

        #region RPT

        public DbQuery<ReportBranchFullDetailsDTO> ReportBranchFullDetailsDTO { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbQuery<NotificationListDTO> NotificationListDTOs { get; set; }
        public DbQuery<NotificationDetailsDTO> NotificationDetailsDTOs { get; set; }
        public DbQuery<NotificationRecipientDTO> NotificationRecipientDTOs { get; set; }
        public DbQuery<OrgRptNameField> OrgRptNameFields { get; set; }
        public DbQuery<OrgUnitRptMinDetail> OrgUnitRptMinDetails { get; set; }
        public DbQuery<MPDAddressRptMinDetail> MPDAddressRptMinDetails { get; set; }
        public DbQuery<LicenseRptMinDetailSgdChiefName> LicenseRptMinDetailsSgdChiefNames { get; set; }
        public DbQuery<LicenseRptMinDetailOrgFopLocation> LicenseRptMinDetailsOrgFopLocations { get; set; }
        public DbQuery<MessageTypeDTO> MessageTypeDTOs { get; set; }
        public DbQuery<OrgInnEdrpouRptMinDetail> OrgInnEdrpouRptMinDetails { get; set; }

        #endregion

        #region UserArea

        public DbQuery<UserAreaOrgDetailDTO> UserAreaOrgDetailDTOs { get; set; }
        public DbQuery<UserAreaEmployeeDetailDTO> UserAreaEmployeeDetailDTOs { get; set; }
        public DbQuery<UserAreaPersonDetailDTO> UserAreaPersonDetailDTOs { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbQuery<FeedBackDetailsDTO> FeedBackDetailsDTOs { get; set; }

        #endregion

        #region Audit

        public DbQuery<LogAuditEntryListDTO> LogAuditEntryListDTO { get; set; }
        public DbQuery<LogAuditListOfChangesDTO> LogAuditListOfChangesDTO { get; set; }

        #endregion

        #region CRV

        public DbSet<LimsRP> LimsRP { get; set; }
        public DbQuery<LimsListRPDTO> LimsListRPDTO { get; set; }
        public DbQuery<LimsRPMinDTO> LimsRPMinDTO { get; set; }
        public DbQuery<LimsDetailsRPDTO> LimsDetailsRPDTO { get; set; }

        #endregion

        #region P902

        public DbSet<ResultInputControl> ResultInputControls { get; set; }
        public DbSet<ConclusionMedicine> ConclusionMedicines { get; set; }
        public DbSet<AppConclusion> AppConclusions { get; set; }

        public DbQuery<ResultInputControlDetailsDTO> ResultInputControlDetailsDTO { get; set; }
        public DbQuery<ResultInputControlListDTO> ResultInputControlListDTO { get; set; }

        public DbQuery<AppConclusionListDTO> AppConclusionsListDTO { get; set; }

        #endregion

        public DbSet<DepartmentalSubordination> DepartmentalSubordinations { get; set; }
        public DbSet<EntityEnumRecords> EntityEnumRecordses { get; set; }

        public MigrationDbContext() : base() { }

        public MigrationDbContext(DbContextOptions<MigrationDbContext> options) : base(options) { }

        public MigrationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppDecision>().HasAlternateKey(p => p.AppId);
            builder.Entity<AppLicenseMessage>().HasAlternateKey(p => new { p.AppId, p.MessageNumber });
            builder.Entity<AppPreLicenseCheck>().HasAlternateKey(p => p.AppId);
            //builder.Entity<PrlApplication>().HasAlternateKey(p => p.RegNumber);
            builder.Entity<Models.MSG.Message>().HasIndex(p => p.RegNumber).IsUnique();
            builder.Entity<LimsRP>().HasIndex(x => x.EndDate);
            builder.Entity<ImlMedicine>().HasIndex(x => x.ApplicationId);

            builder.Entity<City>().HasIndex(p => new { p.Name, p.Code });

            var useSnakeCase = true; // TODO: get this variable from service or from somewhere else
            if (useSnakeCase)
            {
                // from Pascal case to snake case
                foreach (var entity in builder.Model.GetEntityTypes())
                {
                    // Replace table names
                    entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                    // Replace column names            
                    foreach (var property in entity.GetProperties())
                    {
                        property.Relational().ColumnName = property.Name.ToSnakeCase();
                    }

                    foreach (var key in entity.GetKeys())
                    {
                        key.Relational().Name = key.Relational().Name.ToSnakeCase();
                    }

                    foreach (var key in entity.GetForeignKeys())
                    {
                        key.Relational().Name = key.Relational().Name.ToSnakeCase();
                    }

                    foreach (var index in entity.GetIndexes())
                    {
                        index.Relational().Name = index.Relational().Name.ToSnakeCase();
                    }
                }
            }
        }
    }
}
