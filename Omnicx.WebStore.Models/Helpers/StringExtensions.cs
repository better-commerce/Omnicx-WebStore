using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;


namespace Omnicx.WebStore.Models.Helpers
{
    public static class StringExtensions
    {

        //This Method is used to capatalize first Character to upperCase and set the remaining string as it is
        public static string ToSentenceCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            return value = char.ToUpper(value[0]) + value.Substring(1);
        }
    }
}
