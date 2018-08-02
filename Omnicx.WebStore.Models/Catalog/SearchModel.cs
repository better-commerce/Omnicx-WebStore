using System.Collections.Generic;
using Omnicx.WebStore.Models.Helpers;
using System;
namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class SearchRequestModel
    {
        public string FreeText { get; set; }
        public int Page { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string CollectionId { get; set; }
        public string Collection { get; set; }
        public string BrandId { get; set; }
        public string Brand { get; set; }
        public string Facet { get; set; }
        public string CategoryId { get; set; }
        public List<string> CategoryIds { get; set; }
        public string Category { get; set; }
        public string Gender { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<SearchFilter> Filters { get; set; }
        public bool AllowFacet { get; set; }
        public string BreadCrumb { get; set; }
        public int ResultCount { get; set; }

    }
    [Serializable]
    public class SearchModel
    {
        public string FreeText { get; set; }

        public IEnumerable<SearchFilter> Criteria { get; set; }

        public IEnumerable<SearchResultEntry> Results { get; set; }

        public IEnumerable<Facet> Facets { get; set; }
    }

    public class CategorySearchModel
    {
        public List<string> CategoryIds { get; set; }
        public bool IsFeatured { get; set; }
        public bool GenderSpecific { get; set; }
        public List<string> SubCategoryIds { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
    public class BlogSearchModel
    {
        public List<string> CategoryIds { get; set; }
        public List<string> Editors { get; set; }
        public string FreeText { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
    public class SortByModel
    {
        public string Name { get; set; }
    }
    [Serializable]
    public class KeywordRedirectModel
    {
        public string Keywords { get; set; }
        public string Url { get; set; }
    }
}