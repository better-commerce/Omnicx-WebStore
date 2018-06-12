using System.Collections.Generic;
using Newtonsoft.Json;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Helpers;
using RestSharp;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Models;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Infra;

namespace Omnicx.API.SDK.Api.Catalog
{
    public class ProductApi : ApiBase, IProductApi
    {
      
        public ResponseModel<PaginatedResult<ProductModel>> GetProducts(SearchRequestModel criteria)
        {
            return CallApi<PaginatedResult<ProductModel>>(ApiUrls.Products, JsonConvert.SerializeObject(criteria), Method.POST);
        }
        public ResponseModel<TypeAheadSearchModel> SearchFreeText(string freeText)
        {
            return CallApi<TypeAheadSearchModel>(string.Format(ApiUrls.AutoSearch, freeText), "", Method.POST);
        }
        public ResponseModel<ProductDetailModel> GetProductDetail(string id)
        {           
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var key = string.Format(CacheKeys.PRODUCT_MODEL_BY_ID, id, sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, sessionContext.CurrentUser?.CompanyId);

            return FetchFromCacheOrApi<ProductDetailModel>(key, string.Format(ApiUrls.ProductDetail, id),"");           
        }

        public ResponseModel<ProductDetailModel> GetProductDetailBySlug(string slug)
        {                   
            var sessionContext= DependencyResolver.Current.GetService<ISessionContext>();
            var key = string.Format(CacheKeys.PRODUCT_MODEL_BY_SLUG, slug, sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, sessionContext.CurrentUser?.CompanyId);

            return FetchFromCacheOrApi<ProductDetailModel>(key, ApiUrls.ProductDetailBySlug, slug,Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }
        public ResponseModel<List<ProductModel>> GetRelatedProducts(string id)
        {
            return CallApi<List<ProductModel>>(string.Format(ApiUrls.RelatedProducts, id), "");
        }
        //public ResponseModel<List<SortByModel>> GetSortBy()
        //{
        //    return CallApi<List<SortByModel>>(ApiUrls.SortByList, "");
        //}
        public ResponseModel<bool> AddProductReview(string id,ProductReviewAddModel productReview)
        {
            return CallApi<bool>(string.Format(ApiUrls.AddProductReview, id), JsonConvert.SerializeObject(productReview), Method.POST);
        }

        public ResponseModel<List<KeywordRedirectModel>> GetKeywordRedirect()
         {
            return FetchFromCacheOrApi<List<KeywordRedirectModel>>(string.Format(CacheKeys.ReturnRedirect, ConfigKeys.OmnicxDomainId), ApiUrls.KeywordRedirections);

        }
       

    }
}
