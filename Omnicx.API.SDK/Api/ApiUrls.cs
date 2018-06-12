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
        public const string GetAddressById = "commerce/customer/{0}/addresses/{1}";
        public const string DeleteAddress = "commerce/customer/{0}/addresses/{1}/delete";
        public const string changeDefaultAddress = "commerce/customer/{0}/addresses/{1}/setdefault";
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
        public const string AddToBasket = "commerce/basket/{0}/add";
        public const string ApplyPromoCode = "commerce/basket/{0}/promo/{1}/apply";
        //public const string ApplyPromoCodeBulk = "commerce/basket/{0}/promo/{1}/apply";
        public const string RemovePromoCode = "commerce/basket/{0}/promo/{1}/remove";
        //public const string RemovePromoCodeBulk = "commerce/basket/{0}/promo/{1}/removebulk";
        public const string UpdateShipping = "commerce/basket/{0}/shipping/{1}/update";

        public const string ShippingMethods = "commerce/shipping/country/{0}/{1}/{2}"; //0:basketId, 1:2LetterCountryIso
        public const string ShippingNominatedDays = "commerce/shipping/nominateddeiveries/{0}";//0:Date
        public const string ShippingClickAndCollect = "commerce/shipping/clickandcollect/{0}/{1}";//0:basketId,1:PostCode

        public const string BulkAddProduct = "commerce/basket/{0}/bulkAdd";
        public const string PersistentBasket = "commerce/basket/{0}/merge/{1}";
        public const string UpdateBasketUser = "commerce/basket/{0}/user/{1}/update";
        public const string BasketRelatedProducts = "commerce/basket/{0}/relatedproducts";
        public const string GetUserBaskets = "commerce/basket/{0}/baskets";
        public const string UpdateBasketLineCustomInfo = "commerce/basket/{0}/updateinfo";



        //brand
        public const string BrandDetail = "catalog/brand/{0}";
        public const string BrandDetailBySlug = "catalog/brand/slug";
        public const string Brand = "catalog/brand";
        

        //addToWishlist
        public const string AddToWishlist = "commerce/customer/{0}/wishlist/{1}/add/{2}";
        public const string GetWishlist = "commerce/customer/{0}/wishlist/{1}";
        public const string RemoveWishList = "commerce/customer/{0}/wishlist/{1}/remove/{2}";


        //Address
        public const string SaveAddress = "commerce/customer/{0}/addresses/create";
        public const string GetAddress = "commerce/customer/{0}/addresses";
        public const string UpdateCustomerAddress = "commerce/customer/{0}/addresses/{1}/update";


        //Survey
        public const string Survey = "content/survey/{0}";
        public const string SurveyByCode = "content/survey/code/{0}";
        public const string SurveyProfile = "content/survey/profile/{0}";
        public const string SurveySaveAnswer = "content/survey/{0}/{1}/answer";//0=surveyId, 1=questionId
        public const string SurveySaveAllAnswers = "content/survey/answers";

        //blog
        public const string BlogByGroup = "/blogs/groups/{0}?groupType={1}&currentPage={2}&pageSize={3}";
        public const string BlogGroups = "/blogs/groups";
        public const string BlogSearch = "/blogs/search/{0}?searchText={0}&currentPage={1}&pageSize={2}";
        public const string BlogDetail = "blogs/slug";

        //Order
        public const string GetOrders = "commerce/customer/{0}/orders";
        public const string GetOrderDetail = "commerce/order/{0}";

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
        //Collection
        public const string CollectionBySlug = "catalog/collection/slug";
        public const string CollectionList = "catalog/collection";

        //Chekout
        /// <summary>
        /// Pass basketId for checout
        /// </summary>
        public const string Checkout = "commerce/checkout/{0}";

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


        

        //Feed
        public const string FeedLink = "content/feed";

        //B2B
        public const string GetUsers = "commerce/b2b/{0}/users"; //{0}: CompanyId
        public const string GetQuotes = "commerce/b2b/{0}/quotes"; //{0}: UserId
        public const string CompanyDetail = "commerce/b2b/{0}/company"; //{0}: UserId
        public const string CreateQuote = "commerce/b2b/quote/create";

        public const string ValidateQuotePayment = "commerce/basket/{0}/validatepayment"; //{0}: Payment link ? 
        public const string RequestQuoteChange = "commerce/b2b/quote/{0}/{1}/requestchange"; //{0}:userid , {1}:quoteNumber
    }
}
