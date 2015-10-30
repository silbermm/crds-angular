using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactAttributeService
    {
        List<ContactAttribute> GetCurrentContactAttributes(string token, int contactId, bool useMyProfile, int? attributeTypeIdFilter = null);
        int CreateAttribute(string token, int contactId, ContactAttribute attribute, bool useMyProfile);
        void UpdateAttribute(string token, ContactAttribute attribute, bool useMyProfile);
    }
}