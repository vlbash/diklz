using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Astum.Core.Data.Interfaces
{
    public interface IDerivableEntity : IBaseEntity
    {
        string DerivedClass { get; set; }
    }
}
