using System;
using System.Configuration;
using System.Reflection;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace Crossroads.ChildcareRsvp
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            //IUnityContainer container = new UnityContainer();
            //var lifetimeManager = new PerResolveLifetimeManager();
            //var constructor = new InjectionConstructor();
            //container.RegisterType<MinistryPlatform.Translation.PlatformService.PlatformServiceClient>(lifetimeManager,constructor);
            //container.RegisterType<IConfigurationWrapper, ConfigurationWrapper>();
            //container.RegisterType<IEventParticipantService, EventParticipantService>();
            //container.RegisterType<ICommunicationService, CommunicationService>();
            //container.RegisterType<IMinistryPlatformService, MinistryPlatformServiceImpl>();
            //container.RegisterType<IAuthenticationService, AuthenticationServiceImpl>();
            //container.RegisterType<ICommunicationService, CommunicationService>();
            //container.RegisterType<IContactService, ContactService>();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            try
            {
                var cs = container.Resolve<ChildcareService>();
                cs.SendRequestForRsvp();
            }
            catch (Exception ex)
            {
                Log.Error("Childcare RSVP Email Process failed.", ex);
                Environment.Exit(9999);
            }

            //var configurationWrapper = new Crossroads.Utilities.Services.ConfigurationWrapper();
            ////var platformService = new PlatformServiceClient();
            //var ministryPlatformServiceImpl = new MinistryPlatformServiceImpl(platformService, configurationWrapper);
            //var authenticationService = new AuthenticationServiceImpl(platformService, ministryPlatformServiceImpl);
            //var eventParticipantService = new EventParticipantService(ministryPlatformServiceImpl, authenticationService, configurationWrapper);
            //var communicationService = new CommunicationService(ministryPlatformServiceImpl, authenticationService, configurationWrapper);
            //var contactService = new ContactService(ministryPlatformServiceImpl, authenticationService, configurationWrapper);


            //try
            //{
            //    var childcareService = new ChildcareService(eventParticipantService, communicationService, configurationWrapper, contactService);

            //    childcareService.SendRequestForRsvp();

            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Childcare RSVP Email Process failed.", ex);
            //    Environment.Exit(9999);
            //}
        }
    }
}