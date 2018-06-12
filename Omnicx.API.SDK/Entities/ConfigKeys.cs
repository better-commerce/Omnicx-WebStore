using System;
using System.Configuration;

namespace Omnicx.API.SDK.Entities
{
    public class ConfigKeys
    {
        public static string OmnicxApiBaseUrl = ConfigurationManager.AppSettings.Get("OmniCXApiBaseUrl");
        public static string OmnicxOutboundIp = ConfigurationManager.AppSettings.Get("OmniCXOutboundIP");
        public static string OmnicxAppId = ConfigurationManager.AppSettings.Get("OmniCXAppId");
        public static string OmnicxSharedSecret = ConfigurationManager.AppSettings.Get("OmniCXSharedSecret");
        public static string OmnicxDomainId = ConfigurationManager.AppSettings.Get("OmniCXDomainId");
        public static string OmnicxOrgId = ConfigurationManager.AppSettings.Get("OmniCXOrgId");

        public static string PageSize = ConfigurationManager.AppSettings.Get("PageSize");
        public static string SortOrder = ConfigurationManager.AppSettings.Get("SortOrder");
        public static string SortBy = ConfigurationManager.AppSettings.Get("SortBy");
        public static string PageSizeforblog = ConfigurationManager.AppSettings.Get("PageSizeforblog");
        public static string NoImagePath = ConfigurationManager.AppSettings.Get("NoImagePath");

        public static Boolean EnableCaching = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableCaching"));
        public static string RedisCacheConnetionString = Convert.ToString(ConfigurationManager.AppSettings.Get("RedisCacheConnetionString"));
        public static string CachingType = Convert.ToString(ConfigurationManager.AppSettings.Get("CachingType"));
        public static bool EnforceSSL = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnforceSSL"));
        public static string OmnilyticUrl = ConfigurationManager.AppSettings.Get("OmnilyticUrl");
        public static string OmnilyticId = ConfigurationManager.AppSettings.Get("OmnilyticId");


        public static Boolean OnePageCheckout = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("OnePageCheckout"));
        public static Boolean DisplayUserActivity = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("DisplayUserActivity"));
        public static string HideGiftOption = ConfigurationManager.AppSettings.Get("HideGiftOption");
        public static string StoreTheme = ConfigurationManager.AppSettings.Get("StoreTheme");
        public static string StoreThemeDefaultLayout = ConfigurationManager.AppSettings.Get("StoreThemeDefaultLayout");
    }

    public static class Constants
    {
        public static string COOKIE_DEVICEID = "dvid";
        public static string COOKIE_SESSIONID = "sid";
        public static string COOKIE_BASKETID = "bid";
        //public static string COOKIE_USERID = "uid";

        public static string COOKIE_DEFAULT_SETTINGS = "config";
        public static string COOKIE_DOMAIN_SETTINGS = "settings";
        public static string COOKIE_DOMAIN_CURRENCY = "curr";
        public static string COOKIE_DOMAIN_LANGCULTURE = "lang";
        public static string COOKIE_DOMAIN_COUNTRYOFORIGIN = "coo";

        //device cookie should NEVER expire - hence its set for next 10 years.
        public static int COOKIE_DEVICEID_EXPIRES_DAYS = 10 * 365; //TODO make configurable
        
        //session Cookie set to 30 minutes.
        public static int COOKIE_SESSIONID_EXPIRES_MINUTES = 30;//TODO make configurable, currently set to 30 minutes. 

        public static string SESSION_USERID = "uid";
        public static string SESSION_CACHED_USER = "cached_usr";
        public static string SESSION_COMPANYID = "cid";
        public static string SESSION_COMPANYUSERROLE = "curole";
        public static string SESSION_CONFIG_SETTINGS = "config";
        public static string SESSION_IPADDRESS = "ip";
        public static string SESSION_TOKEN = "token";
        public static string SESSION_HAS_BASKET_ACTION = "hasbasketaction";

        public static string HTTP_CONTEXT_ITEM_SITENAV = "sitenav";
        
        

        public static string PRICE_FILTER = "price";//Added for price display order in facet
        public static string SEARCH_FILTER_QUERYSTRING = "filter";
        public static string SEARCH_SURVEY_QUERYSTRING = "servey";
        public static string ATTRIBUTE_FILTER_PREFIX = "attributes.value~";

        public static string SURVEY_BUNDLE_PREFIX = "Interactive Bundle";
    }
}
