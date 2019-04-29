using System;
using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using Omnicx.WebStore.Models.Common;
using RestSharp;
using Omnicx.WebStore.Models.Commerce.Subscription;
using Newtonsoft.Json;

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
        public ResponseModel<List<SubscriptionSeedOrder>> GetSubscriptions(string customerId)
        {
            return CallApi<List<SubscriptionSeedOrder>>(string.Format(ApiUrls.GetSubscriptions, customerId), "");
        }

        public ResponseModel<List<ReturnModel>> GetAllReturns(string id)
        {
            return CallApi<List<ReturnModel>>(string.Format(ApiUrls.GetAllReturns, id), "");
        }

        public ResponseModel<FileResponse> DownloadInvoice(Guid id)
        {
            return CallApi<FileResponse>(string.Format(ApiUrls.DownloadInvoice, id), "", Method.POST);
        }
        public ResponseModel<OrderModel> GetRecentOrderByEmail(string email)
        {
            return CallApi<OrderModel>(string.Format(ApiUrls.GetOrderByEmail, email), string.Empty, Method.GET);
        }

        public ResponseModel<SubscriptionFulfilmentOrderModel> GetSubscriptionDetail(Guid seedOrderId)
        {
            return CallApi<SubscriptionFulfilmentOrderModel>(string.Format(ApiUrls.SubscriptionDetail, seedOrderId), "", Method.GET);
        }
        public ResponseModel<SubscriptionFulfilmentOrderModel> UpdateSubscriptionStatus(UpdateSubscriptionStatusModel subscriptionUpdateStatus)
        {
            return CallApi<SubscriptionFulfilmentOrderModel>(string.Format(ApiUrls.UpdateSubscriptionStatus), JsonConvert.SerializeObject(subscriptionUpdateStatus), Method.POST);
        }
    }
}
