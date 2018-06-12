using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Models.Common;
using System;
using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Infrastructure.Settings
{
    [Serializable]
   public class RegionalSettings : ISettings
    {
        private string _defaultLanguageCulture;
        private string _defaultLanguageCode;
        private string _defaultCurrencyCode;
        private string _defaultCurrencySymbol;
        private string _defaultCountryCode;
        private string _currencyDecimalSeparator;
        private int _currencyDigitsAfterDecimal;
        public string DefaultLanguageCode { get
            {
                //if value is NULL, simply return "en". This is hardcoded for worse case scenario when value is NOT populated
                return _defaultLanguageCode ?? "en";
            }
            set { _defaultLanguageCode = value; } }
        /// <summary>
        /// Default language culture for the domain
        /// </summary>
        public string DefaultLanguageCulture
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultLanguageCulture)) _defaultLanguageCulture = this.DefaultLanguageCode;

                //if its empty or its just 2 letters (lang code ONLY)
                if (!string.IsNullOrEmpty(_defaultLanguageCulture))
                {
                    if (_defaultLanguageCulture.Length == 2)
                    {
                        _defaultLanguageCulture = _defaultLanguageCulture.ToLower() + "-" + DefaultCountry.ToUpper();
                    }
                }
                return _defaultLanguageCulture;
            }
            set
            {
                _defaultLanguageCulture = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the default Language Culture
        /// </summary>
        public bool EnableLanguageAutoDetect { get; set; }

        /// <summary>
        /// Default country code associated with the domain
        /// </summary>
        //public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Default country code associated with the domain
        /// </summary>
        public string DefaultCountry { get {
                //if value is NULL, simply return "en". This is hardcoded for worse case scenario when value is NOT populated
                return _defaultCountryCode ?? "GB";
            }
            set { _defaultCountryCode = value; } }

        public string DefaultTimeZoneId { get; set; }

        public string DefaultUiDateFormat { get; set; }

        //public bool RoundPricesDuringCalculation { get; set; }
        /// <summary>
        /// Currencies avaialble for the domain
        /// </summary>
        public IList<CurrencyModel> AvailableCurrenciesList
        {
            get
            {
                var currencies = new List<CurrencyModel>();
                if (string.IsNullOrEmpty(AvailableCurrencies)) return currencies;
                foreach (var itm in AvailableCurrencies.JsonToObjectArray())
                {
                    var currency = new CurrencyModel { CurrencyCode = itm };
                    currencies.Add(currency);
                }
                return currencies;

            }
        }
        /// <summary>
        /// Languages allowed for the domain
        /// </summary>
        public IList<LanguageModel> AvailableLanguagesList
        {
            get
            {
                var languages = new List<LanguageModel>();
                if (string.IsNullOrEmpty(AvailableLanguages)) return languages;
                foreach (var itm in AvailableLanguages.JsonToObjectArray())
                {
                    var language = new LanguageModel { LanguageCode = itm };
                    languages.Add(language);
                }
                return languages;
            }
        }


        /// <summary>
        /// Currencies avaialble for the domain
        /// </summary>
        public string AvailableCurrencies { get; set; }
        /// <summary>
        /// Languages allowed for the domain
        /// </summary>
        public string AvailableLanguages { get; set; }


        public string DefaultCurrencyCode {
            get
            {
                //if value is NULL, simply return "GBP". This is hardcoded for worse case scenario when value is NOT populated
                return _defaultCurrencyCode ?? "GBP";
            }
            set
            {
                _defaultCurrencyCode = value;
            }
        }

        public string DefaultCurrencySymbol
        {
            get
            {
                //if value is NULL, simply return "£". This is hardcoded for worse case scenario when value is NOT populated
                return _defaultCurrencySymbol ?? "£";
            }
            set
            {
                _defaultCurrencySymbol = value;
            }
        }
        public string CurrencyDecimalSeparator { get {
                //if value is NULL, simply return ",". This is hardcoded for worse case scenario when value is NOT populated
                return _currencyDecimalSeparator ?? ",";
            }
            set {
                _currencyDecimalSeparator = value;
            } }

        public int CurrencyDigitsAfterDecimal { get {
                //if value is NULL, simply return "2". This is hardcoded for worse case scenario when value is NOT populated
                return _currencyDigitsAfterDecimal;
            } set {
                _currencyDigitsAfterDecimal = value;
            } }
    }
}
