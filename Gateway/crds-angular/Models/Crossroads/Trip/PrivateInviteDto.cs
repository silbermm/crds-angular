using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Trip
{
    public class PrivateInviteDto
    {
        [JsonProperty(PropertyName = "pledgeCampaignId")]
        public int PledgeCampaignId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "recipientName")]
        public string RecipientName { get; set; }
    }
}