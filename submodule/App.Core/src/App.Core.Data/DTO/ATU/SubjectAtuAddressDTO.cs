using App.Core.Data.DTO.Common;
using System;
using App.Core.Data.Entities.ATU;
using App.Core.Security;

namespace App.Core.Data.DTO.ATU
{
    public class SubjectAtuAddressDTO : BaseDTO
    {
        public string AddressType { get; set; }
        public string PostIndex { get; set; }
        public string Building { get; set; }
        public Guid StreetId { get; set; }
        public Guid CityId { get; set; }
        public string FullAddress { get; set; }
    }
}
