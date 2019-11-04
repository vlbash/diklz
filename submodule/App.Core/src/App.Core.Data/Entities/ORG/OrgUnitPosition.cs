using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.CDN;
using App.Core.Data.Entities.Common;

namespace App.Core.Data.Entities.ORG
{
    [Display(Name = "Посади у базовій організації")]
    public class OrgUnitPosition : BaseEntity
    {
        public Guid OrgUnitId { get; set; }
		public OrgUnit OrgUnit { get; set; }
		public Guid PositionId { get; set; }
		public Position Position { get; set; }
        [DisplayName("Тип посади")]
        public string PositionType { get; set; }
        public bool IsResource { get; set;}
        [NotMapped]
        public string PositionTypeName { get; set; }
        public ICollection<Employee> Employees { get; set; }

        //[NotMapped]
        //[DisplayName("Назва")]
        //public string Name
        //{
        //    get => string.Format("{0}-{1}", OrgUnit.Name, OrgPosition.Name);
        //    set { }
        //}
    }
}
