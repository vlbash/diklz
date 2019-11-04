using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Core.Common.Extensions;
using App.Core.Security;

namespace App.Core.Data.Helpers
{
    public class SqlRepositoryHelper: ISqlRepositoryHelper
    {
        private readonly IQueryConditionsHelper _queryConditionsHelper;
        private static readonly Dictionary<Type, string> _texts = new Dictionary<Type, string>();
        private static readonly string _queryAlias = "qry";
        private static readonly string _conditionsPlaceholder = "where (true)";
        private static readonly string _startOfQuery = "select * from";
        private static readonly bool _useSnakeCase = true;

        public SqlRepositoryHelper(IQueryConditionsHelper queryConditionsHelper)
        {
            _queryConditionsHelper = queryConditionsHelper;
        }

        public string GetSqlText(Type type)
        {
            if (_texts.TryGetValue(type, out var result))
            {
                return result;
            }

            using (var stream = GetResourceStream(type))
            {
                using (var reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            _texts[type] = result;
            return result;
        }

        public async Task<string> GetSqlTextAsync(Type type)
        {
            if (_texts.TryGetValue(type, out var result))
            {
                return result;
            }

            using (var stream = GetResourceStream(type))
            {
                using (var reader = new StreamReader(stream))
                {
                    result = await reader.ReadToEndAsync();
                }
            }
            _texts[type] = result;

            return result;
        }

        public string NormalizeSqlQueryText(string queryText)
        {
            var resultString = queryText;
            var endQueryPlaceHolder = $"as {_queryAlias} {_conditionsPlaceholder}";
            var needsWrapping = !resultString.Contains(_startOfQuery) || !resultString.Contains(endQueryPlaceHolder);
            if (needsWrapping)
            {
                resultString = $"{_startOfQuery} ({resultString}) {endQueryPlaceHolder}";
            }

            return resultString;
        }

        public string AddRecordCountToQueryText(string queryText)
        {
            var resultString = queryText;
            var firstSelectOccurrence = resultString.IndexOf(_startOfQuery);
            var startOfFinalQuery = "select *, count(*) over() as total_record_count from";
            var endOfFinalQuery = resultString.Substring(firstSelectOccurrence + _startOfQuery.Length);
            resultString = startOfFinalQuery + " " + endOfFinalQuery;

            return resultString;
        }

        public string AddConditionsToQueryText(Type type, string queryText, IDictionary<string, string> parameters)
        {
            var result = queryText;
            var conditions = _queryConditionsHelper.GetQueryConditionsString(type, parameters, _queryAlias);
            if (!string.IsNullOrEmpty(conditions))
            {
                result = result.Replace(_conditionsPlaceholder, _conditionsPlaceholder + " and " + conditions);
            }
            return result;
        }

        /// <summary>
        /// Gets rights query conditions string, that will be added to sql query as "where" clause
        /// </summary>
        /// <exception cref="NoRightsException">If user doesn't have permission at all</exception>
        /// <param name="type"></param>
        /// <param name="rights"></param>
        /// <returns></returns>
        public string GetRightsQueryString(Type type, UserApplicationRights rights)
        {
            var rightsQueryString = "";
            var rls = rights.GetTypeFieldsRlsRights(type);
            foreach (var right in rls)
            {
                if (right.PermissionType == RowLevelModelPermissionType.No)
                {
                    // this should never happen because such restriction will be resolved when entire entity is checked
                    throw new NoRightsException($"Access to {type.Name} is denied by field {right.Name}");
                }
                else if (right.PermissionType == RowLevelModelPermissionType.All)
                {
                    continue;
                }
                else if (right.PermissionType == RowLevelModelPermissionType.Specified
                  || right.PermissionType == RowLevelModelPermissionType.Except)
                {

                    var idsString = "";
                    foreach (var id in right.Entities)
                    {
                        if (string.IsNullOrEmpty(idsString))
                        {
                            idsString += "(";
                        }
                        else
                        {
                            idsString += ", ";
                        }
                        idsString += "'" + id.ToString() + "'";
                    }

                    if (!string.IsNullOrEmpty(idsString))
                    {
                        idsString += ")";
                    }

                    if (right.PermissionType == RowLevelModelPermissionType.Specified)
                    {
                        idsString = "in " + idsString;
                    }
                    else
                    {
                        idsString = "not in " + idsString;
                    }

                    var fieldName = right.Name;
                    if (_useSnakeCase)
                    {
                        fieldName = fieldName.ToSnakeCase();
                    }
                    var startOfString = string.IsNullOrEmpty(_queryAlias) ? "" : _queryAlias + "." + fieldName;
                    idsString = startOfString + " " + idsString;
                    if (string.IsNullOrEmpty(rightsQueryString))
                    {
                        rightsQueryString += idsString;
                    }
                    else
                    {
                        rightsQueryString += " and " + idsString;
                    }

                }
            }
            return rightsQueryString;
        }

        public string GetParameterizedQueryString(Type type,
            IDictionary<string, string> parameters = null,
            bool addCount = false,
            UserApplicationRights rights = null,
            params object[] formatParams)
        {
            var queryString = GetFullQueryText(type, addCount, rights);
            queryString = AddConditionsToQueryText(type, queryString, parameters);
            return string.Format(queryString, formatParams);
        }

        public async Task<string> GetParameterizedQueryStringAsync(Type type,
            IDictionary<string, string> parameters = null,
            bool addCount = false,
            UserApplicationRights rights = null,
            params object[] formatParams)
        {
            var queryString = await GetFullQueryTextAsync(type, addCount, rights);
            queryString = AddConditionsToQueryText(type, queryString, parameters);
            return string.Format(queryString, formatParams);
        }

        public string GetFullQueryText(Type type, bool addCount = false, UserApplicationRights rights = null)
        {
            var queryString = GetSqlText(type);
            return GetFullQueryTextInternal(queryString, type, addCount, rights);
        }

        public async Task<string> GetFullQueryTextAsync(Type type, bool addCount = false, UserApplicationRights rights = null)
        {
            var queryString = await GetSqlTextAsync(type);
            return GetFullQueryTextInternal(queryString, type, addCount, rights);
        }

        private string GetFullQueryTextInternal(string queryString, Type type, bool addCount = false, UserApplicationRights rights = null)
        {
            var normalizedQueryString = NormalizeSqlQueryText(queryString);

            var result = normalizedQueryString;
            if (addCount)
            {
                result = AddRecordCountToQueryText(normalizedQueryString);
            }

            if (rights != null)
            {
                var rightsQueryString = GetRightsQueryString(type, rights);
                if (!string.IsNullOrEmpty(rightsQueryString))
                {
                    result += $" and {rightsQueryString}";
                }
            }

            return result;
        }

        private Stream GetResourceStream(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            var resourceName = type.Namespace + "." + type.Name + ".sql";

            resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.ToLowerInvariant() == resourceName.ToLowerInvariant());

            if (resourceName == null)
            {
                throw new Exception($"Resource file for {type.Name} not found! If file does exist check that 'Embedded resource' property is set");
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            return stream;
        }
    }
}
