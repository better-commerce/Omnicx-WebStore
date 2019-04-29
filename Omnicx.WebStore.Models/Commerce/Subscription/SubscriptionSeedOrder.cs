using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce.Subscription
{
    public class SubscriptionSeedOrder
    {
        public Guid Id { get; set; }
        public Guid CompanyOrderId { get; set; }
        public Guid SubscriptionPlanId { get; set; }

        public string SubscriptionPlanName { get; set; }

        public SubscriptionPlanType PlanType { get; set; }
        public SubscriptionInterval Interval { get; set; }
        public SubscriptionTermTypes TermType { get; set; }
        public SubscriptionPricingTypes PricingType { get; set; }
        public SubscriptionStatus Status { get; set; }
        public SubscriptionInterval UserTerm { get; set; }
        public UserPricingType UserPricingPreference { get; set; }
        public DayOfWeek OrderTriggerDayOfWeek { get; set; }
        public SubscriptionOrderTriggerMonths OrderTriggerMonth { get; set; }
        public SubscriptionOrderTriggerType OrderTriggerType { get; set; }

        public bool AutoRenewal { get; set; }
        public bool AllowPause { get; set; }
        public bool EnableOneTimeFee { get; set; }
        public bool IsTaxInclusive { get; set; }
        public bool AllowCancellation { get; set; }
        public bool AllowOrderEditing { get; set; }

        public int EditOrderBeforeDays { get; set; }
        public int RenewalAlertDays { get; set; }
        public int OrderTriggerDayOfMonth { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }

        //Subscription Pricing
        public Amount TotalFee { get; set; }
        public Amount OneTimeFee { get; set; }
        public Amount RecurringFee { get; set; }
        public Amount SignUpFee { get; set; }
        public Amount CancellationFee { get; set; }
    }
}
