using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Entities
{
    public class CustomViews
    {
        private static string Default_Layout = string.IsNullOrEmpty(ConfigKeys.StoreThemeDefaultLayout) == false ? ConfigKeys.StoreThemeDefaultLayout: "Layout1";
        
        public static string CATEGORY_LAYOUT_FOLDER = "~/Views/Category/" + Default_Layout + "/";

        public static string CATEGORY_LANDING = CATEGORY_LAYOUT_FOLDER + "CategoryLanding.cshtml"; //"CategoryLanding2.cshtml";
        public static string CATEGORY_LIST = CATEGORY_LAYOUT_FOLDER + "CategoryList.cshtml";
        public static string CATEGORY_PRODUCTS = CATEGORY_LAYOUT_FOLDER + "CategoryProducts.cshtml";

        //PRODUCT CUSTOM VIEWS
        public static string PRODUCT_LAYOUT_FOLDER = "~/Views/Product/" + Default_Layout + "/";

        public static string PRODUCT_DETAIL = PRODUCT_LAYOUT_FOLDER + "ProductDetail.cshtml"; //"ProductDetail2.cshtml";

        //SEARCH CUSTOM VIEWS
        public static string SEARCH_LAYOUT_FOLDER = "~/Views/Search/" + Default_Layout + "/";

        public static string SEARCH = SEARCH_LAYOUT_FOLDER + "Search.cshtml";
        public static string DYNAMIC_LIST = SEARCH_LAYOUT_FOLDER + "DynamicList.cshtml";
        public static string DYNAMIC_LIST_ITEMS = SEARCH_LAYOUT_FOLDER + "DynamicListItems.cshtml";
        public static string DYNAMIC_LIST_PRODUCTS = SEARCH_LAYOUT_FOLDER + "DynamicListProduct.cshtml";

        //BASKET CUSTOM VIEWS
        public static string BASKET_LAYOUT_FOLDER = "~/Views/Basket/" + Default_Layout + "/";

        public static string BASKET = BASKET_LAYOUT_FOLDER + "Index.cshtml";
        public static string HEADER_BASKET_VIEW = BASKET_LAYOUT_FOLDER + "_HeaderBasketView.cshtml";
        public static string HEADER_BULKORDER_VIEW = BASKET_LAYOUT_FOLDER + "_HeaderBulkOrder.cshtml";
        public static string BULK_ORDER_MESSAGE = BASKET_LAYOUT_FOLDER + "_BulkOrderMessage.cshtml";
        public static string QUICK_ORDER_PAD = BASKET_LAYOUT_FOLDER + "_QuickOrderPad.cshtml";

        //BLOG CUSTOM VIEWS
        public static string BLOG_LAYOUT_FOLDER = "~/Views/Blog/" + Default_Layout + "/";

        public static string BLOGS = BLOG_LAYOUT_FOLDER + "Blogs.cshtml";
        public static string BLOGS_DETAIL = BLOG_LAYOUT_FOLDER + "BlogDetail.cshtml";
        public static string BLOG_CATEGORY = BLOG_LAYOUT_FOLDER + "BlogCategory.cshtml";
        public static string BLOG_BY_EDITOR = BLOG_LAYOUT_FOLDER + "BlogsByEditor.cshtml";
        public static string BLOG_SEARCH = BLOG_LAYOUT_FOLDER + "Search.cshtml";


        //CHECKOUT CUSTOM VIEWS
        public static string CHECKOUT_LAYOUT_FOLDER = "~/Views/Checkout/" + Default_Layout + "/";

        public static string ONE_PAGE_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "OnePageCheckout.cshtml";
        public static string PAYMENT_RESPONSE = CHECKOUT_LAYOUT_FOLDER + "PaymentResponse.cshtml";
        public static string ORDER_CONFIRMATION = CHECKOUT_LAYOUT_FOLDER + "OrderConfirmation.cshtml";
        public static string STANDARD_CHECKOUT = CHECKOUT_LAYOUT_FOLDER + "StandardCheckout.cshtml";

        //ACCOUNT CUSTOM VIEWS
        public static string ACCOUNT_LAYOUT_FOLDER = "~/Views/Account/" + Default_Layout + "/";

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

        //COMMON CUSTOM VIEWS
        public static string COMMON_LAYOUT_FOLDER = "~/Views/Common/" + Default_Layout + "/";

        public static string FAQ_VIEW = COMMON_LAYOUT_FOLDER + "_FaqsView.cshtml";
        public static string ERROR = COMMON_LAYOUT_FOLDER + "Error.cshtml";
        public static string ERROR_500 = COMMON_LAYOUT_FOLDER + "Error500.cshtml";
        public static string PAGE_NOT_FOUND = COMMON_LAYOUT_FOLDER + "PageNotFound.cshtml";
        public static string BASKET_NOT_FOUND = COMMON_LAYOUT_FOLDER + "BasketNotFound.cshtml";

        //BRAND CUSTOM VIEWS
        public static string BRAND_LAYOUT_FOLDER = "~/Views/Brand/" + Default_Layout + "/";

        public static string BRAND_PRODUCTS = BRAND_LAYOUT_FOLDER + "BrandProducts.cshtml";
        public static string BRAND_LIST = BRAND_LAYOUT_FOLDER + "BrandList.cshtml";
        public static string BRAND_LANDING = BRAND_LAYOUT_FOLDER + "BrandLanding.cshtml";
        public static string BRAND_DETAIL = BRAND_LAYOUT_FOLDER + "BrandDetail.cshtml";

        //SURVEY CUSTOM VIEWS
        public static string SURVEY_LAYOUT_FOLDER = "~/Views/Survey/" + Default_Layout + "/";
        public static string SURVEY_CAPTURE = SURVEY_LAYOUT_FOLDER + "Capture.cshtml";

        //PAGE CUSTOM VIEWS
        public static string PAGE_LAYOUT_FOLDER = "~/Views/Page/" + Default_Layout + "/";

        public static string INDEX = PAGE_LAYOUT_FOLDER + "Index.cshtml";
        public static string DYNAMICPAGE = PAGE_LAYOUT_FOLDER + "DynamicPage.cshtml";

        //SHARED CUSTOM VIEWS
        public static string SHARED_LAYOUT_FOLDER = "~/Views/Shared/" + Default_Layout + "/";

        public static string CURRENCY_VIEW = SHARED_LAYOUT_FOLDER + "_currencyView.cshtml";
        public static string SITE_LOGO = SHARED_LAYOUT_FOLDER + "_SiteLogo.cshtml";
        public static string MAIN_MENU = SHARED_LAYOUT_FOLDER + "_MainMenu.cshtml";
        public static string MAIN_MENU_MOBILE = SHARED_LAYOUT_FOLDER + "_MobileMenu.cshtml";        
        public static string MAIN_FOOTER = SHARED_LAYOUT_FOLDER + "_MainFooter.cshtml";
        public static string MAIN_FOOTER_MOBILE = SHARED_LAYOUT_FOLDER + "_MainFooter.mobile.cshtml";
        public static string HEADER_LOGIN_INFO= SHARED_LAYOUT_FOLDER + "_HeaderLoginInfo.cshtml";
        public static string HEADER_SEARCHBOX = SHARED_LAYOUT_FOLDER + "_SearchBox.cshtml";

        //B2B CUSTOM VIEWS
        public static string B2B_LAYOUT_FOLDER = "~/Views/B2B/" + Default_Layout + "/";
        public static string MY_COMPANY_B2B = B2B_LAYOUT_FOLDER + "MyCompany.cshtml";
        public static string USERS_B2B = B2B_LAYOUT_FOLDER + "Users.cshtml"; 
        public static string QUOTES_B2B = B2B_LAYOUT_FOLDER + "Quotes.cshtml";
    }
}