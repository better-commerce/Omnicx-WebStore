namespace Omnicx.WebStore.Models.Commerce
{
    public class PaymentModel
    {
        public string Id { get; set; }
        public string CardNo { get; set; }
        public long OrderNo { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public bool IsValid { get; set; }
        public int Status { get; set; }
        public string AuthCode { get; set; }
        public string IssuerUrl { get; set; }
        public string PaRequest { get; set; }
        public string PspSessionCookie { get; set; }
        public string PspResponseCode { get; set; }
        public string PspResponseMessage { get; set; }
        public int PaymentGatewayId { get; set; }
        public string PaymentGateway { get; set; }
        public string Token { get; set; }
        public string PayerId { get; set; }
        public string CvcResult { get; set; }
        public string AvsResult { get; set; }
        public string Secure3DResult { get; set; }
        public string CardHolderName { get; set; }
        public string IssuerCountry { get; set; }
        public string Info1 { get; set; }
        public string FraudScore { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsVerify { get; set; }
        public bool IsValidAddress { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}