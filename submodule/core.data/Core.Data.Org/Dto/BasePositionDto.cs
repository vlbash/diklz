using System;
using System.ComponentModel.DataAnnotations;
using Core.Base.Data;

namespace Core.Data.Org.Dto
{
    public abstract class BasePositionDetailDto : BaseDto
    {
        [Display(Name = "Назва")]
        public virtual string Name { get; set; }
    }

    //[RightsCheckList(nameof(OrgUnitPosition))]
    //[RlsRight(nameof(Organization), nameof(OrganizationId))]
    public abstract class BasePositionListDto: BaseDto, IPagingCounted
    {
        public virtual int TotalRecordCount { get; set; }
    
        [Required]
        [Display(Name = "Назва")]
        public virtual string Name { get; set; }

        public virtual Guid OrganizationId { get; set; }
    }
}

