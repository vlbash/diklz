using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Cdn.Dto
{
    public abstract class BaseEmployeeSpecialityDto: BaseDto
    {
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid EmployeeId { get; set; }

        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual Guid SpecialityId { get; set; }

        [Display(Name = "Основна")]
        [CaseFilter(CaseFilterOperation.Equals)]
        public virtual bool IsMainSpeciality { get; set; }
    }

    public abstract class BaseEmployeeSpecialityDetailDto: BaseEmployeeSpecialityDto
    {
        [Display(Name = "Співробітник")]
        public virtual string EmployeeName { get; set; }

        [Display(Name = "Підпорядкування")]
        public virtual string ParentCaption { get; set; }
    }

    public abstract class BaseEmployeeSpecialityListDto: BaseEmployeeSpecialityDto, IPagingCounted
    {
        public int TotalRecordCount { get; set; }
    }
}
