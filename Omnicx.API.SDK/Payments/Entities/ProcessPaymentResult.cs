using System.Collections.Generic;
using Omnicx.API.SDK.Models.Common;

namespace Omnicx.API.SDK.Payments.Entities
{
  public  class ProcessPaymentResult
    {
      
        public IList<string> Errors { get; set; }
        public ProcessPaymentResult()
        {
            this.Errors = new List<string>();
        }
        public bool Success
        {
            get { return (this.Errors.Count == 0); }
        }
        public void AddError(string error)
        {
            if (error != null)
            {
                this.Errors.Add(error);
            }

        }
        public string OrderId { get; set; }
        public string OrderTotal { get; set; }
        public string CurrencyCode { get; set; }

        public string TimeStamp { get; set; }

        public string Hash { get; set; }

        public string RefOrderId { get; set; }

        public string AuthorizationTransactionUrl { get; set; }
        public string AuthorizationTransactionCode { get; set; }

        public bool UseAuthUrlToRedirect { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public Amount BalanceAmount { get; set; }

        public string ReturnUrl { get; set; }

        public string PostForm { get; set; }
        public bool UsePostForm { get; set; }
        public string AuthorizationRiskScore { get; set; }
        public string Settingkey { get; set; }
        public string PspSessionCookie { get; set; }
    }
}
