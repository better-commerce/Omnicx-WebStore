using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omnicx.API.SDK.Recomendation;
using Omnicx.API.SDK.Recomendation.Models;
using Omnicx.WebStore.Core.Controllers;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.Site.Core.Controllers
{
    public class RecomendationController : BaseController
    {
        private readonly IProductApi _productApi;
        public RecomendationController(IProductApi productApi)
        {
            _productApi = productApi;
        }

        // GET: Recomendation
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetItemRecommendations(string itemId, List<string> recentViewedProductList, string pageCategory)
        {
            var recommendationClient = new RecommendationsAPI(new Uri(ConfigKeys.RecommendationsEndPointUri));
            recommendationClient.HttpClient.DefaultRequestHeaders.Add("x-api-key", ConfigKeys.RecommenderKey);  
            string userId = string.Empty;
            string visitorId = string.Empty;
            var resp = new List<RecommendationResult>();
            if (_sessionContext.CurrentUser != null)
            {
                userId = _sessionContext.CurrentUser.UserId.ToString();
            }
            else
            {
                visitorId = _sessionContext.DeviceId;
            }
            var usageEvent = new List<UsageEvent>();
            if (pageCategory == PageCategory.Product.ToString())
            {
                resp = recommendationClient.Models.GetItemRecommendationsFromDefaultModel(itemId, Convert.ToInt32(ConfigKeys.RecommendationCount))?.ToList();
            }
            else
            {
                resp = recommendationClient.Models.GetPersonalizedRecommendationsFromDefaultModel(usageEvent, userId != string.Empty ? userId : visitorId, Convert.ToInt32(ConfigKeys.RecommendationCount))?.ToList();
            }

            if (resp != null && resp.Count > 0)
            {
                SearchRequestModel criteria = new SearchRequestModel
                {
                    Filters = new List<SearchFilter>()
                };
                foreach (var data in resp)
                {
                    var searchFilter = new SearchFilter
                    {
                        Key = "recordId",
                        Value = data.RecommendedItemId
                    };
                    criteria.Filters.Add(searchFilter);
                }
                var response = _productApi.GetProducts(criteria);
                if (response != null && response.Result != null)
                {
                    return JsonSuccess(response.Result.Results, JsonRequestBehavior.AllowGet);
                }
            }
            return JsonSuccess("", JsonRequestBehavior.AllowGet);
        }
    }
}