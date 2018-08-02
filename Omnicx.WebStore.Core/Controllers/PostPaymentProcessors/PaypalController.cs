using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using Omnicx.API.SDK.Payments;
using Omnicx.API.SDK.Payments.Entities;

using System.Linq;
using System.Web.Mvc;
using Omnicx.WebStore.Models.Keys;

namespace Omnicx.WebStore.Core.Controllers.PostPaymentProcessors
{
    public class PaypalController : Controller
    {
        private readonly ICheckoutApi _checkoutApi;
        private readonly IOrderApi _orderApi;
        public PaypalController(ICheckoutApi checkoutApi, IOrderApi orderApi)
        {
            _checkoutApi = checkoutApi;
            _orderApi = orderApi;
        }
        public ActionResult Notification()
        {
            var response = new BoolResponse { IsValid = false, Message = "" };
            response.RecordId = Request.Params["bid"];
            PaymentModel payment = null;
            var settingRespose = _checkoutApi.PaymentSetting(PaymentMethodTypes.Paypal.ToString());
            var setting = settingRespose.Result;
            if (Request["oid"] != null)
            {
                var refOrderId = Request["oid"];
                string orderId = Request.Params["oid"];
                var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
                var order = orderResponse.Result;
                string paymentId = Request.Params["payId"];
                string token = Request.Params["token"];
                string payerId = Request.Params["payerId"];
                payment = order.Payments.FirstOrDefault(x => x.Id == paymentId);
                var paymentRequest = new PostProcessPaymentRequest
                {
                    CurrencyCode = order.CurrencyCode,
                    Order = order,
                    OrderTotal = payment.OrderAmount,
                    Payment = payment,
                    Token = token,
                    PayerId = payerId

                };
                var paymentResponse = setting.PostProcessPayment(paymentRequest);
                if (paymentResponse.Success == true) paymentResponse.Payment.IsValid = true;
                _checkoutApi.UpdatePayment(refOrderId, paymentResponse.Payment);
                response.RecordId = order.Id;
                response.IsValid = paymentResponse.Success;

                if (!response.IsValid)
                {
                    response.RecordId = Request.Params["bid"];
                    if (paymentResponse.Errors.Any())
                        response.Message = paymentResponse.Errors[0];
                }
            }

            return View(CustomViews.PAYMENT_RESPONSE, response);
        }

        
    }
}