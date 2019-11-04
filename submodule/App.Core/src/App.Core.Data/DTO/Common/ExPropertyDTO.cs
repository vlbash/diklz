using System;
using App.Core.Data.Entities.Common;
using App.Core.Security;

namespace App.Core.Data.DTO.Common
{
    public class ExPropertyDTO: BaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        // checkbox, number, text, etc
        public string PropTypeEnum { get; set; }
        public string PropType { get; set; }
        // Name of object that extended like ConstructionObject

        public string Group { get; set; }

        public string KindEnum { get; set; }
        public string Kind { get; set; }
        public string COTypeEnum { get; set; }
        public string SortOrder { get; set; }
    }

    public class ExPropertyCOTypeDTO: BaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Guid? EntityId { get; set; }

        public Guid? ExPropertyId { get; set; }

        public string Value { get; set; }

        public string ValueEnum { get; set; }

        public string KindEnum { get; set; }

        public string PropTypeEnum { get; set; }

        public string Group { get; set; }
        public string COTypeEnum { get; set; }
        public string SortOrder { get; set; }
    }
}
