using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;

namespace Omnicx.API.SDK.Payments.Cod
{
    public class CodPaymentProcessor : IPaymentMethod
    {
        public PaymentMethodModel Settings { get; set; }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult
            {
                OrderId = processPaymentRequest.OrderId,
                CurrencyCode = processPaymentRequest.CurrencyCode,
                AuthorizationTransactionUrl = Settings.NotificationUrl + "/" + processPaymentRequest.OrderId,
                UseAuthUrlToRedirect = true,
                AuthorizedAmount = processPaymentRequest.OrderTotal
            };
            return resp;
        }
        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var resp = new PostProcessPaymentResponse();
            return resp;
        }
    }
}
