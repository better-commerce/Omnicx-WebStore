using System.Collections.Generic;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IShippingApi
    {
        ResponseModel<List<ShippingModel>> GetShippingMethods(string basketId, string shipToCountryIso, string postCode = "");
        ResponseModel<List<PhysicalStoreModel>> GetClickAndCollectOptions(string basketId, string postCode);
        ResponseModel<List<NominatedDeliveryModel>> GetNominatedDays(string date);
    }
}