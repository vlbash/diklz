using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Data.Interfaces;

namespace App.Core.Data.Entities.ORG
{
    [Display(Name = "Базова організація")]
    public sealed class OrgUnit : BaseEntity, IOrgUnit, IDerivableEntity
    {
        public string DerivedClass { get; set; }

        [DisplayName("Назва")]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        
        public Guid? SubjectAddressId { get; set; }
        public SubjectAddress SubjectAddress { get; set; }
        
        public string Description { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
    }
}
