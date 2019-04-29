using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
    public class AddSubscriptionToBasket
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public Guid SubscriptionTermId { get; set; }
        public UserPricingType UserPricingType { get; set; }
    }
}
