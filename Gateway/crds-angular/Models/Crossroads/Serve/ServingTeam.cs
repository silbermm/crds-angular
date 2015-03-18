using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Serve
{
    public class ServingTeam
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "members")]
        public List<TeamMember> Members { get; set; }

        //[JsonProperty(PropertyName = "eventTypes")]
        //public List<tmEventType> EventTypes { get; set; }

        //[JsonProperty(PropertyName = "events")]
        //public List<ServeEvent> Events { get; set; }

        public ServingTeam()
        {
            this.Members = new List<TeamMember>();
        }
    }
}