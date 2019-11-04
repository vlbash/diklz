using System;
using System.Threading.Tasks;

namespace App.Business.Services.RedisProvider
{
    public interface IRedisProviderService
    {
        Task SetValue(string key, string value, TimeSpan expirationTime);

        bool GetValue(string key, out string value);
    }
}
