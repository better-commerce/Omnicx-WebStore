using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.API.SDK.Api.Site;
using System.Threading.Tasks;
using Omnicx.API.SDK.Api.Catalog;

namespace Omnicx.Site.Core.Controllers
{
    public class LookbookController : Controller
    {
        // GET: Lookbook
        private readonly ICollectionApi _collectionApi;
        public LookbookController(ICollectionApi collectionApi)
        {
            _collectionApi = collectionApi;
        }
        public ActionResult Index()
        {
            var response = _collectionApi.GetAllLookbooksBySlug("lookbook/");
            return View(response.Result);
           
        }
       
        public ActionResult LookbookDetail()
        {
            var slug = SiteUtils.GetSlugFromUrl();
            var response = _collectionApi.GetCollectionBySlug(slug);
            return View(CustomViews.LOOKBOOK_DETAIL, response.Result);            
        }
    }
}