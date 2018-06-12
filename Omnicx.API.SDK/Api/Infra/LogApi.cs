using Newtonsoft.Json;

using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Models.Infrastructure;
using RestSharp;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public class LogApi:ApiBase, ILogApi
    {
        public ResponseModel<BoolResponse> InsertLog(LogEntry log)
        {
            return CallApi<BoolResponse>(ApiUrls.InsertLog, JsonConvert.SerializeObject(log), Method.POST);
        }
    }
}
