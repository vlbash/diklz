using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник міст")]
    [Display(Name = "Довідник міст")]
    [Table("AtuCity")]
    //[RlsRight(nameof(Region), nameof(RegionId))]
    public abstract class BaseCity : BaseEntity
	{
	    public virtual string Code { get; set; }
	    public virtual string TypeEnum { get; set; }
        public virtual Guid RegionId { get; set; }
    }
}
