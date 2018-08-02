using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Net;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.API.SDK.Helpers
{
    public class Utils
    {
       
        public Utils()
        {

        }
        public static string GetSlugFromUrl()
        {
            if (HttpContext.Current == null) return "";
            var slug = HttpContext.Current.Request.Url.LocalPath.ToLower();
            if (slug == "/") return slug;
            return slug.Remove(0, 1);
        }
        //public Utils(HttpContextBase httpContext)
        //{
        //    this._httpContext = httpContext;
        //}

        public static string GenerateSHA1Hash(string hashInput, string secretKey)
        {

            SHA1 sha = new SHA1Managed();
            Encoder enc = System.Text.Encoding.ASCII.GetEncoder();
            String hashStage1 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput))) + "." +
                secretKey;

            String hashStage2 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)));

            return hashStage2;
        }
        private static string HexEncode(byte[] data)
        {

            String result = "";
            foreach (byte b in data)
            {
                result += b.ToString("X2");
            }
            result = result.ToLower();

            return (result);
        }

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        public static string GetCurrentIpAddress()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
                return string.Empty;

            var result = "";
            if (HttpContext.Current.Request.Headers != null)
            {
                //look for the X-Forwarded-For (XFF) HTTP header field
                //it's used for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer. 
                string xff = HttpContext.Current.Request.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => HttpContext.Current.Request.Headers[k])
                    .FirstOrDefault();

                //if you want to exclude private IP addresses, then see http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
            }

            if (String.IsNullOrEmpty(result) && HttpContext.Current.Request.UserHostAddress != null)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            //some validation
            if (result == "::1")
                result = "127.0.0.1";
            //remove port
            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;

        }
        /// <summary>
        ///  get the masked server ip. show the digits after the last period. ie. x.x.x.123 
        /// </summary>
        /// <returns></returns>
        public static string GetMaskedServerIpAddress()
        {
            var server = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();
            return "x.x.x." + server.Split('.')[server.Split('.').Length - 1];
        }
        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <returns>Page name</returns>
        public static string GetThisPageUrl(bool includeQueryString)
        {
            bool useSsl = IsCurrenCtonnectionSecured();
            return GetThisPageUrl(includeQueryString, useSsl);
        }

        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public static string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;
            var  httpContext=System.Web.HttpContext.Current;
            //URL referrer is null in some case (for example, in IE 8)
            if (httpContext != null &&
                httpContext.Request != null &&
                httpContext.Request.UrlReferrer != null)
                referrerUrl = httpContext.Request.UrlReferrer.ToString();//.PathAndQuery;

            return referrerUrl;
        }
        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <param name="useSsl">Value indicating whether to get SSL protected page</param>
        /// <returns>Page name</returns>
        public static string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            var httpContext = System.Web.HttpContext.Current;
            string url = string.Empty;
            if (httpContext == null || httpContext.Request == null)
                return url;

            if (includeQueryString)
            {
                string storeHost = GetStoreHost(useSsl);
                if (storeHost.EndsWith("/"))
                    storeHost = storeHost.Substring(0, storeHost.Length - 1);
                url = storeHost + httpContext.Request.RawUrl;
            }
            else
            {
                if (httpContext.Request.Url != null)
                {
                    url = httpContext.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }
            url = url.ToLowerInvariant();
            return url;
        }

        /// <summary>
        /// Gets store host location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Store host location</returns>
        public static string GetStoreHost(bool useSsl)
        {
            var result = "";
            var httpHost = ServerVariables("HTTP_HOST");
            if (!String.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;
                if (!result.EndsWith("/"))
                    result += "/";
            }
            if (useSsl)
            {
                //Secure URL is not specified.
                //So a store owner wants it to be detected automatically.
                result = result.Replace("http:/", "https:/");
            }

            if (!result.EndsWith("/"))
                result += "/";
            return result.ToLowerInvariant();
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public static bool IsCurrenCtonnectionSecured()
        {
            var httpContext = System.Web.HttpContext.Current;
            bool useSsl = false;
            if (httpContext != null && httpContext.Request != null)
            {
                useSsl = httpContext.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                var httpContext = System.Web.HttpContext.Current;
                if (httpContext == null || httpContext.Request == null)
                    return result;

                if (httpContext.Request.ServerVariables[name] != null)
                {
                    result = httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }
        public static UserBrowser GetBrowserInfo()
        {
            var browser = new UserBrowser
            {
                Name = HttpContext.Current.Request.Browser.Browser,
                Version = HttpContext.Current.Request.Browser.Version,
                Platform = HttpContext.Current.Request.Browser.Platform,
                IsMobileDevice = HttpContext.Current.Request.Browser.IsMobileDevice,

            };
            return browser;
        }

        public static string FormatDescription(string stringVal)
        {
            var regex = "&#39;";
            if (string.IsNullOrEmpty(stringVal))
            {
                return "";
            }
            else
            {
                stringVal = Regex.Replace(stringVal, regex, String.Empty);
                return stringVal;
            }
        }
        public static string GetReferrer()
        {
            string referrer = string.Empty;

            try
            {
                var httpContext = System.Web.HttpContext.Current;
                if (httpContext == null || httpContext.Request == null)
                    return referrer;

                if (httpContext.Request.UrlReferrer != null)
                {
                    referrer = httpContext.Request.UrlReferrer.ToString();
                }
            }
            catch
            {
                referrer = string.Empty;
            }
            return referrer;
        }
        /// <summary>
        /// Captures the UTM information from the query string and returns a populated object
        /// </summary>
        /// <returns></returns>
        public static UtmInfo GetUtm()
        {
            var utm = new UtmInfo();

            try
            {
                var httpContext = System.Web.HttpContext.Current;
                if (httpContext == null || httpContext.Request == null)
                    return utm;

                if (httpContext.Request.QueryString != null)
                {
                    if (httpContext.Request.QueryString["utm_campaign"] != null) utm.Campaign = httpContext.Request.QueryString["utm_campaign"].ToString();
                    if (httpContext.Request.QueryString["utm_medium"] !=null) utm.Medium = httpContext.Request.QueryString["utm_medium"].ToString();
                    if (httpContext.Request.QueryString["utm_source"] != null) utm.Source = httpContext.Request.QueryString["utm_source"].ToString();
                    if (httpContext.Request.QueryString["utm_content"] != null) utm.Content = httpContext.Request.QueryString["utm_content"].ToString();
                    if (httpContext.Request.QueryString["utm_term"] != null) utm.Term = httpContext.Request.QueryString["utm_term"].ToString();
                }
            }
            catch
            {
                return utm;
            }
            return utm;
        }
        public static Int16 GetEnumValueByName(Type enumObject, string enumname)
        {
            var enumStrings = Enum.GetValues(enumObject);
            const short enumvalue = 1;
            foreach (var enumString in enumStrings)
            {
                var name = Enum.GetName(enumObject, Convert.ToInt16(enumString));
                if (name != null && name.ToLower() == enumname.ToLower())
                {
                    return Convert.ToInt16(enumString);                    
                }
            }
            return enumvalue;
        }

        public static string GetThreeChracterCountryCode(string name)
        {
            var cultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
            string code = "";
            if (string.IsNullOrEmpty(name) == false)
            {
                foreach (var cultre in cultures)
                {

                    var region = new System.Globalization.RegionInfo(cultre.Name);
                    if (region.TwoLetterISORegionName.ToLower() == name.ToLower())
                    {
                        code = region.ThreeLetterISORegionName;
                    }
                }
            }
            return code;
        }
    }
}
