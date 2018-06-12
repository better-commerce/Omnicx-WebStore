using System.Collections.Generic;
using System.Linq;
using Omnicx.API.SDK.Models.Helpers;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class PaymentMethodModel
    {
        public int Id { get; set; }

        public string SystemName { get; set; }

        public string DisplayName { get; set; }

        public string Slug { get; set; }

        public bool Enabled { get; set; }

        public string Description { get; set; }

        public string IconCssClass { get; set; }
        public string NotificationUrl { get; set; }
        public IList<KeyValuePair<string, string>> Settings { get; set; }
        public bool IsBillingAddressRequired { get; set; }
        public  bool IsImmediateCapture { get; set; }
        public string UserName {
            get
            {
                return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.UserName).Value ?? "";
              
            }
        }
        public string Password
        {
            get
            {
                
              return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.Password).Value ?? "";
            }
        }
        public string AccountCode
        {
            get
            {
                
              return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.AccountCode).Value ?? "";
            }
        }
        public string Signature
        {
            get
            {
                
                return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.Signature).Value ?? "";
            }
        }
        public string Version
        {
            get
            {
                
              return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.Version).Value ?? "";
            }
        }
        public string UseSandbox
        {
            get
            {
                
               return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.UseSandbox).Value ?? "";
            }
        }
        public string TestUrl
        {
            get
            {
                
                return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.TestUrl).Value ?? "";
            }
        }
        public string ProductionUrl
        {
            get
            {
                
                return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.ProductionUrl).Value ?? "";
            }
        }
        public string CancelUrl
        {
            get
            {
                
                return Settings?.FirstOrDefault(x => x.Key == PaymentSettingKey.CancelUrl).Value ?? "";
            }
        }

        private CardInfoModel _cardInfo;
        public CardInfoModel CardInfo
        {
            get
            {
                if (_cardInfo == null)  _cardInfo=new CardInfoModel();
                return _cardInfo;
            }
            set { _cardInfo = value; }
        }
    }

    public class CardInfoModel
    {
        public string CardNo { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
    }
}