using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace App.Core.Business.Extensions
{
    public static class IDatabaseExtensions
    {
        public static T GetValueFromString<T>(this IDatabase source, string key)
        {
            var stringValue = source.StringGet(key).ToString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(stringValue);
        }

        public static void SaveValueAsString(this IDatabase source, string key, object value, TimeSpan? expiry = null)
        {
            var stringValue = JsonConvert.SerializeObject(value);
            source.StringSet(key, stringValue, expiry);
        }

        public static async Task<T> GetValueFromStringAsync<T>(this IDatabase source, string key)
        {
            var value = await source.StringGetAsync(key);
            var stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(stringValue);
        }

        public static async Task SaveValueAsStringAsync(this IDatabase source, string key, object value, TimeSpan? expiry = null)
        {
            var stringValue = JsonConvert.SerializeObject(value);
            await source.StringSetAsync(key, stringValue, expiry);
        }

        public static bool ClearKey(this IDatabase source, string key)
        {
            return source.KeyDelete(key);
        }
    }
}
