using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Core.Business.Services.DistributedCacheService;
using Microsoft.Extensions.Caching.Memory;

namespace App.Business.Services.Common
{
    public class DistributedMemoryCacheService: IDistributedCacheService
    {
        private IMemoryCache _memoryCache { get; }
        public DistributedMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetValue<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public Task<T> GetValueAsync<T>(string key)
        {
            return (Task<T>)_memoryCache.Get(key);
        }

        public void SetValue(string key, object value, TimeSpan? expiry = null)
        {
            if (expiry == null)
            {
                _memoryCache.Set(key, value);
            }
            else
            {
                _memoryCache.Set(key, value, (TimeSpan)expiry);
            }
        }

        public Task SetValueAsync(string key, object value, TimeSpan? expiry = null)
        {
            throw new NotImplementedException();
        }

        public bool ClearKey(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }

        public Task Reset(bool allSources = false)
        {
            throw new NotImplementedException();
        }
    }
}
