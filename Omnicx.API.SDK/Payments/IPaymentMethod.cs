using Omnicx.API.SDK.Payments.Entities;
using Omnicx.WebStore.Models.Commerce;
namespace Omnicx.API.SDK.Payments
{
    public partial interface IPaymentMethod
    {
        /// <summary>
        /// Process a paymetn
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymenteRquest);

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest);

        PaymentMethodModel Settings { get; set; }
    }
}
