using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IContactService
    {
        MyContact GetMyProfile(string token);
        int CreateContactForGuestGiver(string emailAddress, string displayName);
    }
}