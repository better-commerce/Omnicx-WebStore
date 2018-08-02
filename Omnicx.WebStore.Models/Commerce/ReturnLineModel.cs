using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Models.Commerce
{
    public class ReturnLineModel 
    {
        public string RecordId { get; set; }
        public string ProductId { get; set; }
        public string ProductIid { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SubManufacturer { get; set; }
        public int Qty { get; set; }
        public Amount Price { get; set; }
        public Amount DiscountAmt { get; set; }
        public int ShippedQty { get; set; }
        public Amount TotalPrice { get; set; }
        public int ReturnQtyRequested { get; set; }
        public int ReturnQtyRecd { get; set; }
        public int AvailableQty { get; set; }
        public string Slug { get; set; }

    }
}