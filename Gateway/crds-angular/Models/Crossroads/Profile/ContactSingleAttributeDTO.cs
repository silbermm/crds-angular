using crds_angular.Models.Crossroads.Attribute;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ContactSingleAttributeDTO
    {
        [JsonProperty(PropertyName = "attribute")]
        public AttributeDTO Value { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }        
    }
}