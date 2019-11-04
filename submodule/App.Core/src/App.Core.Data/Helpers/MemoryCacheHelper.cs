using App.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Core.Data.Helpers
{
    public class MemoryCacheHelper
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IList<T> GetCachedEntity<T>(DbContext context,
            double expirationTimeSeconds,
            string cacheKey = null,
            Expression<Func<T, bool>> expression = null) where T : class
        {
            var cacheData = new List<T>();

            if (cacheKey == null)
            {
                cacheKey = typeof(T).ToString();
            }

            string expressionKey = null;
            
            if (expression != null)
            {
                expressionKey = LocalCollectionExpander.Rewrite(Evaluator.PartialEval(expression, QueryResultCache.CanBeEvaluatedLocally)).ToString();
                if (expressionKey.Length > 50)
                {
                    expressionKey = expressionKey.ToMd5Fingerprint();
                }
            }

            try
            {
                if (!_memoryCache.TryGetValue(cacheKey + expressionKey, out cacheData))
                {
                    if (expression != null)
                    {
                        cacheData = context.Set<T>().Where(expression).ToList();
                    }
                    else
                    {
                        cacheData = context.Set<T>().ToList();
                    }
                    
                    if (expirationTimeSeconds > 0)
                    {
                        _memoryCache.Set(cacheKey + expressionKey, cacheData,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(expirationTimeSeconds)));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MemoryCaching error {ex.Message}");
            }

            return cacheData;
        }

        public dynamic Cache(string cacheKey, out object cacheObject, double expirationTimeSeconds, Func<object> getObject)
        {
            cacheObject = null;

            try
            {
                if (!_memoryCache.TryGetValue(cacheKey, out cacheObject))
                {
                    cacheObject = getObject();
                    if (expirationTimeSeconds > 0)
                    {
                        _memoryCache.Set(cacheKey, cacheObject, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(expirationTimeSeconds)));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"MemoryCaching error {ex.Message}");
            }

            return cacheObject;
        }
    }
}
