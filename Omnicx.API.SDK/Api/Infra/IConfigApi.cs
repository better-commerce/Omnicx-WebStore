using Omnicx.API.SDK.Models.Infrastructure;
using Omnicx.API.SDK.Models;

namespace Omnicx.API.SDK.Api.Infra
{
   public  interface IConfigApi
    {
       ResponseModel<ConfigModel> GetConfig();
   
    }
}
