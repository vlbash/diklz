using System.ComponentModel.DataAnnotations;

namespace App.Core.Eq.Entities
{
    public interface IEqBaseEntity
    {
        [Key]
        long Id { get; set; }
    }
}
