using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using RestSharp;
using Omnicx.API.SDK.Models;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Api.Catalog
{
    public class CategoryApi : ApiBase, ICategoryApi
    {
        public ResponseModel<List<CategoryModel>> GetCategories()
        {
            return CallApi<List<CategoryModel>>(ApiUrls.Categories, "", Method.GET);
        }

        public ResponseModel<CategoryModel> GetCategory(string slug)
        {
            var key = string.Format(CacheKeys.CATEGORY_BY_SLUG, slug);
            return FetchFromCacheOrApi<CategoryModel>(key, ApiUrls.Category, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
            // return CallApi<CategoryModel>(ApiUrls.Category, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }
    }
}
