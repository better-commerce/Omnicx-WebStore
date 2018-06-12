using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Entities;

namespace Omnicx.WebStore.Core.Services.Log
{
    public partial interface ILogService
    {
        BoolResponse InsertLog(LogLevel logLevel, string shortMessage, string fullMessage, string userName, string userId);

    }
}