using Omnicx.API.SDK.Payments.Entities;
using Omnicx.WebStore.Models.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Payments.Klarna
{
    public class KlarnaPaymentProcessor : IPaymentMethod
    {
        public PaymentMethodModel Settings { get; set; }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var klarna = new KlarnaApi(Settings);
            var sessionId= processPaymentRequest.Order.Payments?.Where(x => x.Id != processPaymentRequest.PaymentId && !string.IsNullOrEmpty(x.PspSessionCookie)).FirstOrDefault()?.PspSessionCookie;
            var clientToken = processPaymentRequest.Order.Payments?.Where(x => x.Id != processPaymentRequest.PaymentId && !string.IsNullOrEmpty(x.AuthCode)).FirstOrDefault()?.AuthCode;
            TokenResponse tokenResp = null;
            if(string.IsNullOrEmpty(sessionId))
                tokenResp = klarna.CreateSession(processPaymentRequest);
            else
                tokenResp = klarna.UpdateSession(processPaymentRequest, sessionId, clientToken);

            var resp = new ProcessPaymentResult
            {
                OrderId = processPaymentRequest.OrderId,
                CurrencyCode = processPaymentRequest.CurrencyCode,
                AuthorizationTransactionUrl = Settings.NotificationUrl + "/" + processPaymentRequest.OrderId,
                UseAuthUrlToRedirect = false,
                RefOrderId = processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId,
                AuthorizationTransactionCode = tokenResp.ClientToken,
                PspSessionCookie = tokenResp.SessionId
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
