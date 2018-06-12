using System.Threading.Tasks;
using Omnicx.API.SDK.Models.Site;
using Omnicx.API.SDK.Models;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api.Site
{
    public interface ISiteViewApi
    {
        ResponseModel<PageModel>  PageBySlug(string slug);

        ResponseModel<SiteViewModel>  GetSiteViewComponents(string slug);
        ResponseModel<List<string>> GetSiteViewAllSlug();

        Task<ResponseModel<SiteViewModel>> GetSiteViewComponentsAsync(string slug);
        ResponseModel<FeedModel> GetFeedLink(string slug);
    }
}