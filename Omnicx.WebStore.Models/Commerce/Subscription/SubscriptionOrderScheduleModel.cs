using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
   public class SubscriptionOrderScheduleModel
    {
        public Guid ParentOrderId { get; set; }

        /// <summary>
        /// Foreign Key Reference to the SubscriptionSeedOrder.Id. Reference to the Seed Order
        /// </summary>
        public Guid SeedOrderId { get; set; }

        /// <summary>
        /// OrderId for the order generated against this specific scheduled order.
        /// This field shall remain blank until the order is generated on the scheduled date
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Order No. with reference to OrderId 
        /// </summary>
        public string OrderCustomNo { get; set; }

        public DateTime ScheduledGenerationDate { get; set; }
        public Guid ProductId { get; set; }
        public string StockCode { get; set; }
        public SubscriptionRecurringOrderStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
