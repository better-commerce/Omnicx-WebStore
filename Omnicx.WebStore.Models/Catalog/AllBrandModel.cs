using System.Collections.Generic;
using System.Linq;
using Omnicx.WebStore.Models.Catalog;

namespace Omnicx.WebStore.Models.Catalog
{
    /// <summary>
    /// REVIEW & REFACTOR - Vikram - 24Apr2017
    /// </summary>
    public class AllBrandModel
    {
        public List<BrandModel> Brands { get; set; }
        public string[] PaginationWords { get; set; }

        public IEnumerable<IGrouping<string, BrandModel>> CategoryGroups { get; set; }
    }
}