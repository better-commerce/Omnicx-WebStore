using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Entities;

namespace Omnicx.WebStore.Core.Services.Authentication
{
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        #region Fields
        
        private readonly HttpContextBase _httpContext;
        private readonly TimeSpan _expirationTimeSpan;
        private CustomerModel _cachedUser;
        
        private readonly ICustomerApi _customerRepository;
        private readonly ISessionApi _sessionRepository;
        #endregion

        #region Ctor
        public FormsAuthenticationService(HttpContextBase httpContext, ICustomerApi customerRepository, ISessionApi sessionRepository)
        {
            _httpContext = httpContext;
            
            _expirationTimeSpan = FormsAuthentication.Timeout;
            _customerRepository = customerRepository;
            _sessionRepository = sessionRepository;
        }
        #endregion

        public CustomerModel Login(string userName, string password, bool createPersistentCookie = true)
        {
            var response = _customerRepository.Login(userName, password);
            var user = response.Result;

            if (user == null) return null;

            var now = DateTime.UtcNow.ToLocalTime();

             var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var sessionId = sessionContext.SessionId;

            var session = new SessionUpdateModel()
            {
                CustomerId = user.UserId.ToString(),
                SessionId = sessionId
            };
            _sessionRepository.UpdateUserSession(session);
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/, user.UserId.ToString(),
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie, user.UserId.ToString() + "~" + user.Username+"~"+sessionId.ToString(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            //added the following line assuming that this will set IsAuthenticated=true
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            //refer to teh following links, if the above does not works
            //http://stackoverflow.com/questions/1064271/asp-net-mvc-set-custom-iidentity-or-iprincipal
            //http://stackoverflow.com/questions/21679836/custom-identity-using-mvc5-and-owin
            //http://www.windowsdevcenter.com/pub/a/dotnet/2004/02/02/effectiveformsauth.html

            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = user;
            user.SessionId = sessionId.ToString();
            return user;
        }

        public CustomerModel GhostLogin(CustomerModel user)
        {
            
            var now = DateTime.UtcNow.ToLocalTime();
            bool isGhostLogin = true;
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            

            var session = new SessionUpdateModel()
            {
                CustomerId = user.UserId.ToString(),
                SessionId = user.SessionId
            };
            _sessionRepository.UpdateUserSession(session);
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/, user.UserId.ToString(),
                now,
                now.AddMinutes(Constants.COOKIE_SESSIONID_EXPIRES_MINUTES),
                true, user.UserId.ToString() + "~" + user.Username + "~"  + user.SessionId + "~" + isGhostLogin.ToString() + "~"+user.AdminUserName,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            //added the following line assuming that this will set IsAuthenticated=true
            FormsAuthentication.SetAuthCookie(user.Username, true);
            //refer to teh following links, if the above does not works
            //http://stackoverflow.com/questions/1064271/asp-net-mvc-set-custom-iidentity-or-iprincipal
            //http://stackoverflow.com/questions/21679836/custom-identity-using-mvc5-and-owin
            //http://www.windowsdevcenter.com/pub/a/dotnet/2004/02/02/effectiveformsauth.html

            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = user;
            return user;
        }
        public void Logout()
        {
            _cachedUser = null;
            FormsAuthentication.SignOut();
            var httpContext = DependencyResolver.Current.GetService<HttpContextBase>();
            httpContext.Session[Constants.SESSION_USERID] = Guid.Empty;
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            sessionContext.CurrentUser = new CustomerModel { UserId = Guid.Empty };
            sessionContext.CreateUserSession(true);
            httpContext.Session[Constants.SESSION_CACHED_USER] = null;
            httpContext.Session.Abandon();

        }

        public virtual CustomerModel GetAuthenticatedUser()
        {
            try
            {
                if (_cachedUser != null)
                    return _cachedUser;

                if (_httpContext == null ||
                    _httpContext.Request == null ||
                    !_httpContext.Request.IsAuthenticated ||
                    !(_httpContext.User.Identity is FormsIdentity))
                {
                    return null;
                }

                var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
                var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
                if (user != null)
                    _cachedUser = user;
                return _cachedUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual CustomerModel GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var userData = ticket.UserData;
            var usernameOrEmail = ticket.UserData;
            var sessionId = "";
            var userId = "";
            var adminUserName = "";
            bool isGhostLogin = false;
            if (String.IsNullOrWhiteSpace(userData))
                return null;
            if (userData.Split('~').Length == 3)
            {
                userId = userData.Split('~')[0];
                usernameOrEmail = userData.Split('~')[1];
                sessionId = userData.Split('~')[2];
            }
            if (userData.Split('~').Length == 5) {
                userId = userData.Split('~')[0];
                usernameOrEmail = userData.Split('~')[1];
                sessionId = userData.Split('~')[2];
                bool.TryParse(userData.Split('~')[3], out isGhostLogin);
                adminUserName = userData.Split('~')[4];
            }
           // var user = _omnicxRepository.GetUserdetailsByUserName(usernameOrEmail); //_userRepository.GetUserByEmail(usernameOrEmail);
            var response = _customerRepository.GetUserdetailsById<CustomerModel>(userId);   //---in case of (Not working by username/Email)
            var user = response.Result;
            //if the userId (email) is NOT found in the User DB, then sign out the user and return null
            if (user == null)
            {
                FormsAuthentication.SignOut();
                return null;
            }
            
            user.SessionId = sessionId;
            user.AdminUserName = adminUserName;
            user.IsGhostLogin = isGhostLogin;
            return user;
        }

        public CustomerModel SocialLogin(string id)
        {
            var createPersistentCookie = true;
            var response = _customerRepository.GetUserdetailsById<CustomerModel>(id);
            var user = response.Result;
            if (user == null) return null;

            var now = DateTime.UtcNow.ToLocalTime();

            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var sessionId = sessionContext.SessionId;

            var session = new SessionUpdateModel()
            {
                CustomerId = user.UserId.ToString(),
                SessionId = sessionId
            };

            _sessionRepository.UpdateUserSession(session);
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/, user.UserId.ToString(),
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie, user.UserId.ToString() + "~" + user.Username + "~" + sessionId.ToString(),
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            //added the following line assuming that this will set IsAuthenticated=true
            FormsAuthentication.SetAuthCookie(user.Username, createPersistentCookie);
           
            _httpContext.Response.Cookies.Add(cookie);
            _cachedUser = user;
            user.SessionId = sessionId.ToString();
            return user;
        }

        public void UpdateSession(SessionUpdateModel info)
        {
            _sessionRepository.UpdateUserSession(info);
        }
       
    }
}