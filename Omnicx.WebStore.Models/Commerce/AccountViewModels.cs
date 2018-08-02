using System.ComponentModel.DataAnnotations;
using Omnicx.WebStore.Models.Common;
using System;
using Omnicx.WebStore.Models.B2B;

namespace Omnicx.WebStore.Models.Commerce
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "Username", Prompt = "Username")]
        [System.Web.Mvc.AdditionalMetadata("ng-pattern", "/^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$/")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter valid Username.")]
        public string Username { get; set; }
    }

    public class RegisterViewModel :GdprAttrModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [System.Web.Mvc.AdditionalMetadata("ng-pattern", "/^[^\\s@]+@[^\\s@]+\\.[^\\s@]{2,}$/")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "gm.isPasswordValid=gm.checkPassword(registrationForm,'gm.model.registerViewModel.password','gm.model.registerViewModel.confirmPassword')")]    
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "gm.isPasswordValid=gm.checkPassword(registrationForm,'gm.model.registerViewModel.password','gm.model.registerViewModel.confirmPassword')")]
        [Display(Name = "Confirm password")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("input.ng-init", "gm.passwordStrength('am.model.password')")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "am.isPasswordValid=gm.checkPassword(recoveryPasswordForm,'am.model.password','am.model.confirmPassword')")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "am.isPasswordValid=gm.checkPassword(recoveryPasswordForm,'am.model.password','am.model.confirmPassword')")]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public long Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public bool IsValid { get; set; }
    }

    public class UserModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public bool isLogin { get; set;}
    }

    public class CustomerProfileModel
    {
        public CustomerProfileModel()
        {
            CustomerDetail = new CustomerDetailModel();
            CustomerAddress = new AddressModel();
            ChangePasswordViewModel = new ChangePasswordViewModel();
            CompanyDetail = new CompanyDetailModel();
        }
        public CustomerDetailModel CustomerDetail { get; set; }
        public AddressModel CustomerAddress { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public CompanyDetailModel CompanyDetail { get; set; }
        //public CompanyRegisterModel CompanyDetail { get; set; }
    }

    public class CustomerDetailModel:GdprAttrModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        //[Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "First Name", Prompt = "First Name")]
        [MaxLength(50, ErrorMessage = "FirstName Value can't be more than 50")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name", Prompt = "Last Name")]
        [MaxLength(50, ErrorMessage = "LastName Value can't be more than 50")]
        public string LastName { get; set; }

       // [Required] akc requirement
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        [Display(Name = "Birth Date", Prompt = "Birth Date")]
        public string BirthDate { get; set; }

        //[Required]
        [Display(Name = "Post Code", Prompt = "Post Code")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "10")]
        public string PostCode { get; set; }

        [Required]
        [Display(Name = "Phone No", Prompt = "Phone No")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Phone Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "30")]
        [System.Web.Mvc.AdditionalMetadata("input.MinLength", "10")]
        [System.Web.Mvc.AdditionalMetadata("input.Pattern", @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")]
        [MaxLength(15, ErrorMessage = "PhoneNo Value can't be more than 30")]
        public string Telephone { get; set; }

        [Required]
        [Display(Name = "Mobile No", Prompt = "Mobile No")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Mobile Number")]
        [System.Web.Mvc.AdditionalMetadata("input.MaxLength", "30")]
        [System.Web.Mvc.AdditionalMetadata("input.MinLength", "10")]
        [System.Web.Mvc.AdditionalMetadata("input.Pattern", @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$")]
        [MaxLength(15, ErrorMessage = "PhoneNo Value can't be more than 30")]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid Email.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Day Of Birth", Prompt = "Day Of Birth")]
        public string DayOfBirth { get; set; }
        /// <summary>
        /// Gets or sets the user LastName
        /// </summary>
        /// 
        [Required]
        [Display(Name = "Month Of Birth", Prompt = "Month Of Birth")]
        public string MonthOfBirth { get; set; }
        /// <summary>
        /// Gets or sets the user LastName
        /// </summary>
        [Required]
        [Display(Name = "Year Of Birth", Prompt = "Year Of Birth")]
        public string YearOfBirth { get; set; }


        //TEMP COMPANY PROPERTIES.
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public bool Cheque { get; set; }
        public bool AccountCredit { get; set; }
        public decimal CreditAvailable { get; set; }
        public Guid PriceListId { get; set; }
        public string RegisteredNumber { get; set; }
        public CompanyAddress Address { get; set; }
        public string CompanyUserRole { get; set; }

    }


    public class ChangePasswordViewModel :GdprAttrModel
    {
        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Current password", Prompt = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("input.ng-init", "gm.passwordStrength('am.model.changePasswordViewModel.newPassword')")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "am.isPasswordValid=gm.checkPassword(changePasswordForm,'am.model.changePasswordViewModel.newPassword','am.model.changePasswordViewModel.confirmPassword')")]
        [Display(Name = "New password", Prompt = "New password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [Display(Name = "Confirm new password", Prompt = "Confirm new password")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "am.isPasswordValid=gm.checkPassword(changePasswordForm,'am.model.changePasswordViewModel.newPassword','am.model.changePasswordViewModel.confirmPassword')")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password did not match.")]
        public string ConfirmPassword { get; set; }
    }
    //public class ChangePasswordModel
    //{
    //    public string UserId { get; set; }
    //    [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
    //    public string Password { get; set; }
    //}

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Username", Prompt = "Username")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter valid Username.")]
        public string Username { get; set; }

        public string Password { get; set; }
    }
    //Name to to changed to companyDetailModel
 

    public class SocialViewConfig
    {
        public bool FacebookEnabled { get; set; }
        public bool GooglePlusEnabled { get; set; }
        public bool TwitterEnabled { get; set; }
    }

    
}
