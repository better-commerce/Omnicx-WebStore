namespace Omnicx.API.SDK.Models.Commerce
{
    public class PromotionModel
    {
        public string PromoCode { get; set; }
        public string Name { get; set; }
        public string VoucherCode { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
    }
}