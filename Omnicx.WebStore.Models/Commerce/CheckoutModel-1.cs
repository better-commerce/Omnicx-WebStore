using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Commerce
{
    public class CheckoutModel1
    {
        public string Id { get; set; }

        public string BasketId { get; set; }

        public string CustomerId { get; set; }

        public BasketModel Basket { get; set; }
        public AddressModel ShippingAddress { get; set; }

        public AddressModel BillingAddress { get; set; }

        public ShippingModel SelectedShipping { get; set; }

        public PaymentModel SelectedPayment { get; set; }

        public IList<ShippingModel> ShippingOptions { get; set; }

        public IList<PaymentModel> PaymentOptions { get; set; }
    }
}