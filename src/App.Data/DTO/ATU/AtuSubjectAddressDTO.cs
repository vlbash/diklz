using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.ATU;
using App.Core.Security;

namespace App.Data.DTO.ATU
{
    [RightsCheckList(nameof(SubjectAddress))]
    public class AtuSubjectAddressDTO : BaseDTO
    {
        [Required(ErrorMessage = "Не вірно вказана адреса")]
        public Guid StreetId { get; set; }
        
        [DisplayName("Вулиця")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Не вірно вказаний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")]
        public string CityName { get; set; }
        public string CityEnum { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        [DisplayName("Район")]
        public string DistrictName { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Поштовий індекс")]
        public string PostIndex { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Номер будинку, корпус або будівля, номер квартири або офісу")]
        public string Building { get; set; }
        public string AddressType { get; set; }

        public string Address
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

        [DisplayName("Код")]
        public string Code { get; set; }

    }
}
