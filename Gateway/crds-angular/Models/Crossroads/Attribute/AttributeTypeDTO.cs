using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Attribute
{
    public class AttributeTypeDTO
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<crds_angular.Models.Crossroads.Attribute.AttributeDTO> Attributes { get; private set;  }
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }

        public AttributeTypeDTO()
        { 
            Attributes = new List<crds_angular.Models.Crossroads.Attribute.AttributeDTO>();            
        }
    }
}
