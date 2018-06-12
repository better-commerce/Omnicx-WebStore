namespace Omnicx.API.SDK.Entities
{
    public enum DataLayerObjectType
    {
        ProductDetail = 1,
        BrandDetail = 2,
        SearchResult = 3,
        BrandList = 4,
        BlogList = 5,
        BlogDetail = 6,
        CategoryList = 7,
        CategoryDetail = 8,
        CategoryProducts = 9,
        Basket = 10,
        CmsPage = 11,
        CollectionDetail = 12,
        CollectionList = 13
    }

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
    public enum CachingTypes
    {
        Memory,
        Redis
    }
    public enum LinkType
    {
        None = 0,
        Href = 1,
        Button = 2
    }
    public enum MenuTypes
    {
        SimpleLink = 1,
        DynamicCategory = 3,
        ProductGrid = 6,
        Content = 7,
        BrandList = 8,
        DynamicCollection = 9
    }
    public enum EntitySlugTypes
    {
        Manufacturer,
        Category

    }
    public enum GeoLocatorPlugin
    {
        PCA
    }

    public enum TransactionType
    {
        A = 0,
        E = 1
    }
    public enum ServiceType
    {
        Cutting = 1,
        Coating = 2
    }
    public enum BasketStage
    {
        Anonymous = 1,
        LoggedIn = 2, // could be registered user or a guest email also.
        ShippingMethodSelected = 3,
        ShippingAddressProvided = 4,
        Placed = 5

    }
    public enum ShippingTypes
    {
        Flat = 1,
        ClickAndCollect = 4,
        CollectPlus = 5,
        Shutl = 6
    }
    public enum ItemTypes
    {
        Product = 1,
        GiftCard = 4,
        Variant = 6,
        Bundle = 7,
        DynamicBundle = 8
    }
    public enum ResultEntryType
    {
        Product,
        Brand,
        SubBrand,
        Collection

    }
    public enum FacetType
    {
        List,
        Slider

    }
    public enum GroupTypes
    {
        SubCategory = 1,
        SubCategoryList = 2,
        FeaturedBrands = 3,
        Facet = 4
    }
    public enum SubscriptionTypes
    {
        Monthly = 1,
        Qtr = 2,
        HalfYearly = 3,
        Yearly = 4
    }
    public enum AcknowledgeType
    {
        Failure = 0,
        Success = 1
    }
    public enum SiteViewComponentTypes
    {
        Image = 1,
        ImageGallery = 2,
        Slider = 3,
        List = 4,
        JsSnippet = 5,
        ContentBlock = 6,
        StaticHtml = 7,
        Navbar = 8,
        BlogHeader = 11,
        PageHeader = 12,
        Paragraph = 13,
        VideoGallery = 17,
        BrandList = 51,
        ProductList = 52,
        CategoryList = 53,
        BlogList = 54

    }
    public enum BlogGroupType
    {
        Editor = 1,
        Category = 2,
        Timeline = 3,
        BlogType = 4
    }
    public enum ImageObjectTypes
    {
        Product = 1,
        Brand = 2,
        SubBrand = 3,
        Promos = 4,
        Widgets = 5,
        Blog = 6,
        Content = 5,
    }
    public enum OrderStatus
    {

        Unknown = 0,
        Incomplete = 1,
        /// <summary>
        /// Pending
        /// </summary>
        PreOrderUntrusted = 10,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40,
        Pending = 2,
        Approved = 3,
        SentToWarehose = 4,
        AcceptedInWarehouse = 5,
        OrderSettled = 7,
        Dispatch = 9,
        ReadyToDispatch = 12,
        IdRequired = 107,
        AwaitingSettlement = 6,
        PreOrderApproved = 11,
        CancelledByStore = 102,
        CancelledByStorePaymentIssue = 103,
        CancelledByStoreStockIssue = 104,
        CancelledByCustomer = 105,
        CancelledFailedFraudScreening = 110,
    }
    public enum ListDatasetSource
    {
        None = 0,
        Product = 1,
        Brand = 2,
        Category = 3,
        SubBrand = 4,
        SubCategory = 5,
        Blog = 6,
        Image = 7

    }
    public enum PurchasingAsGiftOrMe
    {
        None = 0,
        ForMe = 1,
        Gift = 2,
        GiftToMe = 3
    }
    public enum UserSourceTypes
    {
        Web = 1,
        Store = 2, // through the OLD WebServices invoked by Store
        Newsletter = 7,
        Mobile = 8,
        GuestCheckout = 9
    }

    public enum CompanyUserRole
    {
        None = 0,
        Admin = 1,
        SalesUser = 2,
        User = 3
    }
    public enum ExistingBasket
    {
        Merge = 1,
        Disregard = 2
    }
    public enum LogLevel
    {
        Unknown = 0,
        Debug = 10,
        Information = 20,
        Warning = 30,
        Error = 40,
        Fatal = 50
    }

    public enum RedirectType
    {
        RedirectPermanent = 1,
        RedirectTemporary = 2
    }
}
