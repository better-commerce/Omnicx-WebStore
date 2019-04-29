using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class ShippingPlanLine
    {
        public Guid RecordId { get; set; }
        public long BasketLineId { get; set; }
        public Guid OrderLineRecordId { get; set; }
        public string StockCode { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }

    }
}
