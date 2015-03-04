using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Controllers.API
{
    public class EventLocationController : MPAuth
    {
        [ResponseType(typeof(List<Event>))]
        [Route("api/events/{site}")]
        public IHttpActionResult Get(string site)
        {
            //TODO Move logic to service?
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["TodaysEventLocationRecords"]);
            string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]);

            var todaysEvents = MinistryPlatformService.GetRecordsDict(pageId, token, site, "5 asc");//Why 5 you ask... Think Ministry

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