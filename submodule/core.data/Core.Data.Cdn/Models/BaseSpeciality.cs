using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Cdn.Models
{
    [Display(Name = "Довідник спеціальностей")]
    [Table("CdnSpeciality")]
    public abstract class BaseSpeciality: BaseDirectory
    {
        public virtual Guid? ParentId { get; set; }
    }
}
