using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    
    //[AuditInclude]
    //[AuditDisplay("Довідник областей")]
    [Display(Name = "Довідник областей")]
    [Table("AtuRegion")]
    //[RlsRight(nameof(Region), nameof(Id))]
    public abstract class BaseRegion : BaseEntity
    {
        [Display(Name = "Посилання на батьківський запис")]
        public virtual Guid? ParentId { get; set; }

        [MaxLength(200)]
        public virtual string Name { get; set; }
        [MaxLength(64)]
        public virtual string Code { get; set; }

        public virtual Guid CountryId { get; set; }

        [MaxLength(15)]
        [Display(Name = "КОАТУУ")]
        public virtual string KOATUU { get; set; }
    }
}
