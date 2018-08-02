
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Infrastructure.Settings;
using Omnicx.WebStore.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class PaymentBasicModel
    {
        public int Id { get; set; }

        public string SystemName { get; set; }

        public string DisplayName { get; set; }

        public string Slug { get; set; }

        public bool Enabled { get; set; }
        public IList<KeyValuePair<string, string>> BasicSettings { get; set; }
        public string Description { get; set; }
        public string IconCssClass { get; set; }
        public string NotificationUrl { get; set; }
        public bool IsBillingAddressRequired { get; set; }
        public bool IsImmediateCapture { get; set; }
        public string AccountCode
        {
            get
            {

                return BasicSettings?.FirstOrDefault(x => x.Key == PaymentSettingKey.AccountCode).Value ?? "";
            }
        }
        public string Version
        {
            get
            {

                return BasicSettings?.FirstOrDefault(x => x.Key == PaymentSettingKey.Version).Value ?? "";
            }
        }

        private CardInfoModel _cardInfo;
        public CardInfoModel CardInfo
        {
            get
            {
                if (_cardInfo == null) _cardInfo = new CardInfoModel();
                return _cardInfo;
            }
            set { _cardInfo = value; }
        }

    }

}
