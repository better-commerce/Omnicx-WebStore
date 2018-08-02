using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Site
{
    public interface IBlogApi
    {
        ResponseModel<PaginatedResult<BlogModel>>  BlogByGroup(int groupType, string groupId, int currentPage, int pageSize);
        ResponseModel<BlogGroups>  BlogGroups();
        ResponseModel<PaginatedResult<BlogModel>>  BlogSearch(string searchText, int currentPage, int pageSize);
        ResponseModel<BlogModel>  BlogDetail(string slug);
    }
}
