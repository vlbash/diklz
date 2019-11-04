using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Common.Models
{
    [Display(Name = "Сутність доданого поля")]
    [Table("EntityExProperty")]
    public abstract class BaseEntityExProperty : BaseEntity
    {
        public virtual Guid EntityId { get; set; }
        public virtual Guid ExPropertyId { get; set; }
        public virtual string Value { get; set; }
        public virtual string ValueEx { get; set; }
    }
}
