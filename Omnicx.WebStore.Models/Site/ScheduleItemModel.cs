using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Site
{
    [Serializable]
   public class ScheduleItemModel
    {
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool NeverExpire { get; set; }
        public int VersionNo { get; set; }
    }
}
