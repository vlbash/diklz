using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.DTO.HelperDTOs;
using App.Data.Models.ORG;
using App.Data.Models.PRL;

namespace App.Data.DTO.BRN
{
    [RightsCheckList(nameof(PrlApplication))]
    [RlsRight(nameof(OrganizationExt), nameof(OrganizationId))]
    public class BranchAltDetailsDTO: BaseDTO
    {
        public Guid ApplicationId { get; set; }

        public Guid OrganizationId { get; set; }

        [NotMapped]
        public string AppType { get; set; }

        public Guid ApplicationBranchId { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("Найменування структурного підрозділу (або найменування юридичної особи):")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Phone(ErrorMessage = "Не правильний формат номеру телефону")]
        [DisplayName("Номер телефону")]
        [StringLength(20, ErrorMessage = "Максимальна кількість символів - 20")]
        public string PhoneNumber { get; set; }

        [DisplayName("Номер факсу")]
        [Phone(ErrorMessage = "Не правильний формат номеру факсу")]
        [StringLength(20, ErrorMessage = "Максимальна кількість символів - 20")]
        public string FaxNumber { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [DisplayName("E-mail")]
        [StringLength(100, ErrorMessage = "Максимальна кількість символів - 100")]
        [EmailAddress(ErrorMessage = "Не вірний формат Email")]
        public string EMail { get; set; }

        #region OrgDepartment info

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        [Display(Name = "Адреса місця провадження діяльності (англійською)")]
        [RegularExpression("[\'\"0-9a-zA-Z\\D ]+", ErrorMessage = "Поле не має містити символів кирилиці")]
        public string AdressEng { get; set; }


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

        [DisplayName("Адреса")]
        public Guid AddressId { get; set; }

        [DisplayName("Код КОАТУУ")]
        public string KoatuuCode { get; set; }

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

        [DisplayName("Адреса")]
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

        [DisplayName("Виробничі дільниці з переліком лікарських форм*")]
        public bool PRLIsAvailiableProdSites { get; set; }

        [DisplayName("Зони контролю якості")]
        public bool PRLIsAvailiableQualityZone { get; set; }

        [DisplayName("Складські зони (приміщення для зберігання)")]
        public bool PRLIsAvailiableStorageZone { get; set; }

        [DisplayName("Зони здійснення видачі дозволу на випуск лікарських засобів")]
        public bool PRLIsAvailiablePickupZone { get; set; }

        [DisplayName("Перелік лікарських форм та виробничих операцій, які заплановані до виробництва за певним місцем провадження діяльності та потребують ліцензування* (вибрати необхідне із списку)")]
        public string OperationListForm { get; set; }// to Json

        public string OperationListFormChanging { get; set; }
        //import

        [DisplayName("Складські зони (приміщення для зберігання)")]
        public bool IMLIsAvailiableStorageZone { get; set; }

        [DisplayName("Зони здійснення видачі дозволу на випуск (реалізацію) серії лікарського засобу")]
        public bool IMLIsAvailiablePermitIssueZone { get; set; }

        [DisplayName("Умови щодо контролю якості")]
        public bool IMLIsAvailiableQuality { get; set; }

        //retail
        [DisplayName("Виробництво (виготовлення) лікарських засобів в умовах аптеки")]
        public bool TRLIsManufacture { get; set; }

        [DisplayName("Оптова торгівля лікарськими засобами")]
        public bool TRLIsWholesale { get; set; }

        [DisplayName("Pоздрібна торгівля лікарськими засобами")]
        public bool TRLIsRetail { get; set; }

        #endregion
        [NotMapped]
        public OperationListDTO OperationListDTO { get; set; }

        [NotMapped]
        public string AppSort { get; set; }

    }

    [RightsCheckList(nameof(PrlApplication))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class BranchAltListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid OrgUnitId { get; set; }

        [Display(Name = "Найменування структурного підрозділу")]
        public string Name { get; set; }

        //[Display(Name = "Адреса МПД")]
        //public string Address { get; set; }

        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [DisplayName("Код КОАТУУ")]
        public string KoatuuCode { get; set; }

        [Display(Name = "Помітка на видалення")]
        public bool? LicenseDeleteCheck { get; set; }

        [NotMapped]
        public bool? isEditable { get; set; }

        public bool? IsFromLicense { get; set; }
        
        public string OperationListFormChanging { get; set; }

        [Display(Name = "Змінено")]
        [NotMapped]
        public bool IsChangedOperationListForm => !string.IsNullOrEmpty(OperationListFormChanging);
    }
}
