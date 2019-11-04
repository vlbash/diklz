using System.Collections.Generic;
using System.Linq;

namespace App.Core.Data.Repositories
{
    public interface IDTORepository<TDTO> where TDTO : class
    {
        IQueryable<TDTO> GetDTO(IDictionary<string, string> parameters = null, params object[] paramArray);

        string GetParameterizedQueryString(IDictionary<string, string> parameters = null, params object[] paramArray);
    }
}
