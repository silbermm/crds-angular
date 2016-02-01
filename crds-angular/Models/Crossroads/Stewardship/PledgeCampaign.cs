using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class PledgeCampaign
    {
        [JsonProperty(PropertyName = "destinationId")]
        public int DestinationId { get; set; }

        [JsonProperty(PropertyName = "fundraisingGoal")]
        public decimal FundraisingGoal { get; set; }

        [JsonProperty(PropertyName = "pledgeCampaignId")]
        public int PledgeCampaignId { get; set; }
    }
}