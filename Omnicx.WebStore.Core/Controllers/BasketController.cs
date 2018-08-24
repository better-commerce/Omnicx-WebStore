using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Models.Commerce;
using Microsoft.Security.Application;
using Omnicx.WebStore.Models;
using System.Linq;
using Omnicx.WebStore.Models.Keys;

using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Common;
using Omnicx.API.SDK.Helpers;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Models.Enums;
namespace Omnicx.WebStore.Core.Controllers
{
    public class BasketController : BaseController
    {

        private readonly IBasketApi _basketApi;
        private readonly IShippingApi _shippingApi;
        private readonly ICustomerApi _customerRepository;
        private readonly IOrderApi _orderRepository;
        public BasketController(IBasketApi basketApi,  IShippingApi shippingApi, ICustomerApi customerRepository, IOrderApi orderRepository)
            
        {
            _basketApi = basketApi;
            _shippingApi = shippingApi;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Renders the Basket View where the data is populated via an AJAX call given below
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            var basket = GetIndexBasketData();
            return View(CustomViews.BASKET, basket??new BasketModel { });
        }

        protected BasketModel GetIndexBasketData(string basketId = "")
        {
            var basket = _basketApi.GetBasketData(basketId)?.Result;
            if (Request.UrlReferrer != null)
                ViewBag.PrevPage = Request.UrlReferrer.ToString();
            else
                ViewBag.PrevPage = SiteViewTypes.Home;
            SetDataLayerVariables(basket, WebhookEventTypes.BasketViewed);
            return basket;
        }
        //public ActionResult GetBasketData()
        //{
        //    var basket = _basketApi.GetBasketData("");
        //    return JsonSuccess(basket, JsonRequestBehavior.AllowGet);
        //}
        public virtual async Task<ActionResult> GetBasketData()
        {
            var basket = SiteUtils.HasBasketAction() ? await _basketApi.GetBasketDataAsync("") : null;
            if (basket?.Result != null)
                SiteUtils.SetBasketAction(basket?.Result.Id);
            return JsonSuccess(basket?.Result, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult GetShippingMethods(string countryCode)
        {         
            var shippingMethods = _shippingApi.GetShippingMethods(_sessionContext.SessionId,Sanitizer.GetSafeHtmlFragment(countryCode));
            return JsonSuccess(shippingMethods.Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult AddtoBasket(BasketAddModel model)
        {
            var basketModel = new BasketAddModel
            {
                BasketId = Sanitizer.GetSafeHtmlFragment(model.BasketId),
                DisplayOrder = model.DisplayOrder,
                ProductId = Sanitizer.GetSafeHtmlFragment(model.ProductId),
                Qty = model.Qty,
                ItemType = model.ItemType,
                SubscriptionId = Sanitizer.GetSafeHtmlFragment(model.SubscriptionId),
                PostCode = model.PostCode
            };
            var basket = _basketApi.AddToBasket(basketModel);
            if (basket?.Result != null)
                SiteUtils.SetBasketAction(basket?.Result.Id);
            return JsonSuccess(basket, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult ApplyPromoCode(string id, string promoCode)
        {
            var resp = _basketApi.ApplyPromoCode(Sanitizer.GetSafeHtmlFragment(id), Sanitizer.GetSafeHtmlFragment(promoCode));
            if (resp.Result != null && resp.Result.IsVaild != false) return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
            if (!string.IsNullOrEmpty(resp.Message))
                return JsonError(resp.Message, JsonRequestBehavior.DenyGet);
            else
            {
                return JsonError("false", JsonRequestBehavior.DenyGet);
            }
        }
        
        public virtual ActionResult RemovePromoCode(string id, string promoCode)
        {
            var resp = _basketApi.RemovePromoCode(Sanitizer.GetSafeHtmlFragment(id), Sanitizer.GetSafeHtmlFragment(promoCode));
            return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public virtual ActionResult UpdateShipping(string id,string shippingId,NominatedDeliveryModel nominatedDelivery)
        {
            var response= _basketApi.UpdateShipping(Sanitizer.GetSafeHtmlFragment(id), Sanitizer.GetSafeHtmlFragment(shippingId), nominatedDelivery);
            var basket = response.Result;
            if (basket.LineItems == null)
            {
                basket.LineItems = new List<BasketLineModel>();
            }
            return JsonSuccess(new { basket = basket, BasketStage = BasketStage.ShippingMethodSelected.GetHashCode() }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult HeaderBasket()
        {

            return PartialView(CustomViews.HEADER_BASKET_VIEW);
        }

        /// <summary>
        /// Moving multiple product from basket to wishlist
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BasketToWishlist(List<BasketAddModel> model)
        {
            var response= new ResponseModel<bool>();
            foreach (var product in model)
            {
                 response = _customerRepository.AddProductToWishList(Sanitizer.GetSafeHtmlFragment(product.ProductId.ToLower()), _sessionContext.CurrentUser.UserId,false);
            }
            var customerId = _sessionContext.CurrentUser.UserId.ToString();
            var key = string.Format(CacheKeys.WishList, customerId);
            var result = _customerRepository.GetWishlist(customerId, false);
            System.Web.HttpContext.Current.Session[key] = result.Result;
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Moving multiple product from wishlist to basket
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WishlistToBasket(List<BasketAddModel> model)
        {
            foreach (var product in model)
            {
                var basket = _basketApi.AddToBasket(product);
                _customerRepository.RemoveWishList(Convert.ToString(_sessionContext.CurrentUser.UserId), product.ProductId, false);
                if (_sessionContext.CurrentUser != null)
                {
                    var key = string.Format(CacheKeys.WishList, Convert.ToString(_sessionContext.CurrentUser.UserId));
                    System.Web.HttpContext.Current.Session[key] = null;
                }
                SiteUtils.SetBasketAction(basket?.Result.Id);
            }
            return JsonSuccess(true, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult BulkAddProduct(List<BasketAddModel> model)
        {
            if(model.Any(prod => string.IsNullOrEmpty(prod.StockCode)))
                return JsonError("false", JsonRequestBehavior.DenyGet);

            model.ForEach(prod=>prod.StockCode=prod.StockCode.ToUpper());

            var basket = _basketApi.BulkAddProduct(model);
            SiteUtils.SetBasketAction(basket?.Result.Id);
            return JsonSuccess(basket,JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ReOrder(string id)
        {
            var result = _orderRepository.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(id));
            var model = new List<BasketAddModel>();
            if (result.Result != null && result.Result.Items.Any())
            {
                foreach (var item in result.Result.Items)
                {
                    var line = new BasketAddModel()
                    {
                        StockCode = item.StockCode,
                        Qty = item.Qty,
                        CustomInfo1 = item.CustomInfo1,
                        CustomInfo2 = item.CustomInfo2,
                        CustomInfo3 = item.CustomInfo3,
                        CustomInfo4 = item.CustomInfo4,
                        CustomInfo5 = item.CustomInfo5
                    };
                    model.Add(line);
                }
            }
            var basket = _basketApi.BulkAddProduct(model);
            SiteUtils.SetBasketAction(basket?.Result.Id);
            return JsonSuccess(basket, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult BulkOrderView()
        {
            ViewBag.isB2BEnable = _sessionContext.CurrentSiteConfig.B2BSettings.EnableB2B;
            ViewBag.ShowBulkOrderPad = _sessionContext.CurrentSiteConfig.B2BSettings.ShowBulkOrderPad;

            return PartialView(CustomViews.HEADER_BULKORDER_VIEW);
        }

        /// <summary>
        /// Getting Basket Related Products
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetBasketRelatedProducts(string id)
        {
            var result = new List<ProductModel>();
            if (!(string.IsNullOrEmpty(id) || id==Guid.Empty.ToString()))
             result = _basketApi.GetRelatedProducts(id)?.Result??new List<ProductModel>(); 
            return JsonSuccess(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSavedBaskets()
        {
            if (_sessionContext.CurrentUser != null && _sessionContext.CurrentUser.UserId != null)
            {
                var result = _basketApi.GetAllUserBaskets(_sessionContext.CurrentUser.UserId);
                return JsonSuccess(result, JsonRequestBehavior.DenyGet);
            }
            return JsonSuccess("", JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// This method use for store value of Powder coating and Tube cutting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult UpdateBasketInfo(HeaderCustomInfo model)
        {
            var basket = _basketApi.GetBasketData(model.BasketId.ToString());
            SiteUtils.SetBasketAction(resetAction:true);
            if (model.LineInfo != null)
            {
                if (basket != null)
                {
                    var newLines = (from p in model.LineInfo
                                    where basket.Result.LineItems.Any(l => l.ProductId == p.ProductId.ToUpper()) == false
                                    select new BasketAddModel()
                                    { BasketId = model.BasketId.ToString(), ProductId = p.ProductId, DisplayOrder = 0, Qty = p.Qty,StockCode = p.StockCode, CustomInfo2 = p.CustomInfo2 }).ToList();
                    if (newLines.Where(x => !string.IsNullOrEmpty(x.StockCode)).Any())
                    {
                        var basketData = _basketApi.BulkAddProduct(newLines);
                        model.BasketId = Guid.Parse(basketData.Result.Id);
                    }

                    var existLines = (from p in basket.Result.LineItems
                                      where model.LineInfo.Any(l => l.ProductId.ToUpper() == p.ProductId.ToUpper()) == true
                                      select p).ToList();

                    model.LineInfo.ForEach(l => l.Qty = Math.Max(l.Qty, existLines.FirstOrDefault(li => li.ProductId.ToUpper() == l.ProductId.ToUpper()) != null ? existLines.FirstOrDefault(li => li.ProductId.ToUpper() == l.ProductId.ToUpper()).Qty : 0));

                }
                var resp = _basketApi.UpdateBasketInfo(model.BasketId.ToString(), model);
                return JsonSuccess(resp, JsonRequestBehavior.DenyGet);
            }
            return JsonSuccess(basket, JsonRequestBehavior.DenyGet);
        }
        public virtual ActionResult GetDeliveryMethodsByPostCode(string countryCode, string basketId, string postCode,string appliedShippingId)
        {
            var shippingMethods = _shippingApi.GetShippingMethods(basketId, Sanitizer.GetSafeHtmlFragment(countryCode), postCode);
            bool IsPriceOnRequest = false;
            var response = new ResponseModel<BasketModel> { };
            // If don't found any shipping in apply postcode control will to to else part and remove applied shipping from basket
            if (shippingMethods.Result != null && shippingMethods.Result.Count > 0)
            {
                var shippingMethod = shippingMethods.Result.Where(x => x.Id == Guid.Parse(appliedShippingId)).FirstOrDefault();
                // shippingMethod can have only that case if shippingMethods list having applied shipping id in that case 
                //no need to updateshipping
                if (shippingMethod == null)
                {
                    // In case shippingMethods list don't have applied shipping id default shipping will be apply
                    shippingMethod = shippingMethods.Result.Where(x => x.IsDefault = true).FirstOrDefault();
                    response = _basketApi.UpdateShipping(Sanitizer.GetSafeHtmlFragment(basketId), Sanitizer.GetSafeHtmlFragment(Convert.ToString(shippingMethod.Id)), null);
                    IsPriceOnRequest = shippingMethod.IsPriceOnRequest;
                    if (shippingMethod == null)
                    {
                        // In case default shipping is not set that case first shipping will be apply
                        shippingMethod = shippingMethods.Result.FirstOrDefault();
                        response = _basketApi.UpdateShipping(Sanitizer.GetSafeHtmlFragment(basketId), Sanitizer.GetSafeHtmlFragment(Convert.ToString(shippingMethod.Id)), null);
                        IsPriceOnRequest = shippingMethod.IsPriceOnRequest;
                    }
                }
                else
                {
                    response = _basketApi.UpdateShipping(Sanitizer.GetSafeHtmlFragment(basketId), Sanitizer.GetSafeHtmlFragment(Convert.ToString(shippingMethod.Id)), null);
                    IsPriceOnRequest = shippingMethod.IsPriceOnRequest;
                }
            }
            else
                response = _basketApi.UpdateShipping(Sanitizer.GetSafeHtmlFragment(basketId), Sanitizer.GetSafeHtmlFragment(Convert.ToString(Guid.Empty)), null); //Applied shipping remove in case don't found the shipping

            response.Result.IsPriceOnRequest = IsPriceOnRequest;
            response.Result.shippingMethods = shippingMethods.Result;
            return JsonSuccess(response.Result, JsonRequestBehavior.AllowGet);
        }       
    }
}
