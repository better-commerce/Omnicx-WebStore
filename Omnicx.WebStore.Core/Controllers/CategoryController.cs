using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Entities;
using Omnicx.WebStore.Core.Helpers;
using Microsoft.Security.Application;
using Omnicx.API.SDK.Helpers;

namespace Omnicx.WebStore.Core.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryApi _categoryApi;
        private readonly IProductApi _productApi;
        //private string Layout = "~/Views/Category/Layout2/"; // Add Temp variable for call Layout2 Templates

        public CategoryController(ICategoryApi categoryApi , IProductApi productApi)
        {
            _categoryApi = categoryApi;
            _productApi = productApi;
        }
        // GET: Category
        public ActionResult CategoryList()
        {
            return View(CustomViews.CATEGORY_LIST);
        }
        
        public ActionResult CategoryLanding(string categorySlug, string groupSlug, string linkSlug, string linkSlug1)
        {
            var categoryUrl = SiteUtils.GetSlugFromUrl();
            var slug = SiteUtils.GetSlugFromUrl();
            var response = _categoryApi.GetCategory(categoryUrl);
            if (response.Result == null && response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var MaincategorySlug = SiteRouteUrl.Category + "/" + categorySlug;
                response = _categoryApi.GetCategory(MaincategorySlug);
                if (response.Result == null && response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return RedirectToAction("PageNotFound", "Common");
                }
            }
            var category = response.Result;

            var search = new SearchRequestModel
            {
                CategoryIds = new List<string>(),
                BreadCrumb = "<li><a href=\"{0}\" > {1}</a></li><li>{2}</li>",
                AllowFacet = true,
                CategoryId = category.Id //Added CategoryId for search criteria
            };
            search.CategoryId = category.Id;
            var groupTypeColor = category.LinkGroups.FirstOrDefault(x => x.AttributeInputType == 6);

            if (!string.IsNullOrEmpty(linkSlug))
            {
                var group = category.LinkGroups.FirstOrDefault(x => x.Items.Any(y => y.Link == slug));
                var cat = category.SubCategories.FirstOrDefault(x => x.SubCategories.Any(y => y.Link == slug));
                if (group == null)
                {
                    if (cat != null)
                    {
                        group = new CategoryLinkGroupModel
                        {
                            GroupType = GroupTypes.SubCategoryList,
                            Link = cat.Link,
                            Items = (from s in cat.SubCategories
                                     select new CategoryLinkModel
                                     {
                                         Id = s.Id,
                                         Link = s.Link,
                                         Name = s.Name
                                     }).ToList()
                        };
                    }
                }
                if (group != null)
                {
                    var link = group.Items.Any() ? group.Items.FirstOrDefault(x => x.Link == slug) : null;
                    if (group.GroupType == GroupTypes.FeaturedBrands)
                    {
                        search.CategoryIds.Add(search.CategoryId = category.Id);
                        search.Brand = link.Name;
                    }

                    if (group.GroupType == GroupTypes.Facet)
                    {
                        search.CategoryIds.Add(search.CategoryId = category.Id);
                        search.Facet = link.Name;
                    }

                    if (group.GroupType == GroupTypes.SubCategory)
                    {
                        var subCategory = category.SubCategories.FirstOrDefault(x => link.Id == x.Id);
                        if (subCategory != null)
                            search.CategoryIds = SetCategoryId(subCategory.Id, subCategory.SubCategories);
                    }
                    if (group.GroupType == GroupTypes.SubCategory || group.GroupType == GroupTypes.SubCategoryList)
                    {
                        search.CategoryIds.Add(link.Id);
                    }
                }
                if (cat != null)
                    ViewBag.MetaInfo = cat.SubCategories.FirstOrDefault(x => x.Link == slug);
            }

            var result = SearchHelper.GetPaginatedProducts(search);

            category.ProductList = result;
            SetDataLayerVariables(category, WebhookEventTypes.CategoryViewed);
            if (string.IsNullOrEmpty(groupSlug)) return View(CustomViews.CATEGORY_LANDING, category);
            return View(CustomViews.CATEGORY_PRODUCTS, category);
        }
        private List<string> SetCategoryId(string categoryId, List<CategoryModel> cats)
        {
            var catIds = new List<string> { Sanitizer.GetSafeHtmlFragment(categoryId) };
            if (cats != null && cats.Any())
            {
                foreach (var cat in cats)
                {
                    catIds.AddRange(SetCategoryId(Sanitizer.GetSafeHtmlFragment(cat.Id), cat.SubCategories));
                }
            }
            return catIds;
        }

        public ActionResult CategoryLanding2()
        {
            return View(CustomViews.CATEGORY_LANDING);
        }
    }
}