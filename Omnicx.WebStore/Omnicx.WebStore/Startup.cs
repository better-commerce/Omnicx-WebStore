using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Omnicx.WebStore.App_Start.Startup))]
namespace Omnicx.WebStore.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


        }
    }
}