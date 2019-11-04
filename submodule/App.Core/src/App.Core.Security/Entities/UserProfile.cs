using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(UserProfile))]
    [Display(Name = "Профіль співробітника")]
    public class UserProfile : CoreEntity
    {
        public Guid UserId { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
