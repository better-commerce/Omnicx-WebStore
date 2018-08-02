using System;
using System.ServiceModel;
using Omnicx.API.SDK.GivexService;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;
namespace Omnicx.API.SDK.Payments.Givex
{
    public  class GivexPaymentProcessor : IPaymentMethod
    {
        private transPortTypeClient _givexClient;
        public PaymentMethodModel Settings { get; set; }
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult
            {
                OrderId = processPaymentRequest.OrderId,
                CurrencyCode = processPaymentRequest.CurrencyCode,
                AuthorizationTransactionUrl = Settings.NotificationUrl + "/" + processPaymentRequest.OrderId,
                UseAuthUrlToRedirect = true
            };
            var req = new PreAuth
            {
                id = GetGivexIdentification(),
                givexNumber = processPaymentRequest.CardNo,
                reference = processPaymentRequest.OrderNo + "." + processPaymentRequest.PaymentId,
                amount = processPaymentRequest.OrderTotal,
                securityCode = processPaymentRequest.Cvv
            };

            try
            {
                var givResp = _givexClient.PreAuth(req);
                if ((givResp != null))
                {

                    resp.AuthorizationTransactionCode = givResp.authCode.ToString();
                    resp.AuthorizedAmount = givResp.amount;
                   // resp.BalanceAmount = givResp.certBalance;
                }
                else
                {
                    resp.AddError(ErrorCodesPayment.InvalidResponseFromPSP.ToString());
                }
            }
            catch (EndpointNotFoundException endPointExc)
            {
                resp.AddError(ErrorCodesPayment.EndPointNotFound.ToString() + " " + endPointExc.Message);
            }
            catch (FaultException ex)
            {
                resp.AddError(ErrorCodesPayment.RejectedByPSP.ToString() + " " + ex.Message);
            }
            catch (Exception genEx)
            {
                resp.AddError(ErrorCodesPayment.ExceptionInServiceCall.ToString() + " " + genEx.Message);

            }
            return resp;
        }
        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

       

        private Identification GetGivexIdentification()
        {
            _givexClient = new transPortTypeClient();
            var id = new Identification
            {
                token = Settings.Signature,
                user = Settings.UserName,
                userPasswd = Settings.Password,
                language = "en"
            };

            return id;
        }
    }
}
