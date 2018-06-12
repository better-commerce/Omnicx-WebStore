using System.Collections.Generic;
using Omnicx.API.SDK.Models.Catalog;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IShippingApi
    {
        ResponseModel<List<ShippingModel>> GetShippingMethods(string basketId, string shipToCountryIso, string postCode = "");
        ResponseModel<List<PhysicalStoreModel>> GetClickAndCollectOptions(string basketId, string postCode);
        ResponseModel<List<NominatedDeliveryModel>> GetNominatedDays(string date);
    }
}