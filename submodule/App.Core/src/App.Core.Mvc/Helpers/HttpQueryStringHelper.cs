using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Core.Mvc.Helpers
{
    public static class HttpQueryStringHelper
    {
        /// <summary>
        /// Gets paging parameters from the dictionary of parameters and returns these parameters along with other parameters left as new Dictionary
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public static (int pageSize, int pageNumber, string orderBy, IDictionary<string, string> otherParameters) GetQueryParametersFromQueryParamList(IDictionary<string, string> paramList)
        {
            var pageSize = 0;
            var pageNumber = 0;
            var orderBy = "";
            var usedNames = new string[] { nameof(pageSize).ToLower(), nameof(pageNumber).ToLower(), nameof(orderBy).ToLower() };

            var pagingParameters = paramList.Where(x => usedNames.Contains(x.Key.ToLower())).ToDictionary(x => x.Key.ToLower(), y => y.Value);
            var filteredParameters = paramList.Where(x => !usedNames.Contains(x.Key.ToLower())).ToDictionary(x => x.Key, y => y.Value);

            if (pagingParameters.TryGetValue(nameof(pageSize).ToLower(), out var stringPageSize))
            {
                int.TryParse(stringPageSize, out pageSize);
            }

            if (pagingParameters.TryGetValue(nameof(pageNumber).ToLower(), out var stringPageNumber))
            {
                int.TryParse(stringPageNumber, out pageNumber);
            }

            if (!pagingParameters.TryGetValue(nameof(orderBy).ToLower(), out orderBy))
            {
                orderBy = "Id";
            }

            return (pageSize, pageNumber, orderBy, filteredParameters);
        }
    }
}
