using Newtonsoft.Json;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Commerce.Subscription;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Keys;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class SubscriptionApi : ApiBase, ISubscriptionApi
    {
        public ResponseModel<SubscriptionPlanModel> GetSubscriptionPlan(Guid productId)
        {
            return CallApi<SubscriptionPlanModel>(string.Format(ApiUrls.GetSubscriptionPlan, productId), "", Method.GET);
        }
       
    }
}
