using Omnicx.API.SDK.Models.Commerce;
using Omnicx.API.SDK.Models.Infrastructure;
using System.Threading.Tasks;
using Omnicx.API.SDK.Entities;

namespace Omnicx.API.SDK.Api.Infra
{
    public interface ISessionContext
    {
        CustomerModel CurrentUser { get; set; }
        string SessionId { get; }
        Task<string> CreateUserSession(bool resetSession = false);
        //DefaultSettingModel DefaultSetting { get; set; }

        ConfigModel CurrentSiteConfig { get; set; }
        string IpAddress { get; }
        CompanyUserRole CurrentUserRole { get; }

    }
}
