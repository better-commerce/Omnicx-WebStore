namespace Omnicx.API.SDK.Models.Commerce
{
    public class PromoResponseModel
    {
        public bool IsVaild { get; set; }
        public string Message { get; set; }
        public BasketModel Basket { get; set; }
        public OrderModel Order { get; set; }
    }
}