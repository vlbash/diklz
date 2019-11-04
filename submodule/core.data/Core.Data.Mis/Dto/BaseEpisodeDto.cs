using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Mis.Dto
{
    public abstract class BaseEpisodeListDto: BaseDto, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        public virtual Guid DoctorId { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [Display(Name = "№ епізоду")]
        public virtual string Number { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; } = null;

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid PatientCardId { get; set; }

        public virtual string EventStateEnum { get; set; }

        [Display(Name = "Стан епізоду")]
        public virtual string EventState { get; set; }

        [NotMapped]
        [Display(Name = "Період дат")]
        public virtual string Terms
        {
            get
            {
                if (EventStateEnum.Equals("Finished"))
                {
                    return StartDate.Value.ToString("dd.MM.yyyy") + " - " + EndDate.Value.ToString("dd.MM.yyyy");
                }
                return StartDate.Value.ToString("dd.MM.yyyy");
            }
        }

        [Display(Name = "Примітки")]
        public virtual string Description { get; set; }
    }

    public abstract class BaseEpisodeDetailDto: BaseDto
    {
        [Display(Name = "Пацієнт")]
        public virtual string PersonFullName { get; set; }

        public virtual string PersonPhone { get; set; }

        [Display(Name = "№ епізоду")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string Number { get; set; }

        [DocumentDate]
        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Дата початку")]
        public virtual DateTime? StartDate { get; set; } = DateTime.Now;

        [DocumentDate]
        [Display(Name = "Дата завершення")]
        public virtual DateTime? EndDate { get; set; } = DateTime.Now;


        [Display(Name = "Стан епізоду")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string EventStateEnum { get; set; } = "Started";

        [Display(Name = "Стан епізоду")]
        public virtual string EventState { get; set; }

        [Display(Name = "Пацієнт")]
        public virtual Guid PatientCardId { get; set; }

        [Display(Name = "Лікар")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid DoctorId { get; set; }

        public virtual Guid OrganizationId { get; set; }

        [Display(Name = "Лікар")]
        public virtual string DoctorFullName { get; set; }

        [Display(Name = "Примітки")]
        public virtual string Description { get; set; }

        [NotMapped]
        [Display(Name = "Період дат")]
        public virtual string Terms 
        {
            get
            {
                if (EventStateEnum != null && EventStateEnum.Equals("Finished"))
                {
                    return StartDate.Value.ToString("dd.MM.yyyy") + " - " + EndDate.Value.ToString("dd.MM.yyyy");
                }
                return StartDate.Value.ToString("dd.MM.yyyy");
            }
        }
    }

    public abstract class BaseEpisodeMinDto: BaseDto
    {
        public virtual Guid OrganizationId { get; set; }

        public virtual Guid PatientCardId { get; set; }

        public virtual string EventStateEnum { get; set; }
    }
}
