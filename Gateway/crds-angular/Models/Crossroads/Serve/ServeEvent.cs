using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServeEvent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "startDateTime")]
        public DateTime StarDateTime { get; set; }

        [JsonProperty(PropertyName = "dateOnly")]
        public string DateOnly { get; set; }

        [JsonProperty(PropertyName = "timeOnly")]
        public string TimeOnly { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }
    }
}