using System.Threading.Tasks;
using Newtonsoft.Json;
using Omnicx.API.SDK.Models.Common;
using RestSharp;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public class SessionApi : ApiBase, ISessionApi
    {
        public ResponseModel<string> CreateUserSession(SessionInfo session)
        {
            var sessionId = CallApi<string>(ApiUrls.CreateSession, JsonConvert.SerializeObject(session), Method.POST);
            return sessionId;
        }

        public async Task<ResponseModel<string>> CreateUserSessionAsync(SessionInfo session)
        {
            var sessionId = await CallApiAsync<string>(ApiUrls.CreateSession, JsonConvert.SerializeObject(session), Method.POST);
            return sessionId;
        }
        public void UpdateUserSession(SessionUpdateModel session)
        {
            CallApi<string>(ApiUrls.UpdateSession, JsonConvert.SerializeObject(session), Method.POST);
        }
    }
}