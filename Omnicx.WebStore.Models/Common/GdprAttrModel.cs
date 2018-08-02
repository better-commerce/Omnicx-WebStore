using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Common
{
    public class GdprAttrModel
    {
        public bool NewsLetterSubscribed { get; set; }
        public bool NotifyByEmail { get; set; }
        public bool NotifyBySMS { get; set; }
        public bool NotifyByPost { get; set; }
        public string SourceProcess { get; set; }

    }
}
