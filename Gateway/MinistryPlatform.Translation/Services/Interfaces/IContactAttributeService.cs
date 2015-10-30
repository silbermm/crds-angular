using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactAttributeService
    {
        List<ContactAttribute> GetCurrentContactAttributes(int contactId);
        int CreateAttribute(string token, int contactId, ContactAttribute attribute, bool useMyProfile);
        void UpdateAttribute(string token, ContactAttribute attribute, bool useMyProfile);
    }
}