using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public interface IBrandApi
    {
        ResponseModel<PaginatedResult<BrandModel>>  GetBrands();
        ResponseModel< BrandDetailModel> GetBrandDetails(string brandId);
        ResponseModel<BrandDetailModel>  GetBrandDetailsBySlug(string slug);
    }
}
