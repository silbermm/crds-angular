using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactRelationshipService
    {
        IEnumerable<ContactRelationship> GetMyImmediateFamilyRelationships(int contactId, string token);
        IEnumerable<Relationship> GetMyCurrentRelationships(int contactId);
        IEnumerable<ContactRelationship> GetMyCurrentRelationships(int contactId, string token);
        int AddRelationship(Relationship relationship, int toContact);
    }
    
}
