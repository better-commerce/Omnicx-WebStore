using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;


namespace Omnicx.WebStore.Models.Helpers
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
    }
}
