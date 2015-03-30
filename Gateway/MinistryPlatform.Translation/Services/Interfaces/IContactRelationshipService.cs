using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactRelationshipService
    {
        IEnumerable<Contact_Relationship> GetMyImmediatieFamilyRelationships(int contactId, string token);
        IEnumerable<Contact_Relationship> GetMyCurrentRelationships(int contactId, string token);
    }
    
}