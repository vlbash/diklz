using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Mis.Dto
{
    public abstract class BaseAppointmentListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid OrganizationId { get; set; }

        [CaseFilter(CaseFilterOperation.Overlaps, Group = "Period")]
        [Display(Name = "Дата початку")]
        public virtual DateTime StartDate { get; set; }
        
        [NotMapped]
        [Display(Name = "Дата та час початку")]
        public virtual string DateAndTime
        {
            get
            {
                return StartDate.ToString("dd'.'MM'.'yyyy' 'H:mm");
            }
        }

        [CaseFilter(CaseFilterOperation.Overlaps, Group = "Period")]
        [Display(Name = "Дата завершення")]
        public virtual DateTime EndDate { get; set; }

        [Display(Name = "Лікар")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string DoctorName { get; set; }

        [Display(Name = "Лікар")]
        public virtual string DoctorNameSpec { get; set; }

        [Display(Name = "Спеціалізація")]
        public virtual string DoctorSpeciality { get; set; }

        [Display(Name = "Стан консультації")]
        public virtual string EventState { get; set; }

        [Display(Name = "Картка пацієнта")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid? PatientCardId { get; set; }

        [Display(Name = "Картка пацієнта")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PatientCardCaption { get; set; }
    }

    public abstract class BaseAppointmentDetailDto: BaseDto
    {
        public virtual string PersonFullName { get; set; }

        public virtual string PersonPhone { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Дата та час початку")]
        public virtual DateTime StartDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Дата та час початку")]
        public virtual string BeginDateAndTime 
        {
            get
            {
                return StartDate.ToString("dd'.'MM'.'yyyy' 'H:mm");
            }
        }

        [Display(Name = "Дата та час завершення")]
        public virtual DateTime EndDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Дата та час завершення")]
        public virtual string EndDateAndTime
        {
            get
            {
                return EndDate.ToString("dd'.'MM'.'yyyy' 'H:mm");
            }
        }

        [Display(Name = "Дата та час підпису")]
        public virtual DateTime SignatureDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Дата та час підпису")]
        public virtual string SignatureDateAndTime
        {
            get
            {
                return SignatureDate.ToString("dd'.'MM'.'yyyy' 'H:mm");
            }
        }

        [Display(Name = "Картка пацієнта")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid PatientCardId { get; set; }
        [Display(Name = "Картка пацієнта")]
        public virtual string PatientCardCaption { get; set; }

        [Display(Name = "Рекомендації")]
        public virtual string DoctorRecomendation { get; set; }

        [Display(Name = "Скарги")]
        public virtual string Complaints { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Лікар")]
        public virtual Guid DoctorId { get; set; }
        [Display(Name = "Лікар")]
        public virtual string DoctorName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Спеціалізація")]
        public virtual Guid DoctorSpecialityId { get; set; }
        [Display(Name = "Спеціалізація")]
        public virtual string DoctorSpecialityName { get; set; }

        [Display(Name = "Первинна")]
        public virtual bool IsFirst { get; set; } = true;

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Тип взаємодії")]
        public virtual string InteractionTypeEnum { get; set; }
        [Display(Name = "Тип взаємодії")]
        public virtual string InteractionType { get; set; }

        [Display(Name = "Примітки")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Стан консультації")]
        public virtual string EventStateEnum { get; set; } = "Proceed";
        [Display(Name = "Стан консультації")]
        public virtual string EventState { get; set; }

        [NotMapped]
        [Display(Name = "Стан")]
        public virtual string EventStateWithTime
        {
            get
            {
                if (EventStateEnum != "Finished") {
                    return EventState;
                }
                TimeSpan time = EndDate - StartDate;
                var timeString = " " + time.Minutes + "хв. " + time.Seconds + "сек.";
                return EventState + timeString;
            }
        }

    }
}
