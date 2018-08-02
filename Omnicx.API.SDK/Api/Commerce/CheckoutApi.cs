using Newtonsoft.Json;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using RestSharp;
using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public class CheckoutApi :ApiBase, ICheckoutApi
    {
        public ResponseModel<CheckoutModel> Checkout(string basketId)
        {
            return CallApi<CheckoutModel>(string.Format(ApiUrls.Checkout, basketId),"");
        }

        public ResponseModel<OrderModel> ConvertToOrder(string basketId, CheckoutModel checkout)
        {
            return CallApi<OrderModel>(string.Format(ApiUrls.ConvertToOrder, basketId),JsonConvert.SerializeObject(checkout), Method.POST);
        }

        public ResponseModel<BoolResponse> UpdateBasketDeliveryAddress(string basketId, CheckoutModel checkout)
        {
            var model = new BasketAddressModel
            {
                BillingAddress = checkout.BillingAddress,
                ShippingAddress = checkout.ShippingAddress
            };
            return CallApi<BoolResponse>(string.Format(ApiUrls.UpdateBasketDeliveryAddress, basketId), JsonConvert.SerializeObject(model), Method.POST);
        }

        public ResponseModel<OrderModel> UpdatePayment(string orderId, PaymentModel payment)
        {
            return CallApi<OrderModel>(string.Format(ApiUrls.UpdatePayment, orderId), JsonConvert.SerializeObject(payment), Method.POST);
        }
        public ResponseModel<PaymentMethodModel> PaymentSetting(string systemName)
        {
            return CallApi<PaymentMethodModel>(string.Format(ApiUrls.PaymentSetting, systemName), "");
        }
        public ResponseModel<bool> SetOrderRisk(string orderId, string avsResult)
        {
            return CallApi<bool>(string.Format(ApiUrls.SetOrderRisk, orderId,avsResult), "",Method.POST);
        }
        public ResponseModel<BasketModel> UpdateUserToBasket(string basketId, string userId)
        {
            return CallApi<BasketModel>(string.Format(ApiUrls.UpdateBasketUser, basketId, userId), "", Method.POST);
        }
    }
}
