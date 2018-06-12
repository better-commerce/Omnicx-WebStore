using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using Omnicx.API.SDK.Entities;

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
                                       SpecialChars = p.Element("specialChars").Value
                                   }).First();

            StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);
            //min and max
            sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");

            //numbers length
            sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");

            //a-z characters
            sbPasswordRegx.Append(@"(?=.*[a-z])");

            //A-Z length
            sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

            //special characters length
            sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

            //(?!.*\s) - no spaces
            //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
            sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");

            return sbPasswordRegx.ToString();
        }
        /// <summary>
        /// Remove unnessary call to basket api on all page if no basket available
        /// </summary>
        /// <returns></returns>
        public static bool HasBasketAction()
        {
            if (System.Web.HttpContext.Current.Session[Constants.SESSION_HAS_BASKET_ACTION] != null) return bool.Parse(System.Web.HttpContext.Current.Session[Constants.SESSION_HAS_BASKET_ACTION].ToString());
            return true;
        }
        /// <summary>
        /// Set basket action
        /// </summary>
        /// <param name="basketId"></param>
        public static void SetBasketAction(string basketId="",bool resetAction=false)
        {
            if (resetAction)
                System.Web.HttpContext.Current.Session[Constants.SESSION_HAS_BASKET_ACTION] = null;
            else
                System.Web.HttpContext.Current.Session[Constants.SESSION_HAS_BASKET_ACTION] = !(string.IsNullOrEmpty(basketId) || basketId == Guid.Empty.ToString());
        }
    }
}