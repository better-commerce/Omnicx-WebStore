namespace Omnicx.API.SDK.Models.Common
{
    public interface IPluginSettings
    {
        bool IsEnabled { get; set; }
    }
      

    public class PaymentSettings : IPluginSettings
    {
        public bool IsEnabled { get; set; }
        public bool UseSandbox { get; set; }
        public bool EnableImmediateCapture { get; set; }
        public string UserName { get; set; }
        public string Signature { get; set; }
        public string Password { get; set; }
        public string RefundPassword { get; set; }
        public string AccountCode { get; set; }
        public string TestUrl { get; set; }
        public string ProductionUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Version { get; set; }

        public string RecuringUserName { get; set; }
        public string RecuringAccountCode { get; set; }
        public string RecuringPassword { get; set; }
        public string RecuringReturnUrl { get; set; }
        public string RecuringCancelUrl { get; set; }
    }

    

   
    public class SitePluginSettingsModel : IPluginSettings
    {
        public bool IsEnabled { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
   

    
}