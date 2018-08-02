using Omnicx.API.SDK.Payments.Entities;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Helpers;
namespace Omnicx.API.SDK.Payments.MasterCard
{
    public class MasterCardPaymentProcessor : IPaymentMethod
    {
        public PaymentMethodModel Settings { get; set; }
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult
            {
                TimeStamp = MasterCardApi.GetTimestamp(Settings.Signature,
                    processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId,
                    processPaymentRequest.OrderTotal, processPaymentRequest.CurrencyCode)
            };
            ;
            resp.OrderId = processPaymentRequest.OrderId;          
            resp.OrderTotal = processPaymentRequest.OrderTotal.ToString();
            resp.CurrencyCode = processPaymentRequest.CurrencyCode;
            resp.RefOrderId = processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId;
            resp.ReturnUrl = Settings.NotificationUrl;
            return resp;
        }

        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var resp = new PostProcessPaymentResponse {};
            var payment = postProcessPaymentRequest.Payment;
            var mcard = new MasterCardApi(Settings);
            Billing billingAddress = new Billing();
            billingAddress.address = new Address() { };
            billingAddress.address.country = Utils.GetThreeChracterCountryCode(postProcessPaymentRequest.Order.BillingAddress.CountryCode); // "GBR";//orderDetail.Result.BillingAddress.CountryCode;
            if (string.IsNullOrEmpty(billingAddress.address.country) == true)
            {
                billingAddress.address.country = "GBR";
            }
            billingAddress.address.postcodeZip = postProcessPaymentRequest.Order.BillingAddress.PostCode;
            billingAddress.address.street = postProcessPaymentRequest.Order.BillingAddress.Address1;
            billingAddress.address.city = postProcessPaymentRequest.Order.BillingAddress.City;
            if(!string.IsNullOrEmpty(postProcessPaymentRequest.Order.BillingAddress.Address2))
            billingAddress.address.street2 = postProcessPaymentRequest.Order.BillingAddress.Address2;
            billingAddress.address.stateProvince = postProcessPaymentRequest.Order.BillingAddress.State;
            var boolResonse= mcard.CaptureNotification(ref payment, billingAddress);
            resp.Payment = payment;
            resp.Order = postProcessPaymentRequest.Order;
            if (!boolResonse.IsValid)
            {
                resp.AddError(boolResonse.Message);
            }
            return resp;
        }
    }
}
