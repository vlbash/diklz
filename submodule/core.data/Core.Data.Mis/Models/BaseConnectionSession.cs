using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Table("MisConnectionSession")]
    public abstract class BaseConnectionSession: BaseEntity/*, IEvent, IDerivedEntity*/
    {
        //#region IDerivedEntity
        //static Type _baseType = typeof(Event);
        //static Type _type = typeof(ConnectionSession);
        //static IMapper _mapper = new MapperConfiguration(cfg => cfg.CreateMap<ConnectionSession, Event>()
        //    .ForMember(x => x.DerivedClass, opt => opt.MapFrom(o => _type.Name)).MapOnlyIfChanged()).CreateMapper();
        //public override IMapper _Mapper { get; } = _mapper;
        //public override Type _BaseType { get; } = _baseType;
        //public override Type _Type { get; } = _type;
        //public string BaseClass { get; set; } = _baseType.Name;
        //public override IEntity _BaseQuery(DbContext context) => context.Set<Event>().SingleOrDefault(x => x.Id == Id);
        //#endregion

        //IEvent
        //public string DerivedClass { get; set; }

        public virtual DateTime BeginDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual string EventStateEnum { get; set; }

        public virtual Guid? ParentId { get; set; }

        public virtual string Description { get; set; }
    }
}
