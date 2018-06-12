using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.API.SDK.Helpers;

namespace Omnicx.API.SDK.Payments.AccountCredit
{
    public class AccountCreditPaymentProcessor : IPaymentMethod
    {
        public PaymentMethodModel Settings { get; set; }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult
            {
                OrderId = processPaymentRequest.OrderId,
                CurrencyCode = processPaymentRequest.CurrencyCode,
                AuthorizationTransactionUrl = AuthorizationTransactionUrl.AccountCredit + "/" + processPaymentRequest.OrderId,
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
