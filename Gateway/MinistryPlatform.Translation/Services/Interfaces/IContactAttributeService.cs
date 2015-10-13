using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactAttributeService
    {
        List<ContactAttribute> GetCurrentContactAttributes(int contactId);
    }
}