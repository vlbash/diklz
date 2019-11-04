using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Common.Dto
{
    public abstract class BasePersonDetailDto : BaseDto
    {
        public virtual string FullName { get; set; }

        [MaxLength(200)]
        [Display(Name = "По батькові")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string MiddleName { get; set; }

        [MaxLength(200)]
        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string LastName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string Name { get; set; }

        [Display(Name = "Стать")]
        public virtual string GenderEnum { get; set; }

        public virtual string GenderName { get; set; }

        [Display(Name = "Дата народження")]
        public virtual DateTime? Birthday { get; set; }

        [Display(Name = "Телефон")]
        public virtual string Phone { get; set; }

        [Display(Name = "Електронна адреса")]
        public virtual string Email { get; set; }

        [MaxLength(50)]
        [Display(Name = "ІПН")]
        public virtual string IPN { get; set; }

        [Display(Name = "ІПН відсутній")]
        public virtual bool NoIPN { get; set; }

        [MaxLength(50)]
        [Display(Name = "Документ що посвідчує особу")]
        public virtual string IdentityDocumentTypeEnum { get; set; }

        public virtual string IdentityDocumentTypeName { get; set; }

        [Display(Name = "Серія документу")]
        public virtual string DocPrefix { get; set; }

        [MaxLength(20)]
        [Display(Name = "Номер документу")]
        public virtual string PersonalDocumentNumber { get; set; }

        [MaxLength(100)]
        [Display(Name = "Ким виданий")]
        public virtual string PersonalDocumentAuthority { get; set; }

        [Display(Name = "Дата видачі")]
        public virtual DateTime? DocumentIssueDate { get; set; }

        [Display(Name = "Діє до")]
        public virtual DateTime? ExpirationDate { get; set; }

    }

    public abstract class BasePersonListDto : BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [Display(Name = "ПІБ")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string FullName { get; set; }
    }
}
