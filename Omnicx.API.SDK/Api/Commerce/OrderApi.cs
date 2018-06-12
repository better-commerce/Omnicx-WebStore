using System;
using System.Collections.Generic;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public class OrderApi : ApiBase, IOrderApi
    {
        public ResponseModel<OrderModel> GetOrdDetail(string id)
        {
            Guid orderId;
            if (!Guid.TryParse(id, out orderId))
            {
                if (id.Contains("-"))
                {
                    int idx = id.IndexOf('-');
                    id = id.Substring(0, idx);
                }
            }
            return CallApi<OrderModel>(string.Format(ApiUrls.GetOrderDetail, id), "");
        }
        public ResponseModel<List<OrderModel>> GetOrders(string customerId)
        {
            return CallApi<List<OrderModel>>(string.Format(ApiUrls.GetOrders, customerId), "");
        }

       public ResponseModel<List<ReturnModel>> GetAllReturns(string id)
        {
            return CallApi<List<ReturnModel>>(string.Format(ApiUrls.GetAllReturns, id), "");
        }
    }
}
