using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SingingFinder.Web.Services;

namespace SingingFinder.Web.App_Start
{
    public static class IocConfig
    {
        public static void Configure()
        {
            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<ISingingService, SingingService>(Lifestyle.Singleton);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}