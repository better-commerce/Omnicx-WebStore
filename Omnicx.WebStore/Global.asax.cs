using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using Omnicx.WebStore.Models.Keys;
using Omnicx.API.SDK.Api.Infra;
using System.Web.Helpers;

namespace Omnicx.WebStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (!string.IsNullOrEmpty(ConfigKeys.AppInsightKey))
                TelemetryConfiguration.Active.InstrumentationKey = ConfigKeys.AppInsightKey;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (!arg.Equals("Session", StringComparison.InvariantCultureIgnoreCase))
                return base.GetVaryByCustomString(context, arg);
            var sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            var user = sessionContext.CurrentUser;
            return string.Format("{0}@{1}@{2}@{3}@{4}@{5}", sessionContext.CurrencyCode, !string.IsNullOrWhiteSpace(sessionContext.LangCulture) ? sessionContext.LangCulture : sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture, context.Request?.Browser?.IsMobileDevice, context.Request?.Browser?.Crawler, user?.UserId ?? Guid.Empty, user?.CompanyId ?? string.Empty);
        }
    }
}
