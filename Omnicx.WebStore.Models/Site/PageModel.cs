using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Helpers;

namespace Omnicx.WebStore.Models.Site
{
    public class PageModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTags { get; set; }
        public List<ZoneModel> Zones { get; set; }
        public List<SnippetModel> Snippets { get; set; }
        public PaginatedResult<ProductModel> CollectionResult { get; set; }
        public SearchCriteriaModel SearchFilter { get; set; }
    }
}