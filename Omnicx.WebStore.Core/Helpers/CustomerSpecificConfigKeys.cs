using System.Configuration;

namespace Omnicx.Site.Core.Helpers
{
    /// <summary>
    /// THIS IS A TEMP CODE TO MANAGE CONFIG KEY SPECIFIC TO AKC
    /// THIS NEEDS TO BE MOVED TO "Apps" section. and driven by AppConfig. 
    /// and defined ONLY in AKC config file
    /// </summary>
    public static class CustomerSpecificConfigKeys
    {

        public static string AkcTubeCuttingUrl = ConfigurationManager.AppSettings.Get("AkcTubeCuttingUrl");
    }
}