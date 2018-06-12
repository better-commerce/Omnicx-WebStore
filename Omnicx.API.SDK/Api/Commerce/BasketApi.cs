using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Omnicx.API.SDK.Models.Commerce;
using RestSharp;
using Omnicx.API.SDK.Models;
using Omnicx.API.SDK.Entities;
using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class BasketApi : ApiBase, IBasketApi
    {
        public ResponseModel<BasketModel> AddToBasket(BasketAddModel basketLine)
        {
            Guid basketId;
            Guid.TryParse(basketLine.BasketId, out basketId);
            var result = CallApi<BasketModel>(string.Format(ApiUrls.AddToBasket, basketId), JsonConvert.SerializeObject(basketLine), Method.POST);
            if (result.Result != null && !result.Result.IsQuote)
            {
                var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = result.Result.Id, Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
                if ((result.Result.LineItems == null || result.Result.LineItems.Count == 0))
                {
                    cookie_basketId.Expires = DateTime.Now.AddDays(-1);
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie_basketId);
            }
            return result;
        }
        public ResponseModel<PromoResponseModel> ApplyPromoCode(string basketId, string promoCode) 
        {
            return CallApi<PromoResponseModel>(string.Format(ApiUrls.ApplyPromoCode, basketId, promoCode), "", Method.POST);
        }
        //public ResponseModel<PromoResponseModel> ApplyPromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo)
        //{
        //    return CallApi<PromoResponseModel>(string.Format(ApiUrls.ApplyPromoCodeBulk, basketId, promoCode), JsonConvert.SerializeObject(customInfo), Method.POST);
        //}

        public ResponseModel<PromoResponseModel> RemovePromoCode(string basketId, string promoCode)
        {
            return CallApi<PromoResponseModel>(string.Format(ApiUrls.RemovePromoCode, basketId, promoCode), "", Method.POST);
        }
        //public ResponseModel<PromoResponseModel> RemovePromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo)
        //{
        //    return CallApi<PromoResponseModel>(string.Format(ApiUrls.RemovePromoCodeBulk, basketId, promoCode), JsonConvert.SerializeObject(customInfo), Method.POST);
        //}
        public ResponseModel<BasketModel> GetBasketData(string id)
        {
            Guid basketId;
            if (String.IsNullOrEmpty(id))
            {
                id = System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID]?.Value.ToString();
            }
            Guid.TryParse(id, out basketId);
            return CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
        }

        public async Task<ResponseModel<BasketModel>> GetBasketDataAsync(string id)
        {
            Guid basketId;
            Guid.TryParse(id, out basketId);
            System.Web.HttpCookie cookie_basket = null;
            //removed condition in if statement(&& UserId == null)  . Statement not looking meaningfull . 
            if (basketId == Guid.Empty)
            {
                cookie_basket = System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID];
                var cookie_basketId = cookie_basket?.Value;
                Guid.TryParse(cookie_basketId, out basketId);
            }           
            var task = await CallApiAsync<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
            if(task.Result != null)
            {
                if ((task.Result.LineItems == null || task.Result.LineItems.Count == 0) && cookie_basket != null)
                {
                    cookie_basket.Expires = DateTime.Now.AddDays(-1);
                    System.Web.HttpContext.Current.Request.Cookies.Add(cookie_basket);
                }
            }
            return task;
        }
        public ResponseModel<BasketModel> UpdateShipping(string basketId, string shippingId, NominatedDeliveryModel nominatedDelivery)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateShipping, basketId, shippingId), JsonConvert.SerializeObject(nominatedDelivery), Method.POST);
        }
        public ResponseModel<BasketModel> BulkAddProduct(List<BasketAddModel> basketLine)
        {
            Guid basketId;
            var currentbasket = System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID]?.Value;
            Guid.TryParse(currentbasket, out basketId);
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
            return CallApi<BasketModel>(string.Format(ApiUrls.PersistentBasket, id, sourceBasketId), "", Method.POST);
        }

        public ResponseModel<List<BasketModel>> GetAllUserBaskets(Guid userId)
        {
            return CallApi<List<BasketModel>>(string.Format(ApiUrls.GetUserBaskets, userId), "", Method.GET);
        }
        public ResponseModel<BasketModel> UpdateUserToBasket(string basketId, string userId)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketUser, basketId, userId), "", Method.POST);
        }
        public ResponseModel<List<ProductModel>> GetRelatedProducts(string id)
        {
            return CallApi<List<ProductModel>>(string.Format(ApiUrls.BasketRelatedProducts, id), "", Method.POST);
        }
        public ResponseModel<BasketModel> UpdateBasketInfo(string basketId, HeaderCustomInfo info)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketLineCustomInfo, basketId), JsonConvert.SerializeObject(info), Method.POST);
        }
    }
}
