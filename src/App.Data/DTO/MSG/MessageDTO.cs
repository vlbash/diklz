using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.MSG;
using App.Data.Models.ORG;

namespace App.Data.DTO.MSG
{
    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MessageDetailDTO: BaseDTO
    {
        public bool IsCreatedOnPortal { get; set; }

        [DisplayName("Тип повідомлення")]
        [PredicateCase(PredicateOperation.Contains)]
        public string MessageType { get; set; }

        [DisplayName("Ліцензія виробництва")]
        public bool IsPrlLicense { get; set; }

        [DisplayName("Ліцензія опту")]
        public bool IsTrlLicense { get; set; }

        [DisplayName("Ліцензія імпорту")]
        public bool IsImlLicense { get; set; }

        [NotMapped]
        public string License => IsPrlLicense ? "PRL" : IsImlLicense ? "IML" : IsTrlLicense ? "TRL" : "";

        [DisplayName("Текст повідомлення")]
        public string MessageText { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Номер вихідного листа ліцензіата")]
        public string MessageNumber { get; set; }

        [DisplayName("Дата вихідного листа ліцензіата:")]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MessageDate { get; set; } = DateTime.Now;

        public Guid OrgUnitId { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MessageListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        [DisplayName("Дата створення")]
        public DateTime CreatedOn { get; set; }

        public bool IsCreatedOnPortal { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Номер реєстрації")]
        public string RegNumber { get; set; }

        [PredicateCase(PredicateOperation.InputRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegDate { get; set; }

        [DisplayName("Дата і номер реєстрації")]
        [NotMapped]
        public string RegNumDate => !string.IsNullOrEmpty(RegNumber) ? concatDate(RegDate, RegNumber) : "";

        [PredicateCase(PredicateOperation.Contains)]
        [Display(Name = "Найменування СГД / ПІБ ФОП ліцензіата")]
        public string OrgName { get; set; }

        [Display(Name = "ЄДРПОУ")]
        public string EDRPOU { get; set; }

        [DisplayName("Реєстраційний номер облікової картки платника податків")]
        public string INN { get; set; }

        [DisplayName("ЄДРПОУ/ІНН ліцензіата")]
        [NotMapped]
        public string EdrpouOrInn => !string.IsNullOrEmpty(EDRPOU) ? EDRPOU : INN;

        [DisplayName("Тип повідомлення")]
        [PredicateCase(PredicateOperation.Contains)]
        public string MessageTypeName { get; set; }

        public string MessageTypeEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        public string MessageNumber { get; set; }

        [PredicateCase(PredicateOperation.InputRange), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MessageDate { get; set; }

        [DisplayName("Дата і вихідний номер ліцензіата")]
        public string MessageNumDate => !string.IsNullOrEmpty(MessageNumber) ? concatDate(MessageDate, MessageNumber) : "";

        public string MessageHierarchyTypeEnum { get; set; }

        [PredicateCase(PredicateOperation.Contains)]
        [DisplayName("Вид діяльності")]
        public string LicenseTypes { get; set; }

        public string LicenseActivity { get; set; }

        [DisplayName("Користувач")]
        [PredicateCase(PredicateOperation.Contains)]
        public string SenderName { get; set; }

        [DisplayName("Стан повідомлення")]
        [PredicateCase(PredicateOperation.Contains)]
        public string MessageState { get; set; }

        public bool IsPrlLicense { get; set; }
        public bool IsImlLicense { get; set; }
        public bool IsTrlLicense { get; set; }

        public string StatePrl => msgState(StatePrlEnum);
        public string StateIml => msgState(StateImlEnum);
        public string StateTrl => msgState(StateTrlEnum);

        public string StatePrlEnum { get; set; }

        public string StateImlEnum { get; set; }
        public string StateTrlEnum { get; set; }

        [DisplayName("Стан повідомлення")]
        [PredicateCase(PredicateOperation.Contains)]
        public string MessageStateEnum { get; set; }

        public Guid OrgUnitId { get; set; }

        private string concat(string first, string second) =>
            !string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(second) ? first + " / " + second :
            string.IsNullOrEmpty(first) ? second :
            string.IsNullOrEmpty(second) ? first : "";
        private string concatDate(DateTime? first, string second) =>
            first != null && !string.IsNullOrEmpty(second) ? first.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) + " / " + second :
            first == null ? second :
            string.IsNullOrEmpty(second) ? first.Value.ToString("«dd» MMMM yyyy", CultureInfo.CreateSpecificCulture("uk")) : "";

        private string msgState(string msgState)
        {
            switch (msgState)
            {
                case "Accepted":
                    return "Прийнято до відома";
                case "Project":
                    return "Проект";
                case "Registered":
                    return "Зареєстровано";
                case "Rejected":
                    return "Відхилено";
                case "Submitted":
                    return "Подано";
                default:
                    return "";
            }
        }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class SgdChiefNameChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("ПІБ керівника СГД")]
        public string SgdShiefFullName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class SgdNameChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Нова назва юридичної особи / ПІБ фізичної особи-підприємця")]
        public string SgdNewFullName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class OrgFopLocationChangeMessageDTO: MessageDetailDTO
    {
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

        public Guid NewLocationId { get; set; }
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

        [DisplayName("Вулиця")]
        public string NewLocation
        {
            get
            {
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
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDActivitySuspensionMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        public Guid MpdSelectedId { get; set; }

        [DisplayName("Дата початку призупинення діяльності")]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SuspensionStartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Причина призупинення роботи")]
        public string SuspensionReason { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDActivityRestorationMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }
        public Guid MpdSelectedId { get; set; }

        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата відновлення дяльності")]
        public DateTime RestorationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Причина відновлення дяльності")]
        public string RestorationReason { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDClosingForSomeActivityMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }

        public Guid MpdSelectedId { get; set; }

        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата закриття МПД")]
        public DateTime ClosingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Причина закриття МПД")]
        public string ClosingReason { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDRestorationAfterSomeActivityMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }

        public Guid MpdSelectedId { get; set; }

        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата відновлення дяльності")]
        public DateTime RestorationDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Причина відновлення дяльності")]
        public string RestorationReason { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class MPDLocationRatificationMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("МПД щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }

        public Guid MpdSelectedId { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

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

        public Guid AddressBusinessActivityId { get; set; }

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

        [DisplayName("Вулиця")]
        public string AddressBusinessActivity
        {
            get
            {
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
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PharmacyHeadReplacementMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальна кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальна кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Уповноважена особа щодо якої необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }

        public Guid MpdSelectedId { get; set; }

        [NotMapped]
        public string OrgPositionType { get; set; }

        [DisplayName("Тип уповноваженої особи")]
        [NotMapped]
        public string OrgPositionTypeEnum { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [MaxLength(100)]
        [DisplayName("Ім'я")]
        public string PharmacyHeadName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [MaxLength(200)]
        [DisplayName("По-батькові")]
        public string PharmacyHeadMiddleName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [MaxLength(200)]
        [DisplayName("Прізвище")]
        public string PharmacyHeadLastName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PharmacyAreaChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Нова площа аптечного закладу")]
        public long? NewPharmacyArea { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class PharmacyNameChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Аптечний заклад щодо якого необхідно виконати зміни")]
        [NotMapped]
        public string MPDGuidEnum { get; set; }

        public Guid MpdSelectedId { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Нова назва аптечного закладу")]
        public string NewPharmacyName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class LeaseAgreementChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Назва юридичної особи з яким укладено новий договір")]
        public string NewLegalEntity { get; set; }

        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Дата укладання договору")]
        public DateTime? LeaseAgreementStartDate { get; set; }

        [DataType(DataType.Date)]
        [PredicateCase(PredicateOperation.ValueRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Діє до")]
        public DateTime? LeaseAgreementEndDate { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class ProductionDossierChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class SupplierChangeMessageDTO: MessageDetailDTO
    {
        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseType { get; set; }

        [DisplayName("Найменування ліцензії")]
        [StringLength(50, ErrorMessage = "Максимальная кількість символів - 50")]
        [NotMapped]
        public string LicenseTypeName { get; set; }
    }

    [RightsCheckList(nameof(Message))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class AnotherEventMessageDTO: MessageDetailDTO
    {
    }

    public class MsgShortDTO: BaseDTO
    {
        #region LIMS
        [Required]
        [DisplayName("Виконавець")]
        public Guid? PerformerId { get; set; }

        [Required]
        [DisplayName("Виконавець")]
        [NotMapped]
        public string PerformerName { get; set; }

        [Required]
        [DisplayName("Номер реєстрації")]
        public string RegNumber { get; set; }

        [Required]
        [DisplayName("Дата реєстрації")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RegDate { get; set; }

        #endregion

        [DisplayName("Тип повідомлення")]
        public string MessageType { get; set; }

        [DisplayName("Ліцензія виробництва")]
        public bool IsPrlLicense { get; set; }

        [DisplayName("Ліцензія опту")]
        public bool IsTrlLicense { get; set; }

        [DisplayName("Ліцензія імпорту")]
        public bool IsImlLicense { get; set; }

        [NotMapped]
        public string License => IsPrlLicense ? "PRL" : IsImlLicense ? "IML" : IsTrlLicense ? "TRL" : "";
    }
}

