using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Api.Site;

using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Site;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using System;

namespace Omnicx.WebStore.Core.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfigApi _configApi;
        private readonly IContentApi _contentApi;

        private ResponseModel<NavigationModel> NavApiResponse
        {
            get
            {
                var nav = HttpContext.Items[Constants.HTTP_CONTEXT_ITEM_SITENAV];
                if (nav == null) nav = _contentApi.GetMenuDetails();
                return (ResponseModel<NavigationModel>)nav;
            }
        }
        public HomeController(IConfigApi configApi,
            IContentApi contentApi)
        {
            _configApi = configApi;
            _contentApi = contentApi;
            SetDataLayerVariables("", WebhookEventTypes.PageViewed);
        }

        public ActionResult GetFooter()
        {
            var model = NavApiResponse;
            return PartialView(CustomViews.MAIN_FOOTER, model.Result);
        }
        public ActionResult GetFooterMobile()
        {
            var model = NavApiResponse;
            return PartialView(CustomViews.MAIN_FOOTER_MOBILE, model.Result);
        }
        public ActionResult GetNav()
        {
            var model = NavApiResponse;
            return PartialView(CustomViews.MAIN_MENU, model.Result);
        }
        public ActionResult GetMobileNav()
        {
            var model = NavApiResponse;
            return PartialView(CustomViews.MAIN_MENU_MOBILE, model.Result);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexV2()
        {
            return View();
        }
        public ActionResult CurrencySettings()
        {
            var result = _configApi.GetConfig();
            var data = result?.Result;
            var model = new CurrencySettingModel
            {
                countries = data?.ShippingCountries,
                currencies = data?.Currencies,
                languages = data?.Languages
            };
            return JsonSuccess(model, JsonRequestBehavior.AllowGet);
        }

        public bool UpdateCurrencySetting(string currency)
        {
            if (Request.Cookies[Constants.COOKIE_CURRENCY] != null && Request.Cookies[Constants.COOKIE_CURRENCY].Value == currency)
            {
                currency = Request.Cookies[Constants.COOKIE_CURRENCY].Value;
            }
            else
            {
                Response.Cookies[Constants.COOKIE_CURRENCY].Value = currency;
                Response.Cookies[Constants.COOKIE_CURRENCY].Expires = DateTime.Today.AddDays(1); // add expiry time
            }
            return true;
        }

        public bool UpdateLanCultureSetting(string language)
        {
            if (Request.Cookies[Constants.COOKIE_LANGCULTURE] != null && Request.Cookies[Constants.COOKIE_LANGCULTURE].Value == language)
            {
                language = Request.Cookies[Constants.COOKIE_LANGCULTURE].Value;
            }
            else
            {
                Response.Cookies[Constants.COOKIE_LANGCULTURE].Value = language;
                Response.Cookies[Constants.COOKIE_LANGCULTURE].Expires = DateTime.Today.AddDays(1); // add expiry time
            }
            return true;
        }
        //public bool UpdateCurrencySetting(DefaultSettingModel defaultSetting)
        //{
        //    _sessionContext.DefaultSetting = defaultSetting;
        //    return true;
        //}

        [ChildActionOnly]
        public ActionResult SiteLogo()
        {
            var logoUrl = _sessionContext.CurrentSiteConfig.DomainSettings.LogoUrl;
            ViewBag.logoUrl = logoUrl;
            return PartialView(CustomViews.SITE_LOGO);
        }
        [ChildActionOnly]
        public ActionResult HeaderLoginInfo()
        {           
            return PartialView(CustomViews.HEADER_LOGIN_INFO);
        }
        [ChildActionOnly]
        public ActionResult DynamicHeadTag()
        {
            return PartialView(CustomViews.HEADER_DYNAMIC_HEAD_TAG);
        }
        public ActionResult IndexV3()
        {
            return View();
        }

        public ActionResult IndexV4()
        {
            return View();
        }
    }
}