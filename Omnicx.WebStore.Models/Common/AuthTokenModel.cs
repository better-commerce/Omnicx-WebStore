using System;

namespace Omnicx.WebStore.Models.Common
{
    public class AuthTokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}