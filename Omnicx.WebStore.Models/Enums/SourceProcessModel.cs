using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Enums
{
    //Related To GDPR 
    public enum SourceProcessType
    {
        SITE_ACCOUNTREGISTER = 1,
        SITE_CHECKOUTGUEST = 2,
        SITE_CHEKOUTREGISTER = 3,
        SITE_MYACCOUNT = 4,
        SITE_FOOTER = 5
    }

}
