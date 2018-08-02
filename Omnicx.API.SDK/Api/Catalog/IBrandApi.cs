using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public interface IBrandApi
    {
        ResponseModel<PaginatedResult<BrandModel>>  GetBrands();
        ResponseModel< BrandDetailModel> GetBrandDetails(string brandId);
        ResponseModel<BrandDetailModel>  GetBrandDetailsBySlug(string slug);
    }
}
