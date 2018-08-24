using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Common;
using System;
using System.Collections.Generic;

namespace Omnicx.WebStore.Models.B2B
{
    public class CompanyDetailModel
    {
        private string _companyName;
        public string Email { get; set; }
        public Guid CompanyId { get; set; }
        public string BusinessType { get; set; }
        public string CompanyName {
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
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public AddressModel CompanyAddress { get; set; }
        public string CustomerGroup { get; set; }
        public string ErpCompanyCode { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyRegNo { get; set; }
        public string TaxRegNo { get; set; }

        public List<AddressModel> Addresses { get; set; }
        public string TradingCurrency { get; set; }

        public Amount CreditLimit { get; set; }
        public Amount CreditAvailable { get; set; }
        public Amount UsedCredit { get; set; }
        public DateTime CreditLimitLastRefreshed { get; set; }

        public bool Cheque { get; set; }
        public bool AccountCredit { get; set; }

        public Guid PriceListId { get; set; }

        //public Guid UserId { get; set; }

        //Personal User Informations
        //
        //public string UserName { get; set; }
        //public string Title { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Gender { get; set; }
        //public string Telephone { get; set; }       
        //public string Mobile { get; set; }
        //public string PostCode { get; set; }
    }
}
