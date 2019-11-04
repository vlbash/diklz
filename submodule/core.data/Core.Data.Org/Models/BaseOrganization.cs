using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    //[AuditInclude]
    //[AuditDisplay("Організація")]
    [Display(Name = "Організація")]
    [Table("OrgOrganization")]
    //[RlsRight(nameof(Organization), nameof(Id))]
    public abstract class BaseOrganization : BaseEntity, IOrgUnit
    {
        //IOrgUnit
        [Display(Name = "Повна назва")]
        public virtual string FullName { get; set; }
        [MaxLength(20)]
        public virtual string Code { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual string Description { get; set; }
        public virtual string State { get; set; }
        public virtual string Category { get; set; }
    }
}
