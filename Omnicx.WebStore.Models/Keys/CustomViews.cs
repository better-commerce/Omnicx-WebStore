using Omnicx.WebStore.Models.Keys;

namespace Omnicx.WebStore.Models.Keys
{
    public class CustomViews
    {
        //private static string Default_Layout = string.IsNullOrEmpty(ConfigKeys.StoreThemeDefaultLayout) == false ? ConfigKeys.StoreThemeDefaultLayout: "Layout1";
        
        public static string CATEGORY_LAYOUT_FOLDER = "~/Views/Category/" ;

        public static string CATEGORY_LANDING = CATEGORY_LAYOUT_FOLDER + "CategoryLanding.cshtml"; //"CategoryLanding2.cshtml";
        public static string CATEGORY_LIST = CATEGORY_LAYOUT_FOLDER + "CategoryList.cshtml";
        public static string CATEGORY_PRODUCTS = CATEGORY_LAYOUT_FOLDER + "CategoryProducts.cshtml";

        //PRODUCT CUSTOM VIEWS
        public static string PRODUCT_LAYOUT_FOLDER = "~/Views/Product/" ;

        public static string PRODUCT_DETAIL = PRODUCT_LAYOUT_FOLDER + "ProductDetail.cshtml"; //"ProductDetail2.cshtml";

        //SEARCH CUSTOM VIEWS
        public static string SEARCH_LAYOUT_FOLDER = "~/Views/Search/" ;

        public static string SEARCH = SEARCH_LAYOUT_FOLDER + "Search.cshtml";
        public static string DYNAMIC_LIST = SEARCH_LAYOUT_FOLDER + "DynamicList.cshtml";
        public static string DYNAMIC_LIST_ITEMS = SEARCH_LAYOUT_FOLDER + "DynamicListItems.cshtml";
        public static string DYNAMIC_LIST_PRODUCTS = SEARCH_LAYOUT_FOLDER + "DynamicListProduct.cshtml";

        //BASKET CUSTOM VIEWS
        public static string BASKET_LAYOUT_FOLDER = "~/Views/Basket/" ;

        public static string BASKET = BASKET_LAYOUT_FOLDER + "Index.cshtml";
        public static string SHIPMENT_BASKET = BASKET_LAYOUT_FOLDER + "ShipmentBasket.cshtml";
        public static string HEADER_BASKET_VIEW = BASKET_LAYOUT_FOLDER + "_HeaderBasketView.cshtml";
        public static string HEADER_BULKORDER_VIEW = BASKET_LAYOUT_FOLDER + "_HeaderBulkOrder.cshtml";
        public static string BULK_ORDER_MESSAGE = BASKET_LAYOUT_FOLDER + "_BulkOrderMessage.cshtml";
        public static string QUICK_ORDER_PAD = BASKET_LAYOUT_FOLDER + "_QuickOrderPad.cshtml";

        //BLOG CUSTOM VIEWS
        public static string BLOG_LAYOUT_FOLDER = "~/Views/Blog/" ;

        public static string BLOGS = BLOG_LAYOUT_FOLDER + "Blogs.cshtml";
        public static string BLOGS_DETAIL = BLOG_LAYOUT_FOLDER + "BlogDetail.cshtml";
        public static string BLOG_CATEGORY = BLOG_LAYOUT_FOLDER + "BlogCategory.cshtml";
        public static string BLOG_BY_EDITOR = BLOG_LAYOUT_FOLDER + "BlogsByEditor.cshtml";
        public static string BLOG_SEARCH = BLOG_LAYOUT_FOLDER + "Search.cshtml";


        //CHECKOUT CUSTOM VIEWS
        public static string CHECKOUT_LAYOUT_FOLDER = "~/Views/Checkout/" ;

