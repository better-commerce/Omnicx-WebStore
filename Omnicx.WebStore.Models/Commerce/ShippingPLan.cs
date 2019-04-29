using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class ShippingPlan 
    {
        private DeliveryCenterBasic _deliveryCenter;
        private List<ShippingPlanLine> _items;
        public Guid RecordId { get; set; }
        public ShippingPlan()
        {
            if (_deliveryCenter == null) _deliveryCenter = new DeliveryCenterBasic();
            if (_items == null) _items = new List<ShippingPlanLine>();
        }
        public FulfilmentChannels FulfilmentChannel { get; set; }
        public DeliveryTypes DeliveryType { get; set; }
        public DeliveryCenterBasic DeliveryCenter
        {
            get { return _deliveryCenter; }
            set { _deliveryCenter = value; }
        }

        public List<ShippingPlanLine> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public List<BasketLineModel> LineItems
        {
            get;set;
        }

        /// <summary>
        /// Distance in miles from the Destination Post Code
        /// </summary>
        public decimal? DistanceInMiles { get; set; }

        public Guid ShippingMethodId { get; set; }

        public string ShippingMethodName { get; set; }

        //public string StockCode { get; set; }
        //public int AvailableStock { get; set; }
        public decimal Cost { get; set; }

        /// <summary>
        /// consolidated string to be presented to the user, derived based on the LeadTimeUnit & DeliveryType 
        /// Pick up by 14:00 hours - Oxford Street Store OX14 5A
        /// Get it by Mon 31 Dec - FREE Standard Delivery
        /// Get it in 3-4 Weeks - FREE Standard Delivery
        /// structure - 'getit/pickup' by 'targetDeliveryDate/LeadTimeMin-LeadTimeMax' 'LeadTimeUnit' - 'ShippingCost' 'ShippingMethodName'
        /// </summary>
        public string ShippingSpeed
        {
            // TODO: How to Get the domain level Date/Time display format  and Translations. 
            get
            {
                var speed = "";
                switch (DeliveryType)
                {
                    case DeliveryTypes.HomeDelivery:
                        speed = "Get it";
                        break;
                    case DeliveryTypes.PickupFromStore:
                    case DeliveryTypes.ShipIntoStore:
                        speed = "Pick up";
                        break;
                }

                switch (LeadTimeUom)
                {
                    case LeadTimeUnits.Hours:
                        speed += " by" + DateTime.UtcNow.AddHours(LeadTime);
                        break;

                    case LeadTimeUnits.Days:
                        speed += " by " + DateTime.UtcNow.AddDays(LeadTime).ToString("ddd dd MMM");
                        break;
                    case LeadTimeUnits.Weeks:

                        speed += " in " + LeadTime.ToString() + " Week(s)";

                        break;

                }


                return speed; /*+ " - " + ShippingMethodName;*/
            }
        }
        /// <summary>
        /// The target delivery date. to be specified when the delivery leadtime unit is in days
        /// </summary>
        public DateTime DeliveryDateTarget { get; set; }

        /// <summary>
        /// Actual delivery date recorded for SLA reporting purposes.
        /// </summary>
        public DateTime DeliveryDateActual { get; set; }

        /// <summary>
        /// Lead time for the goods to be available at the delivery center
        /// </summary>
        public int LeadTime { get; set; }

        /// <summary>
        /// Units of measure for lead time to pickup from the delivery center - minutes, hours, days, weeks
        /// </summary>
        public LeadTimeUnits LeadTimeUom { get; set; }

        /// <summary>
        /// Minimum lead time
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int LeadTimeMin { get; set; }
        /// <summary>
        /// Maximum lead time
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int LeadTimeMax { get; set; }


    }
    //public class ShippingPLan
    //{
    //    public string Id { get; set; }      
    //    public string Name { get; set; }
    //    public string Address1 { get; set; }
    //    public string Address2 { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string PostCode { get; set; }
    //    public string ExternalRefId { get; set; }
    //    public decimal DistanceInMiles { get; set; }
    //    public string DistanceUnit { get; set; }
    //    public string ShippingPlanId { get; set; }
    //    public string Type { get; set; }
    //    public string LeadTimeUnit { get; set; }
    //    public int LeadTimeMin { get; set; }
    //    public int LeadTimeMax { get; set; }
    //    public string DeliveryOption { get; set; }
    //    public string ShippingAddressId { get; set; }
    //    public string OpeningHours { get; set; }
    //    public List<DeliverySlot> Slots { get; set; }
    //    public string WhyDelayMsg1 { get; set; }
    //    public string WhyDelayMsg2 { get; set; }
    //    public string WhyDelayMsg3 { get; set; }
    //    public string SelectedSlot { get; set; }
    //    public string PhoneNumber { get; set; }

    //}
    //public class DeliverySlot
    //{
    //    public string Slot { get; set; }
    //    public string SlotMessage { get; set; }
    //}
}
