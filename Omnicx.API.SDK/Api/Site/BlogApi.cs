using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Site;
using RestSharp;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Site
{
    public class BlogApi:ApiBase, IBlogApi 
    {
        public ResponseModel<PaginatedResult<BlogModel>> BlogByGroup(int groupType, string groupId, int currentPage, int pageSize)
        {
            return CallApi<PaginatedResult<BlogModel>>(string.Format(ApiUrls.BlogByGroup,groupId, groupType,currentPage, pageSize), "", Method.GET);
        }
        public ResponseModel<BlogGroups> BlogGroups()
        {
            return CallApi<BlogGroups>(ApiUrls.BlogGroups, "", Method.GET);
        }
        public ResponseModel<PaginatedResult<BlogModel>> BlogSearch(string searchText, int currentPage, int pageSize)
        {
            return CallApi<PaginatedResult<BlogModel>>(string.Format(ApiUrls.BlogSearch, searchText, currentPage, pageSize), "", Method.GET);
        }
        public ResponseModel<BlogModel> BlogDetail(string slug)
        {
           return CallApi<BlogModel>(ApiUrls.BlogDetail, slug, Method.GET, "slug", ParameterType.QueryString, "text/plain");
        }
    }
}
