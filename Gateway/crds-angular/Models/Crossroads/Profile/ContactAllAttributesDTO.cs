using System.Collections.Generic;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ContactAllAttributesDTO
    {
        public Dictionary<int, ContactAttributeTypeDTO> MultiSelect { get; set; }
        public Dictionary<int, ContactSingleAttributeDTO> SingleSelect { get; set; }
    }
}