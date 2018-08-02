using System.Collections.Generic;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Models
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