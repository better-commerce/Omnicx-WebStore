using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.API.SDK.Api.Site;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models.Site;
using Omnicx.API.SDK.Entities;
using Omnicx.WebStore.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Omnicx.WebStore.Core.Controllers
{
    public class SurveyController : BaseController
    {
        private readonly ISurveyApi _surveyApi;

        public SurveyController(ISurveyApi surveyApi, ISessionContext sessionContext)
        {
            _surveyApi = surveyApi;
        }

        /// <summary>
        /// Capture input from the user for the selected survey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Capture(string id)
        {
            Guid surveyId;
            if (Guid.TryParse(id, out surveyId) == false)
                RedirectToAction("PageNotFound", "Common");

            var apiResult = _surveyApi.GetSurvey(surveyId);
            var survey = apiResult.Result;

            if (survey != null)
            {
                foreach (var question in survey.Questions)
                {
                    if(!string.IsNullOrEmpty(Convert.ToString(question.InputDataAndStyle)) && Convert.ToString(question.InputDataAndStyle).Contains("_"))
                    {
                        question.InputDataType = (SurveyInputDataTypes)Enum.Parse(typeof(SurveyInputDataTypes), question.InputDataAndStyle.ToString().Split('_').First());
                        question.InputStyle = (SurveyInputStyle)Enum.Parse(typeof(SurveyInputStyle), question.InputDataAndStyle.ToString().Split('_')[1]);
                    }
                }
                return View(CustomViews.SURVEY_CAPTURE, survey);
            }
            else
                return RedirectToAction("PageNotFound", "Common");
        }

        public ActionResult SaveAnswer(string questionId, string answer)
        {
            return JsonSuccess("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddToBag(SurveyResponseModel surveyResponse)
        {
            var bulkAdd = new List<BasketAddModel>();            
            var stockCodes = "";
            foreach (var prod in surveyResponse.Products)
            {
                stockCodes = stockCodes + (string.IsNullOrEmpty( stockCodes)?  prod.Product.StockCode: " " + prod.Product.StockCode);
                bulkAdd.Add( new BasketAddModel { StockCode = prod.Product.StockCode, Qty = prod.qty});
            }
            stockCodes = Omnicx.API.SDK.Entities.Constants.SURVEY_BUNDLE_PREFIX + " " + stockCodes;
            bulkAdd.Add(new BasketAddModel { StockCode =  stockCodes, Qty = 1,ItemType=ItemTypes.DynamicBundle.GetHashCode(),ProductId= surveyResponse.Id.ToString(), ProductName = stockCodes });
            var basketApi = DependencyResolver.Current.GetService<IBasketApi>();
            var basket = basketApi.BulkAddProduct(bulkAdd);
            return JsonSuccess(basket?.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAnswerBulk(Guid surveyId ,List<SurveyProfileAnswerModel> answers)
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
            var apiResult = _surveyApi.SaveAnswers(model);

            var responseModel = GetLinkedProducts(surveyId, answers);

            return JsonSuccess(responseModel, JsonRequestBehavior.AllowGet);
        }

        private SurveyResponseModel GetLinkedProducts(Guid surveyId, List<SurveyProfileAnswerModel> answers)
        {
            //Get Linked Products
            var surveyResult = _surveyApi.GetSurvey(surveyId);
            var survey = surveyResult.Result;
            var searchRequest = new SearchRequestModel
            {
                AllowFacet = false,
                Filters = new List<SearchFilter>()
            };
            foreach (var ans in answers)
            {
                var ques = survey.Questions.FirstOrDefault(x => x.RecordId == ans.QuestionId);
                if (ques != null)
                {
                    var option = ques.InputOptions.FirstOrDefault(x => x.OptionValue.ToLower() == ans.SelectedAnswer.ToLower());
                    if (option != null)
                    {
                        if (option.LinkedStockCodes != null && option.LinkedStockCodes.Any())
                        {
                            ans.LinkedStockCodes = option.LinkedStockCodes;
                            foreach (var linkedStock in option.LinkedStockCodes)
                            {
                                searchRequest.Filters.Add(new SearchFilter { Key = "stockCode", Value = linkedStock.Key });
                            }
                        }
                    }
                }


            }
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