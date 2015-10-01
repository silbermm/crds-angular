using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class MessageToDonorDTO
    {
        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }

        [JsonProperty(PropertyName = "donationDistributionId")]
        public int DonationDistributionId { get; set; }

        [JsonProperty(PropertyName = "tripName")]
        public string TripName { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}