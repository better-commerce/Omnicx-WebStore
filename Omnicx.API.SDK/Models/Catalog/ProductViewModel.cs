using System.Collections.Generic;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.API.SDK.Models.Catalog
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