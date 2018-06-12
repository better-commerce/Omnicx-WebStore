namespace Omnicx.API.SDK.Models.Commerce
{
    public class WishlistModel
    {
        public string WishlistId { get; set; }
        public string Id { get; set; }
        public string RecordId { get; set; }
        public string ProductName { get; set; }
    }

    public class WishlistAddModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
    }
}