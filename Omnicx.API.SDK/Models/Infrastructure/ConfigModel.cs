using System.Collections.Generic;
using Omnicx.API.SDK.Models.Common;
using System;
using Omnicx.API.SDK.Models.Infrastructure.Settings;

namespace Omnicx.API.SDK.Models.Infrastructure
{
    [Serializable]
    public class ConfigModel
    {
        public ConfigModel()
        {
            /// all the objects in the class have been initialized with an empty object to avoid 
            /// throwing Object reference error even in all circumstances. 
            /// these objects SHOULD be initialized with some default values - possibly from web.config for worst case scenarios.
            
            ConfigSettings = new List<ConfigSettingModel>();
            BasketSettings = new BasketSettings();
            B2BSettings = new B2BSettings();
            CatalogSettings = new CatalogSettings();
            DomainSettings = new DomainSettings();
            OrderSettings = new OrderSettings();
            SearchSettings = new SearchSettings();
            SeoSettings = new SeoSettings();
            ShippingSettings = new ShippingSettings();
            SocialSettings = new SocialSettings();
            Currencies = new List<CurrencyModel>();
            ShippingCountries = new List<CountryModel>();
            BillingCountries = new List<CountryModel>();
            Languages = new List<LanguageModel>();
            RegionalSettings = new RegionalSettings();
            ReviewSettings = new List<ProductReviewSection>();
            GeoLocators = new List<GeoLocatorModel>();
        }
        public List<ConfigSettingModel> ConfigSettings { get; set; }

        public RegionalSettings RegionalSettings { get; set; }
        public BasketSettings BasketSettings { get; set; }
        public B2BSettings B2BSettings { get; set; }
        public CatalogSettings CatalogSettings { get; set; }
        public DomainSettings DomainSettings { get; set; }
        public OrderSettings OrderSettings { get; set; }
        public SearchSettings SearchSettings { get; set; }
        public SeoSettings SeoSettings { get; set; }
        public ShippingSettings ShippingSettings { get; set; }
        public SocialSettings SocialSettings { get; set; }

        public List<ProductReviewSection> ReviewSettings { get; set; }

        public List<CurrencyModel> Currencies { get; set; }
        public List<CountryModel> ShippingCountries { get; set; }
        public List<CountryModel> BillingCountries { get; set; }
        public List<LanguageModel> Languages{ get; set; }
        public List<GeoLocatorModel> GeoLocators { get; set; }
    }
    [Serializable]
    public class ConfigKeyModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class ConfigSettingModel
    {
        public string ConfigType { get; set; }
        public List<ConfigKeyModel> ConfigKeys { get; set; }
    }
    [Serializable]
    public class GeoLocatorModel
    {
        public string PluginCode { get; set; }
        public string AccessKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public bool EnableDeliverySlot { get; set; }
    }
}