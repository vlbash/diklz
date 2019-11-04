using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(Role))]
    [Display(Name = "Реєстр ролей")]
    public class Role : CoreEntity
    {
        public bool IsActive { get; set; }

        public List<ProfileRole> Profiles { get; set; }
        public List<RoleRight> Rights { get; set; }
    }
}
