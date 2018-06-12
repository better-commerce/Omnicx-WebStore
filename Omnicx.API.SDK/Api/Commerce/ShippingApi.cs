using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public class ShippingApi:ApiBase, IShippingApi
    {
        public ResponseModel<List<ShippingModel>>  GetShippingMethods(string basketId, string shipToCountryIso, string postCode="")
        {
            return CallApi<List<ShippingModel>>(string.Format(ApiUrls.ShippingMethods, basketId, shipToCountryIso, postCode), "");
        }

        public ResponseModel<List<PhysicalStoreModel>>  GetClickAndCollectOptions(string basketId, string postCode)
        {
            return CallApi<List<PhysicalStoreModel>>(string.Format(ApiUrls.ShippingClickAndCollect, basketId, postCode), "");
        }

        public ResponseModel<List<NominatedDeliveryModel> >  GetNominatedDays(string date)
        {
            return CallApi<List<NominatedDeliveryModel>>(string.Format(ApiUrls.ShippingNominatedDays, date), "");
        }
    }
}