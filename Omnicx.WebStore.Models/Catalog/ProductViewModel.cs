using System.Collections.Generic;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Models.Catalog
{
    public class ProductViewModel
    {
        public PaginatedResult<ProductModel> ProductList { get; set; }
        public List<BrandModel> BrandList { get; set; }
        public BrandDetailModel BrandDetailList { get; set; }
        public List<SearchFilterModel> Filter { get; set; }
    }

    public class ProductReviewLoginModel
    {
        public RegistrationModel Register { get; set; }
        public LoginViewModel Login { get; set; }
    }
}