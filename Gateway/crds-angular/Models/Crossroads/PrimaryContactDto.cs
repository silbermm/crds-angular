using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class PrimaryContactDto
    {
        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}