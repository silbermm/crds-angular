using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripFormResponseDto
    {
        [JsonProperty(PropertyName = "applicants")]
        public List<TripApplicant> Applicants { get; set; }

        [JsonProperty(PropertyName = "groups")]
        public List<TripGroupDto> Groups { get; set; }

        [JsonProperty(PropertyName = "campaign")]
        public PledgeCampaign Campaign { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<TripToolError> Errors { get; set; }
    }

    public class TripApplicant
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "donorId")]
        public int? DonorId { get; set; }
    }
}