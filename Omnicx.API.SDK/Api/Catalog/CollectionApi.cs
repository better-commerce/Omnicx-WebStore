using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using RestSharp;
using Omnicx.WebStore.Models;
using Omnicx.API.SDK.Api.Infra;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Keys;
using Omnicx.API.SDK.Helpers;

namespace Omnicx.API.SDK.Api.Catalog
{
    public class CollectionApi :ApiBase, ICollectionApi
    {
        public ResponseModel<DynamicListModel> GetCollectionBySlug(string slug)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
           var key = string.Format(CacheKeys.DYNAMIC_LIST_BY_SLUG, slug, !string.IsNullOrWhiteSpace(sessionContext.LangCulture) ? sessionContext.LangCulture : sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, sessionContext.CurrentUser?.CompanyId, Utils.GetBrowserInfo().IsMobileDevice, sessionContext.CurrencyCode);

            return FetchFromCacheOrApi<DynamicListModel>(key, ApiUrls.CollectionBySlug, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");
        }

        public ResponseModel<List<DynamicListCollection>> GetCollectionList()
        {
            return CallApi<List<DynamicListCollection>>(ApiUrls.CollectionList, "", Method.GET);
        }

        public ResponseModel<DynamicListsGroupModel> GetAllLookbooksBySlug(string slug)
        {
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var key = string.Format(CacheKeys.ALL_DYNAMIC_LISTS_BY_SLUG, slug, !string.IsNullOrWhiteSpace(sessionContext.LangCulture) ? sessionContext.LangCulture : sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, sessionContext.CurrentUser?.CompanyId, Utils.GetBrowserInfo().IsMobileDevice, sessionContext.CurrencyCode);

            return FetchFromCacheOrApi<DynamicListsGroupModel>(key, ApiUrls.GetAllLookbook, slug, Method.POST, "slug", ParameterType.QueryString, "text/plain");

        }
        public ResponseModel<DynamicListsGroupModel> GetAllLookbooksByGroup(string groupName)
        {
            return CallApi<DynamicListsGroupModel>(ApiUrls.GetLookbookByGroup, groupName, Method.POST, "groupName", ParameterType.QueryString, "text/plain");
        }

    }
}
