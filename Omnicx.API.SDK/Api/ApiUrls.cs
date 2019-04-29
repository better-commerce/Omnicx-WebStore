namespace Omnicx.API.SDK.Api
{
    public static class ApiUrls
    {
        //Customer/Login/Register
        public const string GetExistingUser = "commerce/customer/{0}/exists"; // {0} email address. 
        public const string Login = "commerce/customer/authenticate";
        public const string UserDetailId = "commerce/customer/{0}";
        public const string GetUserByName = "commerce/customer";
        public const string RegisterUser = "commerce/customer/create";
        
        public const string UpdateCustomerDetail = "commerce/customer/{0}/update";
        public const string GetAddressById = "commerce/address/{0}/{1}";
        public const string DeleteAddress = "commerce/address/{0}/{1}/delete";
        public const string changeDefaultAddress = "commerce/address/{0}/{1}/setdefault";
        public const string NewsletterSubscription = "commerce/customer/newsletter/subscribe";
        public const string UnSubscribeNewsletter = "commerce/customer/newsletter/unsubscribe/{0}";

        //Forgot Password
        public const string ChangePassword = "commerce/customer/{0}/password/change";
        public const string ForgotPassword = "commerce/customer/password/forgot?email={0}";
        public const string ResetPassword = "commerce/customer/password/reset";
        // User Activity
        public const string UserActivity = "commerce/customer/{0}/activities?currentPage={1}&pageSize={2}";
        public const string DeleteActivity = "commerce/customer/{0}/activities/delete";
        //Ghost User Authenticate
        public const string GhostUserAuth = "commerce/customer/{0}/ghostuser";

        //Menu
        public const string MenuDetails = "content/nav";

        //Category
        public const string Categories = "catalog/category";
        public const string Category = "catalog/category/slug";

        //Products
        public const string ProductDetail = "catalog/product/{0}";
        public const string ProductDetailBySlug = "catalog/product/slug";
        public const string Products = "catalog/search/advanced";
        public const string PageBySlug = "content/page/slug/{0}";
        public const string AutoSearch = "catalog/search/{0}";
        public const string SortByList = "catalog/search/sortbylist";
        public const string RelatedProducts = "catalog/product/{0}/relatedproducts";
        public const string AddProductReview = "catalog/product/{0}/review";

        //basket 
        public const string GetBasket = "commerce/basket/{0}";
        public const string UpdateBasketCampaign = "commerce/basket/{0}/campaign/{1}/update"; //0:basketId,1:campaignCode
        public const string AddToBasket = "commerce/basket/{0}/add";
        public const string ApplyPromoCode = "commerce/basket/{0}/promo/{1}/apply";
        //public const string ApplyPromoCodeBulk = "commerce/basket/{0}/promo/{1}/apply";
        public const string RemovePromoCode = "commerce/basket/{0}/promo/{1}/remove";
        //public const string RemovePromoCodeBulk = "commerce/basket/{0}/promo/{1}/removebulk";
        public const string UpdateShipping = "commerce/basket/{0}/shipping/{1}/update";

        public const string ShippingMethods = "commerce/shipping/country/{0}/{1}/{2}"; //0:basketId, 1:2LetterCountryIso
        public const string ShippingNominatedDays = "commerce/shipping/nominateddeiveries/{0}";//0:Date
        public const string ShippingPlans = "oms/shipment/plans";
        public const string ClickAndCollectStores = "oms/Store/clickandcollect/{0}";

        public const string BulkAddProduct = "commerce/basket/{0}/bulkAdd";
        public const string PersistentBasket = "commerce/basket/{0}/merge/{1}";
        public const string UpdateBasketUser = "commerce/basket/{0}/user/{1}/update";
        public const string BasketRelatedProducts = "commerce/basket/{0}/relatedproducts";
        public const string UpdateBasketDeliveryPlans = "commerce/basket/{0}/UpdateDeliveryPlan";
        public const string GetUserBaskets = "commerce/basket/{0}/baskets";
        public const string UpdateBasketLineCustomInfo = "commerce/basket/{0}/updateinfo";
        public const string UpdatePoReference = "commerce/basket/{0}/poReference/{1}/update";
        public const string UpdateBasketDeliveryInstruction = "commerce/basket/{0}/updateDeliveryInstruction";
        public const string UpdateBasketSubscriptionInfo = "commerce/basket/{0}/updatesubscriptioninfo/{1}";


        //brand
        public const string BrandDetail = "catalog/brand/{0}";
        public const string BrandDetailBySlug = "catalog/brand/slug";
        public const string Brand = "catalog/brand";
        public const string SubBrands = "catalog/brand?brandIds={0}";


        //addToWishlist
        public const string AddToWishlist = "commerce/customer/{0}/wishlist/{1}/add/{2}";
        public const string GetWishlist = "commerce/customer/{0}/wishlist/{1}";
        public const string RemoveWishList = "commerce/customer/{0}/wishlist/{1}/remove/{2}";


        //Address
        public const string SaveAddress = "commerce/address/create";
        public const string GetAddress = "commerce/address/{0}";
        public const string UpdateCustomerAddress = "commerce/address/{0}/update";
        public const string NoDefaultAddress = "commerce/address/{0}/noDefaultAddress";

        //Survey
        public const string Survey = "survey/{0}";
        public const string SurveyByCode = "content/survey/code/{0}";
        public const string SurveyProfile = "content/survey/profile/{0}";
        public const string SurveySaveAnswer = "survey/saveanswers";

        //Recommendation
        public const string RecommendationAll = "recommendation/getall";

        /// <summary>
        /// 0:UserName
        /// 1:SurveyId
        /// </summary>
        public const string SurveyUserResponse = "survey/userresponse?userName={0}&surveyId={1}";

        /// <summary>
        /// {0}:OrgId,{1}:domainId
        /// </summary>
        public const string ToolboxDataUrl = "api/survey/{0}/{1}/surveytoolboxdata";


        //blog
        public const string BlogByGroup = "/blogs/groups/{0}?groupType={1}&currentPage={2}&pageSize={3}";
        public const string BlogGroups = "/blogs/groups";
        public const string BlogSearch = "/blogs/search/{0}?searchText={0}&currentPage={1}&pageSize={2}";
        public const string BlogDetail = "blogs/slug";

        //Order
        public const string GetOrders = "commerce/customer/{0}/orders";
        public const string GetOrderDetail = "commerce/order/{0}";
        public const string DownloadInvoice = "commerce/order/{0}/invoice";
        public const string GetOrderByEmail = "commerce/order/email?email={0}";

        // Payment Methods
        public const string PaymentMethods = "commerce/payment/paymentmethod";
        
      
        //Log
        public const string InsertLog = "infra/log/create";

        //Session
        public const string CreateSession = "infra/session/create";
        public const string UpdateSession = "infra/session/update";

        //Global
        public const string GetConfigSettings = "infra/config/get";
        
        //Faqs
        public const string FaqsCategories = "content/faq/categories";
        public const string FaqsSubCategories = "content/faq/{0}";

        //SiteView
        public const string SiteViewComponents = "content/siteview/slug";
        public const string SiteViewAllSlug = "content/siteview/slug/all";
        /// <summary>
        /// {0}:id,{1}:versionNo,{2}:langCulture
        /// </summary>
        public const string SiteViewById= "content/siteview/id/{0}/{1}/{2}";
        //Collection
        public const string CollectionBySlug = "catalog/collection/slug";
        public const string CollectionList = "catalog/collection";
        public const string GetAllLookbook = "catalog/collection/GetDynamicListsBySlug";
        public const string GetLookbookByGroup = "catalog/collection/GetDynamicListsByGroupName";

        //Chekout
        /// <summary>
        /// Pass basketId for checout
        /// </summary>
        public const string Checkout = "commerce/checkout/{0}";
        public const string GeneratePaymentLink = "commerce/checkout/{0}/payment/link?userId={1}";

        /// <summary>
        /// Update Basket Delivery Address
        /// </summary>
        public const string UpdateBasketDeliveryAddress = "commerce/checkout/{0}/address/update"; //{0}: BasketId

        /// <summary>
        /// Crate Order against Order Basket
        /// </summary>
        public const string ConvertToOrder = "commerce/checkout/{0}/convert";

        /// <summary>
        /// Update Order Payment Response
        /// </summary>
        public const string UpdatePayment = "commerce/checkout/{0}/payment/response";
        public const string PaymentSetting = "commerce/checkout/paymentsetting/{0}";
        public const string SetOrderRisk = "commerce/checkout/{0}/{1}/payment/response";

        /// <summary>
        /// Get all keyword & url in search
        /// </summary>
        public const string KeywordRedirections = "catalog/search/keywordredirections";

        //Return
        public const string ReturnRequest="commerce/return/{0}/request"; //{0} : OrderId (guid)
        public const string CreateReturn = "commerce/return/create";
        public const string GetAllReturns = "commerce/customer/{0}/returns";
        public const string SendContactEmail = "content/email/sendContactEmail";



        //Feed
        public const string FeedLink = "content/feed";

        //B2B
        public const string GetUsers = "commerce/b2b/{0}/users"; //{0}: CompanyId
        public const string GetQuotes = "commerce/b2b/{0}/quotes"; //{0}: UserId
        public const string CompanyDetail = "commerce/b2b/{0}/company"; //{0}: UserId
        public const string SaveQuote = "commerce/b2b/quote/save";

        public const string ValidateQuotePayment = "commerce/basket/{0}/validatepayment"; //{0}: Payment link ? 
        public const string RequestQuoteChange = "commerce/b2b/quote/{0}/{1}/requestchange"; //{0}:userid , {1}:quoteNumber
        public const string GetCompanies = "commerce/b2b/Companies";

        public const string FindStockInStore = "oms/store/{0}/stock/{1}/{2}";//{0}:postCode, {1}:stockCode,{2}:OrgId
        public const string FindNearestStore = "oms/store/{0}"; //{0}:postCode,{1}:OrgId
        public const string StoreDetail = "oms/store/{0}/detail"; //{0}externalRefId

        //Subscriptions
        public const string GetSubscriptionPlan = "commerce/subscription/plan/{0}";
        public const string AddSubscriptionToBag = "commerce/subscription/AddSubscriptionToBasket";
        public const string GetSubscriptions = "commerce/customer/{0}/subscriptions";
        public const string SubscriptionDetail = "commerce/customer/subscriptionDetail/{0}";
        public const string UpdateSubscriptionStatus = "commerce/order/updatesubscriptionstatus";
    }
}
