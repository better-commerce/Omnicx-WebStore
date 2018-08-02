using System;

namespace Omnicx.WebStore.Models.B2B
{
    public class CompanyUserModel
    {
        private string _firstName;

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
                    _firstName = char.ToUpper(value[0]) + value.Substring(1);
            }
        }
        public string LastName { get; set; }
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
