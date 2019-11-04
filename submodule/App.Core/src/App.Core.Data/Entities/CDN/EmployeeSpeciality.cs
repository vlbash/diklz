using System.ComponentModel.DataAnnotations.Schema;
using Z.EntityFramework.Plus;
using App.Core.Data.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.ORG;

namespace App.Core.Data.Entities.CDN
{
    [AuditInclude]
    [AuditDisplay("Довідник спеціальностей")]
    [Display(Name = "Довідник спеціальностей")]
    [Table("Cdn" + nameof(EmployeeSpeciality))]
    public class EmployeeSpeciality: BaseDictionary
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid SpecialityId { get; set; }
        public Speciality Speciality { get; set; }
        public bool IsMainSpeciality { get; set; }
    }
}
