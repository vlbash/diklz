using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(ProfileRight))]
    [Display(Name = "Права на профіль")]
    public class ProfileRight : CoreEntity
    {
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public Guid RightId { get; set; }
        public Right Right { get; set; }
    }
}
