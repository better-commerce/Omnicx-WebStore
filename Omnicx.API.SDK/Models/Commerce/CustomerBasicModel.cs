using System;

namespace Omnicx.API.SDK.Models.Commerce
{
    public class CustomerBasicModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Company properties populated ONLY when the order is placed by a company
        /// </summary>
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegNo { get; set; }
        public string CompanyTaxRegNo { get; set; }
    }
}