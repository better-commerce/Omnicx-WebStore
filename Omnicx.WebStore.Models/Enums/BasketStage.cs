namespace Omnicx.WebStore.Models.Enums
{
    public enum BasketStage
    {
        Anonymous = 1,
        LoggedIn = 2, // could be registered user or a guest email also.
        ShippingMethodSelected = 3,
        ShippingAddressProvided = 4,
        Placed = 5

    }
}
