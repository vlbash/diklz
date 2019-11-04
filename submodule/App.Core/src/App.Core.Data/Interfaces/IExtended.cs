using App.Core.Data.Entities;
using System;

namespace App.Core.Data.Interfaces
{
    public interface IExtended
    {
        Guid InnerId { get; set; }
    }
}