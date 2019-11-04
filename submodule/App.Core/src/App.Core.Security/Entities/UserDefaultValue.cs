using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Security.Entities
{
    [Table("Sec" + nameof(UserDefaultValue))]
    [Display(Name = "Дефолтні значення користувача")]
    public class UserDefaultValue: CoreEntity
    {
        public Guid UserId { get; set; }
        public string EntityName { get; set; }
        public Guid ValueId { get; set; }
    }
}
