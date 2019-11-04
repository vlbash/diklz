using System.Data.Entity;
using Astum.Core.Common.Entities;
using App.Data.Entities;
using App.Data.Entities.MTA;
using App.Data.Entities.ONL;
using Astum.Core.Common.Enums;
using SocServ.Data.Entities.MTA;
using SocServ.Data.Models.MTA;
using SocServ.Data.Models.CLI;
using Astum.Core.Data.DomainModule.ATU;
using App.Data.Entities.ADM;
using App.Data.Entities.ORG;
using Astum.Core.Data.Entities.ORG;
using SocServ.Data.Models.ORG;
using SocServ.Data.Entities.ORG;

namespace EF6.Diagram
{
    class AdmToolsDbContext : DbContext
    {
        public AdmToolsDbContext()
            : base("DbConnection")
        { }

        public DbSet<Person> Persons { get; set; }
        
        #region ATU
        public DbSet<AtuRegion> AtuRegions { get; set; }
        #endregion

        #region ORG
        public DbSet<OrgDepartment> OrgDepartment { get; set; }
        public DbSet<OrgEmployee> OrgEmployee { get; set; }
        public DbSet<OrgOrganization> OrgOrganization { get; set; }
        public DbSet<OrgPosition> OrgPosition { get; set; }
        public DbSet<OrgUnit> OrgUnit { get; set; }
        public DbSet<OrgUnitEmployees> OrgUnitEmployees { get; set; }
        public DbSet<OrgUnitPosition> OrgUnitPosition { get; set; }
        public DbSet<OrgUnitPositionEmployees> OrgUnitPositionEmployees { get; set; }
        #endregion
        
        #region ADM
        public DbSet<AdmRole> AdmRoles { get; set; }
        public DbSet<AdmRight> AdmRights { get; set; }
        public DbSet<AdmProfile> AdmProfiles { get; set; }
        public DbSet<AdmUser> AdmUsers { get; set; }
        #endregion
    }
}
