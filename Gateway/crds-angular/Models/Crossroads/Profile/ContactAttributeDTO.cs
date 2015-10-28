using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ContactAttributeDTO
    {        
        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "selected")]
        public bool Selected { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "sortOrder")]
        public int SortOrder { get; set; }

    }
}