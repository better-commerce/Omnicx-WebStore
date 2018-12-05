using Omnicx.WebStore.Models.Keys;
using System;
using System.Web.Optimization;


namespace Omnicx.WebStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

            //BundleTable.EnableOptimizations = true;

            var StoreTheme = ConfigKeys.StoreTheme;
            var version = ConfigKeys.CDNVersion ?? "1.0.0";
            var CDNUrl = ConfigKeys.CDNUrl;

            bundles.UseCdn = Convert.ToBoolean(ConfigKeys.UseCDN ?? "false");

            var cdnUrl = CDNUrl + "{0}?v=" + version; //Azure CDN does not support custom domain names with SSL

            bundles.Add(new ScriptBundle("~/bundles/jq-ang", string.Format(cdnUrl, "bundles/jq-ang"))
                .Include("~/assets/core/js-lib/jquery-1.11.0.min.js")
                .Include("~/assets/core/js-lib/angular.min.js")
                .Include("~/assets/core/js-lib/bootstrap.min.js", "~/assets/core/js-lib/modernizr.custom.js")
                .Include("~/assets/core/js-lib/jquery.password-strength.js")
                );

            //bundles.Add(new Bundle("~/bundles/jq-ang")
            //    //.Include("~/assets/core/js-lib/jquery-1.11.0.min.js")
            //    .Include("~/assets/core/js-lib/angular.min.js")
            //    .Include("~/assets/core/js-lib/bootstrap.min.js", "~/assets/core/js-lib/modernizr.custom.js")
            //    .Include("~/assets/core/js-lib/jquery.password-strength.js")
            //    );

            //include the rzslider lib (js & css both)
            bundles.Add(new ScriptBundle("~/bundles/custom-libs", string.Format(cdnUrl, "bundles/custom-libs"))
                .Include("~/assets/custom/libs/rzslider/rzslider.min.js")
                .Include("~/assets/custom/libs/custom-js/custom-functions.js")
                .Include("~/assets/core/js-lib/jquery.cookie.js", "~/assets/core/js-lib/front.js")
                .Include("~/assets/custom/libs/dl-mobile-menu/jquery.dlmenu.js")
                .Include("~/assets/custom/libs/megamenu/redesign.min.js")
                .Include("~/assets/custom/libs/megamenu/custom.js")
                .Include("~/assets/custom/libs/pubsub/pubsub.min.js")
                );

            //LOOKBOOK JS BUNDLE
            bundles.Add(new ScriptBundle("~/bundles/lookbook-js", string.Format(cdnUrl, "bundles/lookbook-js"))                
                .Include("~/assets/custom/libs/owl-carousal/owl.carousel.min.js")
                .Include("~/assets/custom/libs/lookbook-js/scripts.min.js")
                );

            //var bundle = new ScriptBundle("~/bundles/app-js").IncludeDirectory("~/assets/core/js/app/", "*.js", true);
            var bundle = new ScriptBundle("~/bundles/app-js", string.Format(cdnUrl, "bundles/app-js")).IncludeDirectory("~/assets/core/js/app/", "*.js", true);
            bundle.Transforms.Clear();
            bundles.Add(bundle);

            //css bundle
            bundles.Add(new StyleBundle("~/bundles/style-css", string.Format(cdnUrl, "bundles/style-css"))
                .Include("~/assets/core/css/bootstrap.min.css")
                .Include("~/assets/core/css/font-awesome.css")
                .Include("~/assets/theme/" + StoreTheme + "/css/style.default.css", "~/assets/theme/" + StoreTheme + "/css/main.min.css", "~/assets/theme/" + StoreTheme + "/css/mobile.media.css")
                .Include("~/assets/custom/libs/owl-carousal/owl.carousel.css", "~/assets/custom/libs/owl-carousal/owl.theme.css", "~/assets/core/css/e-zoom.css")
               .Include("~/assets/custom/libs/rzslider/rzslider.min.css")
                .Include("~/assets/theme/" + StoreTheme + "/css/dl-menu.css")
                );

            //LOOKBOOK CSS BUNDLE
            bundles.Add(new StyleBundle("~/bundles/lookbook-css", string.Format(cdnUrl, "bundles/lookbook-css"))
                .Include("~/assets/custom/libs/owl-carousal/animate.min.css")
                .Include("~/assets/custom/libs/owl-carousal/owl.theme.css")
                .Include("~/assets/custom/libs/owl-carousal/owl.carousel.css")
                );
        }
    }
}
