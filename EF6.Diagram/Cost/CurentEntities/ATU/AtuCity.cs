using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuCity : BaseEntity
	{
	    [MaxLength(128)]
	    public string Name { get; set; }
	    public string Code { get; set; }
	    public string TypeEnum { get; set; }
        public List<AtuDistrict> Dictrict { get; set; }
	    public List<AtuStreet> Streets { get; set; }
        public Guid AtuRegionId { get; set; }
        public AtuRegion AtuRegion { get; set; }
    }
}