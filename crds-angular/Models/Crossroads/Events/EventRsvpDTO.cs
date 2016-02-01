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

        [JsonProperty(PropertyName = "participants")]
        public List<EventRsvpParticipant> Participants { get; set; }
    }

    public class EventRsvpParticipant
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "childcareRequested")]
        public bool ChildcareRequested { get; set; }
    }
}