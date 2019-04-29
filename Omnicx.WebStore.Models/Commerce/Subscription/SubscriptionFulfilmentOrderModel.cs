using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
    public class SubscriptionFulfilmentOrderModel
    {
        public SubscriptionSeedOrderModel SeedOrderDetail { get; set; }

        /// <summary>
        /// Contains the schedule for which we are generating fulfilment order. 
        /// </summary>
        public List<SubscriptionOrderScheduleModel> OrderSchedule { get; set; }


        /// <summary>
        /// User for which we are creating fulfillment order. 
        /// </summary>
        public Guid UserId { get; set; }
    }
}
