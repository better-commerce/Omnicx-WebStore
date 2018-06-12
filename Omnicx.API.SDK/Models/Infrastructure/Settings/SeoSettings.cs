using System;

namespace Omnicx.API.SDK.Models.Infrastructure.Settings
{
    [Serializable]
    public class SeoSettings : ISettings
    {
        public string DefaultTitle { get; set; }
        public string DefaultMetaKeywords { get; set; }
        public string DefaultMetaDescription { get; set; }

        public bool EnableProductNoIndexTag { get; set; }
        public int NoIndexTagAfterDays { get; set; }

        public bool EnableCrawling { get; set; }
        
        
    }
}
