using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class EventLocationController : MPAuth
    {
        private IMinistryPlatformService _ministryPlatformService;
        private IConfigurationWrapper _configurationWrapper;

        public EventLocationController(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configurationWrapper)
        {
            this._ministryPlatformService = ministryPlatformService;
            this._configurationWrapper = configurationWrapper;
        }

        [ResponseType(typeof(List<Event>))]
        [Route("api/events/{site}")]
        public IHttpActionResult Get(string site)
        {
            //TODO Move logic to service?
            //var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["TodaysEventLocationRecords"]);
            var apiUser = _configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPassword = _configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData =  AuthenticationService.authenticate(apiUser, apiPassword);
            var token = authData["token"].ToString();
            var todaysEvents = _ministryPlatformService.GetRecordsDict("TodaysEventLocationRecords", token, site, "5 asc");//Why 5 you ask... Think Ministry

            var events = ConvertToEvents(todaysEvents);

            return this.Ok(events);
        }


        private List<Event> ConvertToEvents(List<Dictionary<string, object>> todaysEvents)
        {
            //init our return variable
            var events = new List<Event>();

            //iterate over the events
            foreach (var thisEvent in todaysEvents)
            {
                string startDateKey = "Event_Start_Date";
                string nameKey = "Event_Title";
                string locationNameKey = "Room_Name";
                string locationNumberKey = "Room_Number";
                string time = "";
                string meridian = "";
                string name = "";
                string location = "";
                if (thisEvent.ContainsKey(startDateKey) && thisEvent[startDateKey] != null)
                {
                    DateTime startDate = (DateTime)thisEvent[startDateKey];
                    time = startDate.ToString("h:mm");
                    meridian = startDate.ToString("tt");
                }
                if (thisEvent.ContainsKey(nameKey) && thisEvent[nameKey] != null)
                {
                    name = thisEvent[nameKey].ToString();
                }
                if (thisEvent.ContainsKey(locationNameKey) && thisEvent[locationNameKey] != null)
                {
                    location = thisEvent[locationNameKey].ToString();
                }
                if (thisEvent.ContainsKey(locationNumberKey) && thisEvent[locationNumberKey] != null)
                {
                    location += " "+thisEvent[locationNumberKey].ToString();
                }
                var e = new Event
                {
                    time = time,
                    meridian = meridian,
                    name = name,
                    location = location
                };
                events.Add(e);
            }
            return events;
        }
    }
}