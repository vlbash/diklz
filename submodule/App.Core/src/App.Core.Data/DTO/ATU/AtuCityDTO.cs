using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Entities.ATU;
using App.Core.Security;

namespace App.Core.Data.DTO.ATU
{
    [RlsRight(nameof(Region), nameof(RegionId))]
    public class AtuCityListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Заповніть поле")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }

        [DisplayName("Тип")]
        [Required(ErrorMessage = "Заповніть поле")]
        [PredicateCase(PredicateOperation.Equals)]
        public string TypeEnum { get; set; }

        [DisplayName("Тип")]
        public string Type { get; set; }

        [PredicateCase(PredicateOperation.Equals)]
        [DisplayName("Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid RegionId { get; set; }

        [DisplayName("Область")]
        public string RegionName { get; set; }
    }

    [RlsRight(nameof(Region), nameof(RegionId))]
    public class AtuCitySelectDTO: BaseDTO
    {
        [PredicateCase(PredicateOperation.Equals)]
        [DisplayName("Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public Guid RegionId { get; set; }
    }
}
