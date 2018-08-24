using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Helpers;

using Omnicx.WebStore.Core.Helpers;
using Omnicx.API.SDK.Api.Site;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using Microsoft.Security.Application;
using DevTrends.MvcDonutCaching;
using System.Web.UI;

namespace Omnicx.WebStore.Core.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IProductApi _productApi;
        private readonly ICollectionApi _collectionApi;
        private readonly IBlogApi _blogApi;
        public SearchController(IProductApi productApi , ICollectionApi collectionApi , IBlogApi blogApi)
        {
            _productApi = productApi;
            _collectionApi = collectionApi;
            _blogApi = blogApi;
        }
        // GET: Search
        public ActionResult Search(SearchRequestModel searchCriteria)
        {
            var response = _productApi.GetKeywordRedirect();
            var obj = response.Result.Where(x => x.Keywords.Trim().Equals(searchCriteria.FreeText)).FirstOrDefault();
            if (obj != null)
                return Redirect(obj.Url);
            searchCriteria.AllowFacet = true;
            searchCriteria.AllowFacet = Request.QueryString[Constants.SEARCH_SURVEY_QUERYSTRING]==null?true:false;
            if (Request.QueryString[Constants.SEARCH_FILTER_QUERYSTRING] != null)
            {
                searchCriteria.Filters = new List<SearchFilter>();
                var qryfilters = Request.QueryString[Constants.SEARCH_FILTER_QUERYSTRING];
                foreach (var qfilter in qryfilters.Split(';'))
                {
                    var filter = new SearchFilter { Key = Constants.ATTRIBUTE_FILTER_PREFIX + qfilter.Split(':')[0], Value = qfilter.Split(':')[1] };
                    searchCriteria.Filters.Add(filter);
                }
            }
          
            var searchRequestModel = sanitizeInput(searchCriteria);
            var result = SearchHelper.GetPaginatedProducts(searchRequestModel);
            if (result.Results != null && result.Results.Count == 1)
            {
                if(!String.IsNullOrEmpty(result.Results[0].Slug))
                {return RedirectToAction("ProductDetail","Product", new { name = result.Results[0].Slug.Split('/')[1] }); }               
            }
            //TODO: Sort by shoudl be part of the search result itself and NOT a separate call
            //var sortBy = _productApi.GetSortBy();
            searchCriteria.ResultCount = (result.Results==null) ? 0 : result.Results.Count;
            SetDataLayerVariables(searchCriteria, WebhookEventTypes.FreeText);
            return View(CustomViews.SEARCH, result);
        }
        public ActionResult SearchProducts(SearchRequestModel searchCriteria)
        {
            var searchRequestModel = sanitizeInput(searchCriteria);
            var result = SearchHelper.GetPaginatedProducts(searchRequestModel);
            if (searchRequestModel.FreeText != null) return JsonSuccess(result, JsonRequestBehavior.AllowGet);
            if (searchRequestModel.Filters == null) return JsonSuccess(result, JsonRequestBehavior.AllowGet);
            if (result.Filters == null) return JsonSuccess(result, JsonRequestBehavior.AllowGet);
            foreach (SearchFilterModel t2 in result.Filters)
            {
                foreach (FacetKeyValueModel t1 in t2.Items)
                {
                    foreach (SearchFilter t in searchRequestModel.Filters)
                    {
                        if (t.Key == t2.Key && t.Value == t1.Name)
                        {
                            t1.IsSelected = true;
                        }
                    }
                }
            }
            return JsonSuccess(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchAutoComplete(string freeText)
        {
            freeText = Sanitizer.GetSafeHtmlFragment(freeText.ToLower());
            var response = _productApi.SearchFreeText(freeText);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        ///NOT USED ANYMORE. SHOUDL NOT BE USED ANYWAYS.
        //public ActionResult GetSortBy()
        //{
        //     var response = _productApi.GetSortBy();
        //    return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// Displays the list of dynamic lists
        /// </summary>
        /// <returns></returns>
        public ActionResult DynamicList()
        {
            var response = _collectionApi.GetCollectionList();
            var model = new Collections {CollectionList = response.Result };
            SetDataLayerVariables(response.Result, WebhookEventTypes.CollectionViewed);
            return View(CustomViews.DYNAMIC_LIST, model);
        }
        /// <summary>
        /// Displays the items in the selected dynamic list
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        public ActionResult DynamicListItems()
        {
            var slug = SiteUtils.GetSlugFromUrl();
            var response = _collectionApi.GetCollectionBySlug(slug);
            var list = response.Result;
            if (response.Result == null && response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToPageNotFound();
            }
            if (list.Groups != null)
            {
                list.Products.ProductGroupModel = new GroupModel();
                list.Products.ProductGroupModel.AllowGrouping = list.AllowGrouping;
                list.Products.ProductGroupModel.DisplayTitle = list.DisplayTitle;
                list.Products.ProductGroupModel.GroupCode = list.GroupCode;
                list.Products.ProductGroupModel.GroupSeparator = list.GroupSeparator;
                list.Products.ProductGroupModel.Groups = list.Groups;
            }
            list.Products.CustomInfo1 = list.CustomInfo1;
            list.Products.CustomInfo2 = list.CustomInfo2;
            list.Products.CustomInfo3 = list.CustomInfo3;
            list.Products.CustomFieldDisplayOrder = list.CustomFieldDisplayOrder;
            list.Products.CustomFieldValue = list.CustomFieldValue;
            list.Products.FilterCriteria = list.FilterCriteria;
            if (list.Products != null && list.Products.Results!=null && list.Products.Results.Any())
            {
                var searchCriteria = new SearchRequestModel {CollectionId = list.Id ,AllowFacet = list.AllowFacets };
                searchCriteria.CurrentPage = 1;
                searchCriteria.PageSize = list.PageSize == 0 ? Convert.ToInt32(ConfigKeys.PageSize) : list.PageSize;
                searchCriteria.SortBy = string.IsNullOrWhiteSpace(searchCriteria.SortBy) ? _sessionContext?.CurrentSiteConfig?.SearchSettings?.DefaultSortBy : searchCriteria.SortBy;
                list.Products.SearchCriteria = searchCriteria;
                var validFilters = list.Products.Filters.GroupBy(c => new { c.Name }).Select(c => c.First()).ToList();
                list.Products.Filters = validFilters;
                list.Products.Filters = validFilters.Where(p => !validFilters.Any(p2 => p2.Name == p.Name && p.Items.Count == 0)).ToList();
                var priceFacet = list.Products.Filters.Where(x => x.Name.ToLower() == Constants.PRICE_FILTER).FirstOrDefault();
                if(priceFacet!=null)
                {
                    foreach (var itm in priceFacet.Items)
                    {
                        if (itm.From != null)
                        {
                            itm.PriceFilter = ((int)Convert.ToDecimal(itm.From));
                        }
                    }
                }
                SetDataLayerVariables(list, WebhookEventTypes.CollectionViewed);
                return View(CustomViews.DYNAMIC_LIST_PRODUCTS, list);
            }
            SetDataLayerVariables(list, WebhookEventTypes.CollectionViewed);
            return View(CustomViews.DYNAMIC_LIST_ITEMS, list);
        }
        private SearchRequestModel sanitizeInput(SearchRequestModel searchCriteria)
        {
            var searchRequestModel = new SearchRequestModel
            {
                AllowFacet = searchCriteria.AllowFacet,
                Brand = Sanitizer.GetSafeHtmlFragment(searchCriteria.Brand),
                BrandId = Sanitizer.GetSafeHtmlFragment(searchCriteria.BrandId),
                BreadCrumb = Sanitizer.GetSafeHtmlFragment(searchCriteria.BreadCrumb),
                Category = Sanitizer.GetSafeHtmlFragment(searchCriteria.Category),
                CategoryId = Sanitizer.GetSafeHtmlFragment(searchCriteria.CategoryId),
                Collection = Sanitizer.GetSafeHtmlFragment(searchCriteria.Collection),
                CollectionId = Sanitizer.GetSafeHtmlFragment(searchCriteria.CollectionId),
                CurrentPage = searchCriteria.CurrentPage,
                Facet = Sanitizer.GetSafeHtmlFragment(searchCriteria.Facet),
                FreeText = Sanitizer.GetSafeHtmlFragment(searchCriteria.FreeText),
                Gender = Sanitizer.GetSafeHtmlFragment(searchCriteria.Gender),
                Page = searchCriteria.Page,
                PageSize = searchCriteria.PageSize,
                SortBy = Sanitizer.GetSafeHtmlFragment(searchCriteria.SortBy),
                SortOrder = Sanitizer.GetSafeHtmlFragment(searchCriteria.SortOrder),
                CategoryIds = searchCriteria.CategoryIds,
                Filters = searchCriteria.Filters
            };
            return searchRequestModel;
        }
       
       
    }
}