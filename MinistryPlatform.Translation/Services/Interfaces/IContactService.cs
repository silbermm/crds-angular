using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactService
    {
        string GetContactEmail(int contactId);
        MyContact GetContactById(int contactId);
        MyContact GetContactByIdCard(string idCard);
        int GetContactIdByParticipantId(int participantId);
        List<HouseholdMember> GetHouseholdFamilyMembers(int householdId);
        MyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName);
        int CreateContactForSponsoredChild(string firstName, string lastName, string town, string idCard);
        int CreateContactForNewDonor(ContactDonor contactDonor);
        IList<int> GetContactIdByRoleId(int roleId, string token);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string, object> householdDictionary, Dictionary<string, object> addressDictionary);
        void UpdateContact(int contactId, Dictionary<string, object> profileDictionary);
        int GetContactIdByEmail(string email);
        MyContact GetContactByParticipantId(int participantId);
        List<Dictionary<string, object>> StaffContacts();
    }
}