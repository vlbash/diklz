using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Data.Enums;
using App.Core.Data.Interfaces;

namespace App.Core.Data.Entities.Common
{
	[Table("Owner")]
	public class Owner : BaseEntity
	{
		public string Name { get; set; }
		public string Note { get; set; }
	    public string SubjectTypeCode { get; set; } //TODO : ?
        public bool IsSync { get; set; }
		public Language DefaultLanguage { get; set; }
		public void UpdateFromSource(IUpdatableEntity sourceEntity)
		{
			var owner = sourceEntity as Owner;
			if (owner == null)
			{
				return; //todo exception
			}
			//OriginDbId = owner.OriginDbId;
		 //   OriginDbRecordId = owner.OriginDbRecordId;			//OriginDbId = owner.OriginDbId;
		 //   OriginDbRecordId = owner.OriginDbRecordId;
			Name = owner.Name;
			Note = owner.Note;
			IsSync = owner.IsSync;
			DefaultLanguage = owner.DefaultLanguage;
			RecordState = owner.RecordState;
		}
	}
}