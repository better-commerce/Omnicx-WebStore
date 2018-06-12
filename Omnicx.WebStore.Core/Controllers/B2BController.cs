using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Models.B2B;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Entities;
using Omnicx.WebStore.Core.Filters;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Core.Services.Authentication;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Controllers
{
    public class B2BController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerApi _customerRepository;
        private readonly IB2BApi _b2bRepository;
        private readonly IOrderApi _orderRepository;
        // GET: B2B

        public B2BController(IAuthenticationService authenticationService, ICustomerApi customerRepository,IB2BApi b2bRepository, IOrderApi orderRepository)
        {
             _authenticationService = authenticationService;
            _customerRepository = customerRepository;
            _b2bRepository = b2bRepository;
            _orderRepository = orderRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateRequest(CompanyRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!Regex.IsMatch(model.Password, SiteUtils.GetPasswordRegex()))
                {
                    ModelState.AddModelError("Password", "Password does not meet policy!");
                    return JsonValidationError();
                }
            }
            var user = new CustomerModel
            {
                Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                FirstName = Sanitizer.GetSafeHtmlFragment(model.FirstName),
                LastName = Sanitizer.GetSafeHtmlFragment(model.LastName),
                Mobile = Sanitizer.GetSafeHtmlFragment(model.Mobile),
                PostCode = Sanitizer.GetSafeHtmlFragment(model.PostCode),
                Telephone = Sanitizer.GetSafeHtmlFragment(model.Telephone),
                Title = Sanitizer.GetSafeHtmlFragment(model.Title),
                BusinessType = Sanitizer.GetSafeHtmlFragment(model.BusinessType),
                CompanyName = Sanitizer.GetSafeHtmlFragment(model.CompanyName),
                RegisteredNumber = Sanitizer.GetSafeHtmlFragment(model.RegisteredNumber),   
                Password = Sanitizer.GetSafeHtmlFragment(model.Password),
                Address = new CompanyAddress {
                    Address1 = Sanitizer.GetSafeHtmlFragment(model.Address1),
                    Address2 =Sanitizer.GetSafeHtmlFragment(model.Address2),
                    City = Sanitizer.GetSafeHtmlFragment(model.City),
                    State = Sanitizer.GetSafeHtmlFragment(model.State),
                    Country = Sanitizer.GetSafeHtmlFragment(model.Country),
                    PostCode = Sanitizer.GetSafeHtmlFragment(model.PostCode)
                },
            };
            var result = _customerRepository.Register(user);
            if (result.Result.IsValid)
            {
                return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelState.AddModelError("Error", "Registration Request failed!");
                return JsonValidationError();
            }
        }
        [CustomAuthorizeAttribute]
        public ActionResult Quotes()
        {
            var quotes = _b2bRepository.GetQuotes(_sessionContext.CurrentUser.UserId.ToString());
            return View(CustomViews.QUOTES_B2B,quotes.Result);
        }
        [CustomAuthorizeAttribute]
        public ActionResult Users()
        {
            var users = _b2bRepository.GetUsers(_sessionContext.CurrentUser.CompanyId);
            return View(CustomViews.USERS_B2B, users.Result);
        }
        [CustomAuthorizeAttribute]
        public ActionResult MyCompany()
        {
            if (_sessionContext.CurrentUser == null) { return RedirectToAction("SignIn"); }
            var model = new CompanyDetailModel();
            var response = _b2bRepository.GetCompanyDetail(_sessionContext.CurrentUser.UserId.ToString());
            model = response.Result;
            return View(CustomViews.MY_COMPANY_B2B, model);
        }

        [HttpPost]
        public ActionResult SaveCustomerDetail(CompanyDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            var customerModel = new CompanyDetailModel
            {
                //UserId = _sessionContext.CurrentUser.UserId,
                CompanyId = Guid.Parse(_sessionContext.CurrentUser.CompanyId),
                Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                CompanyName = Sanitizer.GetSafeHtmlFragment(model.CompanyName),
                BusinessType = Sanitizer.GetSafeHtmlFragment(model.BusinessType),
                CompanyCode = Sanitizer.GetSafeHtmlFragment(model.CompanyCode),
            };         
            var result = _b2bRepository.UpdateCompanyDetail(customerModel);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateQuote(QuoteInfoModel quote)
        {
            if(_sessionContext.CurrentUser != null) { quote.CompanyId = Guid.Parse(_sessionContext.CurrentUser.CompanyId); }
            var resp = _b2bRepository.CreateQuote(quote);
            return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuoteDetail(string quoteId)
        {
            var resp = _b2bRepository.GetQuoteDetail(quoteId);
            return JsonSuccess(resp?.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateQuotePayment(string link)
        {
            var user = _sessionContext.CurrentUser;
            var resp = _b2bRepository.ValidateQuotePayment(Sanitizer.GetSafeHtmlFragment(link));
            if (resp.Result!= null && resp.Result.QuoteId != Guid.Empty && (user == null || resp.Result.CustomerId == user.UserId.ToString()))
            {
                return RedirectToAction("OnePageCheckout", "Checkout", new { basketId = resp.Result.QuoteId });
            }
            return RedirectToAction("BasketNotFound", "Common");
        }

        public ActionResult RequestQuoteChange(string quoteNo)
        {
            var userId = _sessionContext.CurrentUser?.UserId;
            var resp = _b2bRepository.RequestQuoteChange(userId.ToString(),quoteNo);
            return JsonSuccess(resp.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddQuoteToBasket(string basketId,string basketAction)
        {          
            var basket = _b2bRepository.GetQuoteBasket(basketId, basketAction);
            return JsonSuccess(basket?.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveQuoteBasket()
        {
            _b2bRepository.RemoveQuoteBasket();
            return JsonSuccess(true, JsonRequestBehavior.AllowGet);
        }
    }
}
