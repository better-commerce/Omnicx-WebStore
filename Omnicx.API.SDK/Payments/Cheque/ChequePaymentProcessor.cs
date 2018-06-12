using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;

namespace Omnicx.API.SDK.Payments.Cheque
{
    public class ChequePaymentProcessor : IPaymentMethod
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
