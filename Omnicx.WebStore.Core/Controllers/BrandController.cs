using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using DevTrends.MvcDonutCaching;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Core.Helpers;
using Microsoft.Security.Application;
using System.Net;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.WebStore.Core.Controllers
{
    /// <summary>
    /// Renders Brand Listing, Brand Detail, Sub Brand pages
    /// </summary>
    public class BrandController : BaseController
    {
        private readonly IBrandApi _brandApi;

        public BrandController(IBrandApi brandApi)
        {
            _brandApi = brandApi;
        }
        // GET: Brand
        /// <summary>
        /// Display the list of brands 
        /// </summary>
        /// <returns></returns>
         [DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        public ActionResult BrandList()
        {
            var brands = _brandApi.GetBrands();

            var model = new AllBrandModel
            {
                PaginationWords = SiteUtils.GetWordPagination(),

            };
            if (brands.Result.Results == null) return View(model);

            model.Brands = brands.Result.Results.Where(x => !string.IsNullOrEmpty(x.ManufacturerName)).ToList();
            model.CategoryGroups = model.Brands.GroupBy(p =>
            {
                string c = p.ManufacturerName[0].ToString();
                int n;
                return c == "+" || int.TryParse(c, out n) == true ? "#" : c;
            }).OrderBy(s => s.Key);

            foreach (IGrouping<string, BrandModel> categorygroup in model.CategoryGroups)
            {
                var a = categorygroup.Key;
                var b = categorygroup;
            }
            SetDataLayerVariables(model, WebhookEventTypes.BrandViewed);
            return View(CustomViews.BRAND_LIST, model);
        }


        /// <summary>
        /// Brand Landing page 
        /// </summary>
        /// <returns></returns>
        public ActionResult BrandLanding()
        {
            return View(CustomViews.BRAND_LANDING);
        }
        /// <summary>
        /// Brand detail page with sub brands and products
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subBrandId"></param>
        /// <returns></returns>
        public ActionResult SubBrandProducts(string id, string subBrandId)
        {
            var branddetail = _brandApi.GetBrandDetails(Sanitizer.GetSafeHtmlFragment(id));
            var productViewObject = new ProductViewModel {BrandDetailList = branddetail.Result };
            var productList = productViewObject.BrandDetailList.SubBrands.Where(product => product.Id == Sanitizer.GetSafeHtmlFragment(subBrandId));
            //productViewObject.BrandDetailList.SubBrands

            return JsonSuccess(productList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Search results for brand's products
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
         [DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        public ActionResult BrandDetail(string name)
        {
            var slug = SiteUtils.GetSlugFromUrl();
            return GetBrandProducts(slug);
        }

        private string[] GetWordPagination()
        {
            string[] words = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };
            return words;
        }

        public ActionResult BrandProducts(string name)
        {
            var slug = SiteUtils.GetSlugFromUrl()?.Replace("/all","");
            bool showListing = SiteUtils.GetSlugFromUrl().Contains("all");
            return GetBrandProducts(slug, showListing);
        }

        public ActionResult GetBrandProducts(string slug, bool showListing=false)
        {
            var branddetail = _brandApi.GetBrandDetailsBySlug(slug);
            if (branddetail.Result != null)
            {
                if (branddetail.Result.ShowLandingPage == true && !showListing) return View(CustomViews.BRAND_LANDING, branddetail.Result);
            }
            var searchCriteria = new SearchRequestModel { BrandId = branddetail.Result != null ? branddetail.Result.Id : null, AllowFacet = true, BreadCrumb = branddetail.Result != null ? branddetail.Result.Name:"" };
            var paginatedModel = SearchHelper.GetPaginatedProducts(searchCriteria);
            if (branddetail.Result != null)
            {
                branddetail.Result.ProductList = paginatedModel;
            }
            //products are already fetched in brandDetail call so no need to call searchResult api again.
            //var products = new List<ProductModel>();

            //if (branddetail != null)
            //{
            //    foreach (var subbrand in branddetail.SubBrands)
            //    {
            //        products.AddRange(subbrand.Products);
            //    }

            //}

            //var paginatedModel = new PaginatedResult<ProductModel>
            //{
            //    Results = products,
            //    Total = products.Count,
            //    CurrentPage = 1,
            //    Pages = products.Count / Convert.ToInt32(ConfigKeys.PageSize),
            //};
            if (branddetail.Result == null && branddetail.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToPageNotFound();
            }
            SetDataLayerVariables(branddetail.Result, WebhookEventTypes.BrandViewed);
            return View(CustomViews.BRAND_PRODUCTS, branddetail.Result);
        }

    }
}