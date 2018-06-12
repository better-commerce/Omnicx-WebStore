using System.Collections.Generic;

namespace Omnicx.API.SDK.Helpers
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Set(string key, object data, int cacheTime, string[] tags = null);
        void SetList<T>(string key, IList<T> data, int cacheTime = 1440, string[] tags = null);
        IList<T> GetList<T>(string key);
        bool IsKeyExist(string key);
    }
}
