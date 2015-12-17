using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventRsvpDto
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "childCareNeeded")]
        public bool ChildCareNeeded { get; set; }

        [JsonProperty(PropertyName = "participants")]
        public List<int> Participants { get; set; }
    }
}