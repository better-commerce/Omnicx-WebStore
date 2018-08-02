using System.Collections.Generic;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Site;
using System;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.WebStore.Models.Catalog
{
    [Serializable]
    public class CategoryModel : IHaveSeoInfo
    {
        public string Id { get; set; }
        public string DisplayText { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        
        public string Link { get; set; }
        public string Image { get; set; }
        public List<ImageModel> Images { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
        public List<LocalizedSlugModel> BreadCrumbs { get; set; }

        public List<CategoryLinkGroupModel>  LinkGroups { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string CanonicalTags { get; set; }
        public PaginatedResult<ProductModel> ProductList { get; set; }
        
    }

    [Serializable]
    public class CategoryLinkGroupModel
    {
        public string Name { get; set; }
        /// <summary>
        /// TODO: REVEIW IF WE NEED - Vikram - 24Apr2017
        /// </summary>
        public string Link { get; set; }
        public GroupTypes GroupType { get; set; }
        public int DisplayOrder { get; set; }
        public List<CategoryLinkModel> Items { get; set; }
        public int AttributeInputType { get; set; }
    }

    [Serializable]
    public class CategoryLinkModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public int AttributeInputType { get; set; }
        public string Info1 { get; set; }
        public string Ingo2 { get; set; }
    }
    public class CategoryItemModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

}