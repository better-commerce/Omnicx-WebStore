namespace Omnicx.API.SDK.Models.Helpers
{
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
    public enum ShippingTypes
    {
        Flat = 1,
        ClickAndCollect = 4,
        CollectPlus = 5,
        Shutl = 6
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
        BlogHeader=11,
        PageHeader=12,
        Paragraph=13,
        VideoGallery=17,
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
    public enum OrderStatus : int
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
        IDREQUIRED = 107,
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
        Merge= 1,
        Disregard= 2
    }
    public enum ItemTypes
    {
        SimpleProduct = 1,         
        Variant = 6,     
        Bundle = 7,
        DynamicBundle = 8
    }
}