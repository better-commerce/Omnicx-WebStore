using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Omnicx.WebStore.Framework.Entities;
using Omnicx.WebStore.Framework.Security;
using RestSharp;
using Omnicx.API.SDK.Helpers;
using System.Threading.Tasks;
using System.Threading;
using Omnicx.WebStore.Models;
using System.Security;
using Omnicx.WebStore.Models.Infrastructure;
using Omnicx.WebStore.Models.Keys;
using Omnicx.API.SDK.Api.Infra;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiBase
    {
       
        protected ResponseModel<T> CallApi<T>(string apiUrl, string value, Method method = Method.GET, string paramName = "application/json", ParameterType parameterType = ParameterType.RequestBody, string contentType = "application/json",string apiBaseUrl=null, bool isAuthenticationEnabled=false)
        {

            var restClient = new RestClient(apiBaseUrl ?? ConfigKeys.OmnicxApiBaseUrl);
            var restRequest = new RestRequest(apiUrl, method);

            if (!string.IsNullOrEmpty(value))
            {
                var param = new Parameter()
                {
                    Name = paramName,
                    Type = parameterType,
                    ContentType = contentType,
                    Value = value
                };
                restRequest.AddParameter(param);
            }
          
            //restRequest.AddHeader("token", Token().Token);
            restRequest.AddHeader("DeviceId", ReadCookie(Constants.COOKIE_DEVICEID));
            restRequest.AddHeader("SessionId", ReadCookie(Constants.COOKIE_SESSIONID));

            AddDefaultHeader(ref restRequest);
            //if (isAuthenticationEnabled)
            //{
                var token = OAuthHelper.GetAccessToken(ConfigKeys.OAuthUrl + "/OAuth/Token", ConfigKeys.OmnicxAppId, ConfigKeys.OmnicxSharedSecret);
                restRequest.AddParameter("Authorization", "Bearer " + token.Token, ParameterType.HttpHeader);
           // }

            var restResponse = restClient.Execute(restRequest);
            try
            {
                var result= JsonConvert.DeserializeObject<ResponseModel<T>>(restResponse.Content);
                SetContentSnippets(result);
                return result;
            }
            catch (Exception ex)
            {
                return new ResponseModel<T> {Message=ex.Message };
               // return (T)Activator.CreateInstance(typeof(T));
            }

        }
        public T CallApiExternal<T>(string apiUrl, string value, Method method = Method.GET, string paramName = "application/json", ParameterType parameterType = ParameterType.RequestBody, 
            string contentType = "application/json", string apiBaseUrl = null,  Dictionary<string, string> headers = null)
        {

            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest(apiUrl, method);

            if (!string.IsNullOrEmpty(value))
            {
                var param = new Parameter()
                {
                    Name = paramName,
                    Type = parameterType,
                    ContentType = contentType,
                    Value = value
                };
                restRequest.AddParameter(param);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    restRequest.AddHeader(header.Key, header.Value);
                }
            }              


            var restResponse = restClient.Execute(restRequest);
            try
            {
                var result = JsonConvert.DeserializeObject<T>(restResponse.Content);            
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }

        }

        protected  Task<ResponseModel<T>> CallApiAsync<T>(string apiUrl, string value, Method method = Method.GET, string paramName = "application/json", ParameterType parameterType = ParameterType.RequestBody, string contentType = "application/json")
        {

            var restClient = new RestClient(ConfigKeys.OmnicxApiBaseUrl);
            var restRequest = new RestRequest(apiUrl, method);

            if (!string.IsNullOrEmpty(value))
            {
                var param = new Parameter()
                {
                    Name = paramName,
                    Type = parameterType,
                    ContentType = contentType,
                    Value = value
                };
                restRequest.AddParameter(param);
            }
            //restRequest.AddHeader("token", Token().Token);
            var token = OAuthHelper.GetAccessToken(ConfigKeys.OAuthUrl + "/OAuth/Token", ConfigKeys.OmnicxAppId, ConfigKeys.OmnicxSharedSecret);
            restRequest.AddParameter("Authorization", "Bearer " + token.Token, ParameterType.HttpHeader);

            restRequest.AddHeader("DeviceId", ReadCookie(Constants.COOKIE_DEVICEID));
            restRequest.AddHeader("SessionId", ReadCookie(Constants.COOKIE_SESSIONID));
            AddDefaultHeader(ref restRequest);
            var cancellationTokenSource = new CancellationTokenSource();

            var tcs = new TaskCompletionSource<ResponseModel<T>>();         
            restClient.ExecuteAsync(restRequest,  response =>
            {
                try
                {
                    tcs.SetResult(JsonConvert.DeserializeObject<ResponseModel<T>>(response.Content));
                    //return tcs.Task;
                }
                catch (Exception ex)
                {
                    tcs.SetResult((ResponseModel<T>)Activator.CreateInstance(typeof(ResponseModel<T>)));
                    throw ex;
                }
            });
            SetContentSnippets(tcs.Task.Result);
            return Task.FromResult(tcs.Task.Result);

        }


        private string ReadCookie(string cookieName)
        {
            var httpContext = DependencyResolver.Current.GetService<HttpContextBase>();
            if (httpContext == null || httpContext.Handler == null || httpContext.Request == null) return "";
            if (httpContext.Request.Cookies[cookieName] == null) return "";
            return httpContext.Request.Cookies[cookieName].Value;

        }
        private void SetContentSnippets<T>(ResponseModel<T> response)
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Items == null) return;
            if (response!=null && response.Snippets != null && System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS]==null)
            {
                System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SNIPPETS] = response.Snippets;
            }
        }
        private void AddDefaultHeader(ref RestRequest request)
        {
            var httpContext = DependencyResolver.Current.GetService<HttpContextBase>();
            
            if (httpContext == null || httpContext.Handler == null || httpContext.Session == null) return;

            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
        
            var ipAddress = Utils.GetCurrentIpAddress();
            request.AddHeader("IpAddress", ipAddress);

            var coo = ""; // TODO: based on IP get the country code of the user
            request.AddHeader("coo", coo);

            var browserInfo = Utils.GetBrowserInfo();
            if (browserInfo != null)
            {
                var channel = browserInfo.IsMobileDevice ? "mobile" : "web";
                request.AddHeader("Channel", channel);
                request.AddHeader("BrowserInfo", browserInfo.Name + " - " + browserInfo.Version + ":" + browserInfo.Platform);
            }

            if (httpContext.Request.UrlReferrer != null)
                request.AddHeader("UrlReferrer", httpContext.Request.UrlReferrer.ToString());

            

            //by default, sending Direct:0 as referral Type
            //TODO: populate this value by catpturing teh approrpiate value from query string
            request.AddHeader("ReferralType", "0");

            if (httpContext.Session[Constants.SESSION_USERID] != null)
                request.AddHeader("UserId", httpContext.Session[Constants.SESSION_USERID].ToString());

            if (httpContext.Session[Constants.SESSION_COMPANYID] != null)
                request.AddHeader("CompanyId", httpContext.Session[Constants.SESSION_COMPANYID].ToString());

            if (httpContext.Session[Constants.SESSION_ISGHOSTLOGIN] != null)         
                request.AddHeader("IsGhostLogin", httpContext.Session[Constants.SESSION_ISGHOSTLOGIN].ToString());
            if (httpContext.Session[Constants.SESSION_ADMINUSER] != null)
                request.AddHeader("AdminUserName", httpContext.Session[Constants.SESSION_ADMINUSER].ToString());
           

            var configModel = (ConfigModel)httpContext.Session[Constants.SESSION_CONFIG_SETTINGS];

            if (configModel == null) {

                return;
            }

            //var defaultSetting = (DefaultSettingModel)httpContext.Session[Constants.SESSION_DEFAULT_SETTINGS];
            //if (defaultSetting == null || string.IsNullOrEmpty(defaultSetting.Currency)) return;
            var currencyCode = "";
            if (httpContext.Request.Cookies[Constants.COOKIE_CURRENCY] != null)
            {
                currencyCode = httpContext.Request.Cookies[Constants.COOKIE_CURRENCY].Value;
            }
            else
            {
                currencyCode = configModel.RegionalSettings.DefaultCurrencyCode;
            }
            var LangCulture = "";
            if (httpContext.Request.Cookies[Constants.COOKIE_LANGCULTURE] != null)
            {
                LangCulture = httpContext.Request.Cookies[Constants.COOKIE_LANGCULTURE].Value;
            }
            else
            {
                LangCulture = configModel.RegionalSettings.DefaultLanguageCulture;
            }
            request.AddHeader("Currency", currencyCode);
            request.AddHeader("Language", LangCulture);
            request.AddHeader("Country", configModel.RegionalSettings.DefaultCountry);
        }

        public  ResponseModel<T> FetchFromCacheOrApi<T>(string cacheKey, string apiUrl, string requestJson = "", Method method = Method.GET, string paramName = "application/json", ParameterType parameterType = ParameterType.RequestBody, string contentType = "application/json", string apiBaseUrl = null)
        {
            var data = CacheManager.Get<ResponseModel<T>>(cacheKey);
            if (data != null && ConfigKeys.EnableCaching == true)
            {
                //this is doen because at times, an error is thrown and the error is cached fro the respective key.
                // so, if an error is found in the message, the API call si triggered else teh data is returned from the cache
                if (data.Message == null)  { SetContentSnippets(data); return data; }
                if (!data.Message.Contains("error")) { SetContentSnippets(data); return data; }
            }
            var response = CallApi<T>(apiUrl, requestJson, method,paramName, parameterType, contentType,apiBaseUrl: apiBaseUrl);
            data = response;
            if (data?.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //this is doen because at times, an error is thrown and the error is cached fro the respective key.
                // so, if an error is found in the message, the data is NOT cached by the key
                CacheManager.Set(cacheKey, data);
                SetContentSnippets(data);
            }

            return data;
        }

        protected async Task< ResponseModel<T>> FetchFromCacheOrApiAsync<T>(string cacheKey, string apiUrl, string requestJson = "", Method method = Method.GET, string paramName = "application/json", ParameterType parameterType = ParameterType.RequestBody, string contentType = "application/json")
        {
            var data = CacheManager.Get<ResponseModel<T>>(cacheKey);
            if (data != null && ConfigKeys.EnableCaching == true)
            {
                //this is doen because at times, an error is thrown and the error is cached fro the respective key.
                // so, if an error is found in the message, the API call si triggered else teh data is returned from the cache
                if (data.Message == null) { SetContentSnippets(data);  return data; }
                if (!data.Message.Contains("error")) { SetContentSnippets(data); return data; }
            }
            var task = await CallApiAsync<T>(apiUrl, requestJson, method, paramName, parameterType, contentType);
            
            if (task.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //this is doen because at times, an error is thrown and the error is cached fro the respective key.
                // so, if an error is found in the message, the data is NOT cached by the key
                CacheManager.Set(cacheKey, task);
                SetContentSnippets(task);
            }

            return task;
        }
    }
}
