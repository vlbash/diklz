using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.ATU;
using App.Core.Security;

namespace App.Data.DTO.ATU
{
    [RlsRight(nameof(Region), nameof(RegionId))]
    public class AtuCityDTO : BaseDTO
    {
        [DisplayName("Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid RegionId { get; set; }
        public string RegionName { get; set; }

        [DisplayName("Тип населеного пункту")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string TypeEnum { get; set; }

        public string NameCode { get; set; }

        public string Code { get; set; }

        [NotMapped]
        public Guid DistrictId { get; set; }

        [DisplayName("Район")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string DistrictName { get; set; }

        [DisplayName("Назва населеного пункту")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }

        [NotMapped]
        public string CityFullName
        {
            get
            {
                var district = DistrictName;
                if (!string.IsNullOrEmpty(DistrictName) &&
                    string.Equals(DistrictName.Substring(2, DistrictName.Length - 2), Name, StringComparison.CurrentCultureIgnoreCase))
                    district = "";
                var cityType = "";
                switch (TypeEnum)
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
                return $"{RegionName + ", "}{(string.IsNullOrEmpty(district) ? "" : DistrictName + ", ")}{cityType}{Name}";
            }
        }
    }
}
