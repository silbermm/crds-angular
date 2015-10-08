using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

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
