using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;
using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Common;
using Omnicx.API.SDK.Payments;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.API.SDK.Payments.MasterCard;
using Omnicx.API.SDK.Entities;
using System.Linq;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Controllers.PostPaymentProcessors
{
    public class MasterCardController : BaseController
    {
        private readonly ICheckoutApi _checkoutApi;
        private readonly IOrderApi _orderApi;
        public MasterCardController(ICheckoutApi checkoutApi, IOrderApi orderApi)
        {
            _checkoutApi = checkoutApi;
            _orderApi = orderApi;
        }
        public ActionResult Notification()
        {
            var response = new BoolResponse { IsValid = false, Message = "" };
            response.RecordId = Request.Params["bid"];
            // this can hppen in situation when bid is suffixed to eth query string twice breaking the whole code
            /// so a simple handling fro the same has been put in
            /// 
            if (response.RecordId.Contains(","))
            {
                var tmpId = response.RecordId.Split(',');
                response.RecordId = tmpId[0];
            }
            PaymentModel payment = null;
            var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
            var setting = settingResponse.Result;

            var mcard = new MasterCardApi(setting);
            if (Request["orderId"] != null)
            {
                var refOrderId = Request["transId"];
                string orderId = Request.Params["orderId"];
                var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(refOrderId));
                var order = orderResponse.Result;
                string paymentId = orderId.Split('-')[1];
                payment = order.Payments.FirstOrDefault(x => x.Id == paymentId);
                var paymentRequest = new PostProcessPaymentRequest
                {
                    CurrencyCode = order.CurrencyCode,
                    Order = order,
                    OrderTotal = payment.OrderAmount,
                    Payment = payment,
                };
                var paymentResponse = setting.PostProcessPayment(paymentRequest);
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

        public ActionResult Check3DSecure(string sessionId, decimal amount, string currency, string secure3DauthUrl,
            string secure3DId)
        {
            var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
            var setting = settingResponse.Result;
            var mcard = new MasterCardApi(setting);
            var secure3D = mcard.Check3DSecureEnrollment(sessionId, amount, currency, secure3DauthUrl, secure3DId);
            return JsonSuccess(secure3D, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Authorize(string sessionId, string year, string month, decimal amount, string orderId,
            string transId, string currency)
        {
            var settingResponse = _checkoutApi.PaymentSetting(PaymentMethodTypes.MasterCard.ToString());
            var setting = settingResponse.Result;
            Billing billingAddress = new Billing();
            billingAddress.address = new Address() { };
            var orderDetail = _orderApi.GetOrdDetail(orderId);

            billingAddress.address.country = Utils.GetThreeChracterCountryCode(orderDetail.Result.BillingAddress.CountryCode); // "GBR";//orderDetail.Result.BillingAddress.CountryCode;
            if (string.IsNullOrEmpty(billingAddress.address.country) == true)
            {
                billingAddress.address.country = "GBR";
            }
            billingAddress.address.postcodeZip = orderDetail.Result.BillingAddress.PostCode;
            billingAddress.address.street = orderDetail.Result.BillingAddress.Address1;
            billingAddress.address.city = orderDetail.Result.BillingAddress.City;
            if (!string.IsNullOrEmpty(orderDetail.Result.BillingAddress.Address2))
                billingAddress.address.street2 = orderDetail.Result.BillingAddress.Address2;
            billingAddress.address.stateProvince = orderDetail.Result.BillingAddress.State;
            var mcard = new MasterCardApi(setting);
            var payResult = mcard.Authorize(sessionId, year, month, amount, orderId, transId, currency, billingAddress);
            
            return JsonSuccess(payResult, JsonRequestBehavior.AllowGet);
        }
    }
}