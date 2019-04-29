using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Helpers
{
    public class OAuthHelper
    {
        private static ObjectCache _tokenCache = MemoryCache.Default;

        public static AccessToken GetAccessToken(string authUrl, string clientId, string clientSecret)
        {
            AccessToken token = null;
            string OAuthCacheKey = "oauth.{0}";
            bool isTokenValid = false;
            if (_tokenCache.Get(String.Format(OAuthCacheKey, clientId)) != null)
            {
                token = (AccessToken)_tokenCache.Get(String.Format(OAuthCacheKey, clientId));
                isTokenValid = (token.Expires > DateTime.UtcNow);
            }
            if (!isTokenValid)
            {
                var authClient = new RestClient(authUrl);
                var autRequest = new RestRequest(Method.POST);
                autRequest.AddParameter("grant_type", "client_credentials");
                autRequest.AddParameter("client_id", clientId);
                autRequest.AddParameter("client_secret", clientSecret);
                //make the API request and get the response
                if (authUrl.ToLower().StartsWith("https"))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                IRestResponse response = authClient.Execute(autRequest);

                ////return an AccessToken
                token = JsonConvert.DeserializeObject<AccessToken>(response.Content);
                if (!string.IsNullOrEmpty(token.Token))
                {
                    token.Expires = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
                    var cahePolicy = new CacheItemPolicy() { AbsoluteExpiration = token.Expires };
                    _tokenCache.Set(String.Format(OAuthCacheKey, clientId), token, cahePolicy);
                }
            }
            return token;
        }

    }
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("issued")]
        public DateTime Issued { get; set; }
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}
