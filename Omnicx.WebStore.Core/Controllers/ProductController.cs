using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using DevTrends.MvcDonutCaching;
using Omnicx.API.SDK.Api.Catalog;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Models.Catalog;
using Microsoft.Security.Application;
using System.Net;

using Omnicx.API.SDK.Api.Site;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.FeatureToggle;
using System.Web;

namespace Omnicx.WebStore.Core.Controllers
{
    /// <summary>
    /// For rendering Product Detail page 
    /// </summary>
    public class ProductController : BaseController
    {
        private readonly IProductApi _productApi;
        private readonly ISurveyApi _surveyApi;
        private readonly IBrandApi _brandApi;

        public ProductController(IProductApi productApi, IBrandApi brandApi, ISurveyApi surveyApi)
        {
            _productApi = productApi;
            _brandApi = brandApi;
            _surveyApi = surveyApi;
        }
        // GET: Product

         [DonutOutputCache(CacheProfile = "DefaultCacheProfile", Location = OutputCacheLocation.Server)]
        public ActionResult ProductDetail(string name)
        {
            var slug = SiteUtils.GetSlugFromUrl();
            var product = _productApi.GetProductDetailBySlug(Sanitizer.GetSafeHtmlFragment(slug));
            if (product == null || (product.Result == null && product.StatusCode == HttpStatusCode.NotFound) 
                || (product.Result != null && product.Result.Classification != null && product.Result.Classification.ItemType == ItemTypes.Addon.GetHashCode().ToString()))
            {
                return RedirectToPageNotFound();
            }
            SetDataLayerVariables(product.Result, WebhookEventTypes.ProductViewed);
            //product.Result.BrandSlug = _brandApi.GetBrandDetails(Sanitizer.GetSafeHtmlFragment(product.Result.BrandRecordId)).Result?.Link;
            return View(CustomViews.PRODUCT_DETAIL, product.Result);
        }
        public ActionResult ProductDetails(string id)
        {
            var product = _productApi.GetProductDetail(Sanitizer.GetSafeHtmlFragment(id));
            if(product.Result!=null)
                product.Result.BrandSlug = _brandApi.GetBrandDetails(Sanitizer.GetSafeHtmlFragment(product.Result.BrandRecordId)).Result?.Link;
            _headTagbuilder.AddDataLayer("product", product.Result);
            return JsonSuccess(product.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddProductReview(string id, ProductReviewAddModel productReview)
        {
            var productReviewAddModel = new ProductReviewAddModel
            {
                Comment = Sanitizer.GetSafeHtmlFragment(productReview.Comment),
                Gender = Sanitizer.GetSafeHtmlFragment(productReview.Gender),
                Location = Sanitizer.GetSafeHtmlFragment(productReview.Location),
                Nickname = Sanitizer.GetSafeHtmlFragment(productReview.Nickname),
                Title = Sanitizer.GetSafeHtmlFragment(productReview.Title),
                UserEmail = Sanitizer.GetSafeHtmlFragment(productReview.UserEmail),
                Rating = productReview.Rating,
                Age = productReview.Age,
                AdditionalData = productReview.AdditionalData,
                IsRecommended = productReview.IsRecommended,
                RemainAnonymous = productReview.RemainAnonymous,
                UserId = _sessionContext.CurrentUser != null ? _sessionContext.CurrentUser.UserId.ToString() : null,
                ReviewSections = productReview.ReviewSections
            };
            if(_sessionContext.CurrentUser!=null)
            {
                productReviewAddModel.UserEmail = _sessionContext.CurrentUser.Email;
            }
            var response = _productApi.AddProductReview(Sanitizer.GetSafeHtmlFragment(id), productReviewAddModel);
            return JsonSuccess(response.Result , JsonRequestBehavior.AllowGet);
        }
        public ActionResult VariantTestPage()
        {
            return View();
        }

        /// <summary>
        /// Returns the Configurable Review Blocks.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProductReviewConfig()
        {
            return JsonSuccess(_sessionContext.CurrentSiteConfig.ReviewSettings, JsonRequestBehavior.DenyGet);
        }

        public ActionResult QuestionnaireCode(string questionnaireCode)
        {
            var survey = _surveyApi.GetSurveyByCode(questionnaireCode);
            if (survey != null)
            {
                foreach (var question in survey.Result.Questions)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(question.InputDataAndStyle)) && Convert.ToString(question.InputDataAndStyle).Contains("_"))
                    {
                        question.InputDataType = (SurveyInputDataTypes)Enum.Parse(typeof(SurveyInputDataTypes), question.InputDataAndStyle.ToString().Split('_').First());
                        question.InputStyle = (SurveyInputStyle)Enum.Parse(typeof(SurveyInputStyle), question.InputDataAndStyle.ToString().Split('_')[1]);
                    }
                }
            }
            return JsonSuccess(survey, JsonRequestBehavior.AllowGet);
        }
    }
}