using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.API.SDK.Payments.MasterCard;
using Omnicx.API.SDK.Payments.Paypal;
using Omnicx.API.SDK.Payments.Cod;
using Omnicx.API.SDK.Payments.Givex;
using Omnicx.API.SDK.Payments.Realex;
using Omnicx.API.SDK.Payments.AccountCredit;
using Omnicx.API.SDK.Payments.Cheque;
using Omnicx.Plugin.Payment.Worldpay;

namespace Omnicx.API.SDK.Payments
{
    public static class PaymentProcessorExtension
    {
        public static ProcessPaymentResult ProcessPayment(this PaymentMethodModel paymentMethod, ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult();
            IPaymentMethod paymentProcess = null;
            paymentProcess = GetPaymentProcessor(paymentMethod.SystemName);
            if (paymentProcess != null)
            {
                paymentProcess.Settings = paymentMethod;
                resp = paymentProcess.ProcessPayment(processPaymentRequest);
            }
            return resp;
        }

        public static PostProcessPaymentResponse PostProcessPayment(this PaymentMethodModel paymentMethod, PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var resp = new PostProcessPaymentResponse();
            IPaymentMethod paymentProcess = null;
            paymentProcess = GetPaymentProcessor(paymentMethod.SystemName);
            if (paymentProcess != null)
            {
                paymentProcess.Settings = paymentMethod;
                resp = paymentProcess.PostProcessPayment(postProcessPaymentRequest);
            }
            return resp;
        }
        public static IPaymentMethod GetPaymentProcessor(string systemName)
        {
            IPaymentMethod paymentProcess = null;
            if (systemName == PaymentMethodTypes.MasterCard.ToString())
                paymentProcess = new MasterCardPaymentProcessor();
            if (systemName == PaymentMethodTypes.Paypal.ToString())
                paymentProcess = new PaypalPaymentProcessor();
            if (systemName == PaymentMethodTypes.COD.ToString())
                paymentProcess = new CodPaymentProcessor();
            if (systemName == PaymentMethodTypes.Realex.ToString())
                paymentProcess = new RealexPaymentProcessor();
            if (systemName == PaymentMethodTypes.Givex.ToString())
                paymentProcess = new GivexPaymentProcessor();
            if (systemName == PaymentMethodTypes.AccountCredit.ToString())
                paymentProcess = new AccountCreditPaymentProcessor();
            if (systemName == PaymentMethodTypes.Cheque.ToString())
                paymentProcess = new ChequePaymentProcessor();
            if (systemName == PaymentMethodTypes.Worldpay.ToString())
                paymentProcess = new WorldpayPaymentProcessor();
            return paymentProcess;
        }
    }
}
