using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;

namespace Omnicx.WebStore.Core.Services.Log
{
    public partial interface ILogService
    {
        BoolResponse InsertLog(LogLevel logLevel, string shortMessage, string fullMessage, string userName, string userId);

    }
}