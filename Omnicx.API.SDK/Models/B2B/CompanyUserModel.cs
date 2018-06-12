using System;

namespace Omnicx.API.SDK.Models.B2B
{
    public class CompanyUserModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
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
