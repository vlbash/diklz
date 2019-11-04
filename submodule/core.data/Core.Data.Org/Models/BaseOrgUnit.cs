using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    [Display(Name = "Базова організація")]
    [Table("OrgUnit")]
    public abstract class BaseOrgUnit : BaseEntity, IOrgUnit
    {
        [MaxLength(20)]
        public virtual string Code { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual Guid? SubjectAddressId { get; set; }
        public virtual string Description { get; set; }
        public virtual string State { get; set; }
        public virtual string Category { get; set; }
    }
}
