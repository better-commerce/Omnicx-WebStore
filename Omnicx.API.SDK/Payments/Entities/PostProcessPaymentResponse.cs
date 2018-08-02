using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;
namespace Omnicx.API.SDK.Payments.Entities
{
    public class PostProcessPaymentResponse
    {
        private PaymentStatus _newPaymentStatus = PaymentStatus.Pending;
        public IList<string> Errors { get; set; }

        public PostProcessPaymentResponse()
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
        public PaymentStatus NewPaymentStatus
        {
            get
            {
                return _newPaymentStatus;
            }
            set
            {
                _newPaymentStatus = value;
            }
        }
        public OrderModel Order { get; set; }
        public PaymentModel Payment { get; set; }
    }
}
