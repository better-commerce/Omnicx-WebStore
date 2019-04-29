using System.Linq;
using System.Web.Mvc;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Api.Infra;

using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Site;
using Newtonsoft.Json;
using System.Web.Routing;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;

namespace Omnicx.WebStore.Core.Controllers
{
    /// <summary>
    /// This is an abstract base controller class inherited in the other controller classes.
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected readonly IHeadTagBuilder _headTagbuilder;
        protected readonly ISessionContext _sessionContext;

        protected BaseController()
        {
            _headTagbuilder = DependencyResolver.Current.GetService<IHeadTagBuilder>();
            _sessionContext = DependencyResolver.Current.GetService<ISessionContext>();
            // the below check is added to prevent the multiple calls to SetDataLayerSessionVariables in a single request
           if  (!_headTagbuilder.DataLayerKeyExists("SessionId") && !IsAjaxRequest())
                SetDataLayerSessionVariables();
        }
        protected StandardJsonResult<T> JsonSuccess<T>(T data, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return new StandardJsonResult<T> { Data = data, JsonRequestBehavior = behavior };
        }
        protected StandardJsonResult JsonError(string errorMessage, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = new StandardJsonResult { JsonRequestBehavior = behavior };
            result.AddError(errorMessage);

            return result;
        }
        protected StandardJsonResult JsonValidationError()
        {
            var result = new StandardJsonResult();

            foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
            {
                result.AddError(validationError.ErrorMessage);
            }
            return result;
        }
       
