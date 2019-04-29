using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class SubscriptionPlanModel
    {
        public Guid RecordId { get; set; }
        public Guid MinTermId { get; set; }
        public Guid MaxTermId { get; set; }
        public string Name { get; set; }
        public SubscriptionPricingTypes PricingType { get; set; }
        public SubscriptionTermTypes TermType { get; set; }
        public SubscriptionPlanType PlanType { get; set; }
        public SubscriptionInterval Interval { get; set; }
        public SubscriptionOrderTriggerDays OrderTriggerDayOfWeek { get; set; }
        public SubscriptionOrderTriggerMonths OrderTriggerMonth { get; set; }
        public SubscriptionOrderTriggerType OrderTriggerType { get; set; }

        public List<SubscriptionPlanTerm> Terms { get; set; }

        public Amount TotalFee { get; set; }
        public Amount OneTimeFee { get; set; }
        public Amount RecurringFee { get; set; }
        public Amount SignUpFee { get; set; }
        public Amount CancellationFee { get; set; }

        public bool AutoRenewal { get; set; }
        public bool AllowPause { get; set; }
        public bool EnableOneTimeFee { get; set; }
        public bool IsActive { get; set; }
        public bool IncludeAllProducts { get; set; }
        public bool IsTaxInclusive { get; set; }
        public bool AllowCancellation { get; set; }
        public bool AllowOrderEditing { get; set; }

        public int EditBeforeDays { get; set; }
        public int RenewalAlertDays { get; set; }
        public int OrderTriggerDayOfMonth { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }

    }
    public struct SubscriptionInterval
    {
        public SubscriptionInterval(int duration, SubscriptionTermIntervals type)
        {
            this.Duration = duration;
            this.IntervalType = type;
        }
        public int Duration { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionTermIntervals IntervalType { get; set; }
    }
    public class SubscriptionPlanTerm
    {
        public Guid Id { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public SubscriptionInterval SubscriptionTerm { get; set; }
        
        public bool IsDefault { get; set; }

        public Amount TotalFee { get; set; }

        public Amount OneTimeFee { get; set; }

        public Amount RecurringFee { get; set; }

        public Amount SignUpFee { get; set; }

    }
}
