using System;
using System.Linq;
using App.Core.Base;
using App.Core.Data.CustomAutoMapper;
using App.Core.Data.Entities.ORG;
using App.Core.Data.Interfaces;
using App.Data.Interfaces;
using App.Data.Models.APP;
using App.Data.Models.ORG;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Data.Models.TRL
{
    public class TrlApplication: BaseApplication, ILimsDoc, IDerivedEntity
    {
        #region ILimsDoc
        public Guid? ParentId { get; set; }
        public LimsDoc Parent { get; set; }
        public Guid? PerformerId { get; set; }
        public Employee Performer { get; set; }
        public string RegNumber { get; set; }
        public DateTime? RegDate { get; set; }
        public string Description { get; set; }
        public Guid OrgUnitId { get; set; }
        public OrganizationExt OrgUnit { get; set; }
        public Guid OrganizationInfoId { get; set; }
        public long OldLimsId { get; set; }
        public string ReturnComment { get; set; }
        public bool ReturnCheck { get; set; }
        #endregion

        #region IDerivedEntity

        static Type _baseType = typeof(LimsDoc);
        static Type _type = typeof(TrlApplication);

        static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<TrlApplication, LimsDoc>()
            .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();

        public override IMapper _Mapper { get; } = _mapper;
        public override Type _BaseType { get; } = _baseType;
        public override Type _Type { get; } = _type;
        public string BaseClass { get; set; } = _baseType.Name;

        public override IEntity _BaseQuery(DbContext context) =>
            context.Set<LimsDoc>().SingleOrDefault(x => x.Id == Id);

        #endregion

    }
}
