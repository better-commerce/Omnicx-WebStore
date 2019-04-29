using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Omnicx.WebStore.Models.Commerce;
using RestSharp;
using Omnicx.WebStore.Models;

using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Store;
using System.Linq;
using Omnicx.WebStore.Models.Enums;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Commerce.Subscription;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class BasketApi : ApiBase, IBasketApi
    {
        public ResponseModel<BasketModel> AddToBasket(BasketAddModel basketLine)
        {
            var basketId = GetBasketId(basketLine.BasketId);
            basketLine.BasketId = basketId.ToString();
            var result = CallApi<BasketModel>(string.Format(ApiUrls.AddToBasket, basketId), JsonConvert.SerializeObject(basketLine), Method.POST);
            if (result.Result != null && !result.Result.IsQuote)
            {
                var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = result.Result.Id, Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
                if ((result.Result.LineItems == null || result.Result.LineItems.Count == 0))
                {
                    cookie_basketId.Value =Guid.NewGuid().ToString();
                    result.Result = new BasketModel();
                }              
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie_basketId);
            }
            ResetSessionBasket();
            return result;
        }
        public ResponseModel<PromoResponseModel> ApplyPromoCode(string basketId, string promoCode) 
        {
            ResetSessionBasket();
            return CallApi<PromoResponseModel>(string.Format(ApiUrls.ApplyPromoCode, basketId, promoCode), "", Method.POST);
        }
        //public ResponseModel<PromoResponseModel> ApplyPromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo)
        //{
        //    return CallApi<PromoResponseModel>(string.Format(ApiUrls.ApplyPromoCodeBulk, basketId, promoCode), JsonConvert.SerializeObject(customInfo), Method.POST);
        //}
        
        public ResponseModel<PromoResponseModel> RemovePromoCode(string basketId, string promoCode)
        {
            ResetSessionBasket();
            return CallApi<PromoResponseModel>(string.Format(ApiUrls.RemovePromoCode, basketId, promoCode), "", Method.POST);
        }
        //public ResponseModel<PromoResponseModel> RemovePromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo)
        //{
        //    return CallApi<PromoResponseModel>(string.Format(ApiUrls.RemovePromoCodeBulk, basketId, promoCode), JsonConvert.SerializeObject(customInfo), Method.POST);
        //}
        public ResponseModel<BasketModel> GetBasketData(string id)
        {
            var basketId = GetBasketId(id);
            var basketResponse = GetBasketDataFromSession(basketId);
            if (basketResponse == null)
            {
                basketResponse = CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
                BasketModel basket = basketResponse?.Result;
                basketResponse.Result = basket;
                System.Web.HttpContext.Current.Session[Constants.SESSION_BASKET] = basketResponse;
            }
            return basketResponse;
        }

       
        public async Task<ResponseModel<BasketModel>> GetBasketDataAsync(string id)
        {
            var basketId= GetBasketId(id);
            var basket = GetBasketDataFromSession(basketId);
            if (basket == null)
            {
                var task = await CallApiAsync<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
                System.Web.HttpContext.Current.Session[Constants.SESSION_BASKET] = task.Result;
                if (task.Result != null)
                {
                    var cookie_basket = System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID];
                    if ((task.Result.LineItems == null || task.Result.LineItems.Count == 0) && cookie_basket != null)
                    {
                        cookie_basket.Value = Guid.NewGuid().ToString();
                        System.Web.HttpContext.Current.Request.Cookies.Add(cookie_basket);
                    }
                }
                return task;
            }
               
            return basket;
        }
        public ResponseModel<BasketModel> UpdateShipping(string basketId, string shippingId, NominatedDeliveryModel nominatedDelivery)
        {
            ResetSessionBasket();
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateShipping, basketId, shippingId), JsonConvert.SerializeObject(nominatedDelivery), Method.POST);
        }
        public ResponseModel<BasketModel> BulkAddProduct(List<BasketAddModel> basketLine)
        {
            ResetSessionBasket();
            Guid basketId=GetBasketId("");          
            var result = CallApi<BasketModel>(string.Format(ApiUrls.BulkAddProduct, basketId), JsonConvert.SerializeObject(basketLine), Method.POST);
            if (result.Result != null)
            {
                var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = result.Result.Id, Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie_basketId);
            }
            return result;
        }

        public ResponseModel<BasketModel> AddPersistentBasket(Guid id, Guid sourceBasketId)
        {
            ResetSessionBasket();
            return CallApi<BasketModel>(string.Format(ApiUrls.PersistentBasket, id, sourceBasketId), "", Method.POST);
        }

        public ResponseModel<List<BasketModel>> GetAllUserBaskets(Guid userId)
        {
            return CallApi<List<BasketModel>>(string.Format(ApiUrls.GetUserBaskets, userId), "", Method.GET);
        }
        public ResponseModel<BasketModel> UpdateUserToBasket(string basketId, string userId)
        {
            ResetSessionBasket();
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketUser, basketId, userId), "", Method.POST);
        }
        public ResponseModel<List<ProductModel>> GetRelatedProducts(string id)
        {
            return CallApi<List<ProductModel>>(string.Format(ApiUrls.BasketRelatedProducts, id), "", Method.POST);
        }
        public ResponseModel<BasketModel> UpdateBasketInfo(string basketId, HeaderCustomInfo info)
        {
            ResetSessionBasket();
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketLineCustomInfo, basketId), JsonConvert.SerializeObject(info), Method.POST);
        }
        public ResponseModel<BasketModel> UpdateBasketSubscriptionInfo(Guid basketId, Guid productId, SubscriptionUserSetting userSetting)
        {
            ResetSessionBasket();
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketSubscriptionInfo, basketId, productId), JsonConvert.SerializeObject(userSetting), Method.POST);

        }
        private Guid GetBasketId(string id)
        {
            Guid basketId;
            Guid.TryParse(id, out basketId);
            if (basketId == Guid.Empty)
            {
                var cookie_basket = System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID];
                var cookie_basketId = cookie_basket?.Value;
                Guid.TryParse(cookie_basketId, out basketId);
            }
            return basketId;
        }

        public void PopulateDeliveryPlans(ref BasketModel basket,string requestSource)
        {
            // MA : discussed with Sir, to update the delivery plans only from basket index page and checkout page and only when delivery plan is updated
            requestSource = string.IsNullOrEmpty(requestSource) ? "" : requestSource.ToLower();
            if (basket != null && !string.IsNullOrEmpty(basket.ShippingMethodId) && (requestSource=="index" || requestSource == "opc" || requestSource == "basket"))
            {
                var shippingId = basket.ShippingMethodId;
                var shippingMethod = basket.ShippingMethods?.FirstOrDefault(sp => sp.Id.ToString() == shippingId);
                ShippingPlanRequest shippingPlanRequest = new ShippingPlanRequest()
                {
                    BasketId = Guid.Parse(basket.Id),
                    PostCode = basket.PostCode,
                    AllowPartialOrderDelivery = true,
                    AllowPartialLineDelivery = true,
                    ShippingMethodId = Guid.Parse(basket.ShippingMethodId),
                    ShippingMethodName = shippingMethod?.DisplayName,
                    ShippingMethodCode=shippingMethod?.ShippingCode,
                    ShippingMethodType = (shippingMethod == null) ? ShippingMethodTypes.Standard : shippingMethod.Type,
                    DeliveryItems = (from ol in basket.LineItems
                                     select new DeliveryItemLine()
                                     {
                                         BasketLineId = Convert.ToInt64(ol.Id),
                                         ProductId = Guid.Parse(ol.ProductId),
                                         ParentProductId= Guid.Parse(ol.ParentProductId),
                                         StockCode = ol.StockCode,
                                         Qty = ol.Qty
                                     }
                                                   ).ToList()
                };
                var _shippingApi = DependencyResolver.Current.GetService<IShippingApi>();
                var shipmentResponse = _shippingApi.GetShippingPlans(shippingPlanRequest);
                if (shipmentResponse != null && shipmentResponse.Result != null)
                {
                    var shipmentPlans = shipmentResponse.Result;
                    foreach (ShippingPlan plan in shipmentPlans)
                    {
                        plan.LineItems = basket.LineItems.Where(li => plan.Items.Any(pi => li.StockCode == pi.StockCode)).ToList();
                    }
                    basket.DeliveryPlans = shipmentPlans;
                    var updateResponse=CallApi<BoolResponse>(string.Format(ApiUrls.UpdateBasketDeliveryPlans, basket.Id), JsonConvert.SerializeObject(shipmentPlans), Method.POST);

                }
            }
            ResetSessionBasket(); // reset the session basket  so as to get the latest info from api.
        }
        public async Task<ResponseModel<BoolResponse>> UpdateBasketCampaign(Guid basketId, string campaignCode)
        {
            ResetSessionBasket();
            var task = await CallApiAsync<BoolResponse>(string.Format(ApiUrls.UpdateBasketCampaign, basketId, campaignCode), null, Method.POST);
            return task;
        }

        public ResponseModel<BoolResponse> UpdatePoReference(Guid basketId, string poReferenceNumber)
        {
            ResetSessionBasket();
            return CallApi<BoolResponse>(string.Format(ApiUrls.UpdatePoReference, basketId, poReferenceNumber), null, Method.POST);
        }
        private ResponseModel<BasketModel> GetBasketDataFromSession(Guid basketId)
        {
            var basket = System.Web.HttpContext.Current.Session[Constants.SESSION_BASKET];
            return (ResponseModel<BasketModel>)basket;
        }
        public void ResetSessionBasket()
        {
            System.Web.HttpContext.Current.Session[Constants.SESSION_BASKET] = null;
        }
    }
}
