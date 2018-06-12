using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Helpers;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// applies digits after decimal for all teh number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToApplyDigitsAfterDecimal(this decimal number)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var regionalSettings = sessionContext.CurrentSiteConfig.RegionalSettings;

            if (regionalSettings.CurrencyDigitsAfterDecimal == 0) regionalSettings.CurrencyDigitsAfterDecimal = 2;
            var digits = "";
            for(var i=0; i < regionalSettings.CurrencyDigitsAfterDecimal; i++)
            {
                digits = digits + "#";
            }
            //number.ToString("0.##"); // returns "0"  when decimalVar == 0
            return number.ToString("0." + digits); // returns "0"  when decimalVar == 0
        }
        public static string ToSeo(this string url)
        {
            // make the url lowercase
            string encodedUrl = (url.ReplaceAccentCharacters() ?? "").ToLower();


            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }
        public static string ReplaceAccentCharacters(this string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            var sb = new StringBuilder();
            foreach (char c in text.ToCharArray())
            {

                if ((int)c == 176)
                {
                    sb.Append("o-");
                }
                else if ((int)c == 238)
                {
                    sb.Append("i");
                }
                else if ((int)c == 233)
                {
                    sb.Append("e");
                }
                else
                {
                    sb.Append(c);
                }
            }
            text = sb.ToString();
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ToCamelCaseString(this string the_string)
        {
            string result = "";
            if (the_string == null)
            {
                return the_string;
            }
            else
            {
                the_string = the_string.ToUpper();
            }


            // Split the string into words.
            string[] words = the_string.Split(' ');
            var i = 0;
            foreach (string str in words)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var remaingPart = "";
                    if (str.Length > 1)
                    {
                        remaingPart = str.Substring(1);
                    }
                    result += (i==0? str.First().ToString().ToLower(): str.First().ToString().ToUpper()) + remaingPart + ' ';
                }
                i = i + 1;
            }
            return result;

        }
        public static string ToCamelCase(this string the_string)
        {
            string result = "";
            if (the_string == null)
            {
                return the_string;
            }
            else
            {
                
                the_string = the_string.ToLower();
            }
            
            // Split the string into words.
            if (the_string.Contains(">"))
            {
                the_string = Regex.Replace(the_string, @"[\s+]", "");
                string[] words = the_string.Split('>');

                foreach (string str in words)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var remaingPart = "";
                        if (str.Length > 1)
                        {
                            remaingPart = str.Substring(1);
                        }
                        result += str.First().ToString().ToUpper() + remaingPart + '>';
                    }
                }
            }
            else {
                string[] words = the_string.Split(' ');

                foreach (string str in words)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        var remaingPart = "";
                        if (str.Length > 1)
                        {
                            remaingPart = str.Substring(1);
                        }
                        result += str.First().ToString().ToUpper() + remaingPart + ' ';
                    }
                }
            }
            return result;

        }
        public static string ToFullUrl(this string url)
        {
            if (string.IsNullOrEmpty(url)) return "";

            url = url.ToLower().Trim();
            if(url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith(SiteViewTypes.Home))
                return url;
            else                
                return url = SiteViewTypes.Home + url;
        }
    }

}