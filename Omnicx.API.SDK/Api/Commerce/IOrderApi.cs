using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using System;
using Omnicx.WebStore.Models.Commerce.Subscription;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IOrderApi
    {
        ResponseModel<List<OrderModel>>  GetOrders(string customerId);
        ResponseModel<OrderModel>  GetOrdDetail(string id);

        ResponseModel<List<ReturnModel>>  GetAllReturns(string id);
        ResponseModel<FileResponse> DownloadInvoice(Guid id);

        ResponseModel<List<SubscriptionSeedOrder>> GetSubscriptions(string customerId);
        ResponseModel<OrderModel> GetRecentOrderByEmail(string email);
        ResponseModel<SubscriptionFulfilmentOrderModel> GetSubscriptionDetail(Guid seedOrderId);
        ResponseModel<SubscriptionFulfilmentOrderModel> UpdateSubscriptionStatus(UpdateSubscriptionStatusModel subscriptionUpdateStatus);
    }
}
