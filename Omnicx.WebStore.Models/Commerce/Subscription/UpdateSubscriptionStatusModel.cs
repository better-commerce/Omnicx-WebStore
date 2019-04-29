using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
    public class UpdateSubscriptionStatusModel
    {
        /// <summary>
        /// SeedOrderId for which the subscription order is placed. 
        /// Identifier the unique subscription order. 
        /// </summary>
        public Guid SeedOrderId { get; set; }
        /// <summary>
        /// Status of subscription 
        /// </summary>
        public SubscriptionStatus Status { get; set; }
        /// <summary>
        /// Status of Subscription recurring Order 
        /// </summary>
        public SubscriptionRecurringOrderStatus RecurringOrderStatus { get; set; }
        /// <summary>
        /// Duration till when the subscription will pause
        /// </summary>
        public int PauseDuration { get; set; }
        /// <summary>
        /// Auto Renewal for perticular subscription order
        /// </summary>
        public bool AutoRenew { get; set; }
    }
}
