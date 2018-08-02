namespace Omnicx.WebStore.Models.Common
{
    //[Serializable]
    //public class DefaultSettingModel
    //{
    //    private string _language;
    //    private string _currency;
    //    private string _country;
    //    private string _currencySymbol;
    //    private bool _displayPriceWithoutTax;

    //    /// <summary>
    //    /// The reason a null check condition is implemented for setting the values is
    //    /// because in some places only partial values are passed in the whole model and the whole model 
    //    /// is simply updated in the sessionContext => which means, 
    //    /// certain values can be updated to blank "inadvertently". 
    //    /// Hence this safety check has been put in place for all the properties.
    //    /// </summary>
    //    public string Language
    //    {
    //        get { return _language; }
    //        set { if (string.IsNullOrEmpty(value)) { _language = value; } }
    //    }
    //    public string Currency
    //    {
    //        get { return _currency; }
    //        set { if (string.IsNullOrEmpty(value)) { _currency = value; } }
    //    }
    //    public string Country
    //    {
    //        get { return _country; }
    //        set { if (string.IsNullOrEmpty(value)) { _country = value; } }
    //    }
    //    public string CurrencySymbol
    //    {
    //        get { return _currencySymbol; }
    //        set { if (string.IsNullOrEmpty(value)) { _currencySymbol = value; } }
    //    }
    //    /// <summary>
    //    /// The default value of this property woudl be "false"
    //    /// which means, by default we'd be displaying the price inclusive of Tax amount.
    //    /// A user can however set it differently. 
    //    /// </summary>
    //    bool DisplayPriceWithoutTax
    //    {
    //        get { return _displayPriceWithoutTax; }
    //        set { _displayPriceWithoutTax = value; }
    //    }
    //    //public SocialSettings SocialSettings { get; set; }
    //    //public SocialConfig SocialConfig { get; set; }

    //}

    //public class SocialSettings
    //{
    //    public bool IsTwitterEnabled { get; set; }
    //    public bool IsFacebookEnabled { get; set; }
    //    public bool IsGooglePlusEnabled { get; set; }
    //}

    //public class SocialConfig
    //{
    //    public TwitterConfig twitterConfig { get; set; }
    //    public FacebookConfig facebookConfig { get; set; }
    //    public GooglePlusConfig googlePlusConfig { get; set; }
    //}

    //public class TwitterConfig
    //{
    //    public string ApiKey { get; set; }
    //    public string ApiSecret { get; set; }
    //    public string Url { get; set; }
    //}
    //public class FacebookConfig
    //{
    //    public string ApiKey { get; set; }
    //    public string ApiSecret { get; set; }
    //    public string Url { get; set; }
    //}
    //public class GooglePlusConfig
    //{
    //    public string ApiKey { get; set; }
    //    public string ApiSecret { get; set; }
    //    public string Url { get; set; }
    //}
}
