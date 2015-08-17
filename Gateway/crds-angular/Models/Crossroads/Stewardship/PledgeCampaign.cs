using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class PledgeCampaign
    {
        [JsonProperty(PropertyName = "pledgeCampaignId")]
        public int PledgeCampaignId { get; set; }

        [JsonProperty(PropertyName = "fundraisingGoal")]
        public int FundraisingGoal { get; set; }
    }
}