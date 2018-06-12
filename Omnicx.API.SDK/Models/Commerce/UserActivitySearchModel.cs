namespace Omnicx.API.SDK.Models.Commerce
{
    public class UserActivitySearchModel
    {
        public string UserId { get; set; }
        public string SearchText { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
