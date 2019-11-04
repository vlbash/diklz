using Astum.Core.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using Astum.Core.Data.Entities.Common;

namespace Astum.Core.Data.Entities.ATU
{
    public class AtuDistrict : BaseEntity
    {
	    [MaxLength(128)]
	    public string Name { get; set; }
	    public Guid CityId { get; set; }
        public AtuCity City { get; set; }

        [Range(0,99,ErrorMessage="Код повинен бути не більше 2 цифр")]
        public int Code { get; set; }
    }
}
