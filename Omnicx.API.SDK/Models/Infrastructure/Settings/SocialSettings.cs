using System;

namespace Omnicx.API.SDK.Models.Infrastructure.Settings
{
    [Serializable]
    public class SocialSettings : ISettings
    {
        public bool FacebookEnabled { get; set; }
        public string FacebookApiKey { get; set; }
        public string FacebookApiSecret { get; set; }
        public string FacebookUrl { get; set; }

        public bool TwitterEnabled { get; set; }
        public string TwitterApiKey { get; set; }
        public string TwitterApiSecret { get; set; }
        public string TwitterUrl { get; set; }

        public bool GooglePlusEnabled { get; set; }
        public string GooglePlusApiKey { get; set; }
        public string GooglePlusApiSecret { get; set; }
        public string GooglePlusUrl { get; set; }
    }
}
