using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
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
    }
}