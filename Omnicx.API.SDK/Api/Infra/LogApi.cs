using Newtonsoft.Json;

using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Infrastructure;
using RestSharp;
using Omnicx.WebStore.Models;
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
