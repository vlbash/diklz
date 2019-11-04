using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using App.Core.Data.Entities.Common;
using App.Core.Base;
using Z.EntityFramework.Plus;
using App.Core.Security;

namespace App.Core.Data.Entities.ORG
{
    [AuditInclude]
    [AuditDisplay("Організація")]
    [Display(Name = "Організація")]
    [Table("Org" + nameof(Organization))]
    [RlsRight(nameof(Organization), nameof(Id))]
    public class Organization : BaseEntity, IOrgUnit, IDerivedEntity
    {
        #region IDerivedEntity
        static Type _baseType = typeof(OrgUnit);
        static Type _type = typeof(Organization);
        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<Organization, OrgUnit>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();
        public override IMapper _Mapper { get; } = _mapper;
        public override Type _BaseType { get; } = _baseType;
        public override Type _Type { get; } = _type;
        public string BaseClass { get; set; } = _baseType.Name;
        public override IEntity _BaseQuery(DbContext context) => context.Set<OrgUnit>().SingleOrDefault(x => x.Id == Id);
        #endregion


        //IOrgUnit
        [DisplayName("Назва")]
        public string Name { get; set; }
        [DisplayName("Коротка назва")]
        public string ShortName { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string Category { get; set; }
    }
}
