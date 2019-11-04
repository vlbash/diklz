using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;
using App.Data.Models.APP;

namespace App.Data.DTO.Common
{
    [RightsCheckList(nameof(AppProtocol))]
    public class ProtocolDTO : BaseDTO
    {
        [DisplayName("Стан протоколу")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string StatusName { get; set; }

        [DisplayName("№ протоколу")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string ProtocolNumber { get; set; }

        [DisplayName("Дата протоколу")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ProtocolDate { get; set; }

        [DisplayName("№ наказу")]
        [Required(ErrorMessage = "Заповніть поле")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата наказу")]
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }

        public string Type { get; set; }
    }

    [RightsCheckList(nameof(AppProtocol))]
    public class ProtocolListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }

        [DisplayName("Стан протоколу"), PredicateCase]
        public string StatusName { get; set; }

        [DisplayName("№ протоколу"), PredicateCase(PredicateOperation.Contains)]
        public string ProtocolNumber { get; set; }

        [DisplayName("Дата протоколу"), PredicateCase(PredicateOperation.InputRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProtocolDate { get; set; }

        [DisplayName("№ наказу")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата наказу"), PredicateCase(PredicateOperation.InputRange)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        public string Type { get; set; }
    }
}
