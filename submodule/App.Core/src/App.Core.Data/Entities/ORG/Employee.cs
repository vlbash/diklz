using System;
using System.ComponentModel;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Interfaces;
using Person = App.Core.Data.Entities.Common.Person;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.CDN;
using App.Core.Security;

namespace App.Core.Data.Entities.ORG
{
    [AuditInclude]
    [AuditDisplay("Співробітник")]
    [Display(Name = "Співробітник")]
    [Table("Org" + nameof(Employee))]
    [RlsRight(nameof(Organization), nameof(OrganizationId))]
    public class Employee : BaseEntity, IEmployee
    {
        [DisplayName("Персона")]
        public Guid PersonId { get; set; }
        [DisplayName("Персона")]
        public Person Person { get; set; }
        
        //[Obsolete("Employee property is obsolete. Use " + nameof(OrganizationId) + " instead", true)]
        //public Guid? OrgUnitId { get; set; }
        //[Obsolete("Employee property is obsolete. Use " + nameof(Organization) + " instead", true)]
        //public OrgUnit OrgUnit { get; set; }

        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public List<EmployeeSpeciality> EmployeeSpecialities { get; set; }
    }
}
