using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactService
    {
        string GetContactEmail(int contactId);
        MyContact GetContactById(int contactId);
        List<HouseholdMember> GetHouseholdFamilyMembers(int householdId);
        MyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName);
        int CreateContactForNewDonor(ContactDonor contactDonor);
        IList<int> GetContactIdByRoleId(int roleId, string token);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary);
    }
}