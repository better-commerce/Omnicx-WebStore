namespace Omnicx.WebStore.Models.Enums
{
    /// <summary>
    /// make sure this is in sync with omnicx.Entities.Events.Webhooks.WebhookEntityTypes
    /// </summary>
    public enum WebhookEntityTypes
    {
        Basket = 1,
        Order = 2,
        Customer = 3,
        Product = 4,
        Brand = 5,
        Subbrand = 6,
        Collection = 7,
        Category = 8,
        Session = 9,
        Search = 10,
        Page = 11,
        CmsPage = 12,
        Blog = 13
    }
}