        public static string ONE_PAGE_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "OnePageCheckout.cshtml";
        public static string PAYMENT_RESPONSE = CHECKOUT_LAYOUT_FOLDER + "PaymentResponse.cshtml";
        public static string ORDER_CONFIRMATION = CHECKOUT_LAYOUT_FOLDER + "OrderConfirmation.cshtml";
        public static string STANDARD_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "StandardCheckout.cshtml";
        public static string WIZARD_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "WizardCheckout.cshtml";
        public static string WIZARD_CHECKOUT_DELIVERY = CHECKOUT_LAYOUT_FOLDER + "WizardCheckoutDelivery.cshtml";
        public static string WIZARD_CHECKOUT_BILLING = CHECKOUT_LAYOUT_FOLDER + "WizardCheckoutBilling.cshtml";
        public static string HOTEL_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "HotelCheckout.cshtml";
        public static string HOTEL_CHECKOUT_DELIVERY = CHECKOUT_LAYOUT_FOLDER + "HotelCheckoutDelivery.cshtml";
        public static string HOTEL_CHECKOUT_BILLING = CHECKOUT_LAYOUT_FOLDER + "HotelCheckoutBilling.cshtml";

        //ACCOUNT CUSTOM VIEWS
        public static string ACCOUNT_LAYOUT_FOLDER = "~/Views/Account/" ;

        public static string ORDER_HISTORY = ACCOUNT_LAYOUT_FOLDER + "OrderHistory.cshtml";
        public static string WISHLIST = ACCOUNT_LAYOUT_FOLDER + "Wishlist.cshtml";
        public static string MY_ACCOUNT = ACCOUNT_LAYOUT_FOLDER + "MyAccount.cshtml";
        public static string ORDER_DETAIL = ACCOUNT_LAYOUT_FOLDER + "OrderDetail.cshtml";
        public static string RETURN_REQUEST = ACCOUNT_LAYOUT_FOLDER + "ReturnRequest.cshtml";
        public static string SIGN_IN = ACCOUNT_LAYOUT_FOLDER + "SignIn.cshtml";
        public static string PASSWORD_CHANGE = ACCOUNT_LAYOUT_FOLDER + "PasswordChange.cshtml";
        public static string ADDRESS_BOOK = ACCOUNT_LAYOUT_FOLDER + "AddressBook.cshtml";
        public static string HEADER_LOGIN = ACCOUNT_LAYOUT_FOLDER + "_LoginChildView.cshtml";
        public static string RETURN_HISTORY = ACCOUNT_LAYOUT_FOLDER + "ReturnHistory.cshtml";
        public static string PASSWORD_RECOVERY = ACCOUNT_LAYOUT_FOLDER + "PasswordRecovery.cshtml";
        public static string FORGOT_PASSWORD = ACCOUNT_LAYOUT_FOLDER + "ForgotPassword.cshtml";
        public static string MY_ACTIVITY = ACCOUNT_LAYOUT_FOLDER + "MyActivity.cshtml";
        public static string ITEM_VIEW = ACCOUNT_LAYOUT_FOLDER + "ItemView.cshtml";
        public static string SAVED_BASKET = ACCOUNT_LAYOUT_FOLDER + "SavedBasket.cshtml";
        public static string CONTACT_FORM = ACCOUNT_LAYOUT_FOLDER + "ContactUs.cshtml";
        public static string SUBSCRIPTION_HISTORY = ACCOUNT_LAYOUT_FOLDER + "SubscriptionHistory.cshtml";
        public static string SUBSCRIPTION_DETAIL = ACCOUNT_LAYOUT_FOLDER + "SubscriptionDetail.cshtml";

        //COMMON CUSTOM VIEWS
        public static string COMMON_LAYOUT_FOLDER = "~/Views/Common/" ;

        public static string FAQ_VIEW = COMMON_LAYOUT_FOLDER + "_FaqsView.cshtml";
        public static string ERROR = COMMON_LAYOUT_FOLDER + "Error.cshtml";
        public static string ERROR_500 = COMMON_LAYOUT_FOLDER + "Error500.cshtml";
        public static string PAGE_NOT_FOUND = COMMON_LAYOUT_FOLDER + "PageNotFound.cshtml";
        public static string BASKET_NOT_FOUND = COMMON_LAYOUT_FOLDER + "BasketNotFound.cshtml";
        public static string STORE_LOCATOR = COMMON_LAYOUT_FOLDER + "StoreLocator.cshtml";
        public static string STORE_DETAIL = COMMON_LAYOUT_FOLDER + "StoreDetail.cshtml";

