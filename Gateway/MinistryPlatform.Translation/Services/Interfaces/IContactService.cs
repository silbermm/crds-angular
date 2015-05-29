using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactService
    {
        string GetContactEmail(int contactId);
        MyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName);
    }
}