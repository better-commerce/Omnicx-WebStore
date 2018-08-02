using System;
using System.Security.Cryptography;
using System.Text;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.API.SDK.Payments.Entities;

namespace Omnicx.API.SDK.Payments.Realex
{
    public class RealexPaymentProcessor : IPaymentMethod
    {
        public PaymentMethodModel Settings { get; set; }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var resp = new ProcessPaymentResult();
            var dt = DateTime.Now;
            string timestamp = String.Format("{0:yyyyMMddhhmmss}", dt);
            var hash = GenerateSha1Hash(timestamp + "." +
                                         Settings.UserName + "." +
                                         processPaymentRequest.OrderNo.ToString() + "-" + processPaymentRequest.PaymentId.ToString() + "." +
                                         Convert.ToInt32(processPaymentRequest.OrderTotal * 100).ToString() + "." +
                                         processPaymentRequest.CurrencyCode, Settings.Password);

            resp.AuthorizationTransactionUrl = GetAuthlUrl();
            resp.TimeStamp = timestamp;
            resp.OrderId = processPaymentRequest.OrderId;
            resp.RefOrderId = processPaymentRequest.OrderNo + "-" + processPaymentRequest.PaymentId.ToString();
            resp.OrderTotal = Convert.ToInt32(processPaymentRequest.OrderTotal * 100).ToString();
            resp.CurrencyCode = processPaymentRequest.CurrencyCode;
            resp.ReturnUrl = Settings.NotificationUrl;
            resp.UsePostForm = true;
            // $('<form action="form2.html"></form>').appendTo('body').submit();
            var postForm=new StringBuilder();
            postForm.AppendLine("<form action=\"" + GetAuthlUrl() + "\" >") ;
            postForm.AppendLine("<input type=\"hidden\" MERCHANT_ID=\"" + Settings.UserName + "\" >");
            postForm.AppendLine("<input type=\"hidden\" ORDER_ID=\"" + resp.RefOrderId + "\" >");
            postForm.AppendLine("<input type=\"hidden\" ACCOUNT=\"" + "Internet" + processPaymentRequest.LanuguageCode.Split('-')[0].ToUpper() + "\" >");
            postForm.AppendLine("<input type=\"hidden\" AMOUNT=\"" + Convert.ToInt32(processPaymentRequest.OrderTotal * 100).ToString() + "\" >");
            postForm.AppendLine("<input type=\"hidden\" CURRENCY=\"" + processPaymentRequest.CurrencyCode + "\" >");
            postForm.AppendLine("<input type=\"hidden\" TIMESTAMP=\"" + timestamp + "\" >");
            postForm.AppendLine("<input type=\"hidden\" SHA1HASH=\"" + hash + "\" >");
            //postForm.AppendLine("<input type=\"hidden\" SHIPPING_CODE=\"" + Settings.UserName + "\" >");
            //postForm.AppendLine("<input type=\"hidden\" BILLING_CODE=\"" + Settings.UserName + "\" >");
            postForm.AppendLine("<input type=\"hidden\" AUTO_SETTLE_FLAG=\"0\" />");
            postForm.AppendLine("<input type=\"hidden\" HPP_LANG=\"\" >");
            postForm.AppendLine("<input type=\"hidden\" MERCHANT_RESPONSE_URL=\"" + Settings.NotificationUrl + "\" >");
            postForm.AppendLine("</form>");
            resp.PostForm = postForm.ToString();
            return resp;
        }

        public PostProcessPaymentResponse PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            throw new NotImplementedException();
        }

        private string GetAuthlUrl()
        {
            return Settings.UseSandbox == "True" ? Settings.TestUrl  : Settings.ProductionUrl ;
        }

        private String GenerateSha1Hash(string hashInput, string secretKey)
        {

            SHA1 sha = new SHA1Managed();
            Encoder enc = System.Text.Encoding.ASCII.GetEncoder();
            String hashStage1 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput))) + "." +
                secretKey;

            String hashStage2 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)));

            return hashStage2;
        }

        private String HexEncode(byte[] data)
        {

            String result = "";
            foreach (byte b in data)
            {
                result += b.ToString("X2");
            }
            result = result.ToLower();

            return (result);
        }
    }
}
