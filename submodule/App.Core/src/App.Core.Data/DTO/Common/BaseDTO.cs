using System;
using System.Collections.Generic;
using System.ComponentModel;
using App.Core.Base;

namespace App.Core.Data.DTO.Common
{
    public abstract class BaseDTO: CoreDTO
    {
        [DisplayName("Назва")]
        public virtual string Title => Caption;
    }
}
