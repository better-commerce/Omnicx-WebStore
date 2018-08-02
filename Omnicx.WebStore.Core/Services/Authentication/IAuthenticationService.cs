using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;

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