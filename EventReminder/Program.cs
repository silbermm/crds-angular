using System;
using System.Configuration;
using System.Reflection;
using crds_angular.App_Start;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using EventService = crds_angular.Services.EventService;
using IEventService = crds_angular.Services.Interfaces.IEventService;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace EventReminder
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IEventService _eventService;
        private static IServeService _serveService;

        static void Main(string[] args)
        {
            AutoMapperConfig.RegisterMappings();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            TlsHelper.AllowTls12();

            var exitCode = 0;

            try
            {
                // Dependency Injection
                _eventService = container.Resolve<EventService>();
                _eventService.SendReminderEmails();                                  
            }
            catch (Exception ex)
            {
                exitCode = 1;
                Log.Error("Event Reminder Process failed.", ex);
            }

            try
            {
                _eventService = container.Resolve<EventService>();
                _eventService.SendPrimaryContactReminderEmails();
            }
            catch (Exception ex)
            {
                exitCode = 1;
                Log.Error("Event Primary Contact Reminder Process failed.", ex);
            }

            try
            {
                _serveService = container.Resolve<ServeService>();
                _serveService.SendReminderEmails();
            }
            catch (Exception ex)
            {
                exitCode += 2;
                Log.Error("Serve Reminder Process failed.", ex);
            }

            Log.Info("all done");
            Environment.Exit(exitCode);
        }
    }
}
