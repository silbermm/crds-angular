using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class SaveTripParticipantsDto
    {
        [JsonProperty(PropertyName = "applicants")]
        public List<TripApplicant> Applicants { get; set; }

        [JsonProperty(PropertyName = "group")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "pledgeCampaign")]
        public PledgeCampaign Campaign { get; set; }
    }
}