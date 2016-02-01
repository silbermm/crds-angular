using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class AttributeDTO
    {
        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }
        [JsonProperty(PropertyName = "categoryId")]
        public int? CategoryId { get; set; }
        [JsonProperty(PropertyName = "categoryDescription")]
        public string CategoryDescription { get; set; }
        [JsonProperty(PropertyName = "sortOrder")]
        public int SortOrder { get; set; }        
    }
}