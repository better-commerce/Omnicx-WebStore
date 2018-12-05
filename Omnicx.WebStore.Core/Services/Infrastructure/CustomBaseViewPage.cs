using Microsoft.Practices.Unity;
using System;
using System.Globalization;
using System.Web.Mvc;
using Omnicx.WebStore.Framework.Helpers;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Api.Site;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models;

namespace Omnicx.WebStore.Core.Services.Infrastructure
{
    public abstract partial class CustomBaseViewPage<T> : Omnicx.WebStore.Framework.BaseClasses.BaseViewPage<T>
    {
        private ISessionContext _sessionContext;
        [Dependency]
        public ISessionContext SessionContext {
            get {
               return _sessionContext ?? DependencyResolver.Current.GetService<ISessionContext>();
            }
            set {
                _sessionContext = value;
            }
        }

        public NavigationModel SiteNavigationModel
        {
            get
            {
                var _contentApi = DependencyResolver.Current.GetService<IContentApi>();

                var nav = System.Web.HttpContext.Current.Items[Constants.HTTP_CONTEXT_ITEM_SITENAV];
                if (nav == null)
                {
                    nav = _contentApi.GetMenuDetails();
                }
                return ((ResponseModel<NavigationModel>)nav).Result;

            }
        }
        public CurrencySettingModel SiteCurrenciesModel
        {
            get
            {
                var _configApi = DependencyResolver.Current.GetService<IConfigApi>();
                var result = _configApi.GetConfig();
                var data = result?.Result;
                var model = new CurrencySettingModel
                {
                    countries = data?.ShippingCountries,
                    currencies = data?.Currencies,
                    languages = data?.Languages
                };
                return model;

            }
        }
        private TextLocalizer _textLocalizer;
        private DateLocalizer _dateLocalizer;

        #region Localizer Methods

        public new TextLocalizer LT
        {
            get
            {
                return _textLocalizer ?? (_textLocalizer = (resKey, defaultText, langCulture) =>
                {
                     langCulture = string.IsNullOrEmpty(langCulture)
                      ? SessionContext.CurrentSiteConfig.RegionalSettings.DefaultLanguageCulture
                      : langCulture;
                    return base.LT(resKey, defaultText, langCulture);
                });
            }
        }

        public new DateLocalizer Dt
        {
            get
            {
                return _dateLocalizer ?? (_dateLocalizer = (dateVal, isShortDate, displayTime, culture) =>
                {
                    var cult = new CultureInfo(culture);
                    string resValue = "";
                    if (isShortDate && displayTime)
                        resValue = String.Format(cult, "{0:g}", dateVal);
                    if (isShortDate && (displayTime == false))
                        resValue = String.Format(cult, "{0:d}", dateVal);
                    if ((isShortDate == false) && displayTime)
                        resValue = String.Format(cult, "{0:f}", dateVal);
                    if ((isShortDate == false) && (displayTime == false))
                        resValue = String.Format(cult, "{0:D}", dateVal);
                    /*
                        t	ShortTimePattern
                        d	ShortDatePattern
                        T	LongTimePattern
                        D	LongDatePattern
                        f	(combination of D and t)
                        F	FullDateTimePattern
                        g	(combination of d and t)
                        G	(combination of d and T)
                        m, M	MonthDayPattern
                        y, Y	YearMonthPattern
                        r, R	RFC1123Pattern
                        s	SortableDateTi­mePattern
                        u	UniversalSorta­bleDateTimePat­tern
                    */
                    return resValue;
                });
            }
        }
        #endregion

        public override void Execute()
        {

        }
    }
}