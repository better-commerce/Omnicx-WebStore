using System.Threading.Tasks;
using Omnicx.WebStore.Models.Site;
using RestSharp;
using Omnicx.WebStore.Models;

using Omnicx.API.SDK.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.WebStore.Models.Keys;
using System.Linq;
using System;

namespace Omnicx.API.SDK.Api.Site
{
    public class SiteViewApi: ApiBase, ISiteViewApi
    {
        //TODO: REVIEW THIS TO REPLACE WITH SITEVIEW - Vikram 24Apr2017
        public ResponseModel<PageModel> PageBySlug(string slug)
        {
            return CallApi<PageModel>(string.Format(ApiUrls.PageBySlug, slug), "");
        }
        public ResponseModel<SiteViewModel> GetSiteViewComponents(string slug)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var key = string.Format(CacheKeys.SITEVIEW_MODEL_BY_SLUG, Utils.GetSlugFromUrl(), sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, Utils.GetBrowserInfo()?.IsMobileDevice, sessionContext.CurrencyCode);

            return FetchFromCacheOrApi<SiteViewModel>(key, ApiUrls.SiteViewComponents, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }
        public ResponseModel<List<string>> GetSiteViewAllSlug()
        {
            var cackekey = string.Format(CacheKeys.SiteViewAllSlug, ConfigKeys.OmnicxDomainId);
            var data = CacheManager.Get<List<string>>(cackekey);
            if (data != null) return new ResponseModel<List<string>>{ Result = data };
            var result=  CallApi<List<string>>(string.Format(ApiUrls.SiteViewAllSlug), "");
            CacheManager.Set(cackekey, result.Result);
            return result;
        }
        public async Task<ResponseModel<SiteViewModel>> GetSiteViewComponentsAsync(string slug)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var key = string.Format(CacheKeys.SITEVIEW_MODEL_BY_SLUG, Utils.GetSlugFromUrl(), sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, Utils.GetBrowserInfo()?.IsMobileDevice, sessionContext.CurrencyCode);
           
            var task= await  FetchFromCacheOrApiAsync<SiteViewModel>(key, ApiUrls.SiteViewComponents, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
            //Check PageScheduler
            var siteView = task.Result;
            if (siteView != null)
            {
                if (task.Result.ScheduleItems != null && siteView.ScheduleItems.Any())
                {
                    var activeSchedule = siteView.ScheduleItems.FirstOrDefault(x => x.From <= DateTime.UtcNow && (x.NeverExpire || x.To >= DateTime.UtcNow));
                    if (activeSchedule == null)
                    {
                        activeSchedule = siteView.ScheduleItems.FirstOrDefault(x => x.VersionNo== siteView.VersionNo);
                        if(activeSchedule != null){
                            activeSchedule.VersionNo = 0;
                        }
                    }
                    if (activeSchedule != null)
                    {
                        if (activeSchedule.VersionNo != siteView.VersionNo)
                        {
                            var keyRemove = string.Format(CacheKeys.SITEVIEW_REMOVE_BY_SLUG, siteView.Slug, ConfigKeys.OmnicxDomainId);
                            CacheManager.RemoveByPattern(keyRemove);
                            task = await FetchFromCacheOrApiAsync<SiteViewModel>(key, ApiUrls.SiteViewComponents, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
                        }
                    }
                }
            }
        
            return task;            
        }
        public ResponseModel<SiteViewModel> GetSiteViewById(string id, int versionNo, string langCulture)
        {
            return CallApi<SiteViewModel>(string.Format(ApiUrls.SiteViewById, id, versionNo, langCulture), "");
        }
        public ResponseModel<FeedModel> GetFeedLink(string slug)
        {
            
            return CallApi<FeedModel>(ApiUrls.FeedLink, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }
    }
}
