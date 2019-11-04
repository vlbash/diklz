using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Mis.Dto
{
    public abstract class BaseDoctorDetailDto : BaseDto
    {
        public virtual Guid? PersonId { get; set; }

        [Display(Name = "П.І.Б")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PersonFullName { get; set; }

        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonLastName { get; set; }

        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonName { get; set; }

        [Display(Name = "По-батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonMiddleName { get; set; }

        [Display(Name = "Телефон")]
        public virtual string PersonPhone { get; set; }

        [Display(Name = "Ел.адреса")]
        public virtual string PersonEmail { get; set; }

        [Display(Name = "Організація")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Організація")]
        public virtual string OrganizationCaption { get; set; }

        [Display(Name = "Педіатр")]
        public virtual bool TreatsChildren { get; set; }

        public virtual Guid? MainSpecialityId { get; set; }

        [Display(Name = "Основна спеціалізація")]
        public virtual string MainSpecialityCaption { get; set; }
    }

    public abstract class BaseDoctorListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }
    
        [Display(Name = "П.І.Б")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PersonFullName { get; set; }

        public virtual Guid? OrganizationId { get; set; }

        [Display(Name = "Організація")]
        public virtual string OrganizationCaption { get; set; }

        public virtual Guid? MainSpecialityId { get; set; }

        [Display(Name = "Основна спеціалізація")]
        public virtual string MainSpecialityCaption { get; set; }

        [Display(Name = "Педіатр")]
        public virtual bool TreatsChildren { get; set; }
    }

    public abstract class BaseDoctorInfoDto: BaseDto
    {
        public virtual string PersonFullName { get; set; }

        public virtual string OrganizationCaption { get; set; }

        public virtual Guid? OrganizationId { get; set; }

        public virtual Guid MainSpecialityId { get; set; }

        public virtual string MainSpecialityCaption { get; set; }
    }
}
