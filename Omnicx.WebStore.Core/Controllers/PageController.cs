using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using DevTrends.MvcDonutCaching;
using Omnicx.API.SDK.Api.Site;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

using System.Net;
using System.IO;
using System.Xml;
using System.Text;

namespace Omnicx.WebStore.Core.Controllers
{
    public class PageController : BaseController
    {
        private readonly ISiteViewApi _siteViewApi;
         public PageController(ISiteViewApi siteViewApi)
        {
            _siteViewApi = siteViewApi;
        }

        /// <summary>
        /// Invoked for all dynamic URL pages
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        //[DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        //public ActionResult DynamicPage(string slug)
        //{
        //    slug = SiteUtils.GetSlugFromUrl();
        //    var siteView = _siteViewApi.GetSiteViewComponents(slug);
        //    if (siteView == null) return RedirectToAction("pagenotfound", "common");
        //    return slug == "/" ? View("Index", siteView) : View(siteView);
        //}

        [DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        public async Task<ActionResult> DynamicPage(string slug)
        {
            
            slug = SiteUtils.GetSlugFromUrl();
            string url = SiteUtils.GetFullUrl();
            var result = await _siteViewApi.GetSiteViewComponentsAsync(url);
            if(result != null && result.StatusCode== HttpStatusCode.OK && result.Result!=null)
            {
                if(result.Result.RedirectTypeValue == RedirectType.RedirectTemporary.GetHashCode())
                {
                    return Redirect(result.Result.Slug);
                }
                if(result.Result.RedirectTypeValue == RedirectType.RedirectPermanent.GetHashCode())
                {
                    return RedirectPermanent(result.Result.Slug);
                }             
            }          
            var siteView = result.Result;
            
            if (siteView == null) return RedirectToPageNotFound(); 
            if(slug == "/")
                SetDataLayerVariables(siteView, WebhookEventTypes.PageViewed);
            else
                SetDataLayerVariables(siteView, WebhookEventTypes.CmsPageViewed);

            return slug == "/" ? View(CustomViews.INDEX, siteView) : View(CustomViews.DYNAMICPAGE, siteView);
        }
        public ActionResult GetFeedLink(string slug)
        {
            var resp = _siteViewApi.GetFeedLink(slug);
            XmlDocument doc = new XmlDocument();
            doc.Load(resp.Result.DownloadLink);
            var ms = new MemoryStream(Encoding.ASCII.GetBytes(doc.OuterXml.ToString()));
            return new FileStreamResult(ms, "text/xml");
        }
    }
}