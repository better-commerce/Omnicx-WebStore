using System;
using System.Threading.Tasks;
using System.Web;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;

using Omnicx.API.SDK.Helpers;
using Omnicx.WebStore.Core.Services.Authentication;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Infrastructure;

namespace Omnicx.WebStore.Core.Services.Infrastructure
{
    public class SessionContext : ISessionContext
    {
        private readonly HttpContextBase _httpContext;
        private CustomerModel _cachedUser;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionApi _sessionRepository;
        private readonly IConfigApi _configRepository;
        private ConfigModel _configModel;

        //public const string ConfigType = "DomainSettings";
        //public const string CurrencyKey = "DomainSettings-DefaultCurrencyCode";
        //public const string LanguageKey = "DomainSettings-DefaultLanguageCulture";
        //public const string CountryKey = "DomainSettings-CountryOfOrigin";


        public SessionContext(HttpContextBase httpContext, IAuthenticationService authenticationService,
            ISessionApi sessionRepository, IConfigApi configRepository)
        {
            _httpContext = httpContext;
            _authenticationService = authenticationService;
            _sessionRepository = sessionRepository;
            _configRepository = configRepository;
        }

        public CustomerModel CurrentUser
        {
            get
            {
                CustomerModel user = null;
                //check if the user model already stored in session
                if (_httpContext != null && _httpContext.Session != null)
                {
                    if (_httpContext?.Session[Constants.SESSION_CACHED_USER] != null)
                        _cachedUser = (CustomerModel)_httpContext?.Session[Constants.SESSION_CACHED_USER];

                    if (_cachedUser != null)
                    {
                        _httpContext.Session[Constants.SESSION_USERID] = _cachedUser.UserId;
                        _httpContext.Session[Constants.SESSION_COMPANYID] = _cachedUser.CompanyId;
                        _httpContext.Session[Constants.SESSION_COMPANYUSERROLE] = _cachedUser.CompanyUserRole;
                        return _cachedUser;
                    }
                    user = null ?? _authenticationService.GetAuthenticatedUser();
                    //registered user
                    if (user != null)
                    {
                        _httpContext.Session[Constants.SESSION_USERID] = user.UserId;
                        _httpContext.Session[Constants.SESSION_COMPANYID] = user.CompanyId;
                        if (!Enum.IsDefined(typeof(CompanyUserRole), user.CompanyUserRole)) //Added check for Enum null
                        _httpContext.Session[Constants.SESSION_COMPANYUSERROLE] = (CompanyUserRole)user.CompanyUserRole.GetHashCode();

                        //stored the user object in session. 
                        _httpContext.Session[Constants.SESSION_CACHED_USER] = user;
                    }

                }
                return user;
            }
            set
            {
                //this is NOT needed anymore as we are already removing cookies in the subsequent call - SetUserSession with resetSession = true
                //SetUserCookie(value.UserId);
                _cachedUser = value;
            }
        }

        //protected virtual void SetUserCookie(Guid userGuid)
        //{
        //    if (_httpContext == null || _httpContext.Response == null) return;

        //    var cookie = new HttpCookie(UserCookieName)
        //    {
        //        HttpOnly = true,
        //        Value = userGuid.ToString(),
        //        //if user Guid is empty, expire the cookie immediately, else extend it as per configured duration
        //        Expires = userGuid == Guid.Empty ? DateTime.Now.AddMonths(-1) : DateTime.Now.AddHours(CookieExpires)
        //    };
        //    if (_httpContext.Response.Cookies[UserCookieName] != null)
        //    {
        //        _httpContext.Response.Cookies[UserCookieName].Value = userGuid.ToString();
        //        if (userGuid == Guid.Empty)
        //            _httpContext.Response.Cookies.Add(cookie);

        //    }
        //    else
        //    {
        //        _httpContext.Response.Cookies.Add(cookie);
        //    }


