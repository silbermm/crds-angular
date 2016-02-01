using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Stewardship
{
    public class DonorDTO 
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "Processor_ID")]
        public string ProcessorId { get; set; }
        [JsonProperty(PropertyName = "default_source")]
        public DefaultSourceDTO DefaultSource { get; set; }
        [JsonProperty(PropertyName = "Registered_User")]
        public bool RegisteredUser { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}