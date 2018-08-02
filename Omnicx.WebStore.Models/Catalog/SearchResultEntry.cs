using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System;


namespace Omnicx.WebStore.Models.Catalog
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