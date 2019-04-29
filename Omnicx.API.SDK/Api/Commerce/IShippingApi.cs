using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Store;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IShippingApi
    {
        ResponseModel<List<ShippingModel>> GetShippingMethods(string basketId, string shipToCountryIso, string postCode = "");
        ResponseModel<List<ShippingPlan>> GetShippingPlans(ShippingPlanRequest shippingPlanRequest);
        ResponseModel<List<NominatedDeliveryModel>> GetNominatedDays(string date);
        ResponseModel<List<StoreModel>> GetClickAndCollectStores(ShippingPlanRequest shippingPlanRequest);
    }
}