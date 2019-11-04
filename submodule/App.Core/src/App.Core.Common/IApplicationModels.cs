using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Common
{
    public interface IApplicationModels
    {
        IEnumerable<Type> GetApplicationModels();
    }
}
