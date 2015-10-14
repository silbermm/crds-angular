using System.Collections.Generic;
using crds_angular.Models.Crossroads.Profile;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IContactAttributeService
    {
        List<ContactAttributeTypeDTO> GetContactAttributes(int contactId);
        void SaveContactAttributes(int contactId, List<ContactAttributeTypeDTO> contactAttributes);
    }
}