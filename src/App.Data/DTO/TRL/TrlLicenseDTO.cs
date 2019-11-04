using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.DTO.PRL;
using App.Data.Enums;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.TRL
{
    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class TrlLicenseDetailDTO: BaseDTO
    {
        [NotMapped]
        public OrgType OrgType
        {
            get => string.IsNullOrEmpty(EDRPOU) ? OrgType.FOP : OrgType.Organization;
            set { }
        }

        public Guid BaseApplicationId { get; set; }

        [DisplayName("Вид діяльності")]
        public string LicType { get; set; }

        [DisplayName("Вид діяльності")]
        public string LicTypeName { get; set; }

        public string LicSort { get; set; }

        [DisplayName("Стан ліцензії")]
        public string LicState { get; set; }

        public bool IsRelevant { get; set; }

        [DisplayName("Стан ліцензії")]
        public string LicStateName { get; set; }

        [DisplayName("Номер ліцензії")]
        public string LicenseNumber { get; set; }

        [DisplayName("Вид діяльності")]
        public string ActivityTypeName { get; set; }

        [DisplayName("Код економічної класифікації")]
        public string EconomicClassificationTypeName { get; set; }

        [DisplayName("Дата початку дії ліцензії")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? LicenseDate { get; set; }

        [DisplayName("Номер наказу")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDate { get; set; }
        
        [Display(Name = "Найменування юридичної особи / ПІБ ФОП")]
        public string OrgName { get; set; }

        [DisplayName("Підстава")]
        public string AppSortEnum { get; set; }

        [DisplayName("Підстава")]
        public string EndReasonText { get; set; }

        [DisplayName("№ Наказу")]
        public string EndOrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EndOrderDate { get; set; }

        [DisplayName("Текст наказу")]
        public string EndOrderText { get; set; }

        public Guid OrgUnitId { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Код згідно з ЄДРПОУ")]
        public string EDRPOU { get; set; }

        [DisplayName("Реєстраційний номер облікової картки платника податків")]
        public string INN { get; set; }

        [DisplayName("Серія та номер пасспорту")]
        public string PassportFull => !string.IsNullOrEmpty(PassportSerial)
            ? PassportSerial + " " + PassportNumber
            : PassportNumber;

        [DisplayName("Серія паспорта")]
        public string PassportSerial { get; set; }

        [DisplayName("Номер паспорта")]
        public string PassportNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [DisplayName("Дата видачі")]
        public DateTime? PassportDate { get; set; }

        [DisplayName("Орган, що видав паспорт")]
        public string PassportIssueUnit { get; set; }

        [Display(Name = "Організаційно-правова форма")]
        public string LegalFormType { get; set; }

        [Display(Name = "Організаційно-правова форма")]
        public string LegalFormName { get; set; }

        [Display(Name = "Форма власності")]
        public string OwnershipType { get; set; }

        [Display(Name = "Форма власності")] public string OwnershipTypeName { get; set; }

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

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string OrgDirector { get; set; }

        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [Phone(ErrorMessage = "Не правильний формат номеру телефону")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [Phone(ErrorMessage = "Не правильний формат номеру факсу")]
        [Display(Name = "Номер факсу")]
        public string FaxNumber { get; set; }

        [Display(Name = "E-mail")]
        public string EMail { get; set; }

        [Display(Name = "Номер рахунку в національній валюті")]
        public string NationalAccount { get; set; }

        [Display(Name = "Номер рахунку в іноземній валюті")]
        public string InternationalAccount { get; set; }

        [Display(Name = "Реквізити банку з рахунком в національній валюті")]
        public string NationalBankRequisites { get; set; }

        [Display(Name = "Реквізити банку з рахунком в іноземній валюті")]
        public string InternationalBankRequisites { get; set; }

        [Display(Name = "D-U-N-S номер (за наявності)")]
        public string Duns { get; set; }

        [DisplayName("Лікарські форми")]
        [NotMapped] public string MedicinalForms { get; set; }

        [DisplayName("Активні фармацевтичні інгредієнти")]
        [NotMapped] public string ActiveIngredients { get; set; }
        [DisplayName("Зберігання")]
        [NotMapped] public string StorageForms { get; set; }
        [DisplayName("Виробництво досліджуваних ЛЗ")]
        [NotMapped] public string ProdResearchDrugs { get; set; }

        [NotMapped] public IEnumerable<TrlAppListDTO> ApplicationList { get; set; }

        [NotMapped] public Guid FirstAppId { get; set; }

        public string FirstAppSortName { get; set; }
    }


    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class TrlLicenseListDTO: BaseDTO, IPagingCounted
    {
        [NotMapped]
        public OrgType OrgType
        {
            get => string.IsNullOrEmpty(EDRPOU) ? OrgType.FOP : OrgType.Organization;
            set { }
        }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Статус")]
        public string LicState { get; set; }

        [DisplayName("Статус")]
        public string LicStateName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Номер ліцензії")]
        public string LicenseNumber { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayName("Дата початку дії ліцензії")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LicenseDate { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Філій(з них діючих)")]
        public string Branches { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "Назва СГД/ПІБ ФОП")]
        public string OrgName { get; set; }

        public Guid OrgUnitId { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        public string EDRPOU { get; set; }


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

        [PredicateCase(PredicateOperation.Contains)]
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

        [DisplayName("Юридична адреса")]
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
        [PredicateCase(PredicateOperation.Contains)]
        public string SqlAddress { get; set; }
        #endregion

        public int TotalRecordCount { get; set; }
    }
    public class TrlLicenseRegisterListDTO: BaseDTO, IPagingCounted
    {
        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        public string EDRPOU { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "Назва СГД/ПІБ ФОП")]
        public string OrgName { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayName("Дата початку дії ліцензії")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LicenseDate { get; set; }

        [DisplayName("Номер наказу")] public string OrderNumber { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayName("Дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Статус")] public string LicState { get; set; }

        #region ATU

        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }

        [DisplayName("Вулиця")] public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")] public string CityName { get; set; }
        public string CityEnum { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поле має мати 5 символів")]
        public string PostIndex { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
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
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName,
                        StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                return
                    $"{(string.IsNullOrEmpty(RegionName) ? "" : $"{RegionName}, ")}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{CityName}";
            }
        }

        [DisplayName("Юридична адреса")]
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
                }

                return $"{RegionName}, {district}{cityType}{CityName}, {addressType} {StreetName}, {Building}";
            }
        }

        [PredicateCase(PredicateOperation.Contains)]
        public string SqlAddress { get; set; }
        #endregion

        public int TotalRecordCount { get; set; }
    }

    public class TrlLicenseRegisterDetailDTO: BaseDTO
    {
        public Guid BaseApplicationId { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "ЄДРПОУ/РНОКПП (Індивідуальний податковий номер)")]
        public string EDRPOU { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "Назва СГД/ПІБ ФОП")]
        public string OrgName { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayName("Дата початку дії ліцензії")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LicenseDate { get; set; }

        [DisplayName("Номер наказу")] public string OrderNumber { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayName("Дата наказу")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Статус")] public string LicState { get; set; }

        [DisplayName("ПІБ керівника")]
        public string OrgDirector { get; set; }

        [DisplayName("Телефон")]
        public string PhoneNumber { get; set; }

        [DisplayName("Факс")]
        public string FaxNumber { get; set; }

        [DisplayName("E-mail")]
        public string Email { get; set; }

        #region ATU

        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }

        [DisplayName("Вулиця")] public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")] public string CityName { get; set; }
        public string CityEnum { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Поле має мати 5 символів")]
        public string PostIndex { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
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
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), CityName,
                        StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                return
                    $"{(string.IsNullOrEmpty(RegionName) ? "" : $"{RegionName}, ")}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{CityName}";
            }
        }

        [DisplayName("Юридична адреса")]
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

        [PredicateCase(PredicateOperation.Contains)]
        public string SqlAddress { get; set; }
        #endregion
    }
}
