using System;
using System.Collections.Generic;
using System.Text;

namespace Astum.Core.Data.Entities.Common
{
    // base entity for dictionary tables
    public class BaseDirectory: BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
