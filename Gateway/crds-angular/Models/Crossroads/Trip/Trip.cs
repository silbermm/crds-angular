using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class Trip
    {
        [JsonIgnore]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "tripName")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "tripStart")]
        public long EventStartDate { get; set; }

        [JsonProperty(PropertyName = "tripEnd")]
        public long EventEndDate { get; set; }

        [JsonIgnore]
        public string EventType { get; set; }
    }
}