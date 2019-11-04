using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Org.Dto
{
    public abstract class BaseOrganizationDetailDto: BaseDto
    {
        [Display(Name = "Повна назва організації")]
        public virtual string FullName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Стан")]
        public virtual string State { get; set; } = "Working";
        [Display(Name = "Стан")]
        public virtual string StateName { get; set; } = "Діє";

        [Display(Name = "Ел.адреса")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Неприпустиме значення")]
        public virtual string Email { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        public virtual string Phone { get; set; }

        [Display(Name = "Підпорядкування")]
        public virtual string Parent { get; set; }

        public virtual Guid? ParentId { get; set; }

        [Display(Name = "Коментар")]
        public virtual string Description { get; set; }

        [Display(Name = "Банковські реквізити")]
        public virtual string BankDetails { get; set; }

        //Обязательна
        [Display(Name = "Юридична адреса")]
        public virtual string JurAddress { get; set; }
        public virtual Guid? SubjectAtuAddressId { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [RegularExpression(@"([0-9]{8,10})", ErrorMessage = "Можна ввести тільки цифри кількістю від 8-ми до 10-х")]
        [Display(Name = "ЕДРПОУ")]
        [MaxLength(20)]
        public virtual string EDRPOU { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [Display(Name = "Категорія організації")]
        public virtual string Category { get; set; } = "All";
        [Display(Name = "Категорія організації")]
        public virtual string CategoryCaption { get; set; } = "Всі";

        public virtual Guid? AtuCountryId { get; set; }
        [Display(Name = "Область")]
        public virtual Guid? AtuRegionId { get; set; }
        [Display(Name = "Район")]
        public virtual Guid? AtuRegionDistrictId { get; set; }
        [Display(Name = "Населений пункт")]
        public virtual Guid? AtuCityId { get; set; }
        [Display(Name = "Вулиця")]
        public virtual Guid? AtuStreetId { get; set; }
        [Display(Name = "Вулиця")]
        public virtual string AtuStreetCaption { get; set; }

        [Display(Name = "Поштовий індекс")]
        public virtual string PostIndex { get; set; }
        [Display(Name = "Тип адреси")]
        public virtual string AddressType { get; set; }
        [Display(Name = "Будинок, номер приміщення, додаткова інформація")]
        public virtual string Building { get; set; }

        [NotMapped]
        public virtual string _ReturnUrl { get; set; }
    }

    public abstract class BaseOrganizationListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [Display(Name = "Підпорядкування")]
        public virtual string Parent { get; set; }

        [Display(Name = "Стан")]
        public virtual string StateCaption { get; set; }

        [Display(Name = "Стан")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual string State { get; set; }

        [Display(Name = "Ел.адреса")]
        public virtual string Email { get; set; }

        [Display(Name = "Телефон")]
        public virtual string Phone { get; set; }

        [Display(Name = "Область")]
        public virtual string Region { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid? RegionId { get; set; }

        [Display(Name = "Адреса")]
        public virtual string Address { get; set; }

        [Display(Name = "ЕДРПОУ")]
        [CaseFilter(CaseFilterOperation.Contains)]
        [MaxLength(20)]
        public virtual string EDRPOU { get; set; }

        [Display(Name = "Категорія організації")]
        public virtual string CategoryCaption { get; set; }

        [Display(Name = "Категорія організації")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual string Category { get; set; }
    }
}
