using System;
using System.ComponentModel;
using App.Core.Data.Entities.ATU;
using App.Core.Data.Entities.Common;
using App.Core.Data.Entities.ORG;

namespace App.Core.Data.Interfaces
{
    public interface IEmployee
    {
        Guid PersonId { get; set; }
        //Guid? OrgUnitSpecializationId { get; set; }
        //Guid? RegionId { get; set; }
    }
}
