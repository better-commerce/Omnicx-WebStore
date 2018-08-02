using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;


namespace Omnicx.API.SDK.Helpers
{
    public static class JsonExtensions
    {
        public static IList<string> JsonToObjectArray(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return new List<string>();
            }
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };


            var res = JsonConvert.DeserializeObject<IList<string>>(json);
            return res == null ? new List<string>() : res;
        }
        public static bool ValidateJSON(this string s)
        {
            try
            {
                if (string.IsNullOrEmpty(s))
                {
                    return false;
                }
                else
                {
                    JToken.Parse(s);
                    return true;
                }
            }
            catch 
            {                
                return false;
            }
        }
    }
}
