using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using Newtonsoft.Json;
using Omnicx.WebStore.Models.Keys;
using RestSharp;
using Omnicx.WebStore.Models.Store;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class ShippingApi:ApiBase, IShippingApi
    {
        public ResponseModel<List<ShippingModel>>  GetShippingMethods(string basketId, string shipToCountryIso, string postCode="")
        {
            return CallApi<List<ShippingModel>>(string.Format(ApiUrls.ShippingMethods, basketId, shipToCountryIso, postCode), "");
        }

        public ResponseModel<List<ShippingPLan>> GetShippingPlans(ShippingPlanRequest shippingPlanRequest)
        {           
            return CallApi<List<ShippingPLan>>(ApiUrls.ShippingPlans, JsonConvert.SerializeObject(shippingPlanRequest), Method.POST, apiBaseUrl: ConfigKeys.OmsApiBaseUrl);
        }

        public ResponseModel<List<NominatedDeliveryModel> >  GetNominatedDays(string date)
        {
            return CallApi<List<NominatedDeliveryModel>>(string.Format(ApiUrls.ShippingNominatedDays, date), "");
        }
    }
}