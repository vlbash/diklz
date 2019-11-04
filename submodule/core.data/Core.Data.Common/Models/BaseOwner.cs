using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;
using Core.Common.Enums;

namespace Core.Data.Common.Models
{
	[Table("Owner")]
	public abstract class BaseOwner : BaseEntity
	{
		public virtual string Note { get; set; }
	    public virtual string SubjectTypeCode { get; set; } //TODO : ?
        public virtual bool IsSync { get; set; }
		public virtual Language DefaultLanguage { get; set; }
	}
}
