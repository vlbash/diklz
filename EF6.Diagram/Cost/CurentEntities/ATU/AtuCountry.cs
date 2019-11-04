using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuCountry : BaseEntity
    {
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }
        public List<AtuRegion> AtuRegions { get; set; }
    }
}
