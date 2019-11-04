using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Core.Common.Attributes;
using App.Core.Data.Attributes;
using App.Core.Data.DTO.Common;
using App.Core.Data.Interfaces;
using App.Core.Security;

namespace App.Data.DTO.SEC
{
    public class RightListDTO: BaseDTO, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
        [DisplayName("Активовано")]
        [PredicateCase(PredicateOperation.Equals)]
        public bool IsActive { get; set; }
        [SearchFilter(LabelName: "Назва сутності", FieldType: "text")]
        [DisplayName("Назва сутності")]
        [PredicateCase(PredicateOperation.Contains)]
        public string EntityName { get; set; }
        [DisplayName("Рівень доступу")]
        [PredicateCase(PredicateOperation.Equals)]
        public EntityAccessLevel EntityAccessLevel { get; set; } = EntityAccessLevel.No;
    }

    public class RightDetailDTO: BaseDTO
    {
        [DisplayName("Активовано")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Назва сутності")]
        public string EntityName { get; set; }
        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Рівень доступу")]
        public EntityAccessLevel EntityAccessLevel { get; set; } = EntityAccessLevel.No;
    }
}
