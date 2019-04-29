using Newtonsoft.Json;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Store;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.API.SDK.Api.Commerce
{
    public class StoreApi:ApiBase, IStoreApi
    {
        public ResponseModel<List<StoreModel>> CheckStoreStockAvailability(string stockCode, string postCode)
        {
            var request = new StoreStockRequest { StockCode = stockCode, DestinationPostCode = postCode, OrgId = ConfigKeys.OmnicxOrgId };
            return CallApi<List<StoreModel>>(string.Format(ApiUrls.FindStockInStore,postCode, stockCode,request.OrgId), "", Method.GET, apiBaseUrl : ConfigKeys.OmsApiBaseUrl);
        }
        public ResponseModel<List<StoreModel>> FindNearestStore(string postCode)
        {
            return CallApi<List<StoreModel>>(string.Format(ApiUrls.FindNearestStore,postCode), "", Method.GET, apiBaseUrl: ConfigKeys.OmsApiBaseUrl);
        }
        public ResponseModel<StoreModel> StoreDetail(int id)
        {
            return CallApi<StoreModel>(string.Format(ApiUrls.StoreDetail, id), "", Method.GET, apiBaseUrl: ConfigKeys.OmsApiBaseUrl);
        }
    }
}
