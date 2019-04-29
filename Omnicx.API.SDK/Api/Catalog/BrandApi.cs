
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Helpers;
using RestSharp;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Catalog
{
    public class BrandApi : ApiBase, IBrandApi
         
    {
        public ResponseModel<PaginatedResult<BrandModel>> GetBrands()
        {
            return CallApi<PaginatedResult<BrandModel>>(ApiUrls.Brand, "");
        }
        public ResponseModel<PaginatedResult<BrandModel>> GetSubBrands(string subBrandIds)
        {
            return CallApi<PaginatedResult<BrandModel>>(string.Format( ApiUrls.SubBrands,subBrandIds), "");
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
