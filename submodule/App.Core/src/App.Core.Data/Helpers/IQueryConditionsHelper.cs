using System;
using System.Collections.Generic;
using System.Text;

namespace App.Core.Data.Helpers
{
    public interface IQueryConditionsHelper
    {
        string GetQueryConditionsString(Type type, IDictionary<string, string> parameters, string queryAlias);
    }
}
