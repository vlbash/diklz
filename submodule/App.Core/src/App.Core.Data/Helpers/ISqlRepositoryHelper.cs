using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Security;

namespace App.Core.Data.Helpers
{
    public interface ISqlRepositoryHelper
    {
        string AddConditionsToQueryText(Type type, string queryText, IDictionary<string, string> parameters);
        string AddRecordCountToQueryText(string queryText);
        string GetFullQueryText(Type type, bool addCount = false, UserApplicationRights rights = null);
        Task<string> GetFullQueryTextAsync(Type type, bool addCount = false, UserApplicationRights rights = null);
        string GetParameterizedQueryString(Type type, IDictionary<string, string> parameters = null, bool addCount = false, UserApplicationRights rights = null, params object[] formatParams);
        Task<string> GetParameterizedQueryStringAsync(Type type, IDictionary<string, string> parameters = null, bool addCount = false, UserApplicationRights rights = null, params object[] formatParams);
        string GetRightsQueryString(Type type, UserApplicationRights rights);
        string GetSqlText(Type type);
        Task<string> GetSqlTextAsync(Type type);
        string NormalizeSqlQueryText(string queryText);
    }
}
