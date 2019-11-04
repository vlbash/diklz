using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.DTO.Common;
using App.Core.Data.Entities.ATU;
using App.Core.Security;

namespace App.Data.DTO.ATU
{
    [RlsRight(nameof(Street), nameof(CityId))]
    public class AtuStreetDTO : BaseDTO
    {
        [Required(ErrorMessage = "Не обраний населений пункт")]
        public Guid CityId { get; set; }

        [DisplayName("Населений пункт")]
        public string CityName { get; set; }

        [DisplayName("Тип вулиці")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string TypeEnum { get; set; }

        [DisplayName("Назва вулиці")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string Name { get; set; }

        public string FullName
        {
            get
            {
                var addressType = "";
                switch (TypeEnum)
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

                return $"{addressType} {Name}";
            }
        }
    }
}
