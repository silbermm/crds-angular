using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactRelationshipService
    {
        IEnumerable<ContactRelationship> GetMyImmediatieFamilyRelationships(int contactId, string token);
        IEnumerable<ContactRelationship> GetMyCurrentRelationships(int contactId, string token);
        int AddRelationship(Relationship relationship);
    }
    
}