using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    [Display(Name = "Довідник адрес")]
    [Table("AtuSubjectAddress")]
    public abstract class BaseSubjectAddress : BaseEntity
    {
        public virtual Guid StreetId { get; set; }
        [MaxLength(20)]
        public virtual string PostIndex { get; set; }
        [MaxLength(300)]
        public virtual string Building { get; set; }
        public virtual string AddressType { get; set; }
        public virtual Guid SubjectId { get; set; }
    }
}
