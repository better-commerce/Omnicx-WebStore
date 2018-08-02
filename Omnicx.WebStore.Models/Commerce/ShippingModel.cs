using System;
using System.Collections.Generic;

using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Helpers;
using Omnicx.WebStore.Models.Keys;

namespace Omnicx.WebStore.Models.Commerce
{
    public class ShippingModel 
    {
        public ShippingModel()
        {
            Price = new Amount();
        }
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int ExpectedDaysToDeliver { get; set; }
        public string DeliveryOnOrBefore { get; set; }
        public Amount Price { get; set; }
        public bool IsDefault { get; set; }
        public bool IsNominated { get; set; }
        public ShippingTypes Type { get; set; }
        public string CarrierCode { get; set; }
        public string CountryCode { get; set; }
        public List<CutOffTime> CutOffTimes { get; set; }
        public string NominatedDeliveryDate { get; set; }
        public bool IsPriceOnRequest { get; set; }
        public bool ShowRecomendation { get; set; }
        public string Recomendation { get; set; }
        //shadowFlatShipMethod.IsNominatedDelivery = shipMethod.IsNominatedDelivery;
        //                    shadowFlatShipMethod.Shipmentcode = shipMethod.ShipmentCode;
        //                    shadowFlatShipMethod.ExpectedDaysToDeliver = shipMethod.ExpectedDaysToDeliver;
        //                    shadowFlatShipMethod.NeverFree = shipMethod.NeverFree;
        //                    shadowFlatShipMethod.FreeDeliveryThreshold = shipMethod.FreeDeliveryThreshold;
        //                    shadowFlatShipMethod.CutOffDay = shipMethod.CutOffDay;
        //                    shadowFlatShipMethod.CutOffHour = shipMethod.CutOffHour;
        //                    shadowFlatShipMethod.CutOffMinute = shipMethod.CutOffMinute;
        //                    shadowFlatShipMethod.DayOfDeliveryFrom = shipMethod.DayOfDeliveryFrom;
        //                    shadowFlatShipMethod.DayOfDeliveryTo = shipMethod.DayOfDeliveryTo;
        //                    shadowFlatShipMethod.DayTimings = shipMethod.DayTimings;
        //public IList<KeyValuePair<string, string>> Settings { get; set; }
    }

    public class CutOffTime
    {
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
    }

    public class NominatedDeliveryModel
    {
        public string DayText { get; set; }
        public int DayNo { get; set; }
        public int CutOffTime { get; set; }
        public Amount Price { get; set; }
        public string CarrierCode { get; set; }
        public string DeliveryDate { get; set; }
    }
}