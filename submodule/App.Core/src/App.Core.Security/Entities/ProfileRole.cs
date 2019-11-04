using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(ProfileRole))]
    [Display(Name = "Роль у профілі")]
    public class ProfileRole : CoreEntity
    {
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
