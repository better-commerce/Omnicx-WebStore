using Omnicx.API.SDK.Api.Commerce;
using Omnicx.WebStore.Core.Controllers;
using Omnicx.WebStore.Models.Commerce.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.Site.Core.Controllers
{
    public class SubscriptionController : BaseController
    {

        #region Fields and Ctor

        private readonly ISubscriptionApi _subscriptionApi;


        public SubscriptionController(ISubscriptionApi subscriptionApi)
        {
            _subscriptionApi = subscriptionApi;
        }

        #endregion

        // GET: Subscription
        [HttpGet]
        public ActionResult GetSubscriptionPlan(Guid productId)
        {
            var resp = _subscriptionApi.GetSubscriptionPlan(productId);
            return JsonSuccess(resp, JsonRequestBehavior.AllowGet);
        }
    }
}