using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Omnicx.API.SDK.Helpers
{
    public class DefaultCache : ICacheManager
    {

        private ObjectCache _cacheContext;
        public DefaultCache()
        {
            _cacheContext = MemoryCache.Default;
        }
        public T Get<T>(string key)
        {
            return (T)_cacheContext[key.ToLower()];
        }
        public void Set(string key, object data, int cacheTime, string[] tags = null)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            _cacheContext.Add(new CacheItem(key.ToLower(), data), policy);
        }
        public IList<T> GetList<T>(string key)
        {
            return (List<T>)_cacheContext[key];
        }

        public bool IsKeyExist(string key)
        {
            return _cacheContext[key] != null;
        }

        public void SetList<T>(string key, IList<T> data, int cacheTime = 1440, string[] tags = null)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            _cacheContext.Add(new CacheItem(key, data), policy);
        }
    }
    }
