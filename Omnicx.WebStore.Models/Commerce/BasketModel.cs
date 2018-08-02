using Newtonsoft.Json;
using System.Collections.Generic;
using Omnicx.WebStore.Models.Common;
using System;
using System.Linq;
using Omnicx.WebStore.Models.B2B;

namespace Omnicx.WebStore.Models.Commerce
{
    public class BasketModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Amount GrandTotal { get; set; }
        public Amount ShippingCharge { get; set; }
        public string ShippingMethodId { get; set; }
        public Amount SubTotal { get; set; }
        public Amount Discount { get; set; }
        public Amount AdditionalCharge { get; set; }       
        
        [JsonProperty(PropertyName = "lineItems")]
        public IList<BasketLineModel> LineItems { get; set; }
        public int LineItemCount { get {
               
                return LineItems==null?0: LineItems.Where(x=>x.ParentProductId==Guid.Empty.ToString()).Sum(x=>x.Qty) ;
            }
        }
        public IList<PromotionModel> PromotionsApplied { get; set; }
       //Added for shippingMethods
       public List<ShippingModel> shippingMethods { get; set; }
       //public IList<ProductModel> RelatedProducts { get; set; }
      // public List<BasketModel> UserBaskets { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsQuote { get; set; }
        public QuoteStatus QuoteStatus { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 { get; set; }
        public string CustomInfo4 { get; set; }
        public string CustomInfo5 { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string PostCode { get; set; }
        public bool IsPriceOnRequest { get; set; }
        public decimal MaxDimensionMm { get; set; }
    }
}