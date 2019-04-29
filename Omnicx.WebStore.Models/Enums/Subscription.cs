namespace Omnicx.WebStore.Models.Enums
{
    public enum SubscriptionTermTypes
    {
        Fixed = 0,
        UserDefined = 1
    }

    public enum SubscriptionPricingTypes
    {
        Flat = 1,
        Term = 2,
        PerUnit = 3
    }

    public enum SubscriptionTermIntervals
    {
        Day = 0,
        Week = 1,
        Month = 2,
        Year = 5
    }

    public enum SubscriptionOrderTriggerType
    {
        FixedDay = 1,
        Rolling = 2,
        UserDefined = 3
    }

    public enum SubscriptionPlanType
    {
        None=0,
        Simple = 1,
        FixedBundle = 2,
        DynamicBundle = 3
    }

    public enum SubscriptionDataSetType
    {
        Products = 1,
        Brands,
        SubBrands,
        Categories
    }
    /// <summary>
    /// values after monday auto incremented 
    /// </summary>
    public enum SubscriptionOrderTriggerDays
    {
        Monday = 0,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    public enum SubscriptionOrderTriggerMonths
    {
        January = 1,
        Febuary,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum UserPricingType
    {
        None=0,
        OneTime=1,
        Recurring=2
    }
    public enum SubscriptionStatus
    {
        None = 0,
        Active = 1,
        Pause = 2,
        Lapse = 3,
        Cancelled = 4
    }
    public enum SubscriptionRecurringOrderStatus
    {
        None = 0,
        Generated = 1,
        Due = 2,
        Cancelled = 3,
        Pause = 4

    }
}
