using System;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.Common;

namespace App.Core.Data.Entities.ORG
{
    [Display(Name = "Співробітники у базовій організації")]
    public class OrgUnitPositionEmployee : BaseEntity
	{
		public Guid OrgUnitPositionId { get; set; }
		public OrgUnitPosition OrgUnitPosition { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
