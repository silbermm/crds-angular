using System;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class FrequentFlyerAttributeDTO : AttributeTypeDTO
    {
        [JsonProperty(PropertyName = "notes")]
        public String Notes { get; set; }
    }
}