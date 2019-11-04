using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Cdn.Models
{
    //[AuditInclude]
    //[AuditDisplay("Довідник координат")]
    [Display(Name = "Довідник координат")]
    [Table("CdnCoordinate")]
    public abstract class BaseCoordinate : BaseEntity
    {
        public virtual string Latitude { get; set; }
        public virtual string Longitude { get; set; }

    }
}
