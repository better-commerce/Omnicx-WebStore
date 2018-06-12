using Omnicx.API.SDK.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class AddressModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [Required]
        [Display(Name = "FirstName", Prompt = "First Name...")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName", Prompt = "Last Name...")]

        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address1", Prompt = "Address 1...")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]

        public string Address1 { get; set; }

        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "Address2", Prompt = "Address 2...")]

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "City", Prompt = "City...")]

        public string City { get; set; }

       
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "County", Prompt = "County...")]

        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "10")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [System.Web.Mvc.AdditionalMetadata("autocomplete", "off")]
        [Display(Name = "PostCode", Prompt = "Post Code...")]

        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Mobile No", Prompt = "Mobile No...")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter valid Phone Number")]
        [System.Web.Mvc.AdditionalMetadata("input.Pattern",@"^(\d{10})$")]
        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }

        public string CustomerId { get; set; }
        public bool IsDefault { get; set; }
        public List<CountryModel> BillingCountries { get; set; }
        public string CompanyId { get; set; }

    }
}