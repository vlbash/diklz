using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Table("MisDoctor")]
    public abstract class BaseDoctor: BaseEntity
    {
        public virtual bool TreatsChildren { get; set; }
    }
}
