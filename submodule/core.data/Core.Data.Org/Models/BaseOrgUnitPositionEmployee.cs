using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    [Display(Name = "Співробітники у базовій організації")]
    [Table("OrgUnitPositionEmployee")]
    public class BaseOrgUnitPositionEmployee : BaseEntity
	{
		public virtual Guid OrgUnitPositionId { get; set; }
        public virtual Guid EmployeeId { get; set; }
    }
}
