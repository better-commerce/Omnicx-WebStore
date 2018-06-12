using System.Collections.Generic;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Site;
using System;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Catalog
{
    [Serializable]
    public class DynamicListModel : IHaveSeoInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ListDatasetSource ListType { get; set; }
        public PaginatedResult<ProductModel> Products { get; set; }
        public List<BrandModel> Brands { get; set; }
        public List<BrandModel> SubBrands { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
        public List<ImageModel> Images { get; set; }
        public IList<BlogModel> Blogs { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
        public bool AllowFacets { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string GroupSeparator { get; set; }
        public string GroupCode { get; set; }
        public bool DisplayTitle { get; set; }
        public List<string> Groups { get; set; }
        public bool AllowGrouping { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }

    }

    [Serializable]
    public class DynamicListCollection : IHaveSeoInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int DataSource { get; set; }
        public int RuleType { get; set; }
        public int NoOfRecords { get; set; }
        public int SortBy { get; set; }
        public IList<ProductModel> Products { get; set; }
        public List<BrandModel> Brands { get; set; }
        public List<BrandModel> SubBrands { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
    }
    [Serializable]
    public class Collections
    { 
     public List<DynamicListCollection> CollectionList { get; set; }
    }

}
