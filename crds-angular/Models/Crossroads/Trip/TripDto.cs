using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class TripDto
    {
        [JsonProperty(PropertyName = "tripEnd")]
        public string EventEnd { get; set; }

        [JsonIgnore]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "tripParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "tripStartDate")]
        public long EventStartDate { get; set; }

        [JsonProperty(PropertyName = "tripStart")]
        public string EventStart { get; set; }

        [JsonProperty(PropertyName = "tripName")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "programId")]
        public int ProgramId { get; set; }

        [JsonProperty(PropertyName = "programName")]
        public string ProgramName { get; set; }

        [JsonProperty(PropertyName = "campaignId")]
        public int CampaignId { get; set; }

        [JsonProperty(PropertyName = "campaignName")]
        public string CampaignName { get; set; }

        [JsonProperty(PropertyName = "pledgeDonorId")]
        public int PledgeDonorId { get; set; }

        [JsonIgnore]
        public string EventType { get; set; }
    }
}