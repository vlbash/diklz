/*using EF6.Diagram.Cost.Entities.PROJ;
using EF6.Diagram.Cost.Entities.PROJ.JoinTables;*/
using System.Data.Entity;
/*using EF6.Diagram.Cost.Entities.ADM;
using EF6.Diagram.Cost.Entities.ADM.JoinTables;
using EF6.Diagram.Cost.Entities.ATU;
using EF6.Diagram.Cost.Entities.CPL;
using EF6.Diagram.Cost.Entities.ORG;
using EF6.Diagram.Cost.Entities.RES;*/

using App.Data.Entities.ACW;
using App.Data.Entities.CDN;
using App.Data.Entities.Common;
using App.Data.Entities.CPL;
using App.Data.Entities.GWS;
using App.Data.Entities.PRJ;
using App.Data.Entities.REP;
using App.Data.Entities.RES;
using Astum.Core.Data.Entities.ATU;

namespace EF6.Diagram.Cost
{
    class TRCDbContext : DbContext
    {
        public TRCDbContext()
            : base("DbConnection")
        {
        }

        #region ADM

        /*public DbSet<AdmProfile> AdmProfiles { get; set; }
        public DbSet<AdmRight> AdmRights { get; set; }
        public DbSet<AdmRole> AdmRoles { get; set; }
        public DbSet<AdmUser> AdmUsers { get; set; }

        public DbSet<AdmProfileAdmRole> AdmProfileAdmRole { get; set; }
        public DbSet<AdmProfileAtuRegion> AdmProfileAtuRegions { get; set; }
        public DbSet<AdmProfileOrgUnit> AdmProfileOrgUnits { get; set; }
        public DbSet<AdmRoleAdmRight> AdmRoleAdmRights { get; set; }
        public DbSet<AdmUserAdmProfile> AdmUserAdmProfiles { get; set; }*/

        #endregion

        #region PROJ
        /*public DbSet<ProjProject> ProjProjects { get; set; }
        public DbSet<ProjProjectAtuCoordinate> ProjProjectAtuCoordinates { get; set; }
        public DbSet<ProjContract> ProjContracts { get; set; }
        public DbSet<ProjObjectCharacteristic> ProjObjectCharacteristics { get; set; }
        public DbSet<ProjParticipant> ProjParticipants { get; set; }*/
        #endregion

        /*#region CPL
        public DbSet<CplCalendarPlan> CplCalendarPlans { get; set; }
        public DbSet<CplCalendarPlanStage> CplCalendarPlanStages { get; set; }
        public DbSet<CplCalendarPlanWork> CplCalendarPlanWorks { get; set; }
        public DbSet<DictionaryWorkType> WorkTypeDictionary { get; set; }
        #endregion*/

        #region RES
        /*public DbSet<ResResourceStatement> ResResourceStatements { get; set; }
        public DbSet<ResPlannedResource> ResPlannedResources { get; set; }
        public DbSet<DictionaryResource> DictionaryResources { get; set; }*/
        #endregion

        #region ATU

        /*public DbSet<AtuRegion> AtuRegions { get; set; }
        public DbSet<AtuCoordinate> AtuCoordinates { get; set; }*/

        #endregion

        #region ORG
        /*public DbSet<OrgDepartment> OrgDepartments { get; set; }
        public DbSet<OrgEmployee> OrgEmployees { get; set; }
        public DbSet<OrgExtraProperty> OrgExtraProperties { get; set; }
        public DbSet<OrgOrganization> OrgOrganizations { get; set; }
        public DbSet<OrgPosition> OrgPositions { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<OrgUnitAtuAddress> OrgUnitAtuAddress { get; set; }        
        public DbSet<OrgUnitEmployee> OrgUnitEmployees { get; set; }
        public DbSet<OrgUnitPosition> OrgUnitPositions { get; set; }
        public DbSet<OrgUnitPositionEmployees> OrgUnitPositionEmployees { get; set; }
        public DbSet<OrgUnitSpecialization> OrgUnitSpecialization { get; set; }        */
        #endregion
        #region CurentEntities

        public DbSet<ActOfCompletedWork> ActOfCompletedWork { get; set; }

        public DbSet<AtuRegion> AtuRegions { get; set; }
        public DbSet<AtuCoordinate> AtuCoordinates { get; set; }

        public DbSet<CdnUnitedPurchase> CdnUnitedPurchase { get; set; }
        public DbSet<CdnUnitOfMeasurement> CdnUnitOfMeasurement { get; set; }
        public DbSet<CdnRecordStatus> CdnRecordStatus { get; set; }
        public DbSet<CdnWorkType> CdnWorkType { get; set; }

        public DbSet<ConstructionObject> ConstructionObject { get; set; }

        public DbSet<CplCalendarPlan> CplCalendarPlan { get; set; }
        public DbSet<CplCalendarPlanStage> CplCalendarPlanStage { get; set; }
        public DbSet<CplCalendarPlanWork> CplCalendarPlanWork { get; set; }

        public DbSet<GeneralWorksheet> GeneralWorksheet { get; set; }
        public DbSet<GwsPerformanceReport> GwsPerformanceReport { get; set; }

        public DbSet<PrjContract> PrjContract { get; set; }
        public DbSet<PrjParticipant> PrjParticipant { get; set; }
        public DbSet<Project> Project { get; set; }

        public DbSet<ReportType> ReportType { get; set; }

        public DbSet<Resource> Resource { get; set; }
        public DbSet<ResPlannedResource> ResPlannedResource { get; set; }
        public DbSet<ResResourceStatement> ResResourceStatement { get; set; }
        #endregion
    }
}
