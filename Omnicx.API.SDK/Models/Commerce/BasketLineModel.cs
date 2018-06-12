using Omnicx.API.SDK.Models.Common;

using System.Collections.Generic;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class BasketLineModel
    {
        public string Id { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public string ProductId { get; set; }
        public string ProductIid { get; set; }
        public string Manufacture { get; set; }
        public string SubManufacture { get; set; }
        public Amount Price { get; set; }
        public Amount SubTotal { get; set; }
        public Amount TotalPrice { get; set; }
        
        public string Image { get; set; }
        public int Qty { get; set; }
        public int DisplayOrder { get; set; }
        public string Slug { get; set; }
        public string ParentProductId { get; set; }
        public int ItemType { get; set; }
        public bool DisplayInBasket { get; set; }
        public string AttributesJson { get; set; }
        public List<CategoryItemModel> CategoryItems { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }

        public Amount Discount { get; set; }
        public Amount AdditionalCharge { get; set; }
    }

    public class CategoryItemModel 
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
    }
}