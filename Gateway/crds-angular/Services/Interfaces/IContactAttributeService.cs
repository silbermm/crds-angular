using System.Collections.Generic;
using crds_angular.Models.Crossroads.Profile;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IContactAttributeService
    {
        ContactAllAttributesDTO GetContactAttributes(string token, int contactId);
        void SaveContactAttributes(int contactId, Dictionary<int, ContactAttributeTypeDTO> contactAttributes, Dictionary<int, ContactSingleAttributeDTO> contactSingleAttributes);
        void SaveContactMultiAttribute(string token, int contactId, ContactAttributeDTO contactAttribute);
    }
}