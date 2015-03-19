using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class EventType
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }
    }
}