using System;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.Entities.Common
{
    [Display(Name = "Сутність доданого поля")]
    public class EntityExProperty : BaseEntity
    {
        public Guid EntityId { get; set; }
        public Guid ExPropertyId { get; set; }
        public string Value { get; set; }
        public string ValueEx { get; set; }
    }
}
