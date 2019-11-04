using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;

namespace App.Core.Data.Interfaces
{
    public interface IDerivableEntity : ICoreEntity
    {
        string DerivedClass { get; set; }
    }
}
