using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

using crds_angular.Controllers.API;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;


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

            container.RegisterType<IPersonService, PersonService>();
            container.RegisterType<IGroupService, GroupService>();
            container.RegisterType<IEventService, EventService>();
            container.RegisterType<IMinistryPlatformService, MinistryPlatformServiceImpl>();
            container.RegisterType<IAuthenticationService, AuthenticationServiceImpl>();
            container.RegisterType<IContactService, ContactService>();
            container.RegisterType<IContactRelationshipService, ContactRelationshipService>();
            container.RegisterType<IOpportunityService, OpportunityServiceImpl>();
            container.RegisterType<IConfigurationWrapper, ConfigurationWrapper>();
            container.RegisterType<IServeService, ServeService>();
            container.RegisterType<IParticipantService, ParticipantService>();
            container.RegisterType<IProgramService, ProgramService>();
            container.RegisterType<IDonorService, DonorService>();
            container.RegisterType<IStripeService, StripeService>();

            container.RegisterType<PlatformServiceClient>(WithLifetime.PerResolve(typeof(PlatformServiceClient)), new InjectionConstructor());

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}