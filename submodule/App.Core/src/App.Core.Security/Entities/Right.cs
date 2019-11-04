using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(Right))]
    [Display(Name = "Реєстр прав")]
    public class Right : CoreEntity
    {
        [MaxLength(64)]
        public string EntityName { get; set; }
        public EntityAccessLevel EntityAccessLevel { get; set; } = EntityAccessLevel.No;

        public bool IsActive { get; set; }

        public List<FieldRight> FieldRights { get; set; }
        public List<RoleRight> Roles { get; set; }
        public List<ProfileRight> Profiles { get; set; }
    }
}
