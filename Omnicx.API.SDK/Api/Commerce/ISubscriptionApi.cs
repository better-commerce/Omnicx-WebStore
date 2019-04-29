using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Commerce.Subscription;
using Omnicx.WebStore.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface ISubscriptionApi
    {
        ResponseModel<SubscriptionPlanModel> GetSubscriptionPlan(Guid productId);
     
    }
}
