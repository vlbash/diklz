using Astum.Core.Data.Entities.Common;
using System;

namespace Astum.Core.Data.Entities.Common
{
    public class EntityExProperty : BaseEntity
    {
        public Guid EntityId { get; set; }
        public Guid ExPropertyId { get; set; }
        public string Value { get; set; }
        public string ValueEx { get; set; }
    }
}
