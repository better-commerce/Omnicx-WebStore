using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Api.Site;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Site;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

using Omnicx.WebStore.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Omnicx.WebStore.Core.Controllers
{
    public class SurveyController : BaseController
    {
        private readonly ISurveyApi _surveyApi;
        public SurveyController(ISurveyApi surveyApi)
        {
            _surveyApi = surveyApi;
        }

        /// <summary>
        /// Capture input from the user for the selected survey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Capture(string id, string viewtype)
        {
            Guid surveyId;         
            if (Guid.TryParse(id, out surveyId) == false)
                return RedirectToPageNotFound();

            var apiResult = _surveyApi.GetSurvey(surveyId);
            var survey = apiResult.Result;          
            if (survey != null)
            {
                if (!string.IsNullOrEmpty(viewtype) && viewtype.ToLower() != UserSourceTypes.Web.ToString())
                    ViewBag.Viewtype = UserSourceTypes.Mobile.ToString();
                else
                    ViewBag.Viewtype = UserSourceTypes.Web.ToString();

             
                ViewBag.StoreId = Request.QueryString["storeId"]??"";
                if (_sessionContext.CurrentUser != null)
                {                   
                    ViewBag.Email = _sessionContext.CurrentUser.Username;
                }

                return View(CustomViews.SURVEY_CAPTURE, survey);
            }
            else
                return RedirectToPageNotFound();
        }       
        public virtual ActionResult SaveAnswer(string surveyId, string answer,string email,string searchResult, string storeId)
        {
            if(_sessionContext.CurrentUser==null && string.IsNullOrEmpty(email))
            {
                return JsonSuccess(false, JsonRequestBehavior.AllowGet);
            }
            var userId = Guid.Empty;
            if (_sessionContext.CurrentUser != null )
            {
                userId = _sessionContext.CurrentUser.UserId;
                email= _sessionContext.CurrentUser.Username;
            }
            var answerModel = new SurveyAnswerModel { SurveyId = new Guid(surveyId), UserId = userId, UserName = email, Answers = answer,SearchResult= searchResult,CreatedBy= email };
            var result = _surveyApi.SaveAnswers(answerModel);
            return JsonSuccess(true, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult GetToolboxDataByType(string id)
        {
            var result = _surveyApi.SurveyToolBoxData();
            var options = (from f in result.Result where f.FieldName.ToLower() == id.ToLower() select f.Options).FirstOrDefault();
            if (options == null) return JsonSuccess("[]", JsonRequestBehavior.AllowGet);
            var model = (from o in options select new { value = o.Value, text = o.Key }).ToList();
            return JsonSuccess(model, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult AddToBag(SurveyResponseModel surveyResponse)
        {
            var bulkAdd = new List<BasketAddModel>();            
            var stockCodes = "";
            foreach (var prod in surveyResponse.Products)
            {
                stockCodes = stockCodes + (string.IsNullOrEmpty( stockCodes)?  prod.Product.StockCode: " " + prod.Product.StockCode);
                bulkAdd.Add( new BasketAddModel { StockCode = prod.Product.StockCode, Qty = prod.qty,ParentProductId= surveyResponse.Id.ToString() });
            }
            stockCodes = Constants.SURVEY_BUNDLE_PREFIX + " " + stockCodes;
            bulkAdd.Add(new BasketAddModel { StockCode =  stockCodes, Qty = 1,ItemType=ItemTypes.DynamicBundle.GetHashCode(),ProductId= surveyResponse.Id.ToString(), ProductName = stockCodes });
            var basketApi = DependencyResolver.Current.GetService<IBasketApi>();
            var basket = basketApi.BulkAddProduct(bulkAdd);
            return JsonSuccess(basket?.Result, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult SaveAnswerBulk(Guid surveyId ,List<SurveyProfileAnswerModel> answers)
        {
            SurveyProfileModel model = new SurveyProfileModel()
            {
                SelectedAnswers = answers,
                SurveyId = surveyId
                //ProductId = productId
            };
            if(_sessionContext.CurrentUser != null && _sessionContext.CurrentUser.UserId != null)
            {
                model.UserId = _sessionContext.CurrentUser.UserId;
            }
            if(model.UserId == null || model.UserId == Guid.Empty) { model.VisitorId = Guid.NewGuid(); }
            //var apiResult = _surveyApi.SaveAnswers(model);

            var responseModel = GetLinkedProducts(surveyId, answers);

            return JsonSuccess(responseModel, JsonRequestBehavior.AllowGet);
        }

        private  SurveyResponseModel GetLinkedProducts(Guid surveyId, List<SurveyProfileAnswerModel> answers)
        {
            //Get Linked Products
            var surveyResult = _surveyApi.GetSurvey(surveyId);
            var survey = surveyResult.Result;
            var searchRequest = new SearchRequestModel
            {
                AllowFacet = false,
                Filters = new List<SearchFilter>()
            };
            //foreach (var ans in answers)
            //{
            //    var ques = survey.Questions.FirstOrDefault(x => x.RecordId == ans.QuestionId);
            //    if (ques != null)
            //    {
            //        var option = ques.InputOptions.FirstOrDefault(x => x.OptionValue.ToLower() == ans.SelectedAnswer.Split(',')[0].ToLower());
            //        if (option != null)
            //        {
            //            if (option.LinkedStockCodes != null && option.LinkedStockCodes.Any())
            //            {
            //                ans.LinkedStockCodes = option.LinkedStockCodes;
            //                foreach (var linkedStock in option.LinkedStockCodes)
            //                {
            //                    searchRequest.Filters.Add(new SearchFilter { Key = "stockCode", Value = linkedStock.Key });
            //                }
            //            }
            //        }
            //    }


            //}
            var responseModel = new SurveyResponseModel { Id = survey.RecordId, Name = survey.Name, Products = new List<SurveyProductModel>() };
            if (searchRequest.Filters.Any())
            {
                var result = SearchHelper.GetPaginatedProducts(searchRequest);
                foreach (var ans in answers)
                {
                    if (ans.LinkedStockCodes != null)
                    {
                        foreach (var opt in ans.LinkedStockCodes)
                        {
                            var prod = result.Results.FirstOrDefault(x => x.StockCode == opt.Key);
                            if (prod != null)
                            {
                                responseModel.Products.Add(new SurveyProductModel { qty = opt.Value, Product = prod });
                            }
                        }
                    }
                    

                }
            }
            return responseModel;
        }
    }
}