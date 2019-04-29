using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class BasketShipmentModel : BasketModel
    {
        public List<ShippingPlan> DeliveryPlans { get; set; }

    }
}
