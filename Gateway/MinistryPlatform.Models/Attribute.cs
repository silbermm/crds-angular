using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class Attribute
    {
        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "categoryId")]
        public int CategoryId { get; set; }
    }
}