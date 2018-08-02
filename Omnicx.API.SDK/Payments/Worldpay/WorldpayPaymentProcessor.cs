using System;
using System.Text;
using Omnicx.API.SDK.Payments;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;
using System.Security.Cryptography;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.Plugin.Payment.Worldpay
{
    public class WorldpayPaymentProcessor : IPaymentMethod 
    {
        public PaymentMethodModel Settings { get; set; }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult();
            resp.AuthorizationTransactionUrl = GetAuthlUrl();
            resp.OrderId = processPaymentRequest.OrderId;
            resp.RefOrderId = processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId.ToString();
            resp.OrderTotal = Convert.ToInt32(processPaymentRequest.OrderTotal * 100).ToString();
            resp.CurrencyCode = processPaymentRequest.CurrencyCode;
            resp.ReturnUrl = Settings.NotificationUrl;
            resp.UsePostForm = true;

            string MD5secretKey = "test1234";

            var callbackhashInputs = new StringBuilder();
            callbackhashInputs.Append(MD5secretKey);
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(processPaymentRequest.CurrencyCode);
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(processPaymentRequest.OrderTotal.ToString("#0.00"));
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(GetTestMode());
            callbackhashInputs.Append(":");
            callbackhashInputs.Append(Settings.UserName);

            string Signature = ByteArrayToHexString(new MD5CryptoServiceProvider().ComputeHash(StringToByteArray(callbackhashInputs.ToString())));

            var postForm = new StringBuilder();
            postForm.AppendLine("<form action=\"" + GetAuthlUrl() + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"instId\" value=\"" + Settings.UserName + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"cartId\" value=\"" + processPaymentRequest.OrderNo + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"amount\" value=\"" + processPaymentRequest.OrderTotal + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"currency\" value=\"" + processPaymentRequest.CurrencyCode + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"testMode\" value=\"" + GetTestMode() + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"desc\" value=\"AKC Order\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"authMode\" value=\"" + TransactionType.A + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"accId1\" value=\"AKCSYSTEMSLTM1\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"name\" value=\"" + processPaymentRequest.Order.BillingAddress.FirstName + " " + processPaymentRequest.Order.BillingAddress.LastName + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"email\" value=\"" + processPaymentRequest.UserEmail + "\" />");
            postForm.AppendLine("<input type=\"hidden\" name=\"address1\" value=\"" + processPaymentRequest.Order.BillingAddress.Address1 + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"address2\" value=\"" + processPaymentRequest.Order.BillingAddress.Address2 + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"address3\" value=\"" + processPaymentRequest.Order.BillingAddress.Address3 + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"postcode\" value=\"" + processPaymentRequest.Order.BillingAddress.PostCode + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"tel\" value=\"" + processPaymentRequest.Order.BillingAddress.MobileNo + "\" />");
            postForm.AppendLine("<input type=\"hidden\" name=\"country\" value=\"" + processPaymentRequest.Order.BillingAddress.CountryCode + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"town\" value=\"" + processPaymentRequest.Order.BillingAddress.City + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"signature\" value=\"" + Signature + "\" />");
            postForm.AppendLine("<input type=\"hidden\" name=\"successURL\" value=\"" + Settings.NotificationUrl + "?transId=" + processPaymentRequest.OrderId + "&orderId=" + processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"returnURL\" value=\"" + Settings.CancelUrl + processPaymentRequest.OrderId + "\" >");
            postForm.AppendLine("<input type=\"hidden\" name=\"MC_callback\" value=\"" + Settings.NotificationUrl + "?transId=" + processPaymentRequest.OrderId + "&orderId=" + processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId + "\" >");
            //postForm.AppendLine("<input type=\"hidden\" name=\"MC_callback\" value=\"" + Settings.NotificationUrl + "?orderId=" + processPaymentRequest.OrderId + "\" >");
            postForm.AppendLine("</form>");
            resp.PostForm = postForm.ToString();

            return resp;
        }

        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        private int GetTestMode()
        {
            return Settings.UseSandbox == "True" ? 100 : 0;
        }
        private string GetAuthlUrl()
        {
            return Settings.UseSandbox == "True" ? Settings.TestUrl : Settings.ProductionUrl;
        }
        public static byte[] StringToByteArray(string source, bool useASCII = true)
        {
            Encoding e;
            if (useASCII)
                e = new ASCIIEncoding();
            else
                e = new UTF8Encoding();
            return e.GetBytes(source);
        }

        public static string ByteArrayToHexString(byte[] source)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(source[i].ToString("x2"));
            }

            return sb.ToString();
        }
        
    }
}
