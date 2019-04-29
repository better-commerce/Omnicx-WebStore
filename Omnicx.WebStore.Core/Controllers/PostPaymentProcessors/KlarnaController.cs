using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.API.SDK.Payments.Klarna;
using Omnicx.WebStore.Core.Controllers;
using Omnicx.WebStore.Core.Helpers;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.Site.Core.Controllers.PostPaymentProcessors
{
   
    public class KlarnaController : BaseController
    {
        #region "Fields"
        private readonly ICheckoutApi _checkoutApi;
        private readonly IOrderApi _orderApi;
        #endregion

        #region "CTOR"
        public KlarnaController(HttpContextBase httpContext, ICheckoutApi checkoutApi, IOrderApi orderApi)
        {
            _checkoutApi = checkoutApi;
            _orderApi = orderApi;
        }
        #endregion

        public ActionResult Notification()
        {
            var response = new BoolResponse { IsValid = false, Message = "" };
            if(Request.Params["status"] == "true")
            {
                response = new BoolResponse { IsValid = true, RecordId = Request.Params["recordId"] };
                SiteUtils.ResetBasketCookieAndSession();
                return View(CustomViews.PAYMENT_RESPONSE, response);
            }
            return View(CustomViews.PAYMENT_RESPONSE, response);

        }

            /// <summary>
            /// Klarna create order
            /// </summary>
            /// <param name="id">authorizationToken</param>
            /// <param name="processPaymentRequest"></param>
            /// <param name="orderId"></param>
            /// <param name="paymentId"></param>
            public ActionResult CreateOrder(string id, CheckoutModel processPaymentRequest, string orderId, string paymentId)
        {
            var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.Klarna.ToString());
            var setting = settingResponse.Result;
            var klarna = new KlarnaApi(setting);
            //var token = klarna.GenerateConsumerToken(processPaymentRequest, id);
            var orderResp = klarna.CreateOrder(processPaymentRequest, id, orderId, paymentId);

            var response = new BoolResponse { IsValid = false, Message = "" };
            PaymentModel payment = null;
            if (orderResp.FraudStatus == "ACCEPTED")
            {
                var refOrderId = orderResp.RefOrderId;
                var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
                var order = orderResponse.Result;

                payment = order.Payments.FirstOrDefault(x => x.Id == orderResp.PaymentId);
                payment.IsValid = true;
                payment.Status = PaymentStatus.Paid.GetHashCode();
                payment.PaidAmount = (Convert.ToDecimal(orderResp.OrderAmount) / 100);
                payment.PspResponseCode = orderResp.OrderId;
                payment.PspResponseMessage = orderResp.FraudStatus;
                //payment.FraudScore = orderResp.FraudStatus;
                _checkoutApi.UpdatePayment(order.Id, payment);

                response = new BoolResponse { IsValid = true, RecordId = order.Id };
                SetDataLayerVariables(order, WebhookEventTypes.CheckoutPayment);

                return JsonSuccess(new { response = response, notificationUrl = setting.NotificationUrl }, JsonRequestBehavior.AllowGet);
            }
            return JsonSuccess(new { response = response, notificationUrl = setting.NotificationUrl }, JsonRequestBehavior.AllowGet);
        }
    }
}