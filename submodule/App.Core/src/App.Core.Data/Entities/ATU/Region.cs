using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using App.Core.Security;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    
    [AuditInclude]
    [AuditDisplay("Довідник областей")]
    [Display(Name = "Довідник областей")]
    [Table("Atu" + nameof(Region))]
    [RlsRight(nameof(Region), nameof(Id))]
    public class Region : BaseEntity
    {
        public Region()
        {
            Children = new HashSet<Region>();
        }

        [DisplayName("Посилання на батьківський запис")]
        public Guid? ParentId { get; set; }
        public Region Parent { get; set; }
        public ICollection<Region> Children { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }

        public List<City> Cities { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }

        [MaxLength(15)]
        [DisplayName("КОАТУУ")]
        public string KOATUU { get; set; }
    }
}
