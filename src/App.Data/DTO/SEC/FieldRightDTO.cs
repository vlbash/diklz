using System;
using System.ComponentModel;
using App.Core.Common.Attributes;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;

namespace App.Data.DTO.SEC
{
    public class FieldRightListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        [PredicateCase(PredicateOperation.Equals)]
        public Guid RightId { get; set; }
        [SearchFilter(LabelName: "Назва поля", FieldType: "text")]
        [DisplayName("Назва поля")]
        [PredicateCase(PredicateOperation.Contains)]
        public string FieldName { get; set; }
        [DisplayName("Рівень доступу")]
        [PredicateCase(PredicateOperation.Equals)]
        public AccessLevel AccessLevel { get; set; } = AccessLevel.No;
    }
}
