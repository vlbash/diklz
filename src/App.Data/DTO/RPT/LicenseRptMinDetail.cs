using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;
using App.Core.Security;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.DTO.RPT
{
    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class LicenseRptMinDetailSgdChiefName: BaseDTO
    {
        [DisplayName("Номер ліцензії")]
        public string LicenseNumber { get; set; }

        [DisplayName("Дата початку дії ліцензії")]
        public DateTime LicenseDate { get; set; }

        [DisplayName("Номер наказу")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        public DateTime OrderDate { get; set; }

        public Guid OrgUnitId { get; set; }

        [Required(ErrorMessage = "Поле необхідне для заповнення")]
        public string OrgDirector { get; set; }
    }

    [RightsCheckList(nameof(LimsDoc))]
    [RlsRight(nameof(OrganizationExt), nameof(OrgUnitId))]
    public class LicenseRptMinDetailOrgFopLocation: BaseDTO
    {
        [DisplayName("Номер ліцензії")]
        public string LicenseNumber { get; set; }

        [DisplayName("Дата початку дії ліцензії")]
        public DateTime LicenseDate { get; set; }

        [DisplayName("Номер наказу")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        public DateTime OrderDate { get; set; }

        public Guid OrgUnitId { get; set; }

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
    }    
}
