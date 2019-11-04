using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Atu.Dto
{
    //[RlsRight(nameof(Region), nameof(RegionId))]
    public abstract class BaseCityListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Заповніть поле")]
        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string Name { get; set; }

        [Display(Name = "Код")]
        public virtual string Code { get; set; }

        [Display(Name = "Тип")]
        [Required(ErrorMessage = "Заповніть поле")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual string TypeEnum { get; set; }

        [Display(Name = "Тип")]
        public virtual string Type { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        [Display(Name = "Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid RegionId { get; set; }

        [Display(Name = "Область")]
        public virtual string RegionName { get; set; }
    }

    //[RlsRight(nameof(Region), nameof(RegionId))]
    public abstract class BaseCitySelectDto: BaseDto
    {
        [CaseFilter(CaseFilterOperation.Equals)]
        [Display(Name = "Область")]
        [Required(ErrorMessage = "Заповніть поле")]
        public virtual Guid RegionId { get; set; }
    }
}
