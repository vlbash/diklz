using System;
using Core.Base.Data;

namespace Core.Data.Common.Dto
{
    public abstract class BaseExPropertyDto: BaseDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        // checkbox, number, text, etc
        public virtual string PropTypeEnum { get; set; }
        public virtual string PropType { get; set; }
        // Name of object that extended like ConstructionObject

        public virtual string Group { get; set; }

        public virtual string KindEnum { get; set; }
        public virtual string Kind { get; set; }
        public virtual string CoTypeEnum { get; set; }
        public virtual string SortOrder { get; set; }
    }

    public abstract class BaseExPropertyCoTypeDto: BaseDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        public virtual Guid? EntityId { get; set; }

        public virtual Guid? ExPropertyId { get; set; }

        public virtual string Value { get; set; }

        public virtual string ValueEnum { get; set; }

        public virtual string KindEnum { get; set; }

        public virtual string PropTypeEnum { get; set; }

        public virtual string Group { get; set; }
        public virtual string CoTypeEnum { get; set; }
        public virtual string SortOrder { get; set; }
    }
}
