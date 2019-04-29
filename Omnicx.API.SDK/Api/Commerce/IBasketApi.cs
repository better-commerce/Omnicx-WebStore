using System.Threading.Tasks;
using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models;
using System.Collections.Generic;
using System;
using Omnicx.WebStore.Models.Catalog;
using Omnicx.WebStore.Models.Common;
using Omnicx.WebStore.Models.Commerce.Subscription;

namespace Omnicx.API.SDK.Api.Commerce
{
    public interface IBasketApi
    {
        ResponseModel<BasketModel>  GetBasketData(string id);
         Task<ResponseModel<BasketModel>> GetBasketDataAsync(string id);
        ResponseModel<BasketModel>  AddToBasket(BasketAddModel model);
        ResponseModel<PromoResponseModel>  ApplyPromoCode(string basketId, string promoCode);
        //ResponseModel<PromoResponseModel> ApplyPromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo);
        ResponseModel<PromoResponseModel>  RemovePromoCode(string basketId, string promoCode);
        //ResponseModel<PromoResponseModel> RemovePromoCodeBulk(string basketId, string promoCode, List<CustomInfo> customInfo);

        ResponseModel<BasketModel>  UpdateShipping(string basketId, string shippingId,NominatedDeliveryModel nominatedDelivery);
        ResponseModel<BasketModel> BulkAddProduct(List<BasketAddModel> model);
        ResponseModel<BasketModel> AddPersistentBasket(Guid id, Guid sourceBasketId);
        ResponseModel<List<ProductModel>> GetRelatedProducts(string id);
        ResponseModel<List<BasketModel>> GetAllUserBaskets(Guid id);
        ResponseModel<BasketModel> UpdateBasketInfo(string basketId,HeaderCustomInfo info);
        ResponseModel<BasketModel> UpdateBasketSubscriptionInfo(Guid basketId, Guid productId, SubscriptionUserSetting userSetting);
        Task<ResponseModel<BoolResponse>> UpdateBasketCampaign(Guid basketId, string campaignCode);
        ResponseModel<BoolResponse> UpdatePoReference(Guid basketId, string poReferenceNumber);
        void ResetSessionBasket();
        void PopulateDeliveryPlans(ref BasketModel basket,string requestSource);

        ResponseModel<BasketModel> UpdateUserToBasket(string basketId, string userId);
    }
}
