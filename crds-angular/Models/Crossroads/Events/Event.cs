using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    //TODO: The properties in this file need to follow C# standards (capitol first letter)
    public class Event
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public String EventType { get; set; }

        [JsonProperty(PropertyName = "primaryContactId")]
        public int PrimaryContactId { get; set; }

        [JsonProperty(PropertyName = "primaryContactEmailAddress")]
        public string PrimaryContactEmailAddress { get; set; }

        // TODO: get rid of the time property and use startDate instead
        public string time { get; set; }

        // TODO: do we really need this if we are using a datetime object in startDate and endDate
        public string meridian { get; set; }

        public string name { get; set; }
        public string location { get; set; }


    }
}