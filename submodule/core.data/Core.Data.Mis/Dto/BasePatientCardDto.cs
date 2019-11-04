using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Mis.Dto
{
    public abstract class BasePatientCardSelectDto: BaseDto
    {
    }

    public abstract class BasePatientCardListDto: BaseDocumentDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        //[SearchFilter(labelName: "ПІБ пацієнта", fieldType: "text", order: 1)]
        [Display(Name = "ПІБ пацієнта")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string PersonFullName { get; set; }

        //[SearchFilter(labelName: "Телефон", fieldType: "text", order: 2)]
        [Display(Name = "Телефон")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string Phone { get; set; }

        //[SearchFilter(labelName: "Електронна адреса", fieldType: "text", order: 3)]
        [Display(Name = "Електронна адреса")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string Email { get; set; }

        [DocumentDate]
        [Display(Name = "Дата народження")]
        public virtual DateTime Birthday { get; set; }

        [Display(Name = "Стан")]
        public virtual bool Enabled { get; set; }

        [Display(Name = "Примітки")]
        public override string Description { get; set; }

        [Display(Name = "Стать")]
        public virtual string GenderEnum { get; set; }

        [Display(Name = "Стать")]
        public virtual string Gender { get; set; }

        [NotMapped]
        [Display(Name = "Картка")]
        public virtual string CardInfo => RegNumber + " від " + RegDate.ToString("dd.MM.yyyy");
    }

    public abstract class BasePatientCardDetailDto: BaseDocumentDto
    {
        [Display(Name = "№ картки")]
        //[Required(ErrorMessage = "Заповніть поле")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public override string RegNumber { get; set; }

        [Display(Name = "Дата картки")]
        [DocumentDate]
        [CaseFilter(CaseFilterOperation.ValueRange)]
        public override DateTime RegDate { get; set; } = DateTime.Today;

        [Display(Name = "Стан картки")]
        public virtual bool Enabled { get; set; }

        public virtual Guid? PersonId { get; set; }

        [Display(Name = "ПІБ пацієнта")]
        public virtual string PersonFullName { get; set; }

        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonLastName { get; set; }

        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonName { get; set; }

        [Display(Name = "По-батькові")]
        //[Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonMiddleName { get; set; }

        [Display(Name = "РНОКПП")]
        public virtual string PersonIPN { get; set; }

        [Display(Name = "Дата народження")]
        [DocumentDate]
        //[Required(ErrorMessage = "Заповніть поле")]
        public virtual DateTime PersonBirthday { get; set; } = DateTime.Now;

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual string PersonPhone { get; set; }

        [Display(Name = "Ел.адреса")]
        public virtual string PersonEmail { get; set; }

        [Display(Name = "Примітки")]
        public override string Description { get; set; }

        [Display(Name = "Стать")]
        public virtual string PersonGenderEnum { get; set; }

        [Display(Name = "Стать")]
        public virtual string PersonGender { get; set; }
    }
}
