using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;
using Core.Common.Attributes;

namespace Core.Data.Mis.Dto
{
    public abstract class BaseAppointmentEpisodeDetailDto: BaseDto
    {
        public virtual Guid PatientCardId { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [Display(Name = "Прийом")]
        public virtual Guid AppointmentId { get; set; }

        [Display(Name = "Прийом")]
        public virtual string AppointmentCaption { get; set; }

        [Display(Name = "Епізод")]
        public virtual Guid? EpisodeId { get; set; }

        [Display(Name = "Епізод")]
        public virtual string EpisodeCaption { get; set; }

        [Display(Name = "Тип діагнозу")]
        public virtual string DiagnosisTypeEnum { get; set; }

        [Display(Name = "Тип діагнозу")]
        public virtual string DiagnosisType { get; set; }

        [Display(Name = "Виявлений вперше")]
        public virtual bool IsFirstTimeDetected { get; set; }

        [Display(Name = "Джерело")]
        public virtual string DiagnosisSourceTypeEnum { get; set; }

        [Display(Name = "Джерело")]
        public virtual string DiagnosisSourceType { get; set; }

        [Display(Name = "Діагноз за ICPC2")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid Icpc2Id { get; set; }

        [Display(Name = "Діагноз за ICPC2")]
        public virtual string Icpc2Caption { get; set; }

        [Display(Name = "Клінічний статус")]
        public virtual string ClinicalStatusTypeEnum { get; set; }

        [Display(Name = "Клінічний статус")]
        public virtual string ClinicalStatusType { get; set; }

        [Display(Name = "Статус верифікації")]
        public virtual string VerificationStatusTypeEnum { get; set; }

        [Display(Name = "Статус верифікації")]
        public virtual string VerificationStatusType { get; set; }

        [Display(Name = "Ступінь тяжкості")]
        public virtual string SeverityStateDegreeEnum { get; set; }

        [Display(Name = "Ступінь тяжкості")]
        public virtual string SeverityStateDegree { get; set; }

        [DocumentDate]
        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Дата початку")]
        public virtual DateTime? DetectionDate { get; set; } = DateTime.Now;

        [DocumentDate]
        [Display(Name = "Дата завершення")]
        public virtual DateTime? ConfirmationDate { get; set; } = DateTime.Now;

        [Display(Name = "Коментар")]
        public virtual string Description { get; set; }
    }

    public abstract class BaseAppointmentEpisodeListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [Display(Name = "Епізод")]
        public virtual Guid? EpisodeId { get; set; }

        [Display(Name = "Епізод")]
        public virtual string EpisodeCaption { get; set; }

        [Display(Name = "Тип діагнозу")]
        public virtual string DiagnosisTypeEnum { get; set; }

        [Display(Name = "Тип діагнозу")]
        public virtual string DiagnosisType { get; set; }

        [Display(Name = "Виявлений вперше")]
        public virtual bool IsFirstTimeDetected { get; set; }

        [Display(Name = "Джерело")]
        public virtual string DiagnosisSourceTypeEnum { get; set; }

        [Display(Name = "Джерело")]
        public virtual string DiagnosisSourceType { get; set; }

        [Display(Name = "Діагноз за ICPC2")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid Icpc2Id { get; set; }

        [Display(Name = "Діагноз за ICPC2")]
        public virtual string Icpc2Caption { get; set; }

        [Display(Name = "Ступінь тяжкості")]
        public virtual string SeverityStateDegreeEnum { get; set; }

        [Display(Name = "Ступінь тяжкості")]
        public virtual string SeverityStateDegree { get; set; }

        [DocumentDate]
        [Display(Name = "Дата початку")]
        public virtual DateTime? DetectionDate { get; set; } = DateTime.Now;

        [DocumentDate]
        [Display(Name = "Дата завершення")]
        public virtual DateTime? ConfirmationDate { get; set; } = DateTime.Now;
    }
}
