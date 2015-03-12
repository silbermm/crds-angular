using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

using MinistryPlatform.Translation.Services;


namespace crds_angular
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // Register defaults - this will allow the container to resolve an interface to an implementation class, by convention.
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);

            // container.RegisterType<IGroupService, GroupService>();


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}