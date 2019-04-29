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
using Omnicx.WebStore.Models.Infrastructure.Settings;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Helpers;

namespace Omnicx.Site.Core.Controllers
{
    public class RecomendationController : BaseController
    {
        private readonly IProductApi _productApi;
        private readonly IBasketApi _basketApi;
        private RecommendationSettings _recommendationSettings;
        private RecommendationsAPI _recommendationClient;
        public RecomendationController(IProductApi productApi, IBasketApi basketApi)
        {
            _productApi = productApi;
            _basketApi = basketApi;
        }

        // GET: Recomendation
        public ActionResult Index()
        {
            ViewBag.UserId = "";
            var noOfItems = 5;
            ViewBag.Type = RecommendationTypes.Personalised.ToString();
            if (Request.QueryString["Uid"]!=null)
            {
                ViewBag.UserId = Request.QueryString["Uid"].ToString();
            }
            if (Request.QueryString["noofitems"] != null)
            {
                noOfItems = int.Parse( Request.QueryString["noofitems"].ToString());
            }
            if (Request.QueryString["type"] != null)
            {
                ViewBag.Type = Request.QueryString["type"].ToString();
            }
            ViewBag.NoOfItems = noOfItems;
            var result = GetItemRecommendations("", null, ViewBag.Type, "", noOfItems, ViewBag.UserId);          
            return View(result.Data);
        }

       
        public JsonResult GetItemRecommendations(string itemId, List<string> recentViewedProductList, string recommedType,string modelId,int noOfItems,string userId)
        {           
            var resp = new List<RecommendationResult>();           
            if (recommedType == RecommendationTypes.RecentView.ToString())
            {
                if(recentViewedProductList!=null && !recentViewedProductList.Any())
                    return JsonSuccess("", JsonRequestBehavior.AllowGet);
                resp = (from o in recentViewedProductList select new RecommendationResult { RecommendedItemId = o }).ToList();
            }
            else
            {
                _recommendationSettings = _sessionContext.CurrentSiteConfig.RecommendationSettings;
                _recommendationClient = new RecommendationsAPI(new Uri(_recommendationSettings.ApiEndPoint));

                _recommendationClient.HttpClient.DefaultRequestHeaders.Add("x-api-key", _recommendationSettings.RecommederKey);
                // string userId = string.Empty;
                string visitorId = string.Empty;
                if (string.IsNullOrEmpty(userId))
                {
                     userId = string.Empty;
                   
                    if (_sessionContext.CurrentUser != null)
                        userId = _sessionContext.CurrentUser.UserId.ToString();
                    else
                        visitorId = _sessionContext.DeviceId;
                }
               

                if (string.IsNullOrEmpty(modelId))
                {
                    modelId = GetRecommendationModelId(recommedType);
                }
                var modelGuId = Guid.Empty;
                Guid.TryParse(modelId, out modelGuId);
                var usageEvent = new List<UsageEvent>();
                if(recommedType == RecommendationTypes.Basket.ToString())
                {
                    var basket = _basketApi.GetBasketData("")?.Result;
                    if(basket!=null  && basket.LineItems != null && basket.LineItems.Any())
                    {
                        itemId = string.Join(",", basket.LineItems.Select(x => x.ProductId));
                    }
                    
                }
                if (!string.IsNullOrEmpty(itemId))
                {
                    if (modelGuId == Guid.Empty)
                        resp = _recommendationClient.Models.GetItemRecommendationsFromDefaultModel(itemId, noOfItems)?.ToList();

                    else
                        resp = _recommendationClient.Models.GetItemRecommendations(modelGuId, itemId, noOfItems)?.ToList();

                }
                else
                {
                    if (modelGuId == Guid.Empty)
                        resp = _recommendationClient.Models.GetPersonalizedRecommendationsFromDefaultModel(usageEvent, userId != string.Empty ? userId : visitorId, noOfItems)?.ToList();
                    else
                        resp = _recommendationClient.Models.GetPersonalizedRecommendations(modelGuId, usageEvent, userId != string.Empty ? userId : visitorId, noOfItems)?.ToList();

                }
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
        private string GetRecommendationModelId(string recommedType)
        {          
            var models= _recommendationClient.Models.GetAll();
            if(models!=null && models.Any())
            {
                var id = Utils.GetEnumValueByName(typeof(RecommendationTypes), recommedType);
                return models.FirstOrDefault(x => x.RecommendationMasterId == id)?.Id??"";
            }           
            return "";

        }
    }
}