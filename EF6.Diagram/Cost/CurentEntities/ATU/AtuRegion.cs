using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuRegion : BaseEntity
    {
        public AtuRegion()
        {
            Children = new HashSet<AtuRegion>();
        }

        [DisplayName("Посилання на батьківський запис")]
        public Guid? ParentId { get; set; }
        public AtuRegion Parent { get; set; }
        public ICollection<AtuRegion> Children { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Code { get; set; }

        public List<AtuCity> Cities { get; set; }
        public Guid AtuCountryId { get; set; }
        public AtuCountry AtuCountry { get; set; }
    }
}
