using Omnicx.WebStore.Models.Commerce;

namespace Omnicx.API.SDK.Payments.Entities
{
    public class ProcessPaymentRequest
    {
        public string OrderId { get; set; }

        public string OrderNo { get; set; }

        public string PaymentId { get; set; }

        public string Description { get; set; }

        public string UserEmail { get; set; }

        public string CurrencyCode { get; set; }

        public string LanuguageCode { get; set; }

        public string CustomerId { get; set; }

        public string BasketId { get; set; }

        public decimal OrderTotal { get; set; }

        public OrderModel Order { get; set; }

        public string CardNo { get; set; }

        public string Cvv { get; set; }
        public string PspSessionCookie { get; set; }
        public string CreditCardType { get; set; }
    }
}
