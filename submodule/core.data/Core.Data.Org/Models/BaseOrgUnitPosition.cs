using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Models
{
    [Display(Name = "Посади у базовій організації")]
    [Table("OrgUnitPosition")]
    public abstract class BaseOrgUnitPosition : BaseEntity
    {
        public virtual Guid OrgUnitId { get; set; }
		public virtual Guid PositionId { get; set; }
        [Display(Name = "Тип посади")]
        public virtual string PositionType { get; set; }
        public virtual bool IsResource { get; set;}
        [NotMapped]
        public virtual string PositionTypeName { get; set; }

        //[NotMapped]
        //[Display(Name = "Назва")]
        //public string Name
        //{
        //    get => string.Format("{0}-{1}", OrgUnit.Name, OrgPosition.Name);
        //    set { }
        //}
    }
}
