using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Org.Dto
{
    public abstract class BaseUnitPositionDetailDto : BaseDto
    {
        public virtual Guid OrgUnitId { get; set; }
        public virtual string OrgUnitName { get; set; }
        public virtual Guid OrgPositionId { get; set; }
        public virtual string OrgPositionName { get; set; }


        public virtual string OrgPositionType { get; set; }
        public virtual string OrgPositionTypeName { get; set; }
        public virtual bool IsResource { get; set; }

        [NotMapped]
        [Display(Name = "Назва")]
        string Name => String.Format("{0}-{1}", OrgUnitName, OrgPositionName);
    }

    //[RightsCheckList(nameof(OrgUnitPosition), nameof(Position))]
    public abstract class BaseUnitPositionListDto : BaseDto
    {
        public virtual Guid OrgUnitId { get; set; }
    }
}
