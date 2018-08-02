using System.Threading.Tasks;
using Omnicx.WebStore.Models.Common;
using SessionInfo = Omnicx.WebStore.Models.Common.SessionInfo;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Infra
{
    public interface ISessionApi
    {
        ResponseModel<string>  CreateUserSession(SessionInfo session);
        Task<ResponseModel<string>> CreateUserSessionAsync(SessionInfo session);
        void UpdateUserSession(SessionUpdateModel session);
    }
}