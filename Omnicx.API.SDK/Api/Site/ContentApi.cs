using System.Collections.Generic;

using Omnicx.WebStore.Models.Site;
using RestSharp;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Common;
using System;
using Newtonsoft.Json;

namespace Omnicx.API.SDK.Api.Site
{
    public class ContentApi : ApiBase, IContentApi
    {
        public ResponseModel<NavigationModel> GetMenuDetails()
        {
            return FetchFromCacheOrApi<NavigationModel> (string.Format(CacheKeys.SITE_NAV_TREE_MODEL), ApiUrls.MenuDetails);
        }
        public ResponseModel<List<FaqsCategoryModel>> GetFaqsCategories()
        {
            return FetchFromCacheOrApi<List<FaqsCategoryModel>>(string.Format(CacheKeys.FaqCategories), ApiUrls.FaqsCategories, "", Method.POST);
        }
        public ResponseModel<List<FaqsSubCategoryModel>> GetFaqsSubCategories(int faqType)
        {
            return FetchFromCacheOrApi<List<FaqsSubCategoryModel>>(string.Format(CacheKeys.FaqSubCategories, ConfigKeys.OmnicxDomainId.ToLower(), faqType), string.Format(ApiUrls.FaqsSubCategories, faqType), "", Method.POST);
        }

        public ResponseModel<bool> SendContactEmail(ContactModel contactForm)
        {
            return CallApi<bool>(string.Format(ApiUrls.SendContactEmail), JsonConvert.SerializeObject(contactForm), Method.POST);
        }
    }
}