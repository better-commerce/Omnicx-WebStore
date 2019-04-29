using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Keys;
using System;
using Omnicx.WebStore.Models.Common;
using System.Threading;

namespace Omnicx.WebStore.Models.Commerce
{
    public class CustomerModel :GdprAttrModel
    {
        private string _firstName;
        private string _postCode;
        private string _companyName;
        

        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string Username { get; set; }
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
        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _postCode = value.ToUpper();
            }
        }

        public string AdminUserName { get; set; }
        public bool IsGhostLogin { get; set; }
        public string UserSourceType { get; set; }

        //TEMP COMPANY PROPERTIES.
        public string CompanyId { get; set; }
        public string CompanyName {
            get
            {
                return _companyName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _companyName=Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                }
            }
        }
        public string BusinessType { get; set; }
      //  public bool Cheque { get; set; }
        //public bool AccountCredit { get; set; }
        public decimal CreditAvailable { get; set; }
        public Guid PriceListId { get; set; }
        public string RegisteredNumber { get; set; }
        public bool IsRegistered { get; set; }
        public CompanyAddress Address { get; set; }
        public CompanyUserRole CompanyUserRole { get; set; }
        public bool IsWelcomeEmailSent { get; set; }
        public bool IsPromptPasswordChange { get; set; }
        public bool IsForcePasswordChange { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public bool IsConvertCompanyRequest { get; set; }
    }

    public class CompanyAddress
    {
        private string _address1;
        private string _address2;
        private string _postCode;
        private string _city;
        public string Address1
        {
            get
            {
                return _address1;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _address1 = char.ToUpper(value[0]) + value.Substring(1);
            }
        }
        public string Address2
        {
            get
            {
                return _address2;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _address2 = char.ToUpper(value[0]) + value.Substring(1);
            }
        }

        public string City {
            get
            {
                return _city;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _city = char.ToUpper(value[0]) + value.Substring(1);
            }
        }
        public string Country { get; set; }
        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _postCode = value.ToUpper();
            }
        }

        public string State { get; set; }

    }
}