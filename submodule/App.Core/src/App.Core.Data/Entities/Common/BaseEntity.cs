using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Base;
using App.Core.Data.Enums;
using App.Core.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Data.Entities.Common
{
    public abstract class BaseEntity : CoreEntity
    {
        #region IDerivedEntity
        public virtual IMapper _Mapper { get; }
        public virtual Type _BaseType { get; }
        public virtual Type _Type { get; }
        public virtual IEntity _BaseQuery(DbContext context) => null;
        public IEntity _BaseClone => (IEntity)_Mapper.Map(this, _Type, _BaseType);
        public IEntity _BaseUpdate(DbContext context)
        {
            var baseEntity = _BaseQuery(context);
            if (baseEntity == null)
            {
                baseEntity = _BaseClone;
                context.Add(baseEntity);
                return baseEntity;
            }
            return _Mapper.Map(this, baseEntity);
        }
        #endregion
    }
}
