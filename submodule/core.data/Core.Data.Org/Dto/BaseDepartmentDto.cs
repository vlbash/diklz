using System;
using Core.Base.Data;
using Core.Common.Attributes;
using Core.Common.Enums;

namespace Core.Data.Org.Dto
{
    //[RightsCheckList(nameof(Department), nameof(Organization))]
    //[RlsRight(nameof(Organization), nameof(ParentId))]
    public abstract class BaseDepartmentDetailDto: BaseDto, IOrgUnitDetailDto
    {
        public virtual string Name { get; set; }
        public virtual string State { get; set; }
        public virtual string Parent { get; set; }
        public virtual Guid? ParentId { get; set; }
        public virtual string Category { get; set; }
        public virtual string Description { get; set; }
    }

    //[RightsCheckList(nameof(Department))]
    public abstract class BaseDepartmentListDto: BaseDto, IOrgUnitListDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }

        [CaseFilter(CaseFilterOperation.Contains)]
        public virtual string Name { get; set; }
    }
}
