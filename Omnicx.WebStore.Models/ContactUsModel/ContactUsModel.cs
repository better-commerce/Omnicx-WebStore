using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omnicx.Website.Models
{
    public class ContactUsModel
    {
        public string Name { get; set; }

        public string CompanyName { get; set; }

        public string Email{ get; set; }

        /// <summary>
        /// contact request type
        /// </summary>
        public string Type { get; set; }

        public bool Notify { get; set; }
        public string PhoneNo { get; set; }
    }
}