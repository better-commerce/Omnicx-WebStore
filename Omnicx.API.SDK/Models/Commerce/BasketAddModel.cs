namespace Omnicx.API.SDK.Models.Commerce
{
    public class BasketAddModel
    {
        public string BasketId { get; set; }
        public string ProductId { get; set; }
        public string SubscriptionId { get; set; }
        public int Qty { get; set; }
        public int DisplayOrder { get; set; }
        public string StockCode { get; set; }
        public int ItemType { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }
        public string ProductName { get; set; }
        public string PostCode { get; set; }
    }
}