using System.Threading.Tasks;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api.Site
{
    public interface ISiteViewApi
    {
        ResponseModel<PageModel>  PageBySlug(string slug);

        ResponseModel<SiteViewModel>  GetSiteViewComponents(string slug);
        ResponseModel<List<string>> GetSiteViewAllSlug();

        Task<ResponseModel<SiteViewModel>> GetSiteViewComponentsAsync(string slug);
        ResponseModel<SiteViewModel> GetSiteViewById(string id,int versionNo,string langCulture);
        ResponseModel<FeedModel> GetFeedLink(string slug);
    }
}