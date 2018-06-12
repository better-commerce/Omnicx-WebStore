using System.Collections.Generic;
using Omnicx.API.SDK.Models.Commerce;

using Omnicx.API.SDK.Models;
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