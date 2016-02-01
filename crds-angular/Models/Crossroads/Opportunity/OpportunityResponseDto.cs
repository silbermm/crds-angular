using System;
using crds_angular.Models.Crossroads.Events;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Opportunity
{
    public class OpportunityResponseDto
    {
        [JsonProperty(PropertyName = "closedId")]
        public bool Closed { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "opportunityId")]
        public int OpportunityId { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "responseDate")]
        public DateTime ResponseDate { get; set; }

        [JsonProperty(PropertyName = "responseId")]
        public int ResponseId { get; set; }

        [JsonProperty(PropertyName = "responseResultId")]
        public int? ResponseResultId { get; set; }

        [JsonProperty(PropertyName = "event")]
        public Event OpportunityEvent { get; set; }
    }
}