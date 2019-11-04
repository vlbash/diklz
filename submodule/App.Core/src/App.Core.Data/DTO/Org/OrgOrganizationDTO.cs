using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.ORG;
using App.Core.Security;

namespace App.Core.Data.DTO.Org
{
    [RightsCheckList(nameof(Organization), nameof(OrgUnit))]
    [RlsRight(nameof(Region), nameof(AtuRegionId))]
    [RlsRight(nameof(Organization), nameof(Id))]
    public class OrgOrganizationDetailDTO: BaseDTO, IOrgUnitDetailDTO
    {
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Повна назва організації")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Стан")]
        public string State { get; set; } = "Working";
        [DisplayName("Стан")]
        public string StateName { get; set; } = "Діє";

        [DisplayName("Ел.адреса")]
        [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неприпустиме значення")]
        public string Email { get; set; }

        [DisplayName("Телефон")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        public string Phone { get; set; }

        [DisplayName("Підпорядкування")]
        public string Parent { get; set; }

        public Guid? ParentId { get; set; }

        [DisplayName("Коментар")]
        public string Description { get; set; }

        [DisplayName("Банковські реквізити")]
        public string BankDetails { get; set; }

        //Обязательна
        [DisplayName("Юридична адреса")]
        public string JurAddress { get; set; }
        public Guid? SubjectAtuAddressId { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [RegularExpression(@"([0-9]{8,10})", ErrorMessage = "Можна ввести тільки цифри кількістю від 8-ми до 10-х")]
        [DisplayName("ЕДРПОУ")]
        [MaxLength(20)]
        public string EDRPOU { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Категорія організації")]
        public string Category { get; set; } = "All";
        [DisplayName("Категорія організації")]
        public string CategoryName { get; set; } = "Всі";

        public Guid? AtuCountryId { get; set; }
        [DisplayName("Область")]
        public Guid? AtuRegionId { get; set; }
        [DisplayName("Район")]
        public Guid? AtuRegionDistrictId { get; set; }
        [DisplayName("Населений пункт")]
        public Guid? AtuCityId { get; set; }
        [DisplayName("Вулиця")]
        public Guid? AtuStreetId { get; set; }
        [DisplayName("Вулиця")]
        public  string AtuStreetName { get; set; }

        [DisplayName("Поштовий індекс")]
        public string PostIndex { get; set; }
        [DisplayName("Тип адреси")]
        public string AddressType { get; set; }
        [DisplayName("Будинок, номер приміщення, додаткова інформація")]
        public string Building { get; set; }

        [NotMapped]
        public string _ReturnUrl { get; set; }
    }

    [RightsCheckList(nameof(Organization))]
    [RlsRight(nameof(Region), nameof(RegionId))]
    [RlsRight(nameof(Organization), nameof(Id))]
    public class OrgOrganizationListDTO: BaseDTO, IOrgUnitListDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Підпорядкування")]
        public string Parent { get; set; }

        [DisplayName("Стан")]
        public string StateName { get; set; }

        [DisplayName("Стан")]
        [PredicateCase(PredicateOperation.Equals)]
        public string State { get; set; }

        [DisplayName("Ел.адреса")]
        public string Email { get; set; }

        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [DisplayName("Область")]
        public string Region { get; set; }

        [PredicateCase(PredicateOperation.Equals)]
        public Guid? RegionId { get; set; }

        [DisplayName("Адреса")]
        public string Address { get; set; }

        [DisplayName("ЕДРПОУ")]
        [PredicateCase(PredicateOperation.Contains)]
        [MaxLength(20)]
        public string EDRPOU { get; set; }

        [DisplayName("Категорія організації")]
        public string CategoryName { get; set; }

        [DisplayName("Категорія організації")]
        [PredicateCase(PredicateOperation.Equals)]
        public string Category { get; set; }
    }

    [RightsCheckList(nameof(Organization))]
    [RlsRight(nameof(Organization), nameof(Id))]
    public class OrgOrganizationSelectDTO: BaseDTO
    {
        [DisplayName("Категорія організації")]
        public string Category { get; set; } = "All";
    }
}
