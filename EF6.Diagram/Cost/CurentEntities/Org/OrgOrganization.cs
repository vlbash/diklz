using Astum.Core.Data.CustomAutoMapper;
using Astum.Core.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ORG
{
    public sealed class OrgOrganization : BaseEntity, IOrgUnit, IDerivedEntity
    {
        #region IDerivedEntity
        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<OrgOrganization, OrgUnit>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => typeof(OrgOrganization).Name)).MapOnlyIfChanged()).CreateMapper();
        static string _baseClass = typeof(OrgUnit).Name;
        public string BaseClass { get; set; } = _baseClass;
        public IEntity BaseClone => _mapper.Map<OrgUnit>(this);
        public IEntity BaseQuery(DbContext context)
        {
            return context.Set<OrgUnit>().Single(x => x.Id == Id);
        }
        public IEntity BaseUpdate(DbContext context)
        {
            return _mapper.Map(this, BaseQuery(context));
        }
        #endregion


        //IOrgUnit
        [DisplayName("Назва")]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public ICollection<OrgUnitAtuAddress> OrgUnitAtuAddresses { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
    }
}
