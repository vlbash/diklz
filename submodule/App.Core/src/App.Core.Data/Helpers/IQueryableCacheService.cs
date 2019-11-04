using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Core.Data.Helpers
{
    public interface IQueryableCacheService
    {
        IEnumerable<T> GetData<T>(string key = null, Expression expression = null) where T : class;
        IEnumerable<T> SetData<T>(IQueryable<T> data, int expirationTime, string key = null) where T : class;
        IEnumerable<T> SetData<T>(IOrderedQueryable<T> data, int expirationTime, string key = null) where T : class;
    }
}
