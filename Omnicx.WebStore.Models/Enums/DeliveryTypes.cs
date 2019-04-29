using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Enums
{
    public enum DeliveryTypes
    {
        HomeDelivery = 1,
        ShipIntoStore = 2,
        PickupFromStore = 3
        //Dropship = 4
    }

    public enum LeadTimeUnits
    {
        Hours = 1,
        Days = 2,
        Weeks = 3
    }
}
