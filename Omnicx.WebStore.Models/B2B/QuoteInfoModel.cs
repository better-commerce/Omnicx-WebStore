using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Enums;
using System;

namespace Omnicx.WebStore.Models.B2B
{
    public class QuoteInfoModel
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string QuoteName { get; set; }
        public Guid QuoteId { get; set; }
        /// <summary>
        /// purchase order no provided by the customer. this would be optional.
        /// </summary>
        public string PurchaseOrderNo { get; set; }
        /// <summary>
        /// Actual date until when the quote is valid
        /// </summary>
        public DateTime ValidUntil { get; set; }
        /// <summary>
        /// Days until which the quote is valid.
        /// Date of created + validDays = ValidUntil
        /// </summary>
        public int ValidDays { get; set; }
        public QuoteStatus Status { get; set; }
        public string CustomQuoteNo { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public bool CreatedByAdmin { get; set; }
        public long OrderNo { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
    public enum QuoteStatus
    {
        NotQuote = 0,
        Draft = 1,
        PaymentLinkSent = 2,
        Converted = 3,
        Abandoned = 4,
        Cancelled = 5,
        QuoteSent = 6
    }
   
}
