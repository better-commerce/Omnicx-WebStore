using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Runtime.Caching;
namespace Omnicx.Site.Core.Helpers
{
    public class CacheManager
    {
        public static Boolean EnableCaching = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableCaching"));
        private static ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        public static T Get<T>(string key, bool hardGetFromCache = false)
        {
            if (EnableCaching == false && hardGetFromCache == false)
                key = Guid.NewGuid().ToString();
            return (T)Cache[key];
        }

        public static void Set(string key, object data, int cacheTime = 1440)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            Cache.Add(new CacheItem(key, data), policy);
        }
    }
}