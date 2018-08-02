using Omnicx.WebStore.Models.Commerce;
namespace Omnicx.API.SDK.Payments.Entities
{
    public class PostProcessPaymentRequest
    {
        public OrderModel Order { get; set; }
        public PaymentModel Payment { get; set; }
        public string Token { get; set; }
        public string PayerId { get; set; }
        public decimal OrderTotal { get; set; }
        public string CurrencyCode { get; set; }
        public string Notification { get; set; }
    }
}
