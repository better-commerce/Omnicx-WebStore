using Omnicx.WebStore.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnicx.WebStore.Models.Commerce
{
    public class AddressReadOnlyModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [Required]
        [ReadOnly(true)]
        [Display(Name = "FirstName", Prompt = "First Name...")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName", Prompt = "Last Name...")]
        [ReadOnly(true)]

        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address1", Prompt = "Address 1...")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [ReadOnly(true)]

        public string Address1 { get; set; }

        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "Address2", Prompt = "Address 2...")]
        [ReadOnly(true)]

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "City", Prompt = "City...")]
        [ReadOnly(true)]

        public string City { get; set; }


        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "County", Prompt = "County...")]
        [ReadOnly(true)]

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
        [ReadOnly(true)]

        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Mobile No", Prompt = "Mobile No...")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Phone Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "30")]
        [System.Web.Mvc.AdditionalMetadata("input.MinLength", "10")]
        [System.Web.Mvc.AdditionalMetadata("input.Pattern", @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")]
        [ReadOnly(true)]

        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }

        public string CustomerId { get; set; }
        public bool IsDefault { get; set; }
        public List<CountryModel> BillingCountries { get; set; }
        public string CompanyId { get; set; }
    }
}
