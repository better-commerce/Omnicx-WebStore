using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
    
    public class SubscriptionUserSetting
    {
        public Guid SubscriptionPlanId { get; set; }
        public Guid SubscriptionTermId { get; set; }
        public UserPricingType UserPricingType { get; set; }
        public string SubscriptionJson { get; set; }
    }
}
