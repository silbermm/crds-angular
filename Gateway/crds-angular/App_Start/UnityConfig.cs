using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

using crds_angular.Controllers.API;

using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;


namespace crds_angular
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register defaults - this will allow the container to resolve an interface to an implementation class, by convention.
            //container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies(),
            //    WithMappings.FromMatchingInterface,
            //    WithName.Default,
            //    WithLifetime.PerResolve);

            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IEventService, EventService>();
            container.RegisterType<IMinistryPlatformService, MinistryPlatformServiceImpl>();
            container.RegisterType<PlatformServiceClient>(WithLifetime.PerResolve(typeof(PlatformServiceClient)), new InjectionConstructor());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}