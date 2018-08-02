using Microsoft.Security.Application;
using Omnicx.API.SDK.Api.Commerce;

using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System.Web.Mvc;

namespace Omnicx.WebStore.Core.Controllers.PostPaymentProcessors
{
    public class CodController : BaseController
    {
        private readonly IOrderApi _orderApi;
        public CodController(IOrderApi orderApi)
        {
            _orderApi = orderApi;
        }
        /// <summary>
        /// Payement response accepted for Cash on Delivery
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PaymentResponse(string id)
        {
            var orderResponse = _orderApi.GetOrdDetail(Sanitizer.GetSafeHtmlFragment(id));
            var order = orderResponse.Result;
            //order.Payment.IsValid = true;
            //order.Payment.Status = order.Payment.IsValid.GetHashCode();
            //_checkoutApi.UpdatePayment(order.Id, order.Payment);
            var response = new BoolResponse { IsValid = true, RecordId = order.Id };
            SetDataLayerVariables(order, WebhookEventTypes.CheckoutPayment);
            return View(CustomViews.PAYMENT_RESPONSE, response);
        }
    }
}