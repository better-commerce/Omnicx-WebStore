using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Catalog;
using System;
using Omnicx.WebStore.Models.Infrastructure;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;

namespace Omnicx.WebStore.Models.Common
{
    public class CheckoutModel 
    {       
        public string Id { get; set; }

        public string BasketId { get; set; }

        public string CustomerId { get; set; }

        public string CompanyId { get; set; }

        [Required]
        [Display(Name = "Email", Prompt = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter valid Username.")]
        public string Email { get; set; }


        [Display(Name = "Password", Prompt = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public BasketModel Basket { get; set; }

        public string ShippingAddressId { get; set; }

        public AddressModel ShippingAddress { get; set; }
        public AddressReadOnlyModel ShippingAddressReadOnly
        {
            get
            {
                var address = new AddressReadOnlyModel();
                if (ShippingAddress == null) return address;
                address.FirstName = ShippingAddress.FirstName;
                address.LastName = ShippingAddress.LastName;
                address.Address1 = ShippingAddress.Address1;
                address.Address2 = ShippingAddress.Address2;
                address.City = ShippingAddress.City;
                address.State = ShippingAddress.State;
                address.Country = ShippingAddress.Country;
                address.CountryCode = ShippingAddress.CountryCode;
                address.PostCode = ShippingAddress.PostCode;
                address.PhoneNo = ShippingAddress.PhoneNo;
                return address;
            }
        }
        public string BillingAddressId { get; set; }

        public AddressModel BillingAddress { get; set; }
        public AddressReadOnlyModel BillingAddressReadOnly
        {
            get
            {
                var address = new AddressReadOnlyModel();
                if (BillingAddress == null) return address;
                address.FirstName = BillingAddress.FirstName;
                address.LastName = BillingAddress.LastName;
                address.Address1 = BillingAddress.Address1;
                address.Address2 = BillingAddress.Address2;
                address.City = BillingAddress.City;
                address.State = BillingAddress.State;
                address.Country = BillingAddress.Country;
                address.CountryCode = BillingAddress.CountryCode;
                address.PostCode = BillingAddress.PostCode;
                address.PhoneNo = BillingAddress.PhoneNo;
                return address;
            }
        }
        public ShippingModel SelectedShipping { get; set; }

        public PaymentMethodModel SelectedPayment { get; set; }

        public List<PaymentBasicModel> PaymentOptions { get; set; }
        public PaymentModel Payment { get; set; }
        public PhysicalStoreModel StoreAddress { get; set; }
        public Amount BalanceAmount { get; set; }

        public Amount PaidAmount { get; set; }

        public PurchasingAsGiftOrMe? GiftOrMe { get; set; }

        public List<ProductModel> WishlistProducts { get; set; }

        public int Stage { get; set; }

        public string CurrencyCode { get; set; }

        public string LanuguageCode { get; set; }
    }

    public class CheckoutViewModel
    {
        public CheckoutModel Checkout { get; set; }
        public LoginViewModel Login { get; set; }
        public RegistrationModel Register { get; set; }
        public List<CountryModel> ShippingCountries { get; set; }
        public List<CountryModel> BillingCountries { get; set; }

        public DateTime CurrentDate { get; set; }
        public bool RegistrationPrompt { get; set; }
    }

    //public class CheckoutResponse
    //{
    //    OrderModel Order { get; set; }
    //}
    public class RegistrationModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Please enter valid Username.")]
        public string Email { get; set; }

        [Required]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "ck.isPasswordValid=ck.checkPassword(registrationForm,'ck.model.register.password','ck.model.register.confirmPassword')")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.AdditionalMetadata("input.autocomplete", "off")]
        [System.Web.Mvc.AdditionalMetadata("ng-keyup", "ck.isPasswordValid=ck.checkPassword(registrationForm,'ck.model.register.password','ck.model.register.confirmPassword')")]
        public string ConfirmPassword { get; set; }
    }

    public class CheckoutResponseModel
    {
        public string OrderNo { get; set; }
        public decimal OrderAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Timestamp { get; set; }
    }
    public class LoginRegistrationModel
    {
        public RegistrationModel Registration { get; set; }
        public LoginViewModel Login { get; set; }
    }
}