        [ValidateInput(false)]
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            var isRequestContainsAngularExpression = false;
            if (Request.Params != null && Request.Params.AllKeys != null && Request.Params.AllKeys.Length > 0)
            {
                isRequestContainsAngularExpression = Request.Params.AllKeys.Any(x => Request[x] != null ? Request[x].Contains("{{") : false || Request[x] != null ? Request[x].Contains("{ {") : false);
            }
            if (isRequestContainsAngularExpression)
            {

                throw new System.Web.HttpRequestValidationException("Potentially dangerous request");
            }

        }

        private void SetDataLayerSessionVariables()
        {
                _headTagbuilder.AddDataLayer("SessionId", _sessionContext.SessionId);
                var uId = _sessionContext.CurrentUser?.UserId;
                var browserInfo = Utils.GetBrowserInfo();
                var channel = "Web";
                if (browserInfo != null)
                   channel = browserInfo.IsMobileDevice ? "mobile-web" : "Web";

                _headTagbuilder.AddDataLayer("VisitorId", uId.ToString(),true);
                
                _headTagbuilder.AddDataLayer("AppId", ConfigKeys.OmnicxDomainId);
                _headTagbuilder.AddDataLayer("OrgId", ConfigKeys.OmnicxOrgId);
                _headTagbuilder.AddDataLayer("DomainId", ConfigKeys.OmnicxDomainId);
                _headTagbuilder.AddDataLayer("Server", Utils.GetMaskedServerIpAddress()); 
                _headTagbuilder.AddDataLayer("DeviceId", System.Web.HttpContext.Current.Request.Cookies[Constants.COOKIE_DEVICEID]?.Value);
                _headTagbuilder.AddDataLayer("VisitorLoggedIn", System.Web.HttpContext.Current.Request.IsAuthenticated);
                _headTagbuilder.AddDataLayer("VisitorExistingCustomer", (_sessionContext.CurrentUser?.UserId != null), true);
                _headTagbuilder.AddDataLayer("VisitorSegment", "");// to be reviewed
                _headTagbuilder.AddDataLayer("VisitorAffiliate", "");// to be reviewed
                _headTagbuilder.AddDataLayer("VisitorEmail", _sessionContext.CurrentUser?.Email,true);
                _headTagbuilder.AddDataLayer("PageCategory", this.GetType().Name.Replace("Controller", ""));
                _headTagbuilder.AddDataLayer("Lang", _sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultLanguageCulture);
                _headTagbuilder.AddDataLayer("Currency", _sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultCurrencyCode);
                _headTagbuilder.AddDataLayer("CurrencySymbol", _sessionContext.CurrentSiteConfig?.RegionalSettings?.DefaultCurrencySymbol);
                //_headTagbuilder.AddDataLayer("UserEmail", _sessionContext.CurrentUser?.Email);
                _headTagbuilder.AddDataLayer("Channel", channel);
                _headTagbuilder.AddDataLayer("IpAddress", _sessionContext.IpAddress);

        }

        protected void SetDataLayerVariables<T>(T obj, WebhookEventTypes activityType)  
        {
            _headTagbuilder.AddDataLayer("EventType", activityType.ToString());            
            if (obj != null)
            {
                var objType = obj.GetType().Name;
                switch (objType)
                {
                    case "ProductDetailModel":
                        var p = (ProductDetailModel)(object)obj;
                        if (p != null) { 
                            var prod = new { id = p.RecordId, sku = p.Sku, name = p.Name,stockCode=p.StockCode,img=p.Image};
                            _headTagbuilder.AddDataLayer("EntityId", p.RecordId);
                            _headTagbuilder.AddDataLayer("EntityName", p.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(prod));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Product.ToString());
                        break;
                    case "BasketModel":
                        var b = (BasketModel)(object)obj;
                        if (b != null)
                        {
                            var lines = (from l in b.LineItems select new { id=l.Id,productId=l.ProductId,basketId=b.Id,stockCode = l.StockCode, name = l.Name, qty = l.Qty, price = l.Price?.Raw?.WithoutTax ,tax=l.Price?.Raw?.Tax , manufacturer = l.Manufacture, img=l.Image }).ToList();
                            var name = string.Join(",",(from l in lines select l.name + " (" + l.qty.ToString() + ")" ).ToArray());
                            var bskt = new {   id = b.Id,
                                                grandTotal = b.GrandTotal?.Raw?.WithoutTax,
                                                tax = b.GrandTotal?.Raw.Tax,
                                                taxPercent=b.TaxPercent,
                                                shipCharge = b.ShippingCharge?.Raw?.WithoutTax,
                                                shipTax = b.ShippingCharge?.Raw.Tax,
                                                lineitems = lines,
                                                promoCode = b.PromotionsApplied };

                            _headTagbuilder.AddDataLayer("BasketTotal", b.GrandTotal?.Raw?.WithoutTax);
                            _headTagbuilder.AddDataLayer("Tax", b.GrandTotal?.Raw?.Tax);
                            _headTagbuilder.AddDataLayer("BasketItems", JsonConvert.SerializeObject(lines));
                            _headTagbuilder.AddDataLayer("BasketItemCount", lines.Sum(l=>l.qty));
                            _headTagbuilder.AddDataLayer("PromoCodes", JsonConvert.SerializeObject(b.PromotionsApplied));
                            _headTagbuilder.AddDataLayer("ShippingCost", b.ShippingCharge?.Raw?.WithoutTax);
                            _headTagbuilder.AddDataLayer("EntityId", b.Id);
                            _headTagbuilder.AddDataLayer("EntityName", name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(bskt));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Basket.ToString());
                        break;
                    case "OrderModel":
                        var ord = (OrderModel)(object)obj;
                        if (ord != null)
                        {
                            var ordlines = (from o in ord.Items select new {id=o.Id,productId=o.ProductId, stockCode = o.StockCode, name = o.Name,discountAmt=o.DiscountAmt?.Raw?.WithoutTax,isSubscription=o.IsSubscription,itemType=o.ItemType ,qty = o.Qty, price = o.Price?.Raw.WithoutTax,tax=o.Price?.Raw?.Tax, manufacturer=o.Manufacturer,categories=o.CategoryItems, img = o.Image }).ToList();
                            //var order = new { id = ord.Id, basketId = ord.BasketId, customerId = ord.CustomerId, OrderNo = ord.OrderNo, grandTotal = ord.GrandTotal?.Formatted, shipCharge = ord.ShippingCharge?.Formatted, subTotal = ord.SubTotal?.Formatted, discount = ord.Discount?.Formatted, lineitems = ordlines, promoCode = ord.Promotions, shippingMethod = ord.Shipping, shippingAddress = ord.ShippingAddress,status=ord.OrderStatus };
                            var payments = (from py in ord.Payments  where !string.IsNullOrEmpty(py.PaymentMethod) select new { methodName = py.PaymentMethod,paymentGateway=py.PaymentGateway, amount = py.OrderAmount }).ToList();

                            var order = new
                            {
                                id = ord.Id,
                                basketId = ord.BasketId,
                                customerId = ord.CustomerId,
                                OrderNo = ord.OrderNo,
                                grandTotal = ord.GrandTotal.Raw.WithTax,
                                tax = ord.GrandTotal.Raw.Tax,
                                taxPercent = ord.TaxPercent,
                                shipCharge = ord.ShippingCharge.Raw.WithoutTax,
                                subTotal = ord.SubTotal.Raw.WithoutTax,
                                discount = ord.Discount.Raw.WithoutTax,
                                lineitems = ordlines,
                                promoCode = ord.Promotions,
                                payments = payments,
                                paidAmount = payments.Sum(py => py.amount),
                                shippingMethod = ord.Shipping,
                                shippingAddress = ord.ShippingAddress,
                                billingAddress = ord.BillingAddress,
                                status = ord.OrderStatus
                            };
                            _headTagbuilder.AddDataLayer("BasketTotal", ord.GrandTotal?.Raw?.WithoutTax);
                            _headTagbuilder.AddDataLayer("Tax", ord.GrandTotal?.Raw?.Tax);
                            _headTagbuilder.AddDataLayer("BasketItems", JsonConvert.SerializeObject(ordlines));
                            _headTagbuilder.AddDataLayer("BasketItemCount", ordlines.Sum(l => l.qty));
                            _headTagbuilder.AddDataLayer("PromoCodes", JsonConvert.SerializeObject(ord.Promotions));
                            _headTagbuilder.AddDataLayer("ShippingCost", ord.ShippingCharge?.Raw?.WithoutTax);
                            _headTagbuilder.AddDataLayer("EntityId", ord.Id);
                            _headTagbuilder.AddDataLayer("EntityName", ord.OrderNo);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(order));

                            _headTagbuilder.AddDataLayer("VisitorId", ord.CustomerId,true); // VisitorId is added here because we don't have userid in session in case of guest checkout 
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Order.ToString());
                        break;
                    case "BlogDetailViewModel":
                        var blg = (BlogDetailViewModel)(object)obj;
                        if (blg != null)
                        {
                            var blog = new { slug = blg.Slug, freeText = blg.FreeText, blg.Detail?.RecordId, blg.Detail?.Tag, blg.Detail?.Title };
                            _headTagbuilder.AddDataLayer("EntityId", blg.Slug);
                            _headTagbuilder.AddDataLayer("EntityName", blg.Detail?.Title);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(blog));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Blog.ToString());
                        break;
                    case "BrandDetailModel":
                        var brnd = (BrandDetailModel)(object)obj;
                        if (brnd != null)
                        {
                            var brand = new { id = brnd.Id, name = brnd.Name, manufName = brnd.ParentManufacturerName };
                            _headTagbuilder.AddDataLayer("EntityId", brnd.Id);
                            _headTagbuilder.AddDataLayer("EntityName", brnd.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(brand));
                        }
                       
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Brand.ToString());
                        break;
                    case "CategoryModel":
                        var cat = (CategoryModel)(object)obj;
                        if (cat != null)
                        {
                            var category = new { id = cat.Id, name = cat.Name };
                            _headTagbuilder.AddDataLayer("EntityId", cat.Id);
                            _headTagbuilder.AddDataLayer("EntityName", cat.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(category));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Category.ToString());
                        break;
                    case "SiteViewModel":
                        var sv = (SiteViewModel)(object)obj;
                        if (sv != null)
                        {
                            var view = new { id = sv.Id, name = sv.Name, metaTitle = sv.MetaTitle, sv.MetaKeywords, sv.MetaDescription, sv.Slug, sv.Title, sv.ViewType };
                            _headTagbuilder.AddDataLayer("EntityId", sv.Id);
                            _headTagbuilder.AddDataLayer("EntityName", sv.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(view));
                        }
                        if (sv?.Slug == "/")
                            _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Page.ToString());
                        else
                            _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.CmsPage.ToString());

                        break;
                    case "DynamicListCollection":
                        var cl = (DynamicListCollection)(object)obj;
                        if (cl != null)
                        {
                            var collections = new { cl.Slug, cl.Name, cl.Id, cl.MetaKeywords, cl.NoOfRecords, cl.MetaTitle };
                            _headTagbuilder.AddDataLayer("EntityId", cl.Id);
                            _headTagbuilder.AddDataLayer("EntityName", cl.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(collections));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Collection.ToString());
                        break;
                    case "DynamicListModel":
                        var dl = (DynamicListModel)(object)obj;
                        if (dl != null)
                        {
                            var collections = new { dl.Name,  dl.Id, dl.MetaKeywords, dl.DisplayTitle, dl.MetaTitle };
                            _headTagbuilder.AddDataLayer("EntityId", dl.Id);
                            _headTagbuilder.AddDataLayer("EntityName", dl.Name);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(collections));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Collection.ToString());
                        break;
                    case "SearchRequestModel":
                        var sm = (SearchRequestModel)(object)obj;
                        if (sm != null)
                        {
                            _headTagbuilder.AddDataLayer("EntityId", sm.FreeText);
                            _headTagbuilder.AddDataLayer("EntityName", sm.FreeText);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(sm));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Search.ToString());
                        break;
                    case "CustomerProfileModel":
                        var user = (CustomerProfileModel)(object)obj;
                        if (user != null)
                        {
                            DateTime dob;
                            DateTime.TryParse(user.CustomerDetail.YearOfBirth + "-" + user.CustomerDetail.MonthOfBirth + "-" + user.CustomerDetail.DayOfBirth, out dob); 

                            var userBase = new
                            {
                                id=user.CustomerDetail.UserId,
                                name = user.CustomerDetail.FirstName + user.CustomerDetail.LastName,
                                dateOfBirth = (dob.Date==DateTime.MinValue) ? "" : dob.ToString(),
                                gender = user.CustomerDetail.Gender,
                                email = user.CustomerDetail.Email,
                                postCode = user.CustomerDetail.PostCode
                            };
                            // making the dob format standard. 
                             user.ChangePasswordViewModel = null; //explicitly setting this view to nulll to avoid sending accidentla pwd data. 
                             user.CustomerDetail.BirthDate = user.CustomerDetail.YearOfBirth + "-" + user.CustomerDetail.MonthOfBirth + "-" + user.CustomerDetail.DayOfBirth;
                            _headTagbuilder.AddDataLayer("EntityId", user.CustomerDetail.UserId);
                            _headTagbuilder.AddDataLayer("EntityName", user.CustomerDetail.FirstName + user.CustomerDetail.LastName);
                            _headTagbuilder.AddDataLayer("Entity", JsonConvert.SerializeObject(userBase));
                        }
                        _headTagbuilder.AddDataLayer("EntityType", WebhookEntityTypes.Customer.ToString());
                        break;
                    default:
                        break;
                }
            }

        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _headTagbuilder.AddDataLayer("Action", requestContext?.RouteData?.Values["action"]?.ToString());

        }
        private static bool IsAjaxRequest()
        {
            var request = System.Web.HttpContext.Current.Request;
            if (request == null)
                return false;
            if (request["X-Requested-With"] == "XMLHttpRequest")
                return true;
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        public CustomJsonResult<T> CustomJson<T>(T model)
        {
            var retObj = new CustomJsonResult<T>() { Data = model };

            if (ModelState.IsValid) return retObj;

            foreach (var key in ModelState.Keys)
            {
                ModelState modelState = null;
                if (!ModelState.TryGetValue(key, out modelState)) continue;
                foreach (var error in modelState.Errors)
                {
                    retObj.AddError(error.ErrorMessage);
                }
            }

            return retObj;
        }

        public ActionResult RedirectToPageNotFound()
        {
            var referrer = System.Web.HttpContext.Current.Request.UrlReferrer;
            var url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            return RedirectToAction("pagenotfound", "common", new { @aspxerrorpath = url, @UrlReferrer = referrer});
        }
    }
   
}