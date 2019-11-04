using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using App.Core.Data.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace App.Core.Data.Helpers
{
    public class QueryableCacheService: IQueryableCacheService
    {
        private readonly IMemoryCache _cache;
        public QueryableCacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public IEnumerable<T> GetData<T>(string key = null, Expression expression = null) where T : class
        {
            var cacheKey = GetCacheKey<T>(expression, key);
            if (_cache.TryGetValue<T[]>(cacheKey, out var data))
            {
                foreach (var record in data)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<T> SetData<T>(IQueryable<T> data, int expirationTime, string key = null) where T : class
        {
            var cacheKey = GetCacheKey<T>(data.Expression, key);
            var result = data.ToArray();
            _cache.Set(cacheKey, result, TimeSpan.FromSeconds(expirationTime));
            foreach (var record in result)
            {
                yield return record;
            }
        }

        public IEnumerable<T> SetData<T>(IOrderedQueryable<T> data, int expirationTime, string key = null) where T : class
        {
            var cacheKey = GetCacheKey<T>(data.Expression, key);
            var result = data.ToArray();
            _cache.Set(cacheKey, result, TimeSpan.FromSeconds(expirationTime));
            foreach (var record in result)
            {
                yield return record;
            }
        }

        private string GetCacheKey<T>(Expression expression, string key) where T : class
        {
            var cacheKey = "";
            if (expression != null)
            {
                cacheKey += LocalCollectionExpander.Rewrite(Evaluator.PartialEval(expression, QueryResultCache.CanBeEvaluatedLocally)).ToString();
            }

            if (!string.IsNullOrEmpty(key)) {
                cacheKey += key;
            }

            if (cacheKey.Length > 50)
            {
                cacheKey = cacheKey.ToMd5Fingerprint();
            }
            cacheKey += typeof(T).Name + "_iqueryable_";

            return cacheKey;
        }
    }
}
