using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.DTO.Org;
using App.Core.Data.Interfaces;

namespace App.Data.DTO.TRL
{
    public class TrlOrganizationExtShortDTO : BaseDTO
    {

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        public string EDRPOU { get; set; }

        public string INN { get; set; }

        [NotMapped]
        [DisplayName("Код ЄРДПОУ/ІНН Ліцензіата")]
        public string edrpouOrInn => !string.IsNullOrEmpty(EDRPOU) ? EDRPOU : INN;
    }

    public class TrlOrganizationExtMediumDTO: BaseDTO/*, IOrgUnitDetailDTO*/
    {
        [DisplayName("Назва організації")]
        public string Name { get; set; }

        [Display(Name = "Код згідно з ЄДРПОУ")]
        public string EDRPOU { get; set; }

        [DisplayName("Реєстраційний номер облікової картки платника податків")]
        public string INN { get; set; }

        [Display(Name = "Прізвище, ім’я, по батькові керівника юридичної особи")]
        public string OrgDirector { get; set; }

        [Display(Name = "E-mail")]
        public string EMail { get; set; }

        #region ATU

        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }

        [DisplayName("Вулиця")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")]
        public string CityName { get; set; }
        public string CityEnum { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поле має мати 5 символів")]
        public string PostIndex { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер будинку, корпус або будівля, номер квартири або офісу")]
        public string Building { get; set; }
        public string AddressType { get; set; }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public Guid AddressId { get; set; }

        public string CityFullName
        {
            get
            {
                var district = DistrictName;
                if (!string.IsNullOrEmpty(DistrictName) &&
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName, StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                return $"{(string.IsNullOrEmpty(RegionName) ? "" : $"{RegionName}, ")}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{CityName}";
            }
        }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public string Address
        {
            get
            {
                if (AddressId == Guid.Empty)
                    return "";
                var district = !string.IsNullOrEmpty(DistrictName) ? $"{DistrictName} ," : "";
                if (!string.IsNullOrEmpty(DistrictName) &&
                    DistrictName.Substring(2, DistrictName.Length - 2) == CityName)
                    district = "";
                var cityType = "";
                switch (CityEnum)
                {
                    case "Village":
                        cityType = "с.";
                        break;
                    case "Hamlet":
                        cityType = "с-ще ";
                        break;
                    case "UrbanTypeVillages":
                        cityType = "смт ";
                        break;
                    case "TownsOfDistrictSubordination":
                        cityType = "м.";
                        break;
                    case "CitiesOfRegionalSubordination":
                        cityType = "м.";
                        break;
                }
                var addressType = "";
                switch (AddressType)
                {
                    case "Street":
                        addressType = "вул.";
                        break;
                    case "Lane":
                        addressType = "пров.";
                        break;
                    case "Boulevard":
                        addressType = "б-р ";
                        break;
                    case "Avenue":
                        addressType = "просп.";
                        break;
                    case "Square":
                        addressType = "площа ";
                        break;
                }

                return $"{RegionName}, {district}{cityType}{CityName}, {addressType} {StreetName}, {Building}";
            }
        }

        #endregion

        [NotMapped]
        [DisplayName("Код ЄРДПОУ/ІНН Ліцензіата")]
        public string edrpouOrInn => !string.IsNullOrEmpty(EDRPOU) ? EDRPOU : INN;
    }

    public class TrlOrganizationExtFullDTO: BaseDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Найменування юридичної особи / ПІБ ФОП")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Код згідно з ЄДРПОУ")]
        public string EDRPOU { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Реєстраційний номер облікової картки платника податків")]
        [StringLength(20)]
        public string INN { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Організаційно-правова форма")]
        [StringLength(255, ErrorMessage = "Максимальна кількість символів - 255")]
        public string LegalFormType { get; set; }

        [DisplayName("Серія паспорта")]
        [StringLength(2, ErrorMessage = "Максимальна кількість символів - 2")]
        public string PassportSerial { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Номер паспорта")]
        [StringLength(9, ErrorMessage = "Максимальна кількість символів - 9")]
        public string PassportNumber { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Форма власності")]
        [StringLength(255, ErrorMessage = "Максимальна кількість символів - 255")]
        public string OwnershipType { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [DisplayName("Дата видачі")]
        public DateTime? PassportDate { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Орган, що видав паспорт")]
        [StringLength(200, ErrorMessage = "Максимальна кількість символів - 200")]
        public string PassportIssueUnit { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Прізвище, ім’я, по батькові керівника юридичної особи")]
        [StringLength(250, ErrorMessage = "Максимальна кількість символів - 250")]
        public string OrgDirector { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "E-mail")]
        [StringLength(100, ErrorMessage = "Максимальна кількість символів - 100")]
        [EmailAddress(ErrorMessage = "Не вірний формат Email")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Phone(ErrorMessage = "Не правильний формат номеру телефону")]
        [StringLength(25, ErrorMessage = "Максимальна кількість символів - 25")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Phone(ErrorMessage = "Не правильний формат номеру факсу")]
        [Display(Name = "Номер факсу")]
        [StringLength(20, ErrorMessage = "Максимальна кількість символів - 20")]
        public string FaxNumber { get; set; }

        #region ATU

        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }

        [DisplayName("Вулиця")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")]
        public string CityName { get; set; }
        public string CityEnum { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поле має мати 5 символів")]
        public string PostIndex { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер будинку, корпус або будівля, номер квартири або офісу")]
        public string Building { get; set; }
        public string AddressType { get; set; }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public Guid AddressId { get; set; }

        public string CityFullName
        {
            get
            {
                var district = DistrictName;
                if (!string.IsNullOrEmpty(DistrictName) &&
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName, StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                return $"{(string.IsNullOrEmpty(RegionName) ? "" : $"{RegionName}, ")}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{CityName}";
            }
        }

        [DisplayName("Місцезнаходження суб'єкта господарювання")]
        public string Address
        {
            get
            {
                if (AddressId == Guid.Empty)
                    return "";
                var district = !string.IsNullOrEmpty(DistrictName) ? $"{DistrictName} ," : "";
                if (!string.IsNullOrEmpty(DistrictName) &&
                    DistrictName.Substring(2, DistrictName.Length - 2) == CityName)
                    district = "";
                var cityType = "";
                switch (CityEnum)
                {
                    case "Village":
                        cityType = "с.";
                        break;
                    case "Hamlet":
                        cityType = "с-ще ";
                        break;
                    case "UrbanTypeVillages":
                        cityType = "смт ";
                        break;
                    case "TownsOfDistrictSubordination":
                        cityType = "м.";
                        break;
                    case "CitiesOfRegionalSubordination":
                        cityType = "м.";
                        break;
                }
                var addressType = "";
                switch (AddressType)
                {
                    case "Street":
                        addressType = "вул.";
                        break;
                    case "Lane":
                        addressType = "пров.";
                        break;
                    case "Boulevard":
                        addressType = "б-р ";
                        break;
                    case "Avenue":
                        addressType = "просп.";
                        break;
                    case "Square":
                        addressType = "площа ";
                        break;
                }

                return $"{RegionName}, {district}{cityType}{CityName}, {addressType} {StreetName}, {Building}";
            }
        }

        #endregion

        [Display(Name = "Номер рахунку в національній валюті")]
        [StringLength(30, ErrorMessage = "Максимальна кількість символів - 30")]
        public string NationalAccount { get; set; }

        [Display(Name = "Реквізити банку з рахунком в національній валюті")]
        [StringLength(60, ErrorMessage = "Максимальна кількість символів - 60")]
        public string NationalBankRequisites { get; set; }

        [Display(Name = "Номер рахунку в іноземній валюті")]
        [StringLength(30, ErrorMessage = "Максимальна кількість символів - 30")]
        public string InternationalAccount { get; set; }

        [Display(Name = "Реквізити банку з рахунком в іноземній валюті")]
        [StringLength(60, ErrorMessage = "Максимальна кількість символів - 60")]
        public string InternationalBankRequisites { get; set; }

        [DisplayName("Коментар")]
        public string Description { get; set; }

        [Display(Name = "D-U-N-S номер (за наявності)")]
        [StringLength(24, ErrorMessage = "Максимальна кількість символів - 24")]
        public string Duns { get; set; }

        [NotMapped]
        [DisplayName("Код ЄРДПОУ/ІНН Ліцензіата")]
        public string edrpouOrInn => !string.IsNullOrEmpty(EDRPOU) ? EDRPOU : INN;
    }

    public class TrlOrganizationExtListDTO: BaseDTO, IOrgUnitListDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва організації")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        public string Parent { get; set; }

        [DisplayName("Телефон")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [DisplayName("Факс")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Неприпустиме значення")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Неприпустиме значення")]
        public string FaxNumber { get; set; }

        [DisplayName("Ел.адреса")]
        [RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Неприпустиме значення")]
        public string EMail { get; set; }

        [MaxLength(250)]
        public string OrgDirector { get; set; }
    }
}
