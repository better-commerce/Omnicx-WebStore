using System;
using System.Collections.Generic;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Models.Commerce
{
    public class OrderModel : BaseModel
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public Amount GrandTotal { get; set; }
        public Amount BalanceAmount { get; set; }
        public string CreatedBy { get; set; }
        public bool CreatedByAdmin { get; set; }
        public Amount PaidAmount { get; set; }
        public Amount ShippingCharge { get; set; }
        public Amount SubTotal { get; set; }
        public Amount Discount { get; set; }
        public List<OrderLineModel> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public List<PromotionModel> Promotions { get; set; }

        public PaymentModel Payment { get; set; }
        public List<PaymentModel> Payments { get; set; }

        /// <summary>
        /// System name for the selected shipping method
        /// </summary>
        public string Shipping { get; set; }
        /// <summary>
        /// System name for the selected payment gateway
        /// </summary>
        public string PaymentMethod { get; set; }

        public CustomerBasicModel Customer { get; set; }
        public string BasketId { get; set; }
        public string CustomerId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }

        public Amount AdditionalCharge { get; set; }
        public Amount CompanyDiscount { get; set; }

    }
}