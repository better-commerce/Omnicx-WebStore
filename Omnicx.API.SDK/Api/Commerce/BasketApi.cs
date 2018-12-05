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
            var basketId = GetBasketId(id);
            Guid.TryParse(id, out basketId);
            return CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
        }

       
        public async Task<ResponseModel<BasketModel>> GetBasketDataAsync(string id)
        {
            var basketId= GetBasketId(id);           
            var task = await CallApiAsync<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
            if(task.Result != null)
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
        public ResponseModel<BasketModel> UpdateShipping(string basketId, string shippingId, NominatedDeliveryModel nominatedDelivery)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateShipping, basketId, shippingId), JsonConvert.SerializeObject(nominatedDelivery), Method.POST);
        }
        public ResponseModel<BasketModel> BulkAddProduct(List<BasketAddModel> basketLine)
        {
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

        public async Task<ResponseModel<BoolResponse>> UpdateBasketCampaign(Guid basketId, string campaignCode)
        {

            var task = await CallApiAsync<BoolResponse>(string.Format(ApiUrls.UpdateBasketCampaign, basketId, campaignCode), null, Method.POST);
            return task;
        }

        public ResponseModel<BoolResponse> UpdatePoReference(Guid basketId, string poReferenceNumber)
        {
            return CallApi<BoolResponse>(string.Format(ApiUrls.UpdatePoReference, basketId, poReferenceNumber), null, Method.POST);
        }
    }
}
