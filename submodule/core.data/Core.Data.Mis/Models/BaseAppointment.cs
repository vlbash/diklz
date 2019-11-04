using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Table("MisAppointment")]
    public abstract class BaseAppointment : BaseEntity
    {
        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual DateTime SignatureDate { get; set; }

        public virtual string EventStateEnum { get; set; }

        public virtual string InteractionTypeEnum { get; set; }

        public virtual string Description { get; set; }

        public virtual string DoctorRecomendation { get; set; }

        public virtual string Complaints { get; set; }

        public virtual Guid DoctorId { get; set; }

        public virtual Guid DoctorSpecialityId { get; set; }

        public virtual Guid PatientCardId { get; set; }

        public virtual bool IsFirst { get; set; }
    }
}
