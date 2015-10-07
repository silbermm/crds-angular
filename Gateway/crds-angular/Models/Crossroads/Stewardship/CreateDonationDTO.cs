using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class CreateDonationDTO
    {
        [JsonProperty(PropertyName = "program_id")]
        public string ProgramId { get; set; }
        
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
        
        [JsonProperty(PropertyName = "donor_id")]
        public int DonorId { get; set; }
        
        [JsonProperty(PropertyName = "email_address")]
        public string EmailAddress { get; set; }
        
        [JsonProperty(PropertyName = "pymt_type")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "pledge_donor_id")]
        public int? PledgeDonorId { get; set; }

        [JsonProperty(PropertyName = "pledge_campaign_id")]
        public int? PledgeCampaignId { get; set; }

        [JsonProperty(PropertyName = "gift_message")]
        public string GiftMessage { get; set; }
    }
}