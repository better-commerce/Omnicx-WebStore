using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models;
using System;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface ICheckoutApi
    {
        ResponseModel<CheckoutModel>  Checkout(string basketId);

        ResponseModel<OrderModel>  ConvertToOrder(string basketId, CheckoutModel checkout);

        ResponseModel<BoolResponse> UpdateBasketDeliveryAddress(string basketId, CheckoutModel checkout);

        ResponseModel<OrderModel>  UpdatePayment(string orderId, PaymentModel payment);

        ResponseModel<PaymentMethodModel>  PaymentSetting(string systemName);

        ResponseModel<bool> SetOrderRisk(string orderId, string avsResult);

        ResponseModel<BasketModel> UpdateUserToBasket(string basketId, string userId);
        ResponseModel<BoolResponse> UpdateBasketDeliveryInstruction(string basketId, UpdateFieldModel deliveryInstruction);
        ResponseModel<BoolResponse> GeneratePaymentLink(string basketId, string userId);
    }
}
