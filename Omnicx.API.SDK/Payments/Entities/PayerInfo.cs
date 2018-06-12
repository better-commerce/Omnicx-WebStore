namespace Omnicx.API.SDK.Payments.Entities
{
    public class PayerInfo
    {
        public string PayerId { get; set; }
        public string Token { get; set; }
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string PostCode { get; set; }

        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string AccountVerifyCode { get; set; }
        public string AddressVerifyCode { get; set; }
        public bool IsValidAddress { get; set; }
        public bool IsVerify { get; set; }
    }
}
