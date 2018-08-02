using Omnicx.WebStore.Models.Infrastructure;
using Omnicx.WebStore.Models;

namespace Omnicx.API.SDK.Api.Infra
{
   public  interface IConfigApi
    {
       ResponseModel<ConfigModel> GetConfig();
   
    }
}
