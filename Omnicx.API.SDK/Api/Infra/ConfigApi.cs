using System.Linq;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Models.Infrastructure;
using Omnicx.API.SDK.Models;
using System.ComponentModel;
using Omnicx.API.SDK.Models.Infrastructure.Settings;
using Omnicx.WebStore.Apps.OAuthHelper;

namespace Omnicx.API.SDK.Api.Infra
{
    public class ConfigApi: ApiBase, IConfigApi
    {
        //public ResponseModel<List<ConfigModel> > GetConfigSettings()
        //{
        //    return FetchFromCacheOrApi<List<ConfigModel>>(CacheKeys.SITE_CONFIG, ApiUrls.GetConfigSettings);
        //}
        public ResponseModel<ConfigModel> GetConfig()
        {
            var responseModel = new ResponseModel<ConfigModel>();
            var configModel = CacheManager.Get<ConfigModel>(CacheKeys.SITE_CONFIG);

            if(configModel != null)
            {
                responseModel.Result = configModel;
                return responseModel;
            }

            var response = CallApi<ConfigModel>(ApiUrls.GetConfigSettings,"");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                configModel = response.Result;
                responseModel.Result= configModel;

                //populate configData by extracting configSettings and populating all teh respective objects. 
                ConvertConfigSettingsToClasses(ref configModel);

                //this is doen because at times, an error is thrown and the error is cached fro the respective key.
                // so, if an error is found in the message, the data is NOT cached by the key
                CacheManager.Set(CacheKeys.SITE_CONFIG, configModel);
            }

            //register the social clients after fetching the details from the API.
            /// this registration is NOT called when the data is fetched from CACHE.
            OAuthClientFactory.RegisterTwitterClient(configModel.SocialSettings.TwitterApiKey, configModel.SocialSettings.TwitterApiSecret, configModel.SocialSettings.TwitterUrl);
            OAuthClientFactory.RegisterGoogleClient(configModel.SocialSettings.GooglePlusApiKey, configModel.SocialSettings.GooglePlusApiSecret, configModel.SocialSettings.GooglePlusUrl);
            OAuthClientFactory.RegisterFacebookClient(configModel.SocialSettings.FacebookApiKey, configModel.SocialSettings.FacebookApiSecret, configModel.SocialSettings.FacebookUrl);

            return responseModel;
            //return FetchFromCacheOrApi<ConfigModel>(CacheKeys.SITE_CONFIG, ApiUrls.GetConfigSettings);

        }
        #region "private helper methods to populate the settinsg classes"
        private void ConvertConfigSettingsToClasses(ref ConfigModel configModel)
        {
            configModel.B2BSettings = LoadSetting<B2BSettings>(configModel.B2BSettings, configModel.ConfigSettings.FirstOrDefault(x=>x.ConfigType.ToLower() == typeof(B2BSettings).Name.ToLower()));
            configModel.BasketSettings = LoadSetting<BasketSettings>(configModel.BasketSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(BasketSettings).Name.ToLower()));
            configModel.CatalogSettings = LoadSetting<CatalogSettings>(configModel.CatalogSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(CatalogSettings).Name.ToLower()));
            configModel.DomainSettings = LoadSetting<DomainSettings>(configModel.DomainSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(DomainSettings).Name.ToLower()));
            configModel.OrderSettings = LoadSetting<OrderSettings>(configModel.OrderSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(OrderSettings).Name.ToLower()));
            configModel.SearchSettings = LoadSetting<SearchSettings>(configModel.SearchSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(SearchSettings).Name.ToLower()));
            configModel.SeoSettings = LoadSetting<SeoSettings>(configModel.SeoSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(SeoSettings).Name.ToLower()));
            configModel.ShippingSettings = LoadSetting<ShippingSettings>(configModel.ShippingSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(ShippingSettings).Name.ToLower()));
            configModel.SocialSettings = LoadSetting<SocialSettings>(configModel.SocialSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(SocialSettings).Name.ToLower()));
            configModel.RegionalSettings= LoadSetting<RegionalSettings>(configModel.RegionalSettings, configModel.ConfigSettings.FirstOrDefault(x => x.ConfigType.ToLower() == typeof(RegionalSettings).Name.ToLower()));
        }
        private T LoadSetting<T>(T settingClass, ConfigSettingModel configSettings)
        {
            //var settings = Activator.CreateInstance<T>();

            var settingsForClass = configSettings?.ConfigKeys;
            /////if prepoulated settings is passed, we dont have to make a DB/Cache call for EVERY setting class
            //if (prepopulatedAllSettings != null && prepopulatedAllSettings.ToList().Count > 0)
            //    _allSettings = prepopulatedAllSettings;

            var settingClassName = settingClass.GetType().Name.Replace("Model", "");

            //foreach (var prop in typeof(T).GetProperties())
            foreach (var prop in settingClass.GetType().GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                //this is done because keys naming convention is classname-property. i.e.DomainSettings-DomainName
                var key = settingClassName + "." + prop.Name;
                var settingKey = settingsForClass.ToList().FirstOrDefault(x => x.Key == key);

                //if teh key does not exist for the domain, move on to the next key
                if (settingKey == null) continue;

                var settingValue = settingKey.Value;
                if (settingValue == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(settingValue))
                    continue;

                object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(settingValue);

                //set property
                prop.SetValue(settingClass, value, null);
            }

            return settingClass;
        }
        #endregion 

    }
}
