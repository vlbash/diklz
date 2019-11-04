using System;

namespace App.Core.Data.Interfaces
{
    public interface ISelfReferenced<T>
    {
        string Name { get; set; }
        Guid? ParentId { get; set; }
        T Parent { get; set; }     

    }

    public interface ISelfReferenced {
        string Name { get; set; }
        Guid? ParentId { get; set; }
        object Parent { get; set; }
    }

}
