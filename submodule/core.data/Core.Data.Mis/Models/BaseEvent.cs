using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Table("MisEvent")]
    public abstract class BaseEvent: BaseEntity, IEvent
    {
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string EventStateEnum { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual string Description { get; set; }
    }
}
