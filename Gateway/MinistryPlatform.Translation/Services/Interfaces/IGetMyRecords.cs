using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IGetMyRecords
    {
        //IEnumerable<Contact_Relationship> GetMyFamily(int contactId, string token);
        List<Attribute> GetMyAttributes(int recordId, string token);
        //List<Group> GetMyServingTeams(int contactId, string token);
        int CreateAttribute(Attribute attribute, int parentRecordId, string token);
        bool DeleteAttribute(int recordId, string token);
    }
}