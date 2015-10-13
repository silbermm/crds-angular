using System.Collections.Generic;

namespace MinistryPlatform.Models
{
    public class ContactAttributeType
    {
        public int AttributeTypeId { get; set; }
        public List<ContactAttributeType> ContactAttributes{ get; set; }

    }
}