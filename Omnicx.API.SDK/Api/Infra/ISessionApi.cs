using System.Threading.Tasks;
using Omnicx.API.SDK.Models.Common;
using SessionInfo = Omnicx.API.SDK.Models.Common.SessionInfo;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public interface ISessionApi
    {
        ResponseModel<string>  CreateUserSession(SessionInfo session);
        Task<ResponseModel<string>> CreateUserSessionAsync(SessionInfo session);
        void UpdateUserSession(SessionUpdateModel session);
    }
}