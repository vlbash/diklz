using System;

namespace App.Core.Base
{
    public interface ICoreEntity: IEntity
    {
        string Caption { get; set; }
        Guid ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
