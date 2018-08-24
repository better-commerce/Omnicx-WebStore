using System;

namespace Omnicx.WebStore.Models.B2B
{
    public class CompanyUserModel
    {
        private string _firstName;
        private string _lastName;

        public Guid UserId { get; set; }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _firstName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _lastName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }
        public string Title { get; set; }
        public string PhoneNo { get; set; }
        public string Gender { get; set; }
        public string Telephone { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyUserRole { get; set; }
    }
}
