using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;

namespace App.Core.Business.Providers
{
    public class RedisDatabaseProvider: IRedisdatabaseProvider
    {
        private ConnectionMultiplexer _redisMultiplexer;
        private readonly IConfiguration _configuration;

        public RedisDatabaseProvider(IConfiguration configuration) => _configuration = configuration;

        public IDatabase GetDatabase()
        {
            if (_redisMultiplexer == null)
            {
                var url = _configuration.GetValue<string>("ConnectionStrings:Redis", "<url not specified>");
                try
                {
                    _redisMultiplexer = ConnectionMultiplexer.Connect(url);
                } catch (Exception ex)
                {
                    Log.Fatal(ex, "Unable to connect to Redis by address {0}", url);
                    throw;
                }
                
            }
            return _redisMultiplexer.GetDatabase();
        }
    }
}
