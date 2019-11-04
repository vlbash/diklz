using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec"+nameof(Profile))]
    [Display(Name = "Профіль")]
    public class Profile : CoreEntity
    {
        public bool IsActive { get; set; }
        public List<ProfileRole> Roles { get; set; }
        public List<ProfileRight> Rights { get; set; }
        public List<RowLevelRight> RowLevelRights { get; set; }
    }
}
