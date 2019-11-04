using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;

namespace Core.Data.Org.Dto
{
    //[RightsCheckList(nameof(OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Employee), nameof(Person), 
    //    nameof(OrgUnit))]
    public abstract class BaseUnitPositionEmployeeDetailDto : BaseDto
    {
        public virtual Guid OrgUnitPositionId { get; set; }
        public virtual string OrgUnitPositionName { get; set; }

        public virtual Guid EmployeeId { get; set; }
        public virtual string OrgEmployeeName { get; set; }
        public virtual string OrgEmployeeMiddleName { get; set; }
        public virtual string OrgEmployeeFullName { get; set; }
        public virtual string OrgPositionCode { get; set; }
        public virtual Guid PersonId { get; set; }

        [Display(Name = "П.I.Б.")]
        public virtual string OrgEmployeeFIO { get; set; }

        [Display(Name = "П.I.Б.")]
        public virtual string OrgEmployeeFIOShort { get; set; }

        public virtual Guid OrgUnitId { get; set; }
        public virtual string OrgUnitName { get; set; }
    }

    //[RightsCheckList(nameof(OrgUnitPosition), nameof(OrgUnitPositionEmployee), nameof(Employee), nameof(Person),
    //    nameof(OrgUnit))]
    public abstract class BaseUnitPositionEmployeeSelectDto : BaseDto
    {
        public virtual Guid OrgUnitId { get; set; }
    }
}
