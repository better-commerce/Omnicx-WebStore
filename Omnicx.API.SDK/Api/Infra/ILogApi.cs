using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Models.Infrastructure;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public interface  ILogApi
    {
        ResponseModel<BoolResponse>  InsertLog(LogEntry log);
    }
}
