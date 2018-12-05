using Newtonsoft.Json;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.B2B;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Keys;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class B2BApi : ApiBase,IB2BApi
    {
        //B2B 
        public ResponseModel<List<QuoteInfoModel>> GetQuotes(string userId)
        {
            return CallApi<List<QuoteInfoModel>>(string.Format(ApiUrls.GetQuotes, userId), "", Method.POST);
        }
        public ResponseModel<List<CompanyUserModel>> GetUsers(string companyId)
        {
            return CallApi<List<CompanyUserModel>>(string.Format(ApiUrls.GetUsers, companyId), "", Method.POST);
        }
        public ResponseModel<bool> UpdateCompanyDetail(CompanyDetailModel model)
        {
            return new ResponseModel<bool>();
            //return CallApi<bool>(string.Format(ApiUrls.UpdateCompanyDetail), JsonConvert.SerializeObject(model), Method.POST);
        }
        //public ResponseModel<T> GetUserdetailsById<T>(string userId)
        //{
        //    return CallApi<T>(string.Format(ApiUrls.CompanyUserDetailId, userId), "");
        //}
        public ResponseModel<CompanyDetailModel> GetCompanyDetail(string userId)
        {
            return CallApi<CompanyDetailModel>(string.Format(ApiUrls.CompanyDetail, userId), "");
        }
        public ResponseModel<bool> SaveQuote(QuoteInfoModel model)
        {
            var resp = CallApi<bool>(string.Format(ApiUrls.SaveQuote), JsonConvert.SerializeObject(model), Method.POST);
            //if (!String.IsNullOrEmpty(resp.Message))
            //{
            //    System.Web.HttpCookie cookie_basket = HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID];
            //    if (cookie_basket != null)
            //    {
            //        cookie_basket.Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS);
            //        cookie_basket.Value = Guid.NewGuid().ToString();
            //        HttpContext.Current.Response.SetCookie(cookie_basket);
            //    }
            //}
            return resp;
        }
        public ResponseModel<BasketModel> GetQuoteDetail(string quoteId)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, quoteId), "");
        }
        public ResponseModel<QuoteInfoModel> ValidateQuotePayment(string link)
        {
            return CallApi<QuoteInfoModel>(string.Format(ApiUrls.ValidateQuotePayment,link),"",Method.POST);
        }
        public ResponseModel<bool> RequestQuoteChange(string userId, string quoteNo)
        {
            return CallApi<bool>(string.Format(ApiUrls.RequestQuoteChange, userId,quoteNo), "", Method.POST);
        }
        public ResponseModel<BasketModel> GetQuoteBasket(string quoteId,string action)
        {
            Guid basketId;
            Guid.TryParse(quoteId, out basketId);
            var result = new ResponseModel<BasketModel>();
            //Get currentBasketId from cookies or by DeviceId(if cookieBasketId is null SP returns by DeviceId) and call PersistentBasket. 
            if (action == ExistingBasket.Merge.GetHashCode().ToString()) {
                var currentBasketId = HttpContext.Current.Request.Cookies[Constants.COOKIE_BASKETID]?.Value.ToString();
                if(String.IsNullOrEmpty(currentBasketId))
                {
                    var currentBasket = CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, String.IsNullOrEmpty(currentBasketId) ? Guid.Empty.ToString() : currentBasketId), "");
                    currentBasketId = currentBasket.Result?.Id;
                }
                result = CallApi<BasketModel>(string.Format(ApiUrls.PersistentBasket, quoteId, currentBasketId), "", Method.POST);             
            }
            //Set basketId in cookies that is quoteBasketId .
            var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = result.Result != null ? result.Result.Id : quoteId, Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };               
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie_basketId);
            return CallApi<BasketModel>(string.Format(ApiUrls.GetBasket, basketId), "");
        }
        public bool RemoveQuoteBasket()
        {
            //removes quoteBasketId from cookies when user has updated his quote and now wishes to get currentBasket back .
            var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = Guid.NewGuid().ToString(), Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie_basketId);
            return true;
        }
        public ResponseModel<List<CompanyNameModel>> GetCompanies()
        {
            var result = new ResponseModel<List<CompanyNameModel>>();
            result = CallApi<List<CompanyNameModel>>(string.Format(ApiUrls.GetCompanies), "", Method.POST);
            return result;
        }
    }
}
