using Core.Base.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Mis.Models
{
    [Display(Name = "Діагноз")]
    [Table("MisAppointmentEpisode")]
    public abstract class BaseAppointmentEpisode: BaseEntity
    {
        public virtual Guid AppointmentId { get; set; }

        public virtual Guid? EpisodeId { get; set; }

        public virtual string DiagnosisTypeEnum { get; set; }

        public virtual bool IsFirstTimeDetected { get; set; }

        public virtual string DiagnosisSourceTypeEnum { get; set; }

        public virtual Guid? Icpc2Id { get; set; }

        public virtual string ClinicalStatusTypeEnum { get; set; }

        public virtual string VerificationStatusTypeEnum { get; set; }

        public virtual string SeverityStateDegreeEnum { get; set; }

        public virtual DateTime DetectionDate { get; set; }

        public virtual DateTime ConfirmationDate { get; set; }

        public virtual string Description { get; set; }
    }
}