        //BRAND CUSTOM VIEWS
        public static string BRAND_LAYOUT_FOLDER = "~/Views/Brand/" ;

        public static string BRAND_PRODUCTS = BRAND_LAYOUT_FOLDER + "BrandProducts.cshtml";
        public static string BRAND_LIST = BRAND_LAYOUT_FOLDER + "BrandList.cshtml";
        public static string BRAND_Menu = BRAND_LAYOUT_FOLDER + "_BrandMenu.cshtml";
        public static string BRAND_LANDING = BRAND_LAYOUT_FOLDER + "BrandLanding.cshtml";
        public static string BRAND_DETAIL = BRAND_LAYOUT_FOLDER + "BrandDetail.cshtml";
        public static string SUB_BRAND_DETAIL = BRAND_LAYOUT_FOLDER + "SubBrandDetail.cshtml";

        //SURVEY CUSTOM VIEWS
        public static string SURVEY_LAYOUT_FOLDER = "~/Views/Survey/" ;
        public static string SURVEY_CAPTURE = SURVEY_LAYOUT_FOLDER + "Capture.cshtml";
        public static string SURVEY_APP_CAPTURE = SURVEY_LAYOUT_FOLDER + "AppCapture.cshtml";

        //PAGE CUSTOM VIEWS
        public static string PAGE_LAYOUT_FOLDER = "~/Views/Page/" ;

        public static string INDEX = PAGE_LAYOUT_FOLDER + "Index.cshtml";
        public static string DYNAMICPAGE = PAGE_LAYOUT_FOLDER + "DynamicPage.cshtml";

        //SHARED CUSTOM VIEWS
        public static string SHARED_LAYOUT_FOLDER = "~/Views/Shared/" ;

        public static string CURRENCY_VIEW = SHARED_LAYOUT_FOLDER + "_currencyView.cshtml";
        public static string SITE_LOGO = SHARED_LAYOUT_FOLDER + "_SiteLogo.cshtml";
        public static string MAIN_MENU = SHARED_LAYOUT_FOLDER + "_MainMenu.cshtml";
        public static string MAIN_MENU_MOBILE = SHARED_LAYOUT_FOLDER + "_MobileMenu.cshtml";        
        public static string MAIN_FOOTER = SHARED_LAYOUT_FOLDER + "_MainFooter.cshtml";
        public static string MAIN_FOOTER_MOBILE = SHARED_LAYOUT_FOLDER + "_MainFooter.mobile.cshtml";
        public static string HEADER_LOGIN_INFO= SHARED_LAYOUT_FOLDER + "_HeaderLoginInfo.cshtml";
        public static string HEADER_DYNAMIC_HEAD_TAG = SHARED_LAYOUT_FOLDER + "_DynamicHeadTag.cshtml";
        public static string HEADER_SEARCHBOX = SHARED_LAYOUT_FOLDER + "_SearchBox.cshtml";

        //B2B CUSTOM VIEWS
        public static string B2B_LAYOUT_FOLDER = "~/Views/B2B/" ;
        public static string MY_COMPANY_B2B = B2B_LAYOUT_FOLDER + "MyCompany.cshtml";
        public static string USERS_B2B = B2B_LAYOUT_FOLDER + "Users.cshtml"; 
        public static string QUOTES_B2B = B2B_LAYOUT_FOLDER + "Quotes.cshtml";

        //LOOKBOOK CUSTOM VIEWS
        public static string LOOKBOOK_LAYOUT_FOLDER = "~/Views/Lookbook/";
        public static string LOOKBOOK_INDEX = LOOKBOOK_LAYOUT_FOLDER + "Index.cshtml";
        public static string LOOKBOOK_DETAIL = LOOKBOOK_LAYOUT_FOLDER + "LookbookDetail.cshtml";
    }
}