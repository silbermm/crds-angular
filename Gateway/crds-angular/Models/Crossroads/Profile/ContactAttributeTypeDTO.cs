using System.Collections.Generic;
using Newtonsoft.Json;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ContactAttributeTypeDTO
    {
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<ContactAttributeDTO> Attributes { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public ContactAttributeTypeDTO()
        {
            Attributes = new List<ContactAttributeDTO>();
        }
    }
}