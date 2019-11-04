using System;
using System.ComponentModel;
using Astum.Core.Data.Entities.Common;
using Astum.Core.Data.Enums;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuPostOfficeByDistrict : BaseEntity
    {
        [DisplayName("Поштове відділення")]
        public Guid AtuPostOfficeId { get; set; }
        public AtuPostOffice AtuPostOffice { get; set; }

        [DisplayName("Район м. Києва")]
        public Guid AtuDistrictId { get; set; }
        public AtuDistrict AtuDistrict { get; set; }
    }
}
