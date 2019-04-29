using System.ComponentModel.DataAnnotations;

namespace Omnicx.WebStore.Models.Common
{
    public class ContactModel
    {
        private string _firstName;

        [Required]
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

        [Required]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter valid email address.")]
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
        public string CompanyName { get; set; }

        public string SendTo { get; set; }
        public string PhoneNo { get; set; }
    }
}