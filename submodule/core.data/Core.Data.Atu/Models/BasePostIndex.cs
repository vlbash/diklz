using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник поштових індексів")]
    [Display(Name = "Довідник поштових індексів")]
    [Table("AtuPostIndex")]
    public abstract class BasePostIndex: BaseEntity
    {
        public virtual Guid CityId { get; set; }
        public virtual string PostIndexStr { get; set; }
    }
}
