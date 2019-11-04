using Microsoft.EntityFrameworkCore;
using App.Core.Security.Entities;
using System.Collections.Generic;
using System;
using App.Core.Common;

namespace App.Core.Security.Entities
{
    public interface ISecurityDbContext: IApplicationModels
    {
        DbSet<FieldRight> FieldRights { get; set; }
        DbSet<ProfileRole> ProfileRoles { get; set; }
        DbSet<ProfileRight> ProfileRights { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Right> Rights { get; set; }
        DbSet<RoleRight> RoleRights { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<ApplicationRowLevelRight> ApplicationRowLevelRights { get; set; }
        DbSet<RowLevelRight> RowLevelRights { get; set; }
        DbSet<RowLevelSecurityObject> RowLevelSecurityObjects { get; set; }
        DbSet<UserProfile> EmployeeProfiles { get; set; }
        DbSet<UserDefaultValue> EmployeeDefaultValues { get; set; }
    }
}
