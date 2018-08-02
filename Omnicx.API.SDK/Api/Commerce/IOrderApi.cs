using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using System;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IOrderApi
    {
        ResponseModel<List<OrderModel>>  GetOrders(string customerId);
        ResponseModel<OrderModel>  GetOrdDetail(string id);

        ResponseModel<List<ReturnModel>>  GetAllReturns(string id);
        ResponseModel<FileResponse> DownloadInvoice(Guid id);
    }
}
