using App.Core.Base;

namespace App.Business.Interfaces
{
    public interface IOldLimsSyncable: IEntity
    {
        long OldLimsId { get; set; }
    }
}
