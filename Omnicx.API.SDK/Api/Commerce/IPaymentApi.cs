using System.Collections.Generic;
using Omnicx.API.SDK.Models.Commerce;

using Omnicx.API.SDK.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IPaymentApi
    {
        ResponseModel<List<PaymentMethodModel> > GetPaymentMethods();
    }
}