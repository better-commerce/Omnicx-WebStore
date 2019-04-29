using System.ComponentModel.DataAnnotations;

namespace Omnicx.WebStore.Models.B2B
{
    public class CompanyRegisterModel
    {
        private string _firstName;
        private string _address1;
        private string _address2;
        private string _companyName;
        private string _city;
        private string _postCode;
        private string _lastName;

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First Name", Prompt = "First Name")]
        [MaxLength(50, ErrorMessage = "FirstName Value can't be more than 50")]
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
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [MaxLength(50, ErrorMessage = "LastName Value can't be more than 50")]
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
        [Display(Name = "Gender", Prompt = "Gender")]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Phone No", Prompt = "Phone No")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Phone Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "23")]
        [System.Web.Mvc.AdditionalMetadata("input.MinLength", "10")]
        [MaxLength(23, ErrorMessage = "PhoneNo Value can't be more than 23")]
        public string Telephone { get; set; }

        [Required]
        [Display(Name = "Mobile No", Prompt = "Mobile No")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Mobile Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "23")]
        [System.Web.Mvc.AdditionalMetadata("input.MinLength", "10")]      
        [MaxLength(23, ErrorMessage = "Mobile No Value can't be more than 23")]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        // [Required]  commented akc requirement
        [Display(Name = "BusinessType", Prompt = "BusinessType")]
        public string BusinessType { get; set; }

        [Required]
        [Display(Name = "CompanyName", Prompt = "CompanyName")]
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

       // [Required]  commented akc requirement
        public string RegisteredNumber { get; set; }

        [Required]
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

        [Required]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
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

      //  [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "PostCode", Prompt = "PostCode")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "10")]
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [System.Web.Mvc.AdditionalMetadata("autocomplete", "off")]
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
        // [Required]  commented akc requirement
        [System.Web.Mvc.AdditionalMetadata("capture-plus", "")]
        [Display(Name = "County", Prompt = "County...")]
        public string State { get; set; }
        [Display(Name = "CompanyCode", Prompt = "CompanyCode")]
        public string CompanyCode { get; set; }
        [Display(Name = "Cheque", Prompt = "Cheque")]
        public bool Cheque { get; set; }
        [Display(Name = "AccountCredit", Prompt = "AccountCredit")]
        public bool AccountCredit { get; set; }
        [Required]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "gm.isPasswordValid=gm.checkPassword(changeForm,'gm.model.company.password','gm.model.company.confirmPassword')")]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")] [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "gm.isPasswordValid=gm.checkPassword(changeForm,'gm.model.company.password','gm.model.company.confirmPassword')")]
        public string ConfirmPassword { get; set; }


    }
}
