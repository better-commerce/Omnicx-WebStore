using System.Collections.Generic;
using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IOrderApi
    {
        ResponseModel<List<OrderModel>>  GetOrders(string customerId);
        ResponseModel<OrderModel>  GetOrdDetail(string id);

        ResponseModel<List<ReturnModel>>  GetAllReturns(string id);
    }
}
