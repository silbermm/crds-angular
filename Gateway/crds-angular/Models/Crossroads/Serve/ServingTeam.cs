using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServingTeam
    {
        [JsonProperty(PropertyName = "index")]
        public long Index { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "eventTypeId")]
        public int EventTypeId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "primaryContact")]
        public string PrimaryContact { get; set; }

        [JsonProperty(PropertyName = "members")]
        public List<TeamMember> Members { get; set; }

        public ServingTeam()
        {
            this.Members = new List<TeamMember>();
        }
    }
}