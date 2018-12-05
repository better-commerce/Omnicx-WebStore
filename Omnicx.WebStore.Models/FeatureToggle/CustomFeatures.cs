using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatureToggle.Toggles;

namespace Omnicx.WebStore.Models.FeatureToggle
{
    public class RecentlyViewedFeature : SimpleFeatureToggle { }
    public class MyActivityFeature : SimpleFeatureToggle { }

    public class StoreStockLocatorFeature : SimpleFeatureToggle { }

    public class SubscriptionFeature : SimpleFeatureToggle { }
}
