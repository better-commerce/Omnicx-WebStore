using System;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class CustomerModel
    {
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string DayOfBirth { get; set; }
        public string MonthOfBirth { get; set; }
        public string YearOfBirth { get; set; }
        public string BirthDate { get; set; }
        public string PostCode { get; set; }
        public bool NewsLetterSubscribed { get; set; }
        
        public string AdminUserName { get; set; }
        public bool IsGhostLogin { get; set; }
        public string UserSourceType { get; set; }

        //TEMP COMPANY PROPERTIES.
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
      //  public bool Cheque { get; set; }
        //public bool AccountCredit { get; set; }
        public decimal CreditAvailable { get; set; }
        public Guid PriceListId { get; set; }
        public string RegisteredNumber { get; set; }
        public bool IsRegistered { get; set; }
        public CompanyAddress Address { get; set; }
        public CompanyUserRole CompanyUserRole { get; set; }
    }

    public class CompanyAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
    }
}