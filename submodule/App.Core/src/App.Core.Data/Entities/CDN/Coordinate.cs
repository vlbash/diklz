using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Entities.Common;
using Z.EntityFramework.Plus;

namespace App.Core.Data.Entities.CDN
{
    [AuditInclude]
    [AuditDisplay("Довідник координат")]
    [Display(Name = "Довідник координат")]
    [Table("Cdn" + nameof(Coordinate))]
    public class Coordinate : BaseEntity
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
