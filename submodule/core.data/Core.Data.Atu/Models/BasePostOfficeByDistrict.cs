using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Atu.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник поштових відділень районів міста")]
    [Display(Name = "Довідник поштових відділень районів міста")]
    [Table("AtuPostOfficeByDistrict")]
    public abstract class BasePostOfficeByDistrict : BaseEntity
    {
        [Display(Name = "Поштове відділення")]
        public virtual Guid PostOfficeId { get; set; }

        [Display(Name = "Район м. Києва")]
        public virtual Guid CityDistrictId { get; set; }
    }
}
