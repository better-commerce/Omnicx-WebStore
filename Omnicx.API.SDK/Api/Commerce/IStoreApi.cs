using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Api.Commerce
{
    public  interface IStoreApi
    {
        ResponseModel<List<StoreModel>> CheckStoreStockAvailability (string stockCode, string postCode);

        ResponseModel<List<StoreModel>> FindNearestStore(string postCode);

        ResponseModel<StoreModel> StoreDetail(int id);
    }
}
