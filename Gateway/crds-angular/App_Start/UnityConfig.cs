using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;


namespace crds_angular
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // TODO This is picking up and registering Angular controllers as well, which is Not Good - need to determine how to selectively include/exclude
            // Register defaults - this will allow the container to resolve an interface to an implementation class, by convention.
            //container.RegisterTypes(
            //    AllClasses.FromLoadedAssemblies(),
            //    WithMappings.FromMatchingInterface,
            //    WithName.Default,
            //    WithLifetime.ContainerControlled);

            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IEventService, EventService>();
            container.RegisterType<IMinistryPlatformService, MinistryPlatformServiceImpl>();
            container.RegisterType<PlatformServiceClient, PlatformServiceClient>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}