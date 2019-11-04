using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    //[AuditInclude]
    //[AuditDisplay("Підрозділ")]
    [Display(Name = "Підрозділ")]
    [Table("OrgDepartment")]
    public abstract class BaseDepartment : BaseEntity, IOrgUnit
    {
        //IOrgUnit
        [Display(Name = "Коротка назва")]
        public virtual string ShortName { get; set; }
        [MaxLength(20)]
        public virtual string Code { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual string Description { get; set; }
        public virtual string State { get; set; }
        public virtual string Category { get; set; }
    }
}
