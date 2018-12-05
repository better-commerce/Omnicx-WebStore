using Omnicx.WebStore.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using Omnicx.WebStore.Models.Helpers;

namespace Omnicx.WebStore.Models.Commerce
{
    public class AddressModel
    {
        private string _firstName;
        private string _lastName;
        private string _address1;
        private string _address2;
        private string _address3;
        private string _companyName;
        private string _postCode;
        private string _city;

        public string Id { get; set; }

        public string Title { get; set; }

        [Required]
        [Display(Name = "FirstName", Prompt = "First Name...")]
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

        [Required]
        [Display(Name = "LastName", Prompt = "Last Name...")]

        public string LastName {
            get
            {
                return _lastName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _lastName = value.ToSentenceCase();
                }
                   
            }
        }

        [Required]
        [Display(Name = "Address1", Prompt = "Address 1...")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]

        public string Address1
        {
            get
            {
                return _address1;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _address1 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "Address2", Prompt = "Address 2...")]

        public string Address2
        {
            get
            {
                return _address2;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _address2 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        public string Address3
        {
            get
            {
                return _address3;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _address3 = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "City", Prompt = "City...")]

        public string City {
            get
            {
                return _city;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _city = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            }
        }

       
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "County", Prompt = "County...")]

        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "11")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [System.Web.Mvc.AdditionalMetadata("autocomplete", "off")]
        [Display(Name = "PostCode", Prompt = "Post Code...")]

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

        [Required]
        [Display(Name = "Mobile No", Prompt = "Mobile No...")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter valid Phone Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "30")]
        [MaxLength(30, ErrorMessage = "Mobile No Value can't be more than 30")]
        [System.Web.Mvc.AdditionalMetadata("input.Pattern", @"^(\+?(\d[\d-.\w ]+)?(\([\d-.\w ]+\))?[\d-.\w ]+\d)|([a-zA-Z]\w*)$")]
        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }

        public string CustomerId { get; set; }
        public bool IsDefault { get; set; }
        public List<CountryModel> BillingCountries { get; set; }
        public string CompanyId { get; set; }
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _companyName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                }
            }
        }
    }
}