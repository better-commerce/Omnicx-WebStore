using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Omnicx.API.SDK.Helpers
{
    public static class CacheManager
    {
        public static Boolean EnableCaching = ConfigKeys.EnableCaching;
        public static ICacheManager _cacheContext;
        public static T Get<T>(string key, bool hardGetFromCache = false)
        {
            key = SanitizeKey(key);
            if (EnableCaching == false && hardGetFromCache == false) // couldnt understand the logic of  creating a new key, this is always going to return null
                key = Guid.NewGuid().ToString();
            return CacheContext().Get<T>(key);
        }
        public static IList<T> GetList<T>(string key, bool hardGetFromCache = false)
        {
            key = SanitizeKey(key);
            if (EnableCaching == false && hardGetFromCache == false) // couldnt understand the logic of  creating a new key, this is always going to return null
                key = Guid.NewGuid().ToString();
            return CacheContext().GetList<T>(key);
        }
        public static void Set(string key, object data, int cacheTime = 1440)
        {
            key = SanitizeKey(key);
            if (data == null) return;
            var tags = new[] { "" };
            //adding orgid to tags
            tags = AddOrgIdToTags(tags);
          
            CacheContext().Set(key.ToLower(), data, cacheTime, tags);
        }
        public static void SetList<T>(string key, IList<T> data, int cacheTime = 1440, string[] tags = null)
        {
            key = SanitizeKey(key);
            if (data == null) return;
            CacheContext().SetList<T>(key, data, cacheTime,tags);
        }
        public static bool IsKeyExist(string key)
        {
            key = SanitizeKey(key);
            return CacheContext().IsKeyExist(key);
        }

        private static  ICacheManager CacheContext()
        {
            if (_cacheContext == null) { 
                if(EnableCaching == true && ConfigKeys.CachingType == CachingTypes.Redis.ToString())
                {
                    _cacheContext = new RedisCache();
                }
                else
                {
                    _cacheContext = new DefaultCache();
                }
            }
            return _cacheContext;
        }
        private static string[] AddOrgIdToTags(string[] tags)
        {
            if (tags == null) tags = new[] { "" };
            //creating a tag of OrgId and adding it to the cache object
            var tagsList = tags.ToList();
            var orgId = ConfigKeys.OmnicxOrgId.ToLower();
            tagsList.Add("Org:" + orgId.ToString());
            tags = tagsList.ToArray();
            return tags;
        }
        /// <summary>
        /// sanitizes the key for redis cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string SanitizeKey(string key)
        {
            //apparently keys with "/" is causing issue when trying to remove 
            var newKey = key.ToLower().Replace("/", "backslash");
            return newKey;
        }
    }
}
