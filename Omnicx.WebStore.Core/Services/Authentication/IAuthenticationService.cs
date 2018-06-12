using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.WebStore.Core.Services.Authentication
{
    public partial interface IAuthenticationService
    {

        CustomerModel Login(string userName, string password, bool createPersistentCookie);
        void Logout();
        CustomerModel GetAuthenticatedUser();

        CustomerModel GhostLogin(CustomerModel user);
        CustomerModel SocialLogin(string id);
        void UpdateSession(SessionUpdateModel info);
    }
}