using Omnicx.WebStore.Models.Common;
using System;
using System.Collections.Generic;

namespace Omnicx.WebStore.Models.Commerce
{
    public class OrderLineModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductIid { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SubManufacturer { get; set; }
        public int Qty { get; set; }
        public int DisplayOrder { get; set; }
        public Amount Price { get; set; }
        public Amount DiscountAmt { get; set; }
        public int ShippedQty { get; set; }
        public Amount TotalPrice { get; set; }
        public Guid ParentProductId { get; set; }

        public string Image { get; set; }
        public string Slug { get; set; }
        public int AvailableQty { get; set; }
        public List<CategoryItemModel> CategoryItems { get; set; }

        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }
        public string CustomInfo1Formatted { get; set; }
        public string CustomInfo2Formatted { get; set; }
        public string CustomInfo3Formatted { get; set; }
        public string CustomInfo4Formatted { get; set; }
        public string CustomInfo5Formatted { get; set; }
        public Amount AdditionalCharge { get; set; }

        public int ItemType { get; set; }
        public bool DisplayInBasket { get; set; }
        public Amount CompanyDiscount { get; set; }
        public Amount ListPrice { get; set; }
        public string ShortDescription { get; set; }
    }
}