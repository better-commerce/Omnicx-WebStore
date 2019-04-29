using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Omnicx.API.SDK.Helpers;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;
using Omnicx.API.SDK.PaypalService;

namespace Omnicx.API.SDK.Payments.Paypal
{
    public class PaypalPaymentProcessor : IPaymentMethod
    {
        private PayPalAPIInterfaceClient _paypalService1;
        private PayPalAPIAAInterfaceClient _paypalService2;
        public PaymentMethodModel Settings { get; set; }
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var paymentResponse = new ProcessPaymentResult();
            var req = new SetExpressCheckoutReq();
            req.SetExpressCheckoutRequest = new SetExpressCheckoutRequestType {Version = Settings.Version};
            var details = new SetExpressCheckoutRequestDetailsType();
            req.SetExpressCheckoutRequest.SetExpressCheckoutRequestDetails = details;

            var currencyCode = (CurrencyCodeType)Utils.GetEnumValueByName(typeof(CurrencyCodeType), processPaymentRequest.CurrencyCode);
            var oPayDetail = new PaymentDetailsType();
            oPayDetail.OrderTotal = new BasicAmountType();
            oPayDetail.OrderTotal.Value = (processPaymentRequest.OrderTotal).ToString("N", new CultureInfo("en-gb"));
            oPayDetail.OrderTotal.currencyID = currencyCode;

            oPayDetail.ShippingMethod = ShippingServiceCodeType.ShippingMethodStandard;

            oPayDetail.ItemTotal = new BasicAmountType();
            oPayDetail.ItemTotal.Value = (processPaymentRequest.OrderTotal - processPaymentRequest.Order.ShippingCharge.Raw.WithTax).ToString("N", new CultureInfo("en-gb"));
            oPayDetail.ItemTotal.currencyID = currencyCode;
            oPayDetail.ShippingMethodSpecified = true;
            oPayDetail.ShippingTotal = new BasicAmountType();
            oPayDetail.ShippingTotal.Value = processPaymentRequest.Order.ShippingCharge.Raw.WithTax.ToString("N", new CultureInfo("en-gb"));
            oPayDetail.ShippingTotal.currencyID = currencyCode;
            oPayDetail.OrderDescription = "Order Total:" + processPaymentRequest.OrderTotal.ToString();
            oPayDetail.PaymentActionSpecified = true;
            if(Settings.EnableImmediateCapture)
                 oPayDetail.PaymentAction = PaymentActionCodeType.Sale;
            else
                 oPayDetail.PaymentAction = PaymentActionCodeType.Authorization;


            oPayDetail.InvoiceID = processPaymentRequest.OrderId;

            var oItems = new List<PaymentDetailsItemType>();
            int i = 1;
            foreach (var itm in processPaymentRequest.Order.Items)
            {

                var oItem = new PaymentDetailsItemType { Number = i.ToString(CultureInfo.InvariantCulture) };
                oItem.Name = itm.StockCode;
                oItem.Description = itm.Name;
                oItem.Amount = new BasicAmountType();
                decimal itmPrice = itm.Price.Raw.WithTax;
                oItem.Amount.Value = itmPrice.ToString("N", new CultureInfo("en-gb"));
                oItem.Amount.currencyID = currencyCode;
                oItem.Quantity = itm.Qty.ToString(CultureInfo.InvariantCulture);
                i = i + 1;
                oItems.Add(oItem);
            }
            if (processPaymentRequest.Order.Discount.Raw.WithTax > 0)
            {
                var odiscountItem = new PaymentDetailsItemType { Number = i.ToString(CultureInfo.InvariantCulture) };
                odiscountItem.Name = "Discount";
                odiscountItem.Description = "Discount";
                odiscountItem.Amount = new BasicAmountType();
                decimal itmPrice = -processPaymentRequest.Order.Discount.Raw.WithTax;
                odiscountItem.Amount.Value = itmPrice.ToString("N", new CultureInfo("en-GB"));
                odiscountItem.Amount.currencyID = currencyCode;
                odiscountItem.Quantity = (1).ToString(CultureInfo.InvariantCulture);
                oItems.Add(odiscountItem);
            }
            if (processPaymentRequest.Order.GrandTotal.Raw.WithTax > processPaymentRequest.OrderTotal)
            {
                var odiscountItem = new PaymentDetailsItemType { Number = i.ToString(CultureInfo.InvariantCulture) };
                odiscountItem.Name = "Paid By Other Source";
                odiscountItem.Description = "Other Source";
                odiscountItem.Amount = new BasicAmountType();
                decimal itmPrice = -(processPaymentRequest.Order.GrandTotal.Raw.WithTax - processPaymentRequest.OrderTotal);
                odiscountItem.Amount.Value = itmPrice.ToString("N", new CultureInfo("en-GB"));
                odiscountItem.Amount.currencyID = currencyCode;
                odiscountItem.Quantity = (1).ToString(CultureInfo.InvariantCulture);
                oItems.Add(odiscountItem);
            }

            oPayDetail.PaymentDetailsItem = oItems.ToArray();

            PaymentDetailsType[] oPayDetails = { oPayDetail };

            details.PaymentDetails = oPayDetails;
          
            details.ReturnURL = Settings.NotificationUrl + "?oid="  + processPaymentRequest.OrderId + "&payid=" +  processPaymentRequest.PaymentId;
            details.CancelURL = Settings.CancelUrl + "/" + processPaymentRequest.BasketId ;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            var credentials = PaypalSecurityHeader();
            SetExpressCheckoutResponseType response = _paypalService2.SetExpressCheckout(ref credentials, req);

            if (response.Ack == AckCodeType.Success)
            {
                paymentResponse.AuthorizationTransactionUrl = GetPaypalUrl(response.Token);
                paymentResponse.UseAuthUrlToRedirect = true;
            }
            else
            {
                foreach (var er in response.Errors)
                {
                    paymentResponse.AddError(er.ErrorCode + " : " + er.ShortMessage);
                }
            }
            return paymentResponse;
        }
        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var paymentResponse = new PostProcessPaymentResponse();
            var req = new DoExpressCheckoutPaymentReq();
            var request = new DoExpressCheckoutPaymentRequestType();
            var payment = postProcessPaymentRequest.Payment;
            var order = postProcessPaymentRequest.Order;
            req.DoExpressCheckoutPaymentRequest = request;
            request.Version = Settings.Version;
            var details = new DoExpressCheckoutPaymentRequestDetailsType();
            request.DoExpressCheckoutPaymentRequestDetails = details;
            if (Settings.EnableImmediateCapture)
                details.PaymentAction = PaymentActionCodeType.Sale;
            else
                details.PaymentAction = PaymentActionCodeType.Authorization;

            details.PaymentActionSpecified = true;
            details.Token = postProcessPaymentRequest.Token;
            details.PayerID = postProcessPaymentRequest.PayerId;
            var payer = GetPayerInfo(postProcessPaymentRequest.Token, postProcessPaymentRequest.PayerId);

            var paymentDetail = new PaymentDetailsType();
            paymentDetail.OrderTotal = new BasicAmountType();
            paymentDetail.OrderTotal.Value = order.GrandTotal.Raw.WithTax.ToString("N", new CultureInfo("en-us"));
            var currencyCode = (CurrencyCodeType)Utils.GetEnumValueByName(typeof(CurrencyCodeType), order.CurrencyCode);

            paymentDetail.OrderTotal.currencyID = currencyCode;
            paymentDetail.ButtonSource = "";
            if (Settings.EnableImmediateCapture)
                paymentDetail.PaymentAction = PaymentActionCodeType.Sale;
            else
                paymentDetail.PaymentAction = PaymentActionCodeType.Authorization;

            paymentDetail.PaymentActionSpecified = true;

            PaymentDetailsType[] paymentDetails = { paymentDetail };
            details.PaymentDetails = paymentDetails;
            //  System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            var credentials = PaypalSecurityHeader();
            DoExpressCheckoutPaymentResponseType response = _paypalService2.DoExpressCheckoutPayment(ref credentials, req);
            if (response.Ack == AckCodeType.Success)
            {
                payment.PaymentMethod = "Paypal";
                payment.CvcResult = payer.AccountVerifyCode;
                payment.AvsResult = payer.AddressVerifyCode;
                payment.Secure3DResult = "";
                payment.CardHolderName = payer.Email;
                payment.IssuerCountry = payer.CountryCode;
                payment.CardNo = payer.PayerId;
                payment.IsVerify = payer.IsVerify;
                payment.IsValidAddress = payer.IsValidAddress;
                payment.Info1 = "";
                if (!string.IsNullOrEmpty(payer.Address1)) payment.Info1 = payer.Address1 + ", ";
                if (!string.IsNullOrEmpty(payer.Address2)) payment.Info1 = payment.Info1 + payer.Address2 + ", ";
                if (!string.IsNullOrEmpty(payer.City)) payment.Info1 = payment.Info1 + payer.City + ", ";
                if (!string.IsNullOrEmpty(payer.State)) payment.Info1 = payment.Info1 + payer.State + ", ";
                if (!string.IsNullOrEmpty(payer.PostCode)) payment.Info1 = payment.Info1 + payer.PostCode + ", ";
                if (!string.IsNullOrEmpty(payer.CountryCode)) payment.Info1 = payment.Info1 + payer.CountryCode;
                var billAddress = new AddressModel
                {
                    FirstName = payer.FirstName,
                    LastName = payer.LastName,
                    Address1 = payer.Address1,
                    Address2 = payer.Address2,
                    City = payer.City,
                    State = payer.State,
                    PostCode = payer.PostCode,
                    Country = payer.Country,
                    CountryCode = payer.CountryCode,
                    PhoneNo = payer.CountryCode
                };
                order.BillingAddress = billAddress;//Mapper.Map<PayerInfo, Address>(payer);
                paymentResponse.Payment = payment;
                paymentResponse.Order = order;
                paymentResponse.Payment.AuthCode = response.DoExpressCheckoutPaymentResponseDetails.PaymentInfo[0].TransactionID;
                paymentResponse.Payment.Status = PaymentStatus.Authorized.GetHashCode();

            }
            else
            {
                paymentResponse.Order = order;
                payment.Status = PaymentStatus.Declined.GetHashCode();
                payment.IsValid = false;
                paymentResponse.Payment = payment;
                paymentResponse.AddError("decline");
                foreach (var er in response.Errors)
                {
                    paymentResponse.AddError(er.ErrorCode + " : " + er.ShortMessage);
                }
            }
            //if (Settings.IsImmediateCapture)
            //{
            //    var capturePaymentRequest = new CapturePaymentRequest();
            //    var pRes = new CapturePaymentResult();
            //    capturePaymentRequest.CurrencyCode = order.CurrencyCode;
            //    capturePaymentRequest.CaptureTransactionId = Convert.ToString(paymentResponse.Payment.AuthCode);
            //    capturePaymentRequest.OrderTotal = paymentResponse.Payment.OrderAmount;

            //    pRes = Capture(capturePaymentRequest);
            //    if (pRes.Success && string.IsNullOrEmpty(pRes.CaptureTransactionCode) == false)
            //    {
            //        paymentResponse.Payment.AuthCode = pRes.CaptureTransactionCode;
            //        paymentResponse.Payment.Status = PaymentStatus.Paid.GetHashCode();
            //        paymentResponse.Payment.PaidAmount = capturePaymentRequest.OrderTotal;
            //        paymentResponse.Order.PaymentStatus = PaymentStatus.Paid;
            //    }
            //    else
            //    {
            //        paymentResponse.Payment.PSPResponseMessage = pRes.CaptureTransactionResult;
            //    }

            //}
            return paymentResponse;
        }
       

        public PayerInfo GetPayerInfo(string token, string payerId)
        {
            var payer = new PayerInfo();
            var req = new GetExpressCheckoutDetailsReq();
            var request = new GetExpressCheckoutDetailsRequestType();
            req.GetExpressCheckoutDetailsRequest = request;
            request.Token = token;
            request.Version = Settings.Version;
            // System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            var credentials = PaypalSecurityHeader();
            GetExpressCheckoutDetailsResponseType response = _paypalService2.GetExpressCheckoutDetails(ref credentials, req);
            if (response.Ack == AckCodeType.Success)
            {
                try
                {
                    payer.Email = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Payer;
                }
                catch (Exception)
                {

                    payer.Email = "";
                }

                payer.FirstName = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.FirstName;
                payer.LastName = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerName.LastName;

                if (response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerStatusSpecified == true)
                {
                    if (response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerStatus == PayPalUserStatusCodeType.verified)
                    {
                        payer.IsVerify = true;
                    }
                    payer.AccountVerifyCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerStatus.ToString();
                }


                payer.Address1 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street1;
                payer.Address2 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street2;
                payer.City = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CityName;
                payer.State = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.StateOrProvince;
                payer.PostCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.PostalCode;
                payer.PhoneNo = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.ContactPhone;
                payer.Country = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CountryName;
                payer.CountryCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Country.ToString();
                payer.PayerId = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;
                payer.Token = response.GetExpressCheckoutDetailsResponseDetails.Token;

                //if (response.GetExpressCheckoutDetailsResponseDetails.Note > "")
                //{
                //    payer.note = payer.note + response.GetExpressCheckoutDetailsResponseDetails.Note;
                //}
                //payer.OrderTotal = response.GetExpressCheckoutDetailsResponseDetails.PaymentDetails(0).OrderTotal.Value;
                //payer.IsValid = true;
                if (response.GetExpressCheckoutDetailsResponseDetails.BillingAddress == null)
                {
                    payer.Address1 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street1;
                    payer.Address2 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street2;
                    payer.City = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CityName;
                    payer.State = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.StateOrProvince;
                    payer.PostCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.PostalCode;
                    payer.Country = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CountryName;

                    if (response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.AddressStatusSpecified == true)
                    {
                        if (response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.AddressStatus == AddressStatusCodeType.Confirmed)
                        {
                            payer.IsValidAddress = true;
                        }
                        payer.AddressVerifyCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.AddressStatus.ToString();
                    }
                }
                else
                {
                    payer.Address1 = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.Street1;
                    payer.Address2 = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.Street2;
                    payer.City = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.CityName;
                    payer.State = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.StateOrProvince;
                    payer.PostCode = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.PostalCode;
                    payer.Country = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.CountryName;
                    if (response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.AddressStatusSpecified == true)
                    {
                        if (response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.AddressStatus == AddressStatusCodeType.Confirmed)
                        {
                            payer.IsValidAddress = true;
                        }
                        payer.AddressVerifyCode = response.GetExpressCheckoutDetailsResponseDetails.BillingAddress.AddressStatus.ToString();
                    }
                }

                //Shipping addd-----------------------------

                if (response.GetExpressCheckoutDetailsResponseDetails.PayerInfo == null)
                {
                    payer.Address1 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street1;
                    payer.Address2 = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Street2;
                    payer.City = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.CityName;
                    payer.State = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.StateOrProvince;
                    payer.PostCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.PostalCode;
                    payer.CountryCode = response.GetExpressCheckoutDetailsResponseDetails.PayerInfo.Address.Country.ToString();

                }
            }
            return payer;
        }
        private string GetPaypalUrl(string token)
        {
            return Settings.UseSandbox == "True" ? Settings.TestUrl + token : Settings.ProductionUrl + token;
            //return Settings.UseSandbox=="True" ? "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + token : "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + token;
        }
        private CustomSecurityHeaderType PaypalSecurityHeader()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var credentials = new CustomSecurityHeaderType();
            credentials.Credentials = new UserIdPasswordType();
            if (Settings.UseSandbox == "True")
            {
                _paypalService2 = new PayPalAPIAAInterfaceClient("PayPalAPISandbox");
                _paypalService1 = new PayPalAPIInterfaceClient("PayPalAPIINTSandbox");
            }
            else
            {
                _paypalService2 = new PayPalAPIAAInterfaceClient("PayPalAPIProduction");
                _paypalService1 = new PayPalAPIInterfaceClient("PayPalAPIINTProduction");
            }
            credentials.Credentials.Username = Settings.UserName;
            credentials.Credentials.Password = Settings.Password;
            credentials.Credentials.Signature = Settings.Signature;
            credentials.Credentials.Subject = "";
            return credentials;
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
