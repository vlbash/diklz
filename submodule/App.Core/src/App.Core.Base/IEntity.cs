using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Base
{
    public interface IEntity
    {
        Guid Id { get; set; }
        RecordState RecordState { get; set; }
    }
}
