using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Infrastructure;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public interface  ILogApi
    {
        ResponseModel<BoolResponse>  InsertLog(LogEntry log);
    }
}
