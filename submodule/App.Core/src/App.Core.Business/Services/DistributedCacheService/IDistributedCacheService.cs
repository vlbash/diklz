using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Business.Services.DistributedCacheService
{
    public interface IDistributedCacheService
    {
        T GetValue<T>(string key);

        Task<T> GetValueAsync<T>(string key);

        void SetValue(string key, object value, TimeSpan? expiry = null);

        Task SetValueAsync(string key, object value, TimeSpan? expiry = null);

        bool ClearKey(string key);

        Task Reset(bool allSources = false);
    }
}
