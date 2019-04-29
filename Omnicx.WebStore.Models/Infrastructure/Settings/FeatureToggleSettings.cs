using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Infrastructure.Settings
{
    [Serializable]
    public class FeatureToggleSettings: ISettings
    {
        public bool SubscriptionEnabled { get; set; }
    }
}
