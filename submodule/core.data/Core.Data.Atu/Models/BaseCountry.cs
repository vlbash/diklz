using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник країн")]
    [Display(Name = "Довідник країн")]
    [Table("AtuCountry")]
    public abstract class BaseCountry : BaseEntity
    {
        [MaxLength(64)]
        public virtual string Code { get; set; }
    }
}
