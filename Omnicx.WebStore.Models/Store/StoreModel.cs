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
        public string Code { get; set; }
        public string StockCode { get; set; }
        public string Address21 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        public decimal? DistanceInMiles { get; set; }
        public int AvailableStock { get; set; }
        public int LeadTime { get; set; }
        public int LeadTimeUom { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
    [Serializable]
    public class StoreStockRequest
    {
        public string StockCode { get; set; }
        public string DestinationPostCode { get; set; }
        public string OrgId { get; set; }
    }
    [Serializable]
    public class ShippingPlanRequest
    {
        public string PostCode { get; set; }
        public Guid OrgId { get; set; }
        public List<DeliveryItemLine> DeliveryItems { get; set; }
    }
    [Serializable]
    public class DeliveryItemLine
    {
        public int LineId { get; set; }
        public string StockCode { get; set; }
        public int Qty { get; set; }
    }
}
