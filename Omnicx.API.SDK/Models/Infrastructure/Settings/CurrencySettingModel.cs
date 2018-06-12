using System.Collections.Generic;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.API.SDK.Models
{
    /// <summary>
    /// REVIEW & REFACTOR - Vikram - 24Apr2017
    /// </summary>
    public class CurrencySettingModel
    {
        public List<CountryModel> countries { get; set; }
        public List<CurrencyModel> currencies { get; set; }
        public List<LanguageModel> languages { get; set; }
    }
}