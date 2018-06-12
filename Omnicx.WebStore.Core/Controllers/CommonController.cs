using Omnicx.API.SDK.Api.Site;
using Omnicx.API.SDK.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Security.Application;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Core.Services.Authentication;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.WebStore.Core.Controllers
{
    public class CommonController : BaseController
    {

        private readonly IContentApi _contentApi;
        private readonly IBasketApi _basketApi;
        private readonly IAuthenticationService _authenticationService;
        public CommonController(IContentApi contentApi, IBasketApi basketApi,IAuthenticationService authenticationService)           
        {
            _contentApi = contentApi;
            _basketApi = basketApi;
            _authenticationService = authenticationService;
        }


        // GET: Common
        public ActionResult PageNotFound()
        {
            if (Request.IsAjaxRequest())
            {
                throw new HttpException(404, "Not found");
            }

            Response.StatusCode = 404;
            return View(CustomViews.PAGE_NOT_FOUND);
        }
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View(CustomViews.ERROR_500);
        }

        /// <summary>
        /// Generic Error page for scenarios where even the Error500 has an error, in that case to avoid a infinite loop of redirection and send the user to an empty error page
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            Response.StatusCode = 503;
            return View(CustomViews.ERROR);
        }

        public ActionResult GetAllFaqs( Omnicx.API.SDK.Models.Catalog.ProductDetailModel model)
        {
            //var model = new Omnicx.API.SDK.Models.Catalog.ProductDetailModel { };
            var response = _contentApi.GetFaqsCategories();
            model.FaqsCategory = response.Result.ToList();
            model.FaqsSubCategory = new List<FaqsSubCategoryModel>();
            foreach (var items in model.FaqsCategory)
            {
                if (items.Key != "0")
                {
                    var listObj = _contentApi.GetFaqsSubCategories(Convert.ToInt32(items.Key));
                    model.FaqsSubCategory.AddRange(listObj.Result);
                }
            }
            model.FaqsCategory = model.FaqsCategory.Where(x => x.Key != ConfigKeys.HideGiftOption).ToList();
            SetDataLayerVariables("", WebhookEventTypes.FaqViewed);
            return PartialView(CustomViews.FAQ_VIEW, model);
        }
        [HttpPost]
        public ActionResult ContactForm(ContactModel model)
        {
            var contactModel = new ContactModel
            {
                Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                FirstName = Sanitizer.GetSafeHtmlFragment(model.FirstName),
                LastName = Sanitizer.GetSafeHtmlFragment(model.LastName),
                Message = Sanitizer.GetSafeHtmlFragment(model.Message),
                Subject = Sanitizer.GetSafeHtmlFragment(model.Subject)
            };
            return !ModelState.IsValid ? JsonValidationError() : JsonSuccess(contactModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPersistentBasket(Guid id,Guid sourceBasketId)
        {
            var resp = _basketApi.AddPersistentBasket(id, sourceBasketId);
            return  JsonSuccess(resp, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BasketNotFound()
        {            
            return View(CustomViews.BASKET_NOT_FOUND);
        }

        public void UpdateSession(string sessionId)
        {
            if(sessionId != null)
            {
                SessionUpdateModel info = new SessionUpdateModel()
                {
                    SessionId = sessionId
                };
                _authenticationService.UpdateSession(info);
            }
            
        }
    }
}