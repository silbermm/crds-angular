using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Events
{
    public class EventRsvpDTO
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "childCareNeeded")]
        public Boolean ChildCareNeeded { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }
        
    }
}
