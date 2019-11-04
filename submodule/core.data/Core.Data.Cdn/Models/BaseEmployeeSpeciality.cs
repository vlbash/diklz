using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Cdn.Models
{
    [Display(Name = "Довідник спеціальностей")]
    [Table("CdnEmployeeSpeciality")]
    public abstract class BaseEmployeeSpeciality: BaseEntity
    {
        public virtual Guid EmployeeId { get; set; }
        public virtual Guid SpecialityId { get; set; }
        public virtual bool IsMainSpeciality { get; set; }
    }
}
