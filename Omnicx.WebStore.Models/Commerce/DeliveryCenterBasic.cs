using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class DeliveryCenterBasic
    {
        public Guid RecordId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public LocationTypes Type { get; set; }
        public string PostCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public enum LocationTypes
    {
        PhysicalStore = 1,
        Warehouse = 2,
        SupplierLocation = 3
    }
}
