using System;
using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class ContactAttribute
    {
        [JsonProperty(PropertyName = "contactAttributeId")]
        public int ContactAttributeId { get; set; }
        [JsonProperty(PropertyName = "attributeId")]
        public int AttributeId { get; set; }
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }
        [JsonProperty(PropertyName = "startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty(PropertyName = "endDate")]
        public DateTime? EndDate { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

    }
}