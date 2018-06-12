using System;

namespace Omnicx.API.SDK.Models.Common
{
    public class AuthTokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

    }
}