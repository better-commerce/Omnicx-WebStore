using System;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Commerce;
using Microsoft.Security.Application;
using Omnicx.API.SDK.Payments;
using Omnicx.WebStore.Core.Services.Authentication;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

using Omnicx.WebStore.Models;

namespace Omnicx.WebStore.Core.Controllers
{
    public class CheckoutController : BaseController
    {
        private readonly ICustomerApi _customerApi;
        private readonly IConfigApi _configApi;
        private readonly IPaymentApi _paymentApi;
        private readonly ICheckoutApi _checkoutApi;
        private readonly IOrderApi _orderApi;
        private readonly IShippingApi _shippingApi;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerApi _customerRepository;
        private readonly IB2BApi _b2bRepository;
        // private readonly IPaymentMethod _paymentMethod;

        public CheckoutController(ICustomerApi customerApi, IConfigApi configApi,
            IPaymentApi paymentApi , ICheckoutApi checkoutApi , IOrderApi orderApi , IShippingApi shippingApi , IAuthenticationService authenticationService , ICustomerApi customerRepository, IB2BApi b2bRepository)
        {
            _customerApi = customerApi;
            _configApi = configApi;
            _paymentApi = paymentApi;
            _checkoutApi = checkoutApi;
            _orderApi = orderApi;
            _shippingApi = shippingApi;
            _authenticationService = authenticationService;
            _customerRepository = customerRepository;
            _b2bRepository = b2bRepository;
        }
        // GET: Checkout
        public virtual ActionResult OnePageCheckout(string basketId)
        {
            var model = GetCheckoutData(basketId);
            if(model==null)
                RedirectToAction("BasketNotFound", "Common");            
            return View(CustomViews.ONE_PAGE_CHECKOUT, model);
        }

        protected CheckoutViewModel GetCheckoutData(string basketId)
        {
            var response = _checkoutApi.Checkout(Sanitizer.GetSafeHtmlFragment(basketId));

            var checkout = response.Result;
            if (checkout.BasketId == null || checkout.Basket == null || checkout.Basket.LineItems.Count < 1)
            {
                return null; 
            }
            foreach (var pay in checkout.PaymentOptions)
            {
                pay.CardInfo.Amount = checkout.BalanceAmount.Raw.WithTax;
            }
            checkout.LanuguageCode = _sessionContext.CurrentSiteConfig.RegionalSettings.DefaultLanguageCulture;
            checkout.CurrencyCode = _sessionContext.CurrentSiteConfig.RegionalSettings.DefaultCurrencyCode;
            var result = _configApi.GetConfig();
            var data = result.Result;
            //checkout.Basket.shippingMethods = 0  is simpl check this implementation later
            data.ShippingCountries = data.ShippingCountries.Where(x => checkout.Basket.shippingMethods.Any(y => y.CountryCode == x.TwoLetterIsoCode) || checkout.Basket.shippingMethods.Count() == 0).Distinct().ToList();
            var model = new CheckoutViewModel
            {
                Checkout = checkout,
                Register = new RegistrationModel(),
                Login = new LoginViewModel(),
                BillingCountries = data.BillingCountries,
                ShippingCountries = data.ShippingCountries,
                CurrentDate = DateTime.Today.Date.AddDays(1)
            };

            if (_sessionContext.CurrentUser == null)
            {
                model.RegistrationPrompt = Convert.ToBoolean(_sessionContext.CurrentSiteConfig.BasketSettings.RegistrationPrompt);
            }

            string returnUrl = string.Empty;
            //So that the user can be referred back to where they were when they click logon
            if (Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            //if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["ReturnURL"] = returnUrl;
            }
            if (_sessionContext.CurrentUser == null)
            {
                if (Guid.Parse(checkout.CustomerId) != Guid.Empty)
                {
                    var customerresult = _customerRepository.GetUserdetailsById<CustomerDetailModel>(checkout.CustomerId);
                    if (customerresult != null)
                    {
                        checkout.Email = customerresult.Result.Email;
                        checkout.CompanyId = customerresult.Result.CompanyId;
                    }
                }
                else
                {
                    _checkoutApi.UpdateUserToBasket(checkout.BasketId, Guid.Empty.ToString());
                    checkout.Stage = BasketStage.Anonymous.GetHashCode();
                }
                SetDataLayerVariables(response.Result?.Basket, WebhookEventTypes.CheckoutStarted);
                return model;
            }
            model.Checkout.CustomerId = _sessionContext.CurrentUser.UserId.ToString();
            model.Checkout.Email = _sessionContext.CurrentUser.Email;
            var WishlistResponse = _customerRepository.GetWishlist(model.Checkout.CustomerId, true);
            model.Checkout.WishlistProducts = WishlistResponse.Result;
            SetDataLayerVariables(response.Result?.Basket, WebhookEventTypes.CheckoutStarted);
            return model;
        }
        public ActionResult UpdateBasketDeliveryAddress(CheckoutModel checkout)
        {
            var response = _checkoutApi.UpdateBasketDeliveryAddress(Sanitizer.GetSafeHtmlFragment(checkout.BasketId), checkout);
            
            return JsonSuccess(new { response = response, BasketStage = BasketStage.ShippingAddressProvided.GetHashCode() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConvertToOrder(CheckoutModel checkout)
        {

            if(!string.IsNullOrEmpty(checkout.CompanyId) && _sessionContext.CurrentUser == null && checkout.CompanyId != Guid.Empty.ToString())
            {
                //execute when company user tries to place order via guest checkout. 
                return JsonSuccess("" , JsonRequestBehavior.DenyGet);
            }
            if (checkout.CustomerId == Guid.Empty.ToString() || checkout.CustomerId == null) {
                var user = new CustomerModel {
                    Email = Sanitizer.GetSafeHtmlFragment(checkout.Email),
                    Password = Sanitizer.GetSafeHtmlFragment(checkout.Password),
                    SourceProcess = SourceProcessType.SITE_CHECKOUTGUEST.ToString()
                };

               var responseTemp = _customerRepository.GetExistingUser(user.Email);              
                if (responseTemp.Result.Count > 0) {
                    checkout.CustomerId = responseTemp.Result[0].UserId.ToString();
                } else {
                    var result = _customerRepository.Register(user);
                    if (result.Result.IsValid) {
                        checkout.CustomerId = result.Result.RecordId;
                    }
                }
            }

            checkout.Payment = new PaymentModel
            {
                PaymentGatewayId = checkout.SelectedPayment.Id,
                PaymentGateway = checkout.SelectedPayment.SystemName,
                OrderAmount = checkout.SelectedPayment.CardInfo.Amount,
                Status = PaymentStatus.Pending.GetHashCode()
            };
            var response = _checkoutApi.ConvertToOrder(Sanitizer.GetSafeHtmlFragment(checkout.BasketId), checkout);           
            if (response.Result==null) {
                return JsonSuccess(response, JsonRequestBehavior.AllowGet);
            }
           
            _b2bRepository.RemoveQuoteBasket(); 
            var order = response.Result;
            var paymentRequest=new ProcessPaymentRequest
            {
                BasketId = checkout.BasketId,
                CurrencyCode = order.CurrencyCode,
                CustomerId = checkout.CustomerId,
                LanuguageCode = _sessionContext.CurrentSiteConfig.RegionalSettings.DefaultLanguageCulture,
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                PaymentId = order.Payment.Id,
                UserEmail = checkout.Email,
                OrderTotal = order.Payment.OrderAmount,
                Order= order 
            };
            if (!string.IsNullOrEmpty(checkout.SelectedPayment.CardInfo?.CardNo) && !string.IsNullOrEmpty(checkout.SelectedPayment.CardInfo.SecurityCode) && checkout.SelectedPayment.CardInfo.Amount>0)
            {
                paymentRequest.CardNo = checkout.SelectedPayment.CardInfo.CardNo;
                paymentRequest.Cvv = checkout.SelectedPayment.CardInfo.SecurityCode;
                paymentRequest.OrderTotal = checkout.SelectedPayment.CardInfo.Amount;
            }
            if (checkout.SelectedPayment.SystemName != Convert.ToString(PaymentMethodTypes.AccountCredit))
            {
                var payResponse = _checkoutApi.PaymentSetting(checkout.SelectedPayment.SystemName);
                checkout.SelectedPayment = payResponse.Result;
            }
            var paymentResponse = checkout.SelectedPayment.ProcessPayment(paymentRequest);
          
            if (paymentResponse.Success && paymentResponse.AuthorizedAmount>0)
            {
                order.Payment.IsValid = true;
                order.Payment.Status = PaymentStatus.Authorized.GetHashCode();
                order.Payment.OrderAmount = paymentResponse.AuthorizedAmount;
                order.Payment.AuthCode = paymentResponse.AuthorizationTransactionCode;
                order.Payment.CardNo = paymentRequest.CardNo;
                order.Payment.PspResponseCode = paymentRequest.PspSessionCookie;

                var paymentResult = _checkoutApi.UpdatePayment(order.Id, order.Payment);
                paymentResponse.BalanceAmount = paymentResult.Result?.BalanceAmount;
            }
            else
            {
                order.Payment.IsValid = false;
                order.Payment.Status = PaymentStatus.Pending.GetHashCode();
                order.Payment.AuthCode = paymentResponse.AuthorizationTransactionCode;
                order.Payment.PspSessionCookie = paymentResponse.PspSessionCookie;
                var paymentResult = _checkoutApi.UpdatePayment(order.Id, order.Payment);
                //paymentResponse.RefOrderId = order.Payment.Id;
            }
           

            return JsonSuccess(paymentResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult OrderConfirmation()
        {
            return RedirectToAction("PageNotFound", "Common", new { @aspxerrorpath = "/checkout/OrderConfirmation" });
        }

        [HttpPost]
        public ActionResult OrderConfirmation(string id)
        {
            var response = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(id));
            SetDataLayerVariables(response.Result, WebhookEventTypes.CheckoutConfirmation);
            return View(CustomViews.ORDER_CONFIRMATION, response.Result);
        }
        
        public ActionResult GetPaymentMethods()
        {
            var paymentMethods = _paymentApi.GetPaymentMethods();
            return JsonSuccess(paymentMethods.Result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult StandardCheckout(string basketId)
        {
            var responxse = _checkoutApi.Checkout(Sanitizer.GetSafeHtmlFragment(basketId));
            var checkout = responxse.Result;
            var result = _configApi.GetConfig();
            var data = result.Result;          
            var model = new CheckoutViewModel
            {
                Checkout = checkout,
                Register = new RegistrationModel() ,
                Login = new LoginViewModel(),
                BillingCountries = data.BillingCountries,
                ShippingCountries = data.ShippingCountries,
                CurrentDate = DateTime.Today.Date
            };

            if (_sessionContext.CurrentUser != null)
            {
                model.Checkout.CustomerId = _sessionContext.CurrentUser.UserId.ToString();
                model.Checkout.Email = _sessionContext.CurrentUser.Email;
                var wishlistResponse= _customerRepository.GetWishlist(model.Checkout.CustomerId, true);
                model.Checkout.WishlistProducts = wishlistResponse.Result;
                //var address = _customerApi.GetCustomerAddress<AddressModel>(_sessionContext.CurrentUser.UserId.ToString());
                //if (address.Any())
                //{
                //    model.Checkout.BillingAddress = address.Last();
                //}
            }
            return View(CustomViews.STANDARD_CHECKOUT, model);
        }

        public ActionResult GetClickAndCollectOptions(string basketId, string postCode)
        {
            var stores = _shippingApi.GetClickAndCollectOptions(basketId, postCode);
            return JsonSuccess(stores.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetNominatedDelivery(string startDate, ShippingModel shipMethod)
        {
            var date = DateTime.Parse(startDate).ToString("yyyy-MM-dd");
            var currentTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if(shipMethod.CutOffTimes != null)
            {
                foreach (var dayTime in shipMethod.CutOffTimes)
                {
                    var cutOffTime = new TimeSpan(Convert.ToInt32(dayTime.Hour), Convert.ToInt32(dayTime.Minute), 0);

                    if (dayTime.Day.ToLower() == DateTime.Now.DayOfWeek.ToString().ToLower() && currentTime > cutOffTime)
                    {
                        date = DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd");
                    }
                }
            }
            var days = _shippingApi.GetNominatedDays(date);
            return JsonSuccess(days.Result, JsonRequestBehavior.AllowGet);
        }

        #region MOVED TO CODCONTROLLER
        /// <summary>
        /// Payement response accepted for Cash on Delivery
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult PaymentResponse(string id)
        //{
        //    var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(id));
        //    var order = orderResponse.Result;
        //    //order.Payment.IsValid = true;
        //    //order.Payment.Status = order.Payment.IsValid.GetHashCode();
        //    //_checkoutApi.UpdatePayment(order.Id, order.Payment);
        //    var response = new BoolResponse { IsValid = true, RecordId = order.Id };
        //    SetDataLayerVariables(order, DataLayerEventType.CheckoutPayment);
        //    return View(CustomViews.PAYMENT_RESPONSE, response);
        //}
        #endregion 

        #region MOVED TO MASTERCARD CONTROLLER
        //#region "mastercard payment"
        //   public ActionResult MasterCardNotification()
        //    {
        //        var response = new BoolResponse { IsValid = false, Message = "" };
        //        response.RecordId = Request.Params["bid"];
        //    // this can hppen in situation when bid is suffixed to eth query string twice breaking the whole code
        //    /// so a simple handling fro the same has been put in
        //    /// 
        //    if (response.RecordId.Contains(","))
        //    {
        //        var tmpId = response.RecordId.Split(',');
        //        response.RecordId = tmpId[0];
        //    }
        //    PaymentModel payment = null;
        //         var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
        //        var setting = settingResponse.Result;

        //        var mcard = new MasterCardApi(setting);
        //        if (Request["orderId"] != null)
        //        {
        //            var refOrderId = Request["transId"];
        //            string orderId = Request.Params["orderId"];
        //            var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
        //            var order = orderResponse.Result;
        //            string paymentId = orderId.Split('-')[1];
        //            payment = order.Payments.FirstOrDefault(x => x.Id == paymentId);
        //            var paymentRequest = new PostProcessPaymentRequest
        //            {
        //                CurrencyCode = order.CurrencyCode,
        //                Order = order,
        //                OrderTotal = payment.OrderAmount,
        //                Payment = payment,
        //            };
        //            var paymentResponse = setting.PostProcessPayment(paymentRequest);
        //            _checkoutApi.UpdatePayment(refOrderId, paymentResponse.Payment);
        //            response.RecordId = order.Id;
        //            response.IsValid = paymentResponse.Success;
        //            if (!response.IsValid)
        //            {
        //                response.RecordId = Request.Params["bid"];
        //            if (paymentResponse.Errors.Any())
        //                response.Message = paymentResponse.Errors[0];
        //            }
        //        }

        //        return View(CustomViews.PAYMENT_RESPONSE, response);
        //    }

        //    public ActionResult MasterCardCheck3DSecure(string sessionId, decimal amount, string currency, string secure3DauthUrl,
        //        string secure3DId)
        //    {
        //        var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
        //        var setting = settingResponse.Result;
        //        var mcard = new MasterCardApi(setting);
        //        var secure3D = mcard.Check3DSecureEnrollment(sessionId, amount, currency, secure3DauthUrl, secure3DId);
        //        return JsonSuccess(secure3D, JsonRequestBehavior.AllowGet);
        //    }

        //    public ActionResult MasterCardAuthorize(string sessionId, string year, string month, decimal amount, string orderId,
        //        string transId, string currency)
        //    {
        //    var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
        //    var setting = settingResponse.Result;
        //    Billing billingAddress = new Billing();
        //    billingAddress.address = new Address() { };
        //    var orderDetail = _orderApi.GetOrdDetail(orderId);

        //    billingAddress.address.country = Utils.GetThreeChracterCountryCode(orderDetail.Result.BillingAddress.CountryCode); // "GBR";//orderDetail.Result.BillingAddress.CountryCode;
        //    if (string.IsNullOrEmpty(billingAddress.address.country) == true)
        //    {
        //        billingAddress.address.country = "GBR";
        //    }
        //    billingAddress.address.postcodeZip = orderDetail.Result.BillingAddress.PostCode;
        //    billingAddress.address.street = orderDetail.Result.BillingAddress.Address1;
        //    billingAddress.address.city = orderDetail.Result.BillingAddress.City;
        //    if (!string.IsNullOrEmpty(orderDetail.Result.BillingAddress.Address2))
        //        billingAddress.address.street2 = orderDetail.Result.BillingAddress.Address2;
        //    billingAddress.address.stateProvince = orderDetail.Result.BillingAddress.State;
        //    var mcard = new MasterCardApi(setting);
        //        var payResult = mcard.Authorize(sessionId, year, month, amount, orderId, transId, currency, billingAddress);
        //    if (payResult.AVSResult != "MATCH")
        //    {
        //        var riskResult = _checkoutApi.SetOrderRisk(orderId, payResult.AVSResult);
        //    }
        //        return JsonSuccess(payResult, JsonRequestBehavior.AllowGet);
        //    }
        //#endregion
        #endregion

        #region MOVED TO PAYPALCONTROLLER
        //#region "PaypalStandard payment"
        //public ActionResult Paypalnotification()
        //{
        //    var response = new BoolResponse { IsValid = false, Message = "" };
        //    response.RecordId = Request.Params["bid"];
        //    PaymentModel payment = null;
        //    var settingRespose = _checkoutApi.PaymentSetting(PaymentMethodTypes.Paypal.ToString());
        //    var setting = settingRespose.Result;
        //    if (Request["oid"] != null)
        //    {
        //        var refOrderId = Request["oid"];
        //        string orderId = Request.Params["oid"];
        //        var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
        //        var order = orderResponse.Result;
        //        string paymentId = Request.Params["payId"];
        //        string token = Request.Params["token"];
        //        string payerId = Request.Params["payerId"];
        //        payment = order.Payments.FirstOrDefault(x => x.Id == paymentId);
        //        var paymentRequest = new PostProcessPaymentRequest
        //        {
        //            CurrencyCode = order.CurrencyCode,
        //            Order = order,
        //            OrderTotal = payment.OrderAmount,
        //            Payment = payment,
        //            Token = token,
        //            PayerId = payerId

        //        };
        //        var paymentResponse = setting.PostProcessPayment(paymentRequest);
        //        if (paymentResponse.Success == true) paymentResponse.Payment.IsValid = true;
        //        _checkoutApi.UpdatePayment(refOrderId, paymentResponse.Payment);
        //        response.RecordId = order.Id;
        //        response.IsValid = paymentResponse.Success;

        //        if (!response.IsValid)
        //        {
        //            response.RecordId = Request.Params["bid"];
        //            if (paymentResponse.Errors.Any())
        //                response.Message = paymentResponse.Errors[0];
        //        }
        //    }

        //    return View(CustomViews.PAYMENT_RESPONSE, response);
        //}

        //#endregion
        #endregion

        public ActionResult Logout()
        {
            _authenticationService.Logout();
            return JsonSuccess(true , JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GuestCheckout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            var responseresult = new ResponseModel<Omnicx.WebStore.Models.Commerce.BasketModel>();
            var response = _customerRepository.GetUserdetailsByUserName(Sanitizer.GetSafeHtmlFragment(model.Email));
            var existingUser = response.Result;
            if (existingUser.Count > 0)
            {
                var customerId = existingUser[0].UserId;
                var companyId = existingUser[0].CompanyId;
                if (model.CustomerId == null)
                {
                     responseresult = _checkoutApi.UpdateUserToBasket(model.BasketId, customerId.ToString());
                }
                return JsonSuccess(new { customerId = customerId, basket = responseresult.Result, BasketStage = BasketStage.LoggedIn.GetHashCode(),companyId= companyId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var user = new CustomerModel
                {
                    Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                    Password = Sanitizer.GetSafeHtmlFragment(model.Password),
                    SourceProcess = SourceProcessType.SITE_CHECKOUTGUEST.ToString()
                };
                var result = _customerRepository.Register(user);
                if (result.Result.IsValid)
                {
                    if (model.CustomerId == null)
                    {
                         responseresult = _checkoutApi.UpdateUserToBasket(model.BasketId, result.Result.RecordId);
                        model.CustomerId = result.Result.RecordId;
                    }
                }
                return JsonSuccess(new { customerId = model.CustomerId, basket = responseresult.Result, BasketStage = BasketStage.LoggedIn.GetHashCode() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ValidateGuestPassword(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            else
            {
                return JsonSuccess(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult GetWishlistProducts()
        {
            var customerId = _sessionContext.CurrentUser.UserId.ToString();
            var result = _customerRepository.GetWishlist(customerId, true);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AddProductToWishlist(string id)
        {
             _customerRepository.AddProductToWishList(Sanitizer.GetSafeHtmlFragment(id), _sessionContext.CurrentUser.UserId, true);
            var response = _customerRepository.GetWishlist(_sessionContext.CurrentUser.UserId.ToString(), true);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RemoveWishList(string id)
        {
            _customerRepository.RemoveWishList(_sessionContext.CurrentUser.UserId.ToString(), Sanitizer.GetSafeHtmlFragment(id), true);
            var response = _customerRepository.GetWishlist(_sessionContext.CurrentUser.UserId.ToString(), true);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }
    }
}