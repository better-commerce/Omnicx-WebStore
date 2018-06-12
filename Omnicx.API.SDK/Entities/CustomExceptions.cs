using System;

namespace Omnicx.API.SDK.Entities
{
    public class TokenException:Exception
    {
        public TokenException()
        {
        }
        public TokenException(string message) : base(message)
        {
           
        }
        public TokenException(string message, Exception inner): base(message, inner)
        {
        }
    }
}