using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using Omnicx.WebStore.Models.Keys;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Common;

namespace Omnicx.WebStore.Core.Helpers
{
    public class SiteUtils
    {
        public static string GetSlugFromUrl()
        {
            if (HttpContext.Current == null) return "";
            var slug = HttpContext.Current.Request.Url.LocalPath.ToLower();
            if (slug == "/") return slug;
            return slug.Remove(0,1);
        }
        public static string GetFullUrl()
        {
            if (HttpContext.Current == null) return "";
            var slug = HttpContext.Current.Request.Url;

            return slug.ToString();
        }

        public static string GenerateSHA1Hash(string hashInput, string secretKey)
        {

            SHA1 sha = new SHA1Managed();
            Encoder enc = System.Text.Encoding.ASCII.GetEncoder();
            String hashStage1 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput))) + "." +
                secretKey;

            String hashStage2 =
                HexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)));

            return hashStage2;
        }

        private static string HexEncode(byte[] data)
        {

            String result = "";
            foreach (byte b in data)
            {
                result += b.ToString("X2");
            }
            result = result.ToLower();

            return (result);
        }

        public static string GetPasswordRegex()
        {
            XDocument xmlDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/assets/core/xml/PasswordPolicy.xml"));

            var passwordSetting = (from p in xmlDoc.Descendants("Password")
                                   select new PasswordSetting
                                   {
                                       Duration = int.Parse(p.Element("duration").Value),
                                       MinLength = int.Parse(p.Element("minLength").Value),
                                       MaxLength = int.Parse(p.Element("maxLength").Value),
                                       NumsLength = int.Parse(p.Element("numsLength").Value),
                                       SpecialLength = int.Parse(p.Element("specialLength").Value),
                                       UpperLength = int.Parse(p.Element("upperLength").Value),
                                       LowerLength=int.Parse(p.Element("lowerLength").Value),
                                       SpecialChars = p.Element("specialChars").Value
                                   }).First();

            StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);
            //min and max
            sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");

            //numbers length
            sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");

            //a-z characters
            //sbPasswordRegx.Append(@"(?=.*[a-z])");

            //A-Z length
            sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

            //a-z length
            sbPasswordRegx.Append(@"(?=(?:.*?[a-z]){" + passwordSetting.LowerLength + "})");

            //special characters length
            sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

            //(?!.*\s) - no spaces
            //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
            sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");

            return sbPasswordRegx.ToString();
        }
        public static void ResetBasketCookieAndSession()
        {
            var cookie_basketId = new System.Web.HttpCookie(Constants.COOKIE_BASKETID) { HttpOnly = true, Value = Guid.NewGuid().ToString(), Expires = DateTime.Now.AddDays(Constants.COOKIE_DEVICEID_EXPIRES_DAYS) };
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie_basketId);
            if(System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session[Constants.SESSION_BASKET] = null;
            }
        }
        public static string[] GetWordPagination()
        {
            string[] words = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N",
            "O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };
            return words;
        }
        #region Encode and Decode string 
        public static string GenerateEncodedString(string encodeString)
        {
            if (!string.IsNullOrEmpty(encodeString))
            {
                byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(encodeString);
                return System.Convert.ToBase64String(toEncodeAsBytes);
            }
            else
                return encodeString;
        }
        public static string GenerateDecodeString(string decodeString)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(decodeString);
            return System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        }
        #endregion
    }
}