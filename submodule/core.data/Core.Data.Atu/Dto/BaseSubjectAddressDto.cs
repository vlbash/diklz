using System;
using Core.Base.Data;

namespace Core.Data.Atu.Dto
{
    public abstract class BaseSubjectAddressDto : BaseDto
    {
        public virtual string AddressType { get; set; }
        public virtual string PostIndex { get; set; }
        public virtual string Building { get; set; }
        public virtual Guid StreetId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual string FullAddress { get; set; }
    }
}
