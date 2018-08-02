using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
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