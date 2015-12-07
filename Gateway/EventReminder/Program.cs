using System;
using System.Configuration;
using System.Reflection;
using crds_angular.App_Start;
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

        static void Main(string[] args)
        {
            AutoMapperConfig.RegisterMappings();
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            try
            {
                // Dependency Injection
                _eventService = container.Resolve<EventService>();
                _eventService.SendReminderEmails();         
                Log.Info("all done");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Log.Error("Event Reminder Process failed.", ex);
                Environment.Exit(9999);
            }
        }
    }
}
