
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Helpers;
using RestSharp;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public class BrandApi : ApiBase, IBrandApi
         
    {
        public ResponseModel<PaginatedResult<BrandModel>> GetBrands()
        {
            return CallApi<PaginatedResult<BrandModel>>(ApiUrls.Brand, "");
        }
        public ResponseModel<BrandDetailModel> GetBrandDetails(string brandId)
        {
            return CallApi<BrandDetailModel>(string.Format(ApiUrls.BrandDetail, brandId), "");
        }

        public ResponseModel<BrandDetailModel> GetBrandDetailsBySlug(string slug)
        {
            return CallApi<BrandDetailModel>(ApiUrls.BrandDetailBySlug, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }
    }
}
