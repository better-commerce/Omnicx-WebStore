using System;
using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class SearchFacetModel
    {
        public string Name { get; set; }
        public List<FacetKeyValueModel> Items { get; set; }
    }
    [Serializable]
    public class SearchFilterModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public int DisplayOrder { get; set; }
        public List<FacetKeyValueModel> Items { get; set; }
    }
    [Serializable]
    public class FacetKeyValueModel
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Count { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsSelected { get; set; }
        public int PriceFilter { get; set;}
    }

   
}