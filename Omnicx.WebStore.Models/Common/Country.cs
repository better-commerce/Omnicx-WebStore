using System;

namespace Omnicx.WebStore.Models.Common
{
    [Serializable]
    public class CountryModel
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
    }
    [Serializable]
    public class CurrencyModel
    {
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
    }
    [Serializable]
    public class LanguageModel
    {
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageCulture { get; set; }
    }
}
