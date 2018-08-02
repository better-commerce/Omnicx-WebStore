using System.Collections.Generic;
using Omnicx.WebStore.Models.Commerce;

using Omnicx.WebStore.Models;
namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IPaymentApi
    {
        ResponseModel<List<PaymentMethodModel> > GetPaymentMethods();
    }
}