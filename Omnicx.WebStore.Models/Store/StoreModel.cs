using Newtonsoft.Json;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Store
{
    public class StoreModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "YourId")]
        public string ExternalRefId { get; set; }
        public string Code { get; set; }
        public string StockCode { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        public double? Distance { get; set; }
        public int AvailableStock { get; set; }
        public int LeadTime { get; set; }
        public LeadTimeUnits LeadTimeUom { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        

    }
    [Serializable]
    public class StoreStockRequest
    {
        public string StockCode { get; set; }
        public string DestinationPostCode { get; set; }
        public string OrgId { get; set; }
    }

    [Serializable]
    public class StoreRequest
    {
        public string PostCode { get; set; }
        public string OrgId { get; set; }
    }
    [Serializable]
    public class ShippingPlanRequest
    {
        public Guid BasketId { get; set; }
        public Guid OrderId { get; set; }
        public string PostCode { get; set; }
        public Guid OrgId { get; set; }
        public ShippingMethodTypes ShippingMethodType { get; set; }
        public Guid ShippingMethodId { get; set; }
        public string ShippingMethodName { get; set; }
        public string ShippingMethodCode { get; set; }
        public List<DeliveryItemLine> DeliveryItems { get; set; }
       /// public List<ShippingPlanRequestItem> DeliveryItems { get; set; }
        public bool AllowPartialOrderDelivery { get; set; }
        public bool AllowPartialLineDelivery { get; set; }
        public Guid PickupStoreId { get; set; }
        /// <summary>
        /// PCA storeId
        /// </summary>
        public string RefStoreId { get; set; } 

    }

    [Serializable]
    public class DeliveryItemLine
    {
        public Guid Id { get; set; }
        public long BasketLineId { get; set; } // this is beucase in basketline we have lineId and in orderline we have recordId
        public Guid OrderLineRecordId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public string StockCode { get; set; }
        public int Qty { get; set; }


    }
    [Serializable]
    public class StoreDetailRequestModel
    {
        public string OrgId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
