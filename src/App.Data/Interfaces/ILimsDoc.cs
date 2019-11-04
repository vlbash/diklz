using System;
using App.Core.Base;
using App.Core.Data.Entities.ORG;
using App.Data.Models;
using App.Data.Models.ORG;

namespace App.Data.Interfaces
{
    public interface ILimsDoc: ICoreEntity
    {
        Guid? ParentId { get; set; }
        LimsDoc Parent { get; set; }

        Guid? PerformerId { get; set; }
        Employee Performer { get; set; }

        string RegNumber { get; set; }
        DateTime? RegDate { get; set; }
        string Description { get; set; }

        Guid OrgUnitId { get; set; }
        OrganizationExt OrgUnit { get; set; }
        
        Guid OrganizationInfoId { get; set; }

        long OldLimsId { get; set; }
    }
}
