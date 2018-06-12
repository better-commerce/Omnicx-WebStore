using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Site;
using Omnicx.API.SDK.Models;
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
