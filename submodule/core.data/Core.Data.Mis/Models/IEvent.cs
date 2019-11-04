using System;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    public interface IEvent: ICoreEntity
    {
        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        string EventStateEnum { get; set; }

        Guid? ParentId { get; set; }

        string Description { get; set; }
    }
}
