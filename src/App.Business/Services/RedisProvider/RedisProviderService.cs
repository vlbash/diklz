using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace App.Business.Services.RedisProvider
{
    public class RedisProviderService : IRedisProviderService
    {
        private ConnectionMultiplexer redis;
        private IDatabase database;
        private readonly IConfiguration _config;
        
        public RedisProviderService(IConfiguration config)
        {
            _config = config;
            redis = ConnectionMultiplexer.Connect(_config["RedisConnection"]);
            database = redis.GetDatabase();
        }

        public async Task SetValue(string key, string value, TimeSpan expirationTime)
        {
            await database.StringSetAsync(key, value, expirationTime);
        }

        public bool GetValue(string key, out string value)
        {
            //database.KeyDelete(key);
            value = database.StringGet(key);
            if (string.IsNullOrEmpty(value))
                return false;
            return true;
        }
    }
}
