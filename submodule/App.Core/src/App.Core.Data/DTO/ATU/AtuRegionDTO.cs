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
    [RlsRight(nameof(Region), nameof(Id))]
    public class AtuRegionListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Заповніть поле")]
        [PredicateCase(PredicateOperation.Contains)]
        public string Name { get; set; }

        [DisplayName("Код")]
        public string Code { get; set; }


        [DisplayName("Посилання на батьківський запис")]
        [PredicateCase(PredicateOperation.Equals)]
        public Guid? ParentId { get; set; }

        [DisplayName("Посилання на батьківський запис")]
        public string ParentName { get; set; }

        public Guid AtuCountryId { get; set; }

        [DisplayName("Країна")]
        public string AtuCountryName { get; set; }
    }

    [RlsRight(nameof(Region), nameof(Id))]
    public class AtuRegionSelectDTO: BaseDTO
    {
        [DisplayName("Посилання на батьківський запис")]
        [PredicateCase(PredicateOperation.Equals)]
        public Guid? ParentId { get; set; }
    }
}