        //}
        /// <summary>
        /// Creates a new session for a user and also reads & creates the required cookies. Makes an API call, creates a new user session record with deviceId, UserId, SessionId
        /// Triggers: Session_Start, Logout
        /// In case, its triggered from Logout, the userId & sessionId are reset and a new Session is generated - however device Id is retained. 
        /// </summary>
        /// <param name="resetSession">passed as true at the time of Logout</param>
        /// <returns></returns>
        public async Task<string> CreateUserSession(bool resetSession = false)
        {
            if (_httpContext == null || _httpContext.Response == null) return "";

            var session = new SessionInfo
            {
                IpAddress = Utils.GetCurrentIpAddress(),
                Browser = Utils.GetBrowserInfo(),
                Referrer = Utils.GetReferrer(),
                Utm = Utils.GetUtm()
            };
            if (CurrentUser != null && CurrentUser.UserId!=null)
                session.CustomerId = CurrentUser.UserId.ToString();

            if (_httpContext.Request.Cookies[Constants.COOKIE_DEVICEID] != null)
                session.DeviceId = _httpContext.Request.Cookies[Constants.COOKIE_DEVICEID].Value;
            else // if deviceId Cookie does not exist, create a new deviceID
                session.DeviceId = Guid.NewGuid().ToString();
           


            //if (_httpContext.Request.Cookies[Constants.COOKIE_USERID] != null)
            //    session.CustomerId = _httpContext.Request.Cookies[Constants.COOKIE_USERID].Value;
            if (_httpContext.Request.Cookies[Constants.COOKIE_SESSIONID] != null && resetSession==false)
                session.SessionId = _httpContext.Request.Cookies[Constants.COOKIE_SESSIONID].Value;
            if (string.IsNullOrEmpty(session.SessionId))
            {
                var cookie_basketId = new HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = "", Expires = DateTime.Now.AddDays(-1) };
                _httpContext.Response.Cookies.Add(cookie_basketId);

                var response = await _sessionRepository.CreateUserSessionAsync(session);
                session.SessionId = response.Result;
            }

            //if (resetSession)
            //{
            //    // session.SessionId = "";
            //    session.CustomerId = "";
            //}


            //var cookie_userId = new HttpCookie(Constants.COOKIE_USERID){HttpOnly = true,Value = session.CustomerId,Expires = DateTime.Now.AddDays(Constants.COOKIE_USERID_EXPIRES_DAYS)};
            var cookie_deviceId = new HttpCookie(Constants.COOKIE_DEVICEID) { HttpOnly = true, Value = session.DeviceId, Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
            var cookie_sessionId = new HttpCookie(Constants.COOKIE_SESSIONID) { Value = session.SessionId, Expires = DateTime.Now.AddMinutes(Constants.COOKIE_SESSIONID_EXPIRES_MINUTES) };

            //_httpContext.Response.Cookies.Add(cookie_userId);          
            _httpContext.Response.Cookies.Add(cookie_deviceId);
            _httpContext.Response.Cookies.Add(cookie_sessionId);
            return session.SessionId;
        }
        public string SessionId
        {
            get
            {
                if (_httpContext == null || _httpContext.Response == null) return "";
                if (_httpContext.Request.Cookies[Constants.COOKIE_SESSIONID] != null)
                    return _httpContext.Request.Cookies[Constants.COOKIE_SESSIONID].Value;

                //if sessionId cookie does NOT exist, just create a NEW session
                return CreateUserSession().Result;

            }
        }
        
        public ConfigModel CurrentSiteConfig
        {
            get {
                //1. get it from session
                _configModel = (ConfigModel)HttpContext.Current.Session[Constants.SESSION_CONFIG_SETTINGS];
                if (_configModel != null) return _configModel;

                //2. Not avaialble in session, fetch it from REPO
                _configModel = _configRepository.GetConfig().Result;

                //3. set it in session
                HttpContext.Current.Session[Constants.SESSION_CONFIG_SETTINGS] = _configModel;

                //4. Return
                return _configModel;

            } set {
                _configModel = value;
            }
        }
      


        public string IpAddress
        {
            get
            {
                string ipAddress = Utils.GetCurrentIpAddress();
                HttpContext.Current.Session[Constants.SESSION_IPADDRESS] = ipAddress;
                return ipAddress;
            }
        }

        public CompanyUserRole CurrentUserRole
        {
            get
            {
                return (CurrentUser==null) ? CompanyUserRole.None : CurrentUser.CompanyUserRole ;
            }
        }
        
    }
}
