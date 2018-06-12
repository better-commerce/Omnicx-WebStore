using System.Web.Optimization;
using Omnicx.API.SDK.Entities;

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

            bundles.Add(new Bundle("~/bundles/jq-ang")
                .Include("~/assets/core/js-lib/jquery-1.11.0.min.js")
                .Include("~/assets/core/js-lib/angular.min.js")
                .Include("~/assets/core/js-lib/bootstrap.min.js", "~/assets/core/js-lib/modernizr.custom.js")
                .Include("~/assets/core/js-lib/jquery.password-strength.js")
                );

            //include the rzslider lib (js & css both)
            bundles.Add(new Bundle("~/bundles/custom-libs")
                .Include("~/assets/custom/libs/rzslider/rzslider.min.js")
                .Include("~/assets/custom/libs/custom-js/custom-functions.js")
                .Include("~/assets/core/js-lib/jquery.cookie.js", "~/assets/core/js-lib/front.js")
                .Include("~/assets/custom/libs/dl-mobile-menu/jquery.dlmenu.js")
                .Include("~/assets/custom/libs/megamenu/redesign.min.js")
                .Include("~/assets/custom/libs/megamenu/custom.js")
                .Include("~/assets/custom/libs/pubsub/pubsub.min.js")
                );


            var bundle = new ScriptBundle("~/bundles/app-js").IncludeDirectory("~/assets/core/js/app/", "*.js", true);
            bundle.Transforms.Clear();
            bundles.Add(bundle);

            //css bundle
            bundles.Add(new StyleBundle("~/bundles/style-css")
                .Include("~/assets/core/css/bootstrap.min.css")
                .Include("~/assets/core/css/font-awesome.css")
                .Include("~/assets/theme/" + StoreTheme + "/css/style.default.css", "~/assets/theme/" + StoreTheme + "/css/main.min.css")
                .Include("~/assets/libs/owl-carousal/owl.carousel.css", "~/assets/libs/owl-carousal/owl.theme.css", "~/assets/core/css/e-zoom.css")
                .Include("~/assets/libs/rzslider/rzslider.min.css")
                .Include("~/assets/theme/" + StoreTheme + "/css/dl-menu.css")
                );
        }
    }
}
