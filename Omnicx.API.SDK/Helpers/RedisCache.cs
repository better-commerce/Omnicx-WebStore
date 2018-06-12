using System;
using System.Collections.Generic;
using System.Linq;
using CachingFramework.Redis;
using StackExchange.Redis;
using Omnicx.API.SDK.Entities;
namespace Omnicx.API.SDK.Helpers
{
    class RedisCache : ICacheManager
    {
        private static Context _redisCacheContext;
        public RedisCache()
        {

            var config = ConfigurationOptions.Parse(ConfigKeys.RedisCacheConnetionString);
            config.ConnectTimeout = 50000;
            config.SyncTimeout = 50000;
            config.ConnectRetry = 3000;
            _redisCacheContext = new Context(config);
           
        }
        public T Get<T>(string key)
        {
            return _redisCacheContext.Cache.GetObject<T>(key.ToLower());
        }

        public bool IsKeyExist(string key)
        {
            return _redisCacheContext.Cache.KeyExists(key);
        }

        public void Set(string key, object data, int cacheTime, string[] tags = null)
        {
             tags = tags?? new[] { "" };
            _redisCacheContext?.Cache.SetObject(key.ToLower(), data, tags, TimeSpan.FromMinutes(cacheTime));
        }

        public void SetList<T>(string key, IList<T> data, int cacheTime = 1440, string[] tags = null)
        {
            var list = _redisCacheContext?.Collections.GetRedisList<T>(key + ":list");
            list?.Clear();
            list?.AddRange(data);
        }
        public IList<T> GetList<T>(string key)
        {
            var list = _redisCacheContext.Collections.GetRedisList<T>(key + ":list");
            var data = list.GetRange().ToList();

            if (data.Count() != 0)
                return data;

            return default(List<T>);
        }
    }
}
