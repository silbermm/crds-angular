using System.Collections.Generic;
using MinistryPlatform.Models;

namespace crds_angular.Services
{
    public interface IContactAttributeService
    {
        List<ContactAttribute> GetContactAttributes(int contactId);
        void SaveContactAttributes(int contactId, List<ContactAttribute> contactAttributes);
    }
}