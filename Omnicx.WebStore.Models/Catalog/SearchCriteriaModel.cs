using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Catalog
{
    public class SearchCriteriaModel
    {
        public string FreeText { get; set; }
        public string CollectionId { get; set; }
        public string BrandId { get; set; }
        public string Brand { get; set; }
        public List<string> BrandIds { get; set; }
        public string Facet { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
        public List<string> CategoryIds { get; set; }
        public string Gender { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<KeyValuePair<string,string>> Filters { get; set; }
        public bool AllowFacet { get; set; }
        public bool IncludeFreeProduct { get; set; }
    }
}