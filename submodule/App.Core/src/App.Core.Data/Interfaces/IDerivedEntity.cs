using System;
using App.Core.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Data.Interfaces
{
    public interface IDerivedEntity : IEntity
    {
        string BaseClass { get; set; }
        IMapper _Mapper { get; }
        Type _BaseType { get; }
        Type _Type { get; }
        IEntity _BaseQuery(DbContext context);
        IEntity _BaseClone { get; }
        IEntity _BaseUpdate(DbContext context);
    }
}
