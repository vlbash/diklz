using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Core.Business.Extensions;
using App.Core.Business.Providers;
using StackExchange.Redis;

namespace App.Core.Business.Services.DistributedCacheService
{
    public class DistributedCacheService: IDistributedCacheService
    {
        private readonly IRedisdatabaseProvider _sourceProvider;
        public DistributedCacheService(IRedisdatabaseProvider sourceProvider)
        {
            _sourceProvider = sourceProvider;
        }

        public T GetValue<T>(string key)
        {
            var source = _sourceProvider.GetDatabase();
            return source.GetValueFromString<T>(key);
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            var source = _sourceProvider.GetDatabase();
            return await source.GetValueFromStringAsync<T>(key);
        }

        public void SetValue(string key, object value, TimeSpan? expiry = null)
        {
            var source = _sourceProvider.GetDatabase();
            source.SaveValueAsString(key, value, expiry);
        }

        public async Task SetValueAsync(string key, object value, TimeSpan? expiry = null)
        {
            var source = _sourceProvider.GetDatabase();
            await source.SaveValueAsStringAsync(key, value, expiry);
        }

        public bool ClearKey(string key)
        {
            var source = _sourceProvider.GetDatabase();
            return source.KeyDelete(key);
        }

        public async Task Reset(bool allSources = false)
        {
            var source = _sourceProvider.GetDatabase();
            if (allSources)
            {
                await source.ExecuteAsync("FLUSHALL");
            } else
            {
                await source.ExecuteAsync("FLUSHDB");
            }
        }
    }
}
