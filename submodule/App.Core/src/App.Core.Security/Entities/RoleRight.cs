using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(RoleRight))]
    [Display(Name = "Права у данній ролі")]
    public class RoleRight : CoreEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public Guid RightId { get; set; }
        public Right Right { get; set; }
    }
}
