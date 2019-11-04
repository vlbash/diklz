using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Base.Data;

namespace Core.Data.Mis.Models
{
    [Display(Name = "Класифікатор груп первинної медичної допомоги")]
    [Table("CdnIcpc2Group")]
    public abstract class BaseIcpc2Group: BaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
