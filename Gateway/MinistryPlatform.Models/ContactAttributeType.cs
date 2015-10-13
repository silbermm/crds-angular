using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class ContactAttributeType
    {
        [JsonProperty(PropertyName = "attributeTypeId")]
        public int AttributeTypeId { get; set; }
        [JsonProperty(PropertyName = "contactAttributes")]
        public List<ContactAttributeType> ContactAttributes{ get; set; }

    }
}