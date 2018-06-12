using System.Collections.Generic;


namespace Omnicx.API.SDK.Models.Catalog
{
    public class PhysicalStoreModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string ExternalRefId { get; set; }
        public decimal DistanceFromPostCode { get; set; }
        public string DistanceUnit { get; set; }
        public string ShippingPlanId { get; set; }
        public string Type { get; set; }
        public string LeadTimeUnit { get; set; }
        public int LeadTimeMin { get; set; }
        public int LeadTimeMax { get; set; }
        public string DeliveryOption { get; set; }
        public string ShippingAddressId { get; set; }
        public string OpeningHours { get; set; }
        public List<DeliverySlot> Slots { get; set; }
        public string WhyDelayMsg1 { get; set; }
        public string WhyDelayMsg2 { get; set; }
        public string WhyDelayMsg3 { get; set; }
        public string SelectedSlot { get; set; }
        public string PhoneNumber { get; set; }

    }
    public class DeliverySlot
    {
        public string Slot { get; set; }
        public string SlotMessage { get; set; }
    }
}