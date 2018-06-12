using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Core.Services.Authentication;
using Omnicx.API.SDK.Models.Catalog;
using Microsoft.Security.Application;
using System.Text.RegularExpressions;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Models.Helpers;
using Omnicx.API.SDK.Models;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.WebStore.Core.Data;
using Omnicx.WebStore.Apps.OAuthHelper.Core;
using Newtonsoft.Json.Linq;
using Omnicx.WebStore.Apps.OAuthHelper;
using Omnicx.WebStore.Core.Filters;
using Omnicx.WebStore.Core.Social;


namespace Omnicx.WebStore.Core.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerApi _customerRepository;
        private readonly IOrderApi _orderRepository;
        private readonly IConfigApi _configApi;
        private readonly IBasketApi _basketApi;
        private readonly ISocialLoginService _socialService;
        public AccountController(IAuthenticationService authenticationService, 
            ICustomerApi customerRepository, IOrderApi orderRepository, IConfigApi configApi, IBasketApi basketApi, ISocialLoginService socialService)
        {
            _authenticationService = authenticationService;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _configApi = configApi;
            _basketApi = basketApi;
            _socialService = socialService;
        }
        // GET: Account

        /// <summary>
        /// Method to get the order history based on the user details 
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute]

        public ActionResult OrderHistory()
        {
            ViewBag.isB2BEnable = _sessionContext.CurrentSiteConfig.B2BSettings.EnableB2B;
            ViewBag.AllowReorder = _sessionContext.CurrentSiteConfig.B2BSettings.AllowReorder;
            return View(CustomViews.ORDER_HISTORY);
        }
        public ActionResult Getorders()
        {
            var userId = _sessionContext.CurrentUser.UserId.ToString();
            var result = _orderRepository.GetOrders(userId);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Wishlist()
        {
            if (_sessionContext.CurrentUser == null) { return RedirectToAction("SignIn"); }
            var customerId = _sessionContext.CurrentUser.UserId.ToString();
            var key = string.Format(CacheKeys.WishList, customerId);
            if (System.Web.HttpContext.Current.Session[key] != null)
            {
                return View(CustomViews.WISHLIST, System.Web.HttpContext.Current.Session[key]);
            }
            var result = _customerRepository.GetWishlist(customerId, false);
            System.Web.HttpContext.Current.Session[key] = result.Result;
            foreach (var res in result.Result)
            {
                res.SeoName = String.IsNullOrEmpty(res.SeoName) ? res.Name.ToSeo() : res.SeoName;
            }
            return View(CustomViews.WISHLIST, result.Result);
        }

        public ActionResult RemoveWishList(string id)
        {
            var customerId = _sessionContext.CurrentUser.UserId.ToString();
            if (_sessionContext.CurrentUser != null)
            {               
                var key = string.Format(CacheKeys.WishList, customerId);
                System.Web.HttpContext.Current.Session[key] = null;
            }
                //var customerId = _sessionContext.CurrentUser.UserId.ToString();

            var resp = _customerRepository.RemoveWishList(customerId, Sanitizer.GetSafeHtmlFragment(id), false);
            return RedirectToAction("Wishlist");
        }
        [CustomAuthorizeAttribute]
        public ActionResult MyAccount()
        {
            var model = new CustomerProfileModel
            {
                CustomerDetail = new CustomerDetailModel(),
            };
            var userId = _sessionContext.CurrentUser.UserId.ToString();
            var response = _customerRepository.GetUserdetailsById<CustomerDetailModel>(userId);
            model.CustomerDetail = response.Result;
            model.CustomerDetail.BirthDate = model.CustomerDetail.DayOfBirth + "/" + model.CustomerDetail.MonthOfBirth + "/" + model.CustomerDetail.YearOfBirth;
            return View(CustomViews.MY_ACCOUNT, model);            
        }

        [CustomAuthorizeAttribute]
        public ActionResult OrderDetail(string id)
        {
            var result = _orderRepository.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(id));
            result.Result.isB2BEnable = _sessionContext.CurrentSiteConfig.B2BSettings.EnableB2B;
            result.Result.AllowReorder = _sessionContext.CurrentSiteConfig.B2BSettings.AllowReorder;
            return View(CustomViews.ORDER_DETAIL, result.Result);
        }
        [CustomAuthorizeAttribute]
        public ActionResult ReturnRequest(string id)
        {
            var resp = _customerRepository.ReturnRequest(id);
            return View(CustomViews.RETURN_REQUEST, resp.Result);
        }

        public ActionResult CreateReturn(ReturnModel returnModel)
        {
            returnModel.Email = _sessionContext.CurrentUser.Email;
            var resp = _customerRepository.CreateReturn(returnModel);
            return JsonSuccess(resp.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignIn()
        {
            var isLogged = _sessionContext.CurrentUser;
            ViewBag.isB2BEnable = _sessionContext.CurrentSiteConfig.B2BSettings.EnableB2B;
            ViewBag.isRegistrationAllowed = _sessionContext.CurrentSiteConfig.B2BSettings.RegistrationAllowed;
            
            if (isLogged != null)
            {
                return Redirect("~");
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
            var userModel = new UserModel { isLogin = true };
            return View(CustomViews.SIGN_IN, userModel);
        }

        public ActionResult SignUp()
        {
            ViewBag.isB2BEnable = _sessionContext.CurrentSiteConfig.B2BSettings.EnableB2B;
            ViewBag.isRegistrationAllowed = _sessionContext.CurrentSiteConfig.B2BSettings.RegistrationAllowed;

            var isLogged = _sessionContext.CurrentUser;
            if (isLogged != null)
            {
                return Redirect("~");
            }
            var userModel = new UserModel { isLogin = false };
            return View(CustomViews.SIGN_IN, userModel);
        }

        [HttpPost]
        public ActionResult SignIn(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            var result = _authenticationService.Login(Sanitizer.GetSafeHtmlFragment(login.Username), Sanitizer.GetSafeHtmlFragment(login.Password), true);
            if (result == null)
            {
                ModelState.AddModelError("", "Invalid Password!");
                return JsonValidationError();
            }
            else
            {
                //returnURL needs to be decoded
                string decodedUrl = string.Empty;
                string returnUrl = TempData["ReturnUrl"] != null ? TempData["ReturnUrl"].ToString() : "";
                if (!string.IsNullOrEmpty(returnUrl))
                    if (returnUrl.ToLower().Contains("passwordrecovery"))
                        returnUrl = "";
                decodedUrl = Server.UrlDecode(returnUrl);
                var resp = new BoolResponse { IsValid = true, MessageCode = result.SessionId, Message = result.FirstName, ReturnUrl = decodedUrl };
                SiteUtils.SetBasketAction(resetAction: true);
                return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        // [ValidateInput(false)]
        public ActionResult Registration(RegisterViewModel register)
        {

            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            if (!string.IsNullOrEmpty(register.Password))
            {
                if (!Regex.IsMatch(register.Password, SiteUtils.GetPasswordRegex()))
                {
                    ModelState.AddModelError("Password", "Password does not meet policy!");
                    return JsonValidationError();
                }
            }
            var response = new BoolResponse();
            //-- to check if user's Email Address already registered
            var existingUser = _customerRepository.GetExistingUser(Sanitizer.GetSafeHtmlFragment(register.Email));
            if (existingUser.Result != null)
            {
                if (existingUser.Result.Count > 0 && existingUser.Result[0].UserSourceType != UserSourceTypes.Newsletter.GetHashCode().ToString())
                {
                    ModelState.AddModelError("Error", "Your email address is already registered with us.");
                    return JsonValidationError();
                }
                var user = new CustomerModel
                {
                    Email = Sanitizer.GetSafeHtmlFragment(register.Email),
                    Password = Sanitizer.GetSafeHtmlFragment(register.Password)

                };
                var result = _customerRepository.Register(user);
                if (result.Result.IsValid)
                {
                    var loginResult = _authenticationService.Login(Sanitizer.GetSafeHtmlFragment(register.Email), Sanitizer.GetSafeHtmlFragment(register.Password), true);
                    response.IsValid = true;
                    SiteUtils.SetBasketAction(resetAction: true);
                    return JsonSuccess(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("Error", "Registration failed!");
                    return JsonValidationError();
                }
            }
            else
            {
                ModelState.AddModelError("Error", " '+' Symbol is not allowed in Email!");
                return JsonValidationError();
            }

        }

        public ActionResult GetBillingCountries()
        {
            var result = _configApi.GetConfig();
            return JsonSuccess(result.Result.BillingCountries, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerAddress()
        {

            string id = _sessionContext.CurrentUser != null ? _sessionContext.CurrentUser.UserId.ToString() : null;
            if (string.IsNullOrEmpty(id)) return JsonSuccess(new List<AddressModel>(), JsonRequestBehavior.AllowGet);
            var result = _customerRepository.GetCustomerAddress<AddressModel>(id);
            if (result.Result != null)
            {
                foreach (var item in result.Result)
                    item.CustomerId = id;
            }
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizeAttribute]
        public ActionResult SaveCustomerAddress(AddressModel model)
        {
            var resp = new BoolResponse();
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            var addressModel = new AddressModel
            {
                Address1 = Sanitizer.GetSafeHtmlFragment(model.Address1),
                Address2 = Sanitizer.GetSafeHtmlFragment(model.Address2),
                Address3 = Sanitizer.GetSafeHtmlFragment(model.Address3),
                City = Sanitizer.GetSafeHtmlFragment(model.City),
                Country = Sanitizer.GetSafeHtmlFragment(model.Country),
                CountryCode = Sanitizer.GetSafeHtmlFragment(model.CountryCode),
                CustomerId = Sanitizer.GetSafeHtmlFragment(model.CustomerId),
                FirstName = Sanitizer.GetSafeHtmlFragment(model.FirstName),
                LastName = Sanitizer.GetSafeHtmlFragment(model.LastName),
                Id = Sanitizer.GetSafeHtmlFragment(model.Id),
                MobileNo = Sanitizer.GetSafeHtmlFragment(model.MobileNo),
                PhoneNo = Sanitizer.GetSafeHtmlFragment(model.PhoneNo),
                PostCode = Sanitizer.GetSafeHtmlFragment(model.PostCode),
                State = Sanitizer.GetSafeHtmlFragment(model.State),
                Title = Sanitizer.GetSafeHtmlFragment(model.Title),
                IsDefault = model.Id == null ? true : model.IsDefault
            };
            if (addressModel.Id == null || addressModel.Id == "")
            {
                addressModel.CustomerId = _sessionContext.CurrentUser.UserId.ToString();
                var response = _customerRepository.SaveCustomerAddress(addressModel);
                resp.IsValid = response.Result;
            }
            else
            {
                var response = _customerRepository.UpdateCustomerAddress(Sanitizer.GetSafeHtmlFragment(addressModel.Id), addressModel);
                resp.IsValid = response.Result;
            }
            return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizeAttribute]
        public ActionResult PasswordChange()
        {
            if (_sessionContext.CurrentUser == null) { return RedirectToAction("SignIn"); }
            return View(CustomViews.PASSWORD_CHANGE);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (!Regex.IsMatch(model.NewPassword, SiteUtils.GetPasswordRegex()))
                {
                    ModelState.AddModelError("Password", "Password does not meet policy!");
                    return JsonValidationError();
                }
            }
            if (model.OldPassword == model.NewPassword)
            {
                ModelState.AddModelError("Error", "Old Password and New Password cannot be same");
                return JsonValidationError();
            }
            var response = new BoolResponse();
            var result = _authenticationService.Login(_sessionContext.CurrentUser.Username, Sanitizer.GetSafeHtmlFragment(model.OldPassword), true);
            if (result != null)
            {
                response.IsValid = _customerRepository.ChangePassword(Sanitizer.GetSafeHtmlFragment(model.OldPassword), Sanitizer.GetSafeHtmlFragment(model.NewPassword), _sessionContext.CurrentUser.UserId.ToString()).Result;
                return JsonSuccess(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ModelState.AddModelError("Error", "Old Password didn't match.");
                return JsonValidationError();
            }
        }
        [CustomAuthorizeAttribute]
        [HttpPost]
        public ActionResult SaveCustomerDetail(CustomerDetailModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }

            if (_sessionContext.CurrentUser.Email != model.Email)
            {
                ModelState.AddModelError("Error", "You cannot save detail to this Email!");
                return JsonValidationError();
            }
            
            var customerModel = new CustomerModel
            {
                UserId = Guid.Parse(model.UserId),
                BirthDate = Sanitizer.GetSafeHtmlFragment(model.BirthDate),
                DayOfBirth = Sanitizer.GetSafeHtmlFragment(model.DayOfBirth),
                Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                FirstName = Sanitizer.GetSafeHtmlFragment(model.FirstName),
                Gender = Sanitizer.GetSafeHtmlFragment(model.Gender),
                LastName = Sanitizer.GetSafeHtmlFragment(model.LastName),
                Mobile = Sanitizer.GetSafeHtmlFragment(model.Mobile),
                MonthOfBirth = Sanitizer.GetSafeHtmlFragment(model.MonthOfBirth),
                PostCode = Sanitizer.GetSafeHtmlFragment(model.PostCode),
                Telephone = Sanitizer.GetSafeHtmlFragment(model.Telephone),
                Title = Sanitizer.GetSafeHtmlFragment(model.Title),
                YearOfBirth = Sanitizer.GetSafeHtmlFragment(model.YearOfBirth),
                NewsLetterSubscribed = model.NewsLetterSubscribed

            };
            customerModel.DayOfBirth = "00";
            customerModel.MonthOfBirth = "00";
            customerModel.YearOfBirth = "00";
            if (customerModel.BirthDate.Split('/').Length == 3)
            {
                customerModel.DayOfBirth = customerModel.BirthDate.Split('/')[0];
                customerModel.MonthOfBirth = customerModel.BirthDate.Split('/')[1];
                customerModel.YearOfBirth = customerModel.BirthDate.Split('/')[2];
            }
           
            var result = _customerRepository.UpdateCustomerDetail(customerModel.UserId.ToString(), customerModel);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            _authenticationService.Logout();
            SiteUtils.SetBasketAction(resetAction: true);
            return Redirect("~");
        }
        [CustomAuthorizeAttribute]
        public ActionResult AddressBook()
        {
            return View(CustomViews.ADDRESS_BOOK);
        }

        public ActionResult GetAddressById(AddressModel customeraddress)
        {
            var result = _customerRepository.GetAddressById(Sanitizer.GetSafeHtmlFragment(customeraddress.CustomerId), Sanitizer.GetSafeHtmlFragment(customeraddress.Id));
            result.Result.CustomerId = Sanitizer.GetSafeHtmlFragment(customeraddress.CustomerId);
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAddress(AddressModel customeraddress)
        {
            var result = _customerRepository.DeleteAddress(Sanitizer.GetSafeHtmlFragment(customeraddress.CustomerId), Sanitizer.GetSafeHtmlFragment(customeraddress.Id));
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult changeDefaultAddress(AddressModel customeraddress)
        {
            var result = _customerRepository.changeDefaultAddress(Sanitizer.GetSafeHtmlFragment(customeraddress.CustomerId), Sanitizer.GetSafeHtmlFragment(customeraddress.Id));
            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddProductToWishlist(string id)
        {
            //WRONG IMPLEMENTATION: 
            //TODO: CHange this - Vikram - 24Apr2017
            if (_sessionContext.CurrentUser == null)
            {
                string errorMessage = "User Login required!";
                return JsonError(errorMessage, JsonRequestBehavior.DenyGet);
            }
            
            var customerId = _sessionContext.CurrentUser.UserId.ToString();
            var key = string.Format(CacheKeys.WishList, customerId);
            var response = _customerRepository.AddProductToWishList(Sanitizer.GetSafeHtmlFragment(id), _sessionContext.CurrentUser.UserId, false);
            //Temporary Fix 
            var result = _customerRepository.GetWishlist(customerId, false);
            System.Web.HttpContext.Current.Session[key] = result.Result;
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewsletterSubscription(string email)
        {
            var customer = new CustomerModel();
            customer.Email = Sanitizer.GetSafeHtmlFragment(email);
            var response = _customerRepository.NewsletterSubscription(customer);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnSubscribeNewsletter(string id)
        {
            var response = _customerRepository.UnSubscribeNewsletter(_sessionContext.CurrentUser.UserId.ToString());
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult HeaderLogin()
        {
            return PartialView(CustomViews.HEADER_LOGIN);
        }

        [CustomAuthorizeAttribute]
        public ActionResult ReturnHistory()
        {
            var userId = _sessionContext.CurrentUser.UserId.ToString();
            var result = _orderRepository.GetAllReturns(userId);
            return View(CustomViews.RETURN_HISTORY, result.Result);
        }

        /// <summary>
        ///Send the mail on forgot password by User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordModel user)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            var model = new LoginModel { Username = user.Username };
            var response = _customerRepository.ForgotPassword(model);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validate forget password email token sent to user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PasswordRecovery(string id)
        {
            var model = new ForgotPasswordViewModel { Token = id };
            return View(CustomViews.PASSWORD_RECOVERY, model);
        }

        /// <summary>
        /// Validate Forgot password token and changes passoword 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult RecoverPassword(ForgotPasswordViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return JsonValidationError();
            }
            var model = new UserPasswordTokenModel
            {
                Id = user.Id,
                Token = user.Token,
                UserName = user.UserName,
                Password = user.Password
            };
            var response = _customerRepository.RecoverPassword(model);
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ForgotPassword()
        {
            return View(CustomViews.FORGOT_PASSWORD);
        }
        [CustomAuthorizeAttribute]
        public ActionResult MyActivity()
        {
            if (ConfigKeys.DisplayUserActivity == false)
            {
                return RedirectToAction("PageNotFound", "Common");
            }
            if (_sessionContext.CurrentUser == null) { return RedirectToAction("SignIn"); }
            return View(CustomViews.MY_ACTIVITY);
        }
        public ActionResult GetMyActivity(UserActivitySearchModel search)
        {
            if (search == null)
            {
                search = new UserActivitySearchModel();
            }
            search.UserId = _sessionContext.CurrentUser.UserId.ToString();
            search.PageSize = Convert.ToInt32(ConfigKeys.PageSize);
            var finalResult = new UserActivityListModel() { PageSize = search.PageSize, CurrentPage = search.CurrentPage };
            var response = _customerRepository.UserActivity(search.UserId, search.CurrentPage, search.PageSize);
            var resp = new List<Activity>() { };
            if (response != null && response.Result != null && response.Result.TotalActivity != 0)
            {
                finalResult.TotalRecord = response.Result.TotalActivity;

                finalResult.Sessions = response.Result.Sessions;
                foreach (var item in response.Result.Sessions)
                {
                    resp.AddRange(item.Activities);
                }
                finalResult.Sessions = (from e in resp
                                       group e by e.Created.Year.ToString() + e.Created.Month.ToString() + e.Created.Day.ToString() into g
                                       orderby g.Key descending
                                       select new UserActivityModel() {FirstActivityCreated=g.Min(a=>a.Created),LastActivityCreated=g.Max(a=>a.Created),ActivityCount=g.Count(), Activities=g.ToList() }).ToList();
            }
            return JsonSuccess(finalResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMyActivity()
        {
           
            var resp = _customerRepository.DeleteActivity(_sessionContext.CurrentUser.UserId.ToString());
            if (resp.Result != null && resp.Result.IsValid != false)
                return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
            else
                return JsonError("false", JsonRequestBehavior.AllowGet);

        }
        public ActionResult ItemView()
        {
            return View(CustomViews.ITEM_VIEW);
        }

        public void GhostLogin()
        {
            Guid ghostSessionId = Guid.Empty;
            Guid ghostBasketId = Guid.Empty;
            if (Request.QueryString["sid"] != null)
            {
                Guid.TryParse(Sanitizer.GetSafeHtmlFragment(Request.QueryString["sid"]), out ghostSessionId);
            }
            if (ghostSessionId != Guid.Empty)
            {
                var ghostLogin = _customerRepository.AuthenticateGhostLogin(Convert.ToString(ghostSessionId));
                if (ghostLogin != null && ghostLogin.Result != null)
                {
                    ghostLogin.Result.AdminUserName = ghostLogin.Message;
                    ghostLogin.Result.SessionId = ghostSessionId.ToString();
                    _authenticationService.GhostLogin(ghostLogin.Result);
                }
                SiteUtils.SetBasketAction(resetAction:true);
                Response.Redirect("~/");

            }
        }
        public ActionResult GetWishlist()
        {
            ResponseModel<List<ProductModel>> result = new ResponseModel<List<ProductModel>>();
            if (_sessionContext.CurrentUser != null)
            {
                var customerId = _sessionContext.CurrentUser.UserId.ToString();
                var key = string.Format(CacheKeys.WishList, customerId);
                if (System.Web.HttpContext.Current.Session[key] == null)
                {
                    result = _customerRepository.GetWishlist(customerId, false);
                    System.Web.HttpContext.Current.Session[key] = result.Result;
                    return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonSuccess(System.Web.HttpContext.Current.Session[key], JsonRequestBehavior.AllowGet);
                }
            }

            return JsonSuccess(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SocialSignIn(string provider = "None")
        {
            string recordId = "";
            dynamic user = null;

            //Addtional check in case client not registered properly.
            //only happens when project throws an exception or restarted.
            if(!OAuthClientFactory.IsFacebookRegistered)
            {
                OAuthClientFactory.RegisterFacebookClient(_sessionContext.CurrentSiteConfig.SocialSettings.FacebookApiKey, _sessionContext.CurrentSiteConfig.SocialSettings.FacebookApiSecret, _sessionContext.CurrentSiteConfig.SocialSettings.FacebookUrl);
            }
            if(!OAuthClientFactory.IsGoogleRegistered)
            {
                OAuthClientFactory.RegisterGoogleClient(_sessionContext.CurrentSiteConfig.SocialSettings.GooglePlusApiKey, _sessionContext.CurrentSiteConfig.SocialSettings.GooglePlusApiSecret, _sessionContext.CurrentSiteConfig.SocialSettings.GooglePlusUrl);
            }
            if(!OAuthClientFactory.IsTwitterRegisterd)
            {
                OAuthClientFactory.RegisterTwitterClient(_sessionContext.CurrentSiteConfig.SocialSettings.TwitterApiKey, _sessionContext.CurrentSiteConfig.SocialSettings.TwitterApiSecret, _sessionContext.CurrentSiteConfig.SocialSettings.TwitterUrl);
            }

            var response = _socialService.SocialSignIn(provider);
            if (response != null)
            {
                var obj = JObject.Parse(response.JsonResponse);
                switch (response.OAuthClient)
                {
                    case OAuthClient.Twitter:
                        {
                            user = new TwitterResponse()
                            {
                                Source = Enum.GetName(typeof(OAuthClient), response.OAuthClient),
                                FullName = Convert.ToString(obj["name"]),
                                UserName = Convert.ToString(obj["screen_name"]),
                                SocialId = Convert.ToString(obj["id"]),
                                Email = Convert.ToString(obj["email"]),
                                Location = Convert.ToString(obj["location"]),
                                FirstName = Convert.ToString(obj["name"]).Split(' ')[0],
                                LastName = Convert.ToString(obj["name"]).Split(' ')[1],
                            };

                            var siteUser = _customerRepository.GetExistingUser(user.Email);
                            if (siteUser.Result.Count == 0) //user not exits, new user
                            {
                                CustomerModel customer = new CustomerModel()
                                {
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    IsRegistered = false
                                };

                                var result = _customerRepository.Register(customer);
                                recordId = result.Result.RecordId;
                                user.RecordId = recordId;

                                CloudTableRepository _cloudRepository = new CloudTableRepository();
                                var table = _cloudRepository.CreateTable("SocialUsers");
                                _cloudRepository.InsertUser(table, user); //insert entry in azure table
                            }
                            else //existing user
                            {
                                recordId = Convert.ToString(siteUser.Result[0].UserId);
                            }
                        }
                        break;
                    case OAuthClient.Facebook:
                        {
                            user = new FacebookResponse()
                            {
                                Source = Enum.GetName(typeof(OAuthClient), response.OAuthClient),
                                FullName = Convert.ToString(obj["name"]),
                                FirstName = Convert.ToString(obj["first_name"]),
                                LastName = Convert.ToString(obj["last_name"]),
                                SocialId = Convert.ToString(obj["id"]),
                                Email = Convert.ToString(obj["email"]),
                                Gender = Convert.ToString(obj["gender"]),
                                Location = Convert.ToString(obj["location"]),
                                HomeTown = Convert.ToString(obj["hometown"]),
                                DateOfBirth = Convert.ToString(obj["birthday"])
                            };

                            var siteUser = _customerRepository.GetExistingUser(user.Email);
                            if (siteUser.Result.Count == 0) //user not exits, new user
                            {
                                CustomerModel customer = new CustomerModel()
                                {
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Gender = user.Gender,
                                    IsRegistered = false
                                };

                                var result = _customerRepository.Register(customer);
                                recordId = result.Result.RecordId;
                                user.RecordId = recordId;

                                CloudTableRepository _cloudRepository = new CloudTableRepository();
                                var table = _cloudRepository.CreateTable("SocialUsers");
                                _cloudRepository.InsertUser(table, user);
                            }
                            else //existing user
                            {
                                recordId = Convert.ToString(siteUser.Result[0].UserId);
                            }
                        }
                        break;
                    case OAuthClient.Google:
                        {
                            user = new GoogleResponse()
                            {
                                Source = Enum.GetName(typeof(OAuthClient), response.OAuthClient),
                                SocialId = Convert.ToString(obj["id"]),
                                FullName = Convert.ToString(obj["name"]),
                                FirstName = Convert.ToString(obj["given_name"]),
                                LastName = Convert.ToString(obj["family_name"]),
                                Email = Convert.ToString(obj["email"]),
                            };

                            var siteUser = _customerRepository.GetExistingUser(user.Email);
                            if (siteUser.Result.Count == 0) //user not exits, new user
                            {
                                CustomerModel customer = new CustomerModel()
                                {
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    IsRegistered = false
                                };

                                var result = _customerRepository.Register(customer);
                                recordId = result.Result.RecordId;
                                user.RecordId = recordId;

                                CloudTableRepository _cloudRepository = new CloudTableRepository();
                                var table = _cloudRepository.CreateTable("SocialUsers");
                                _cloudRepository.InsertUser(table, user);
                            }
                            else //existing user
                            {
                                recordId = Convert.ToString(siteUser.Result[0].UserId);
                            }
                        }
                        break;

                    default:
                        break;
                }

                var results = _authenticationService.SocialLogin(recordId);
                if (results == null) return JsonValidationError();
                else return Redirect("/");
            }
            return null;

        }

        public JsonResult getsocialsettings()
        {
            
            SocialViewConfig config = new SocialViewConfig()
            {
                FacebookEnabled = _sessionContext.CurrentSiteConfig.SocialSettings.FacebookEnabled,
                TwitterEnabled = _sessionContext.CurrentSiteConfig.SocialSettings.TwitterEnabled,
                GooglePlusEnabled = _sessionContext.CurrentSiteConfig.SocialSettings.GooglePlusEnabled
            };
            return Json(config, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizeAttribute]
        public ActionResult SavedBaskets()
        {
            if(_sessionContext.CurrentUser != null && _sessionContext.CurrentUser.UserId != null)
            {
                var basket = _basketApi.GetAllUserBaskets(_sessionContext.CurrentUser.UserId);
                return View(CustomViews.SAVED_BASKET, basket.Result);
            }

            return View(CustomViews.SAVED_BASKET, new List<BasketModel>());           
        }
        public ActionResult GetDefaultCountry()
        {
            if (_sessionContext.CurrentSiteConfig != null && _sessionContext.CurrentSiteConfig.RegionalSettings != null)
            {
                return JsonSuccess(_sessionContext.CurrentSiteConfig.RegionalSettings.DefaultCountry, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var country = _configApi.GetConfig().Result.RegionalSettings.DefaultCountry;
                return JsonSuccess(country, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPasswordPolicy()
        {
            string xml = System.IO.File.ReadAllText(Server.MapPath("~/assets/core/xml/PasswordPolicy.xml"));
            return this.Content(xml, "text/xml");
        }
    }
}