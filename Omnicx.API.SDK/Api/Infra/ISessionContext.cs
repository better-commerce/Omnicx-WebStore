using Omnicx.WebStore.Models.Commerce;
using Omnicx.WebStore.Models.Enums;
using Omnicx.WebStore.Models.Infrastructure;
using Omnicx.WebStore.Models.Keys;
using System.Threading.Tasks;


namespace Omnicx.API.SDK.Api.Infra
{
    public interface ISessionContext
    {
        CustomerModel CurrentUser { get; set; }
        string SessionId { get; }
        string DeviceId { get; }
        Task<string> CreateUserSession(bool resetSession = false);
        //DefaultSettingModel DefaultSetting { get; set; }

        ConfigModel CurrentSiteConfig { get; set; }
        string IpAddress { get; }
        CompanyUserRole CurrentUserRole { get; }

    }
}
