using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.ATU
{
    [AuditInclude]
    [AuditDisplay("Довідник поштових відділень районів міста")]
    [Display(Name = "Довідник поштових відділень районів міста")]
    [Table("Atu" + nameof(PostOfficeByDistrict))]
    public class PostOfficeByDistrict : BaseEntity
    {
        [DisplayName("Поштове відділення")]
        public Guid PostOfficeId { get; set; }
        public PostOffices PostOffice { get; set; }

        [DisplayName("Район м. Києва")]
        public Guid CityDistrictId { get; set; }
        public CityDistricts CityDistrict { get; set; }
    }
}
