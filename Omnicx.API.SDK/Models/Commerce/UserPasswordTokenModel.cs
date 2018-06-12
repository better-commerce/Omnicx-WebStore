namespace Omnicx.API.SDK.Models.Commerce
{
    public class UserPasswordTokenModel
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public bool IsValid { get; set; }
        public string Password { get; set; }
    }
}
