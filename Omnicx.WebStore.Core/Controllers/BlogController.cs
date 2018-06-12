using System;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Site;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Site;
using Microsoft.Security.Application;
using Omnicx.WebStore.Core.Helpers;
using System.Net;
using Omnicx.API.SDK.Entities;
namespace Omnicx.WebStore.Core.Controllers
{
    public class BlogController : BaseController
    {
       
        private readonly IBlogApi _blogApi;
        public BlogController(IBlogApi blogApi)
            
        {
            _blogApi = blogApi;
        }
        // GET: Blog
        public ActionResult Blogs()
        {
            var results = _blogApi.BlogGroups();
            int pageSize = Convert.ToInt32(ConfigKeys.PageSize);
            var blogs = _blogApi.BlogByGroup(0, "0", 1, pageSize);
            foreach (var res in blogs.Result.Results)
            {
                res.Days = ToDays(res.Created);
            } 
            var result = new BlogDetailViewModel
            {
                Categories = results.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                Editors = results.Result.Editors,
                BlogList = blogs.Result,
                BlogTypes = results.Result.BlogTypes
            };
            return View(CustomViews.BLOGS, result);
        }

        public int ToDays(DateTime created)
        {
            var Created = created != DateTime.MinValue ? created : DateTime.Now;
            int days = DateTime.Now.Subtract(Created).Days;
            return days;
        }
        public ActionResult ShowMoreBlogs(string currentPage)
        {
            var results = _blogApi.BlogByGroup(0, "0", Convert.ToInt32(Sanitizer.GetSafeHtmlFragment(currentPage)), 3);
            foreach (var res in results.Result.Results)
            {
                res.Days = ToDays(res.Created);
            } 
            return JsonSuccess(results.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlogsbyCategory(string id, int currentPage)
        {
            var resp = _blogApi.BlogByGroup(BlogGroupType.Category.GetHashCode(), Sanitizer.GetSafeHtmlFragment(id), currentPage, Convert.ToInt32(ConfigKeys.PageSize));
            if (resp != null)
            {
                foreach (var res in resp.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = resp.Result,
                BlogTypes = category.Result.BlogTypes
            };
            if (resp != null && resp.Result.Results.Count > 0)
            ViewBag.selectedGroup = resp.Result.Results[0].Category.ToCamelCase();
            else
              ViewBag.selectedGroup = "";
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.BlogViewed);
            return View(CustomViews.BLOG_CATEGORY, result);
        }

        public ActionResult GetAllBlogsbyCategory(string slug, int currentpage=1)
        {
           // var slug = SiteUtils.GetSlugFromUrl();
            var resp = _blogApi.BlogByGroup(BlogGroupType.Category.GetHashCode(), slug, currentpage, Convert.ToInt32(ConfigKeys.PageSize));
            if (resp == null && resp.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToAction("pagenotfound", "common");
            }
            if (resp != null)
            {
                foreach (var res in resp.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = resp.Result,
                BlogTypes = category.Result.BlogTypes
            };
            result.Slug = slug;
            if (resp != null && resp.Result.Results.Count > 0)
                ViewBag.selectedGroup = resp.Result.Results[0].Category.ToCamelCase();
            else
                ViewBag.selectedGroup = "";
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.FacetSearch);
            return View(CustomViews.BLOG_CATEGORY, result);
        }
        public ActionResult GetAllBlogsByType(string slug, int currentpage=1)
        {
            var results = _blogApi.BlogByGroup(BlogGroupType.BlogType.GetHashCode(), slug, currentpage, Convert.ToInt32(ConfigKeys.PageSize));
            if (results == null && results.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToAction("pagenotfound", "common");
            }
            if (results != null)
            {
                foreach (var res in results.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = results.Result,
                BlogTypes = category.Result.BlogTypes
            };
            if (results != null && results.Result.Results.Count > 0)
                ViewBag.selectedGroup = results.Result.Results[0].Author.ToCamelCase();
            else
                ViewBag.selectedGroup = "";
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.FacetSearch);
            return View(CustomViews.BLOG_CATEGORY, result);
        }
        public ActionResult BlogDetail(string url)
        {
            var blog = _blogApi.BlogDetail(Sanitizer.GetSafeHtmlFragment(url));
            if (blog.Result == null && blog.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("pagenotfound", "common");
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Detail = blog.Result,
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogTypes = category.Result.BlogTypes

            };
            ViewBag.selectedGroup =string.IsNullOrEmpty(blog.Result.Category)?"": blog.Result.Category.ToCamelCase();
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.BlogViewed);
            return View(CustomViews.BLOGS_DETAIL, result);
        }
        public ActionResult GetBlogsByEditor(string groupId,int groupType,int currentPage)
        {
            groupType=groupType==0?BlogGroupType.Editor.GetHashCode():groupType;
            var results = _blogApi.BlogByGroup(groupType, Sanitizer.GetSafeHtmlFragment(groupId) , currentPage , Convert.ToInt32(ConfigKeys.PageSize));
            if (results != null)
            {
                foreach (var res in results.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = results.Result,
                BlogTypes = category.Result.BlogTypes
            };
            if (results != null && results.Result.Results.Count > 0)
                ViewBag.selectedGroup = results.Result.Results[0].Author.ToCamelCase();
            else
                ViewBag.selectedGroup = "";
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.FacetSearch);
            return View(CustomViews.BLOG_BY_EDITOR, result);
        }

        public ActionResult GetAllBlogsByEditor(string slug, int currentpage = 1)
        {
            var results = _blogApi.BlogByGroup(BlogGroupType.Editor.GetHashCode(), slug, currentpage, Convert.ToInt32(ConfigKeys.PageSize));
            if (results == null && results.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToAction("pagenotfound", "common");
            }
            if (results != null)
            {
                foreach (var res in results.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = results.Result,
                BlogTypes = category.Result.BlogTypes
            };
            if (results != null && results.Result.Results.Count > 0)
                ViewBag.selectedGroup = results.Result.Results[0].Author.ToCamelCase();
            else
                ViewBag.selectedGroup = "";
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.FacetSearch);
            return View(CustomViews.BLOG_BY_EDITOR, result);
        }
        public ActionResult Search(string search,int currentPage)
        {
            var response = _blogApi.BlogSearch(Sanitizer.GetSafeHtmlFragment(search), currentPage,Convert.ToInt32(ConfigKeys.PageSize));
            if (!String.IsNullOrEmpty(Sanitizer.GetSafeHtmlFragment(search)))
            {
                foreach (var res in response.Result.Results)
                {
                    res.Days = ToDays(res.Created);
                }
            }
            var category = _blogApi.BlogGroups();
            var result = new BlogDetailViewModel
            {
                FreeText = Sanitizer.GetSafeHtmlFragment(search),
                Categories = category.Result.Categories.Where(x => x.GroupCount > 0).ToList(),
                BlogList = response.Result,
                BlogTypes = category.Result.BlogTypes
            };
            result.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            SetDataLayerVariables(result, WebhookEventTypes.FacetSearch);
            return View(CustomViews.BLOG_SEARCH, result);
        }
    }
}