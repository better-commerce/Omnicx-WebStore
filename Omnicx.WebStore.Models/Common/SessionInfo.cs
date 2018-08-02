namespace Omnicx.WebStore.Models.Common
{
    public class SessionInfo
    {
        public string DeviceId { get; set; }
        public string SessionId { get; set; }
        public string CustomerId { get; set; }
        public string IpAddress { get; set; }
        public string Referrer { get; set; }
        public UserBrowser Browser { get; set; }
        public UtmInfo Utm { get; set; }
    }
    //public class SessionInfo
    //{
    //    public string SessionId { get; set; }
    //    public string GuestId { get; set; }
    //    public string CustomerId { get; set; }
    //    public string IpAddress { get; set; }
    //    public UserBrowser Browser { get; set; }
    //    public DateTime Created { get; set; }
    //    public DateTime LastUpdated { get; set; }
    //}
    public class SessionUpdateModel
    {
        public string SessionId { get; set; }
        public string CustomerId { get; set; }
    }
    public class UserBrowser
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public bool IsMobileDevice { get; set; }
    }
}