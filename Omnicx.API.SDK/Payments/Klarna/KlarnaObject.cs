using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Payments.Klarna
{
    public class TokenRequest
    {
        public string Purchase_country { get; set; }
        public string Purchase_currency { get; set; }
        public string Locale { get; set; }
        public Address Billing_address { get; set; }
        public Address Shipping_address { get; set; }
        public int Order_amount { get; set; }
        public int Order_tax_amount { get; set; }
        public List<OrderLines> Order_lines { get; set; }
        public string Description { get; set; }
        public string Intended_use { get; set; }
        public string Merchant_reference1 { get; set; }
        public string Merchant_reference2 { get; set; }
    }
    public class TokenResponse
    {
        public string SessionId { get; set; }
        public string ClientToken { get; set; }
        public string TokenId { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class OrderResponse
    {
        public string OrderId { get; set; }
        public string RedirectUrl { get; set; }
        public string FraudStatus { get; set; }
        public string PaymentId { get; set; }
        public int OrderAmount { get; set; }
        public string RefOrderId { get; set; }
    }

    public class OrderLines
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public int Quantity { get; set; }
        public int Unit_price { get; set; }
        public int Total_amount { get; set; }
        public string Type { get; set; }
        public string Quantity_unit { get; set; }
        public int Total_discount_amount { get; set; }
        public int Total_tax_amount { get; set; }
        public int Tax_rate { get; set; }
    }

    public enum LineType
    {
        physical = 1,
        discount = 2,
        shipping_fee = 3,
        sales_tax = 4,
        digital = 5,
        gift_card = 6,
        store_credit = 7,
        surcharge = 8
    }

    public class Address
    {
        public string Given_name { get; set; }
        public string Family_name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Street_address { get; set; }
        public string Street_address2 { get; set; }
        public string Postal_code { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
    }

    public static class ResponseVariable
    {
        public const string SessionId = "session_id";
        public const string ClientToken = "client_token";
        public const string OrderId = "order_id";
        public const string RedirectUrl = "redirect_url";
        public const string FraudStatus = "fraud_status";
        public const string TokenId = "token_id";
    }
}
