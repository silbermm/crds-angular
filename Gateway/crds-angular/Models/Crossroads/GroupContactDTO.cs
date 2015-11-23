using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads
{
    public class GroupContactDTO
    {
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }
    }
}