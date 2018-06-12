using System;
using System.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Linq;
using Omnicx.WebStore.Core.Services.Authentication;
using Omnicx.WebStore.Core.Services.Infrastructure;
using Omnicx.API.SDK.Helpers;
using Omnicx.API.SDK.Api.Infra;
using Omnicx.WebStore.Apps.OAuthHelper;
using Omnicx.WebStore.Core.Services.Log;

namespace Omnicx.WebStore.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            //HttpContext object
            container.RegisterType<HttpContextBase>(new InjectionFactory(_ => new HttpContextWrapper(HttpContext.Current)));
            //Request object
            container.RegisterType<HttpRequestBase>(new InjectionFactory(_ => new HttpRequestWrapper(HttpContext.Current.Request)));
            //Response object for injection
            container.RegisterType<HttpResponseBase>(new InjectionFactory(_ => new HttpResponseWrapper(HttpContext.Current.Response)));

            container.RegisterType<IAuthenticationService, FormsAuthenticationService>();
            container.RegisterType<ISessionContext, SessionContext>();
            container.RegisterType<ILogService, LogService>();
            container.RegisterType<ISocialLoginService, SocialLoginService>();

            //per HTTP request. during the lifetime of a single HTTP request.
            container.RegisterType<IHeadTagBuilder, HeadTagBuilder>(new PerRequestLifetimeManager());

            var sdkClasses = AllClasses.FromLoadedAssemblies().Where(x => x.FullName.ToLower().StartsWith("omnicx.api.sdk"));
            container.RegisterTypes(sdkClasses, WithMappings.FromMatchingInterface, WithName.Default);

            container.RegisterTypes(AllClasses.FromLoadedAssemblies().Where(
                x => x.FullName.ToLower().StartsWith("omnicx.site")),
               WithMappings.FromMatchingInterface, WithName.Default);


        }
    }
}
