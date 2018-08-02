using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;

using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public class PaymentApi:ApiBase, IPaymentApi
    {
        public ResponseModel<List<PaymentMethodModel>> GetPaymentMethods()
        {
            return CallApi<List<PaymentMethodModel>>(ApiUrls.PaymentMethods, "");
        }
    }
}