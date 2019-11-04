using App.Core.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using App.Core.Data.CustomAudit;
using System;
using System.Linq;
using System.Collections.Generic;
using Z.EntityFramework.Plus;
using App.Core.Data.Entities.Common;
using App.Core.Data.DTO.Common;
using App.Core.Common;
using App.Core.Common.Extensions;
using App.Core.Business.Services;

namespace App.Core.Data
{
    public class CoreDbContext: DbContext, IApplicationModels
    {
        protected CoreDbContext() {
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
               (context as CoreDbContext)?.AuditEntries.AddRange(audit.Entries.Cast<AuditEntry_Extended>());

            AuditManager.DefaultConfiguration.DataAnnotationDisplayName();
        }

        public CoreDbContext(DbContextOptions options) : base(options) { }

        public IDocumentHelper DocumentHelper { get; set; }

        private static IEnumerable<Type> _applicationModels;

        #region Audit entities
        public UserInfo UserInfo { get; set; }
        //public DbSet<AuditEntry_Extended> AuditEntries { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
        #endregion

        #region Common Entities
        //public DbSet<Owner> Owners { get; set; }
        //public DbSet<App.Core.Data.Entities.ATU.TimeZone> TimeZones { get; set; }
        public DbSet<EnumRecord> EnumRecord { get; set; }
        public DbQuery<EnumRecordDto> EnumRecordDto { get; set; }
        public DbSet<ExProperty> ExProperty { get; set; }
        public DbQuery<ExPropertyDTO> ExPropertyDTO { get; set; }

        public DbSet<EntityExProperty> EntityExProperty { get; set; }

        public DbSet<ApplicationSetting> ApplicationSetting { get; set; }
        public DbSet<ApplicationSettingValue> ApplicationSettingValue { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<EnumRecord>().HasIndex(b => new { b.EnumType, b.Code }).IsUnique(true);
        }

        public virtual IEnumerable<Type> GetApplicationModels()
        {
            if (_applicationModels == null) {
                _applicationModels = GetType().GetPropertyGenericArguments(typeof(DbSet<>));
            }

            return _applicationModels;
        }
    }
}
