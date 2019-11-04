using System;
using System.Collections.Generic;
using App.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Security.Entities
{
    public class SecurityDbContext: DbContext, ISecurityDbContext
    {
        private static IEnumerable<Type> _applicationModels;

        public DbSet<FieldRight> FieldRights { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileRole> ProfileRoles { get; set; }
        public DbSet<ProfileRight> ProfileRights { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleRight> RoleRights { get; set; }
        public DbSet<ApplicationRowLevelRight> ApplicationRowLevelRights { get; set; }
        public DbSet<RowLevelRight> RowLevelRights { get; set; }
        public DbSet<RowLevelSecurityObject> RowLevelSecurityObjects { get; set; }
        public DbSet<UserProfile> EmployeeProfiles { get; set; }
        public DbSet<UserDefaultValue> EmployeeDefaultValues { get; set; }

        public SecurityDbContext(): base() { }

        public SecurityDbContext(DbContextOptions options) : base(options) { }

        public virtual IEnumerable<Type> GetApplicationModels()
        {
            if (_applicationModels == null) {
                _applicationModels = GetType().GetPropertyGenericArguments(typeof(DbSet<>));
            }

            return _applicationModels;
        }
    }
}
