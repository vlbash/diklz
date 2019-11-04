using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.Common
{
    public class ExProperty : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// checkbox, number, text, etc
        /// </summary>
        public string PropTypeEnum { get; set; }

        /// <summary>
        /// Name of object that extended like ConstructionObject
        /// </summary>
        public string Group { get; set; }
        public string KindEnum { get; set; }
    }
}
