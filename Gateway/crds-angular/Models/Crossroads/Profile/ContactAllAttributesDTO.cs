using System.Collections.Generic;
using crds_angular.Services;

namespace crds_angular.Models.Crossroads.Profile
{
    public class ContactAllAttributesDTO
    {
        public Dictionary<int, ContactAttributeTypeDTO> MultiSelect { get; set; }
        public Dictionary<int, ContactSingleAttributeDTO> SingleSelect { get; set; }
    }
}