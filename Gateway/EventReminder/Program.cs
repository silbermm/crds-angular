using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services;
using log4net;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MinistryPlatform.Translation.Models.People;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using EventService = crds_angular.Services.EventService;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Services;
using IEventService = crds_angular.Services.Interfaces.IEventService;


namespace EventReminder
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IEventService _eventService;
        private static IApiUserService _apiUserService;
        private static IChildcareService _childcareService;
        private static ICommunicationService _communicationService;
        private static IContactRelationshipService _contactRelationshipService;

        static void Main(string[] args)
        {
            //Mapper.Initialize(cfg => cfg.AddProfile<EventProfile>());

            AutoMapperConfig.RegisterMappings();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            var container = new UnityContainer();
            section.Configure(container);

            try
            {
                // Dependency Injection
                _eventService = container.Resolve<EventService>();
                _childcareService = container.Resolve<ChildcareService>();
                _apiUserService = container.Resolve<ApiUserService>();
                _communicationService = container.Resolve<CommunicationService>();
                _contactRelationshipService = container.Resolve<ContactRelationshipService>();

                Log.Info("starting event reminder");
                var token = _apiUserService.GetToken();
                var eventList = _eventService.EventsReadyForReminder(token);

                eventList.ForEach(evt =>
                {
                    // get the participants...
                    var participants = _eventService.EventPaticpants(evt.EventId, token);

                    // does the event have a childcare event?
                    var childcare = _childcareService.GetChildcareEvent(evt.EventId);
                    var childcareParticipants = childcare != null ? _eventService.EventPaticpants(childcare.EventId, token) : new List<Participant>();
                    
                    participants.ForEach(participant => SendEmail(evt, participant, childcareParticipants, token));
                });
                Log.Info("all done");
            }
            catch (Exception ex)
            {
                Log.Error("Event Reminder Process failed.", ex);
                Environment.Exit(9999);
            }
        }

        public static void SendEmail(Event evt, Participant participant, IList<Participant> children, string token)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"Nickname", participant.DisplayName},
                {"Event_Name", evt.name},
                {"Event_Start_Date", evt.StartDate.ToShortDateString()},
                {"Event_Start_Time", evt.StartDate.ToShortTimeString()}
            };

            if (children.Any())
            {
                // determine if any of the children are related to the participant
                var relationships = _contactRelationshipService.GetMyCurrentRelationships(participant.ContactId, token);
                var mine = children.Where(child => relationships.Any(rel => rel.Contact_Id == child.ContactId)).ToList();
                // build the HTML for the [Childcare] data
                if (mine.Any())
                {
                    var childcareString = ChildcareData(mine);
                    mergeData.Add("Childcare", childcareString);
                }

            }
            
        }

        public static String ChildcareData(IList<Participant> children)
        {
            var el = new HtmlElement("span",
                                     new Dictionary<string, string>(),
                                     "You have indicated that you need childcare for the following children:")
                                     .Append(new HtmlElement("ul").Append(children.Select(child => new HtmlElement("li", child.DisplayName)).ToList()));                        
            return el.Build();
        }

    }
}
