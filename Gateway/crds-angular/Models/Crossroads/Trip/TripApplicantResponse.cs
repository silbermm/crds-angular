using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripApplicantResponse
    {

        public List<TripApplicant> Applicants { get; set; }
        public List<TripToolError> Errors { get; set; }
        public TripInfo TripInfo { get; set; }
    }

    public class TripInfo
    {
        public int EventId { get; set; }
        public decimal FundraisingGoal { get; set; }
        public int PledgeCampaignId { get; set; }
    }

    public class TripToolError
    {
        [JsonProperty(PropertyName = "messages")]
        public List<string> Messages { get; set; }
    }
}