namespace Omnicx.WebStore.Models.Commerce
{
      public class NewsletterModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PostCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string SourceProcess { get; set; }

        public bool NotifyEmail { get; set; }
        public bool NotifySMS { get; set; }
        public bool NotifyPost { get; set; }
    }
}