using App.Core.Data;
using App.Core.AdminTools.Entities;
using App.Core.AdminTools.Entities.JoinTables;
using App.Core.Data.Helpers;
using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Core.AdminTools.Context
{
    public class AdminToolsDbContext: CoreDbContext
    {
        protected AdminToolsDbContext() : base() { }

        public DbSet<AdmProfile> AdmProfiles { get; set; }
        public DbSet<AdmRight> AdmRights { get; set; }
        public DbSet<AdmRole> AdmRoles { get; set; }
        public DbSet<AdmUser> AdmUsers { get; set; }
        public DbSet<AdmUserAdmProfile> AdmUserAdmProfiles { get; set; }
        public DbSet<AdmProfileAdmRole> AdmProfileAdmRoles { get; set; }
        public DbSet<AdmRoleAdmRight> AdmRoleAdmRights { get; set; }
        public DbSet<AdmProfileAtuRegion> AdmProfileAtuRegions { get; set; }
        public DbSet<AdmProfileOrgUnit> AdmProfileOrgUnits { get; set; }

        public AdminToolsDbContext(DbContextOptions<AdminToolsDbContext> options,
            IUserIdentInfo uinfo,
            MemoryCacheHelper cacheHelper)
            : base(options) {

            UserInfo = uinfo;
            CacheHelper = cacheHelper;
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            #region AdmTools
            builder.Entity<AdmUserAdmProfile>()
                .HasKey(k => new { k.AdmUserId, k.AdmProfileId });

            builder.Entity<AdmUserAdmProfile>()
                .HasOne(adm => adm.AdmUser)
                .WithMany(p => p.AdmProfiles)
                .HasForeignKey(pc => pc.AdmUserId);

            builder.Entity<AdmUserAdmProfile>()
                .HasOne(adm => adm.AdmProfile)
                .WithMany(p => p.AdmUsers)
                .HasForeignKey(pc => pc.AdmProfileId);


            builder.Entity<AdmProfileAdmRole>()
                .HasKey(k => new { k.AdmProfileId, k.AdmRoleId });

            builder.Entity<AdmProfileAdmRole>()
                .HasOne(adm => adm.AdmProfile)
                .WithMany(p => p.Roles)
                .HasForeignKey(pc => pc.AdmProfileId);

            builder.Entity<AdmProfileAdmRole>()
                .HasOne(adm => adm.AdmRole)
                .WithMany(p => p.Profiles)
                .HasForeignKey(pc => pc.AdmRoleId);


            builder.Entity<AdmRoleAdmRight>()
                .HasKey(k => new { k.AdmRoleId, k.AdmRightId });

            builder.Entity<AdmRoleAdmRight>()
                .HasOne(adm => adm.AdmRole)
                .WithMany(p => p.Rights)
                .HasForeignKey(pc => pc.AdmRoleId);

            builder.Entity<AdmRoleAdmRight>()
                .HasOne(adm => adm.AdmRight)
                .WithMany(p => p.Roles)
                .HasForeignKey(pc => pc.AdmRightId);

            builder.Entity<AdmProfileAtuRegion>()
                .HasKey(k => new { k.AdmProfileId, k.AtuRegionId });

            builder.Entity<AdmProfileAtuRegion>()
                .HasOne(adm => adm.AdmProfile)
                .WithMany(p => p.Regions)
                .HasForeignKey(pc => pc.AdmProfileId);

            builder.Entity<AdmProfileOrgUnit>()
                .HasKey(k => new { k.AdmProfileId, k.OrgUnitId });

            builder.Entity<AdmProfileOrgUnit>()
                .HasOne(adm => adm.AdmProfile)
                .WithMany(p => p.Owners)
                .HasForeignKey(pc => pc.AdmProfileId);
            #endregion
        }
    }
}
