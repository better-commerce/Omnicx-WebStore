using Omnicx.WebStore.Core.ErrorHandler;
using System.Web;
using System.Web.Mvc;

namespace Omnicx.WebStore
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}
