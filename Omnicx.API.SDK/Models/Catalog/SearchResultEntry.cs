using System;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Catalog
{
    [Serializable]
    public class SearchResultEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string DetailUrl { get; set; }

        public ResultEntryType Type { get; set; }
    }
}