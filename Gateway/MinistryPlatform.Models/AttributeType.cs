using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class AttributeType
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute> Attributes { get; private set;  }
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }

        public AttributeType()
        { 
            Attributes = new List<Attribute>();            
        }
    }
}
