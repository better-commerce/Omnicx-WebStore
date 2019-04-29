using Omnicx.API.SDK.Api.Infra;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var _sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            if (_sessionContext.CurrentUser == null || !HttpContext.Current.Request.IsAuthenticated) {
                filterContext.Result = new RedirectResult("~/account/signin");
            }
            else{}
        }
    }
}