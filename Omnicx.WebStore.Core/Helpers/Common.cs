using Omnicx.Site.Core.Entities;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Omnicx.Site.Core.Helpers
{
    public class Common
    {
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
    }
}