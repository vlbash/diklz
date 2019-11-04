using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Display(Name = "Класифікатор первинної медичної допомоги")]
    [Table("CdnIcpc2")]
    public abstract class BaseIcpc2: BaseEntity
    {
        public virtual string Code { get; set; }

        public virtual Guid? ParentId { get; set; }

        public virtual Guid? Icpc2GroupId { get; set; }

        public virtual bool IsAction { get; set; }

        public virtual bool IsSymptom { get; set; }

        public virtual bool IsDiagnosis { get; set; }

        public virtual bool IsReason { get; set; }

        public virtual string Description { get; set; }
    }
}
