namespace Omnicx.WebStore.Models.Enums
{
    /// <summary>
    /// make sure this is in sync with omnicx.Entities.Events.Webhooks.WebhookEventTypes
    /// </summary>
    public enum WebhookEventTypes
    {

        BasketItemAdded = 11,
        BasketItemRemoved = 12,
        BasketCheckout = 13,
        BasketViewed = 14,
        CheckoutStarted = 15,
        CheckoutAddress = 16,
        CheckoutPayment = 17,
        CheckoutConfirmation = 18,

        OrderCreated = 21,
        OrderUpdated = 22,
        Paid = 23,

        ProductCreated = 41,
        ProductUpdated = 42,
        ProductDeleted = 43,
        ProductViewed = 44,

        CustomerCreated = 31,
        CustomerUpdated = 32,
        CustomerLoginSucces = 33,
        CustomerLoginFailure = 34,

        SessionCreated = 91,
        SessionUpdated = 92,

        BrandViewed = 50,

        SubbrandViewed = 60,

        CollectionViewed = 70,

        CategoryViewed = 80,

        FreeText = 100,
        FacetSearch = 101,

        BlogViewed = 110,
        CmsPageViewed = 111,
        PageViewed = 112,
        FaqViewed = 113
    }
}
