using System.ComponentModel.DataAnnotations.Schema;
using Z.EntityFramework.Plus;
using App.Core.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Core.Data.Entities.CDN
{
    [AuditInclude]
    [AuditDisplay("Довідник спеціальностей")]
    [Display(Name = "Довідник спеціальностей")]
    [Table("Cdn" + nameof(Speciality))]
    public class Speciality: BaseDictionary
    {
        public Guid? ParentId { get; set; }
        public Speciality Parent { get; set; }
        public ICollection<Speciality> Children { get; set; }
        public List<EmployeeSpeciality> EmployeeSpecialities { get; set; }
    }
}
