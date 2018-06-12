using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Entities;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Payments.Entities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Controllers.PostPaymentProcessors
{
    public class WorldpayController : BaseController
    {
        #region "Fields"
        private readonly HttpContextBase _httpContext;
        private readonly ICheckoutApi _checkoutApi;
        private readonly IOrderApi _orderApi;
        #endregion

        #region "CTOR"
        public WorldpayController(HttpContextBase httpContext, ICheckoutApi checkoutApi, IOrderApi orderApi)
        {
            _httpContext = httpContext;
            _checkoutApi = checkoutApi;
            _orderApi = orderApi;
        }
        #endregion
        public ActionResult Notification()
        {
            var response = new BoolResponse { IsValid = false, Message = "" };

            PaymentModel payment = null;
            var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.Worldpay.ToString());
            var setting = settingResponse.Result;

            if(Request.QueryString["paymentStatus"] == "AUTHORISED")
            {
                //var orderNo = Request.QueryString["orderNo"];
                var refOrderId = Request.Params["transId"];
                string orderId = Request.Params["orderId"];
                var paymentAmount = (Convert.ToDecimal(Request.Params["paymentAmount"]) / 100);
                var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
                var order = orderResponse.Result;

                string paymentId = orderId.Split('-')[1];
                payment = order.Payments.FirstOrDefault(x => x.Id == paymentId);
                payment.IsValid = true;
                payment.Status = PaymentStatus.Paid.GetHashCode();
                payment.PaidAmount = paymentAmount;
                _checkoutApi.UpdatePayment(order.Id, payment);

                response = new BoolResponse { IsValid = true, RecordId = order.Id };
                SetDataLayerVariables(order, WebhookEventTypes.CheckoutPayment);

                return View(CustomViews.PAYMENT_RESPONSE, response);
            }
            return View(CustomViews.PAYMENT_RESPONSE, response);
        }

    }
}