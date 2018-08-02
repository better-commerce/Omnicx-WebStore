using System;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class SearchSettings : ISettings
    {
        public string ElasticIndexPath { get; set; }
        public string ElasticIndexSecondaryPath { get; set; }
        public string SearchEngineUserName { get; set; }
        public string SearchEnginePassword { get; set; }


        public bool EnableSearchInHeader { get; set; }
        public bool TypeAheadSearch { get; set; }
        public bool EnableDidYouMean { get; set; }
        public string ElasticVersion { get; set; }

        public string DefaultSortBy { get; set; }
    }
}
