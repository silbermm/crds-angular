using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class MessageToDonorDTO
    {
        [JsonProperty(PropertyName = "donorId")]
        public int DonorId { get; set; }

        [JsonProperty(PropertyName = "fromContactId")]
        public int FromContactId { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}