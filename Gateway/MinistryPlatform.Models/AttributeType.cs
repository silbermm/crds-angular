using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class AttributeType
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; private set;  }
        public int AttributeTypeId { get; set; }

        public AttributeType()
        { 
            Attributes = new List<Attribute>();            
        }
    }
}
