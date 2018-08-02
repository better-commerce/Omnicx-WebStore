using System;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class SearchFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsSelected { get; set; }
    }
}