using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Astum.Core.Data.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
