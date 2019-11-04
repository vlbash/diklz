using System;
using App.Core.Base;

namespace App.Core.Data.Interfaces
{
    public interface IEvent: ICoreEntity
    {
        string DerivedClass { get; set; }

        string Name { get; set; }

        DateTime BeginDate { get; set; }

        DateTime EndDate { get; set; }

        string EventStateEnum { get; set; }

        Guid? ParentId { get; set; }

        string Description { get; set; }
    }
}
