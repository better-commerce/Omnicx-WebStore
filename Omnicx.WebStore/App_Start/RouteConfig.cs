using Omnicx.API.SDK.Api.Site;

using Omnicx.API.SDK.Helpers;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Omnicx.WebStore
{
    //public class RouteConfig
    //{
    //    public static void RegisterRoutes(RouteCollection routes)
    //    {
    //        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

    //        routes.MapRoute(
    //            name: "Default",
    //            url: "{controller}/{action}/{id}",
    //            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional  },
    //            namespaces: new[] { "Omnicx.WebStore.Core" }
    //        );


    //    }
    //}

    public class RouteConfig
    {
        /// <summary>
        /// This fuction call from Global AppStart and App-BeginRequest
        /// On AppStart this function initialize default route and cms page route.
        /// On App=BeginRequest it chack if Cms Slug exist or not. If exist it do nothing else it clear
        /// all route and recreate new route
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="initialCall"></param>
        public static void RegisterRoutes(RouteCollection routes, bool initialCall = true)
        {
            if (!initialCall)
            {
                var cackekey = string.Format(CacheKeys.SiteViewAllSlug, ConfigKeys.OmnicxDomainId);
                if (CacheManager.IsKeyExist(cackekey))
                    return;
            }
            routes.Clear();
            var siteViewApi = DependencyResolver.Current.GetService<ISiteViewApi>();
            var slugs = siteViewApi.GetSiteViewAllSlug();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            var slugCount = 1;
            if (slugs?.Result != null)
            {
                foreach (var slug in slugs.Result.Distinct())  //Added distict for removing duplicate slugs
                {
                    routes.MapRoute(name: "slug" + slugCount.ToString(), url: slug, defaults: new { controller = "Page", action = "DynamicPage", slug = UrlParameter.Optional });
                    slugCount++;
                }
            }
            routes.MapRoute(name: "sitemap", url: "{slug}.xml", defaults: new { controller = "Page", action = "GetFeedLink", slug = UrlParameter.Optional });
            routes.MapRoute(name: "feed", url: "feed{s}/{slug}", defaults: new { controller = "Page", action = "GetFeedLink", slug = UrlParameter.Optional, s = UrlParameter.Optional });

            routes.MapRoute(name: "brands-all", url: "brands", defaults: new { controller = "Brand", action = "BrandList", id = UrlParameter.Optional });
            routes.MapRoute(name: "brand-detail", url: "brands/{name}", defaults: new { controller = "Brand", action = "BrandDetail", id = UrlParameter.Optional });
            routes.MapRoute(name: "brand-productslist", url: "brands/{name}/all", defaults: new { controller = "Brand", action = "BrandProducts", id = UrlParameter.Optional });
            routes.MapRoute(name: "brand-home", url: "brands/brand-home", defaults: new { controller = "Brand", action = "BrandLanding" });
            routes.MapRoute(name: "myaccount", url: "myaccount", defaults: new { controller = "Account", action = "MyAccount" });
            routes.MapRoute(name: "search", url: "search", defaults: new { controller = "Search", action = "Search", id = UrlParameter.Optional });
            routes.MapRoute(name: "categories-products", url: "categories/categories-products", defaults: new { controller = "Category", action = "CategoryLanding2" });
            routes.MapRoute(name: "categories", url: "categories", defaults: new { controller = "Category", action = "CategoryList" });
            //routes.MapRoute(name: "errorblog", url: "blog/blogs", defaults: new { controller = "Common", action = "PageNotFound", url = UrlParameter.Optional });
            routes.MapRoute(name: "catalogue", url: SiteRouteUrl.Category + "/{categorySlug}/{groupSlug}/{linkSlug}/{linkSlug1}", defaults: new { controller = "Category", action = "CategoryLanding", categorySlug = UrlParameter.Optional, groupSlug = UrlParameter.Optional, linkSlug = UrlParameter.Optional, linkSlug1 = UrlParameter.Optional });
            routes.MapRoute(name: "product", url: "products/{name}", defaults: new { controller = "Product", action = "ProductDetail", name = UrlParameter.Optional });
            routes.MapRoute(name: "bloglisting", url: "blogs", defaults: new { controller = "Blog", action = "Blogs", url = UrlParameter.Optional });
            routes.MapRoute(name: "blogs", url: "blogs/{url}", defaults: new { controller = "Blog", action = "BlogDetail", url = UrlParameter.Optional });
            routes.MapRoute(name: "blog-category", url: "blog-category/{slug}/{currentpage}", defaults: new { controller = "Blog", action = "GetAllBlogsbyCategory", slug = UrlParameter.Optional, currentpage = UrlParameter.Optional });
            routes.MapRoute(name: "blog-type", url: "blog-type/{slug}/{currentpage}", defaults: new { controller = "Blog", action = "GetAllBlogsByType", slug = UrlParameter.Optional, currentpage = UrlParameter.Optional });
            routes.MapRoute(name: "blog-editor", url: "blog-editor/{slug}/{currentpage}", defaults: new { controller = "Blog", action = "GetAllBlogsByEditor", slug = UrlParameter.Optional, currentpage = UrlParameter.Optional });
            routes.MapRoute(name: "checkout", url: "std/{basketId}", defaults: new { controller = "Checkout", action = "StandardCheckout", basketId = UrlParameter.Optional });
            routes.MapRoute(name: "onepagecheckout", url: "opc/{basketId}", defaults: new { controller = "Checkout", action = "OnePageCheckout", basketId = UrlParameter.Optional });
            routes.MapRoute(name: "quotePayment", url: "quote/{link}", defaults: new { controller = "B2B", action = "ValidateQuotePayment", link = UrlParameter.Optional });
            routes.MapRoute(name: "dynamic-list", url: "list/{slug}", defaults: new { controller = "Search", action = "DynamicListItems", slug = UrlParameter.Optional });
            routes.MapRoute(name: "dynamic-page", url: "{slug}", defaults: new { controller = "Page", action = "DynamicPage", slug = UrlParameter.Optional });
            routes.MapRoute(name: "passwordrecovery", url: "passwordrecovery/{id}", defaults: new { controller = "Account", action = "PasswordRecovery", id = UrlParameter.Optional });
           
            //Payment Method post notification route handlers
            routes.MapRoute(name: "MasterCardNotification", url: "checkout/mastercardnotification", defaults: new { controller = "MasterCard", action = "Notification", id = UrlParameter.Optional });
            routes.MapRoute(name: "MasterCardCheck3DSecure", url: "checkout/MasterCardCheck3DSecure", defaults: new { controller = "MasterCard", action = "Check3DSecure", id = UrlParameter.Optional });
            routes.MapRoute(name: "MasterCardAuthorize", url: "checkout/MasterCardAuthorize", defaults: new { controller = "MasterCard", action = "Authorize", id = UrlParameter.Optional });

            routes.MapRoute(name: "Paypalnotification", url: "checkout/Paypalnotification", defaults: new { controller = "Paypal", action = "Notification", id = UrlParameter.Optional });

            routes.MapRoute(name: "CODPaymentResponse", url: "checkout/PaymentResponse", defaults: new { controller = "COD", action = "PaymentResponse", id = UrlParameter.Optional });

            routes.MapRoute(name: "Default", url: "{controller}/{action}/{id}", defaults: new { controller = "Page", action = "DynamicPage", id = UrlParameter.Optional });
            // routes.MapRoute(name: "Default1", url: "{*.*}", defaults: new { controller = "Page", action = "DynamicPage", id = UrlParameter.Optional });



        }
    }
}
