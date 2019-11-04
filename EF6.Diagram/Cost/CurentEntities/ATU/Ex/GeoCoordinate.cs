using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Astum.Core.Data.Interfaces;

namespace Astum.Core.Data.Entities.Ex.ATU
{
    [Table("GeoCoordinate")]
	public class GeoCoordinate : BaseEntity, IOwnedEntity, IUpdatableEntity
    {
        [StringLength(256)]
        public string Latitude { get; set; }
        [StringLength(256)]
        public string Longitude { get; set; }
        public Guid? OwnerId { get; set; }
        public Owner Owner { get; set; }
        public void UpdateFromSource(IUpdatableEntity sourceEntity)
        {
            var entity = sourceEntity as GeoCoordinate;
            if (entity == null)
            {
                return; //todo exception
            }

            OwnerId = entity.OwnerId;
            //OriginDbId = entity.OriginDbId;
            //OriginDbRecordId = entity.OriginDbRecordId;            //OriginDbId = entity.OriginDbId;
            RecordState = entity.RecordState;
        }
    }
}
