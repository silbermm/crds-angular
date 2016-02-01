using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IPrivateInviteService
    {
        PrivateInvite Create(int pledgeCampaignId, string emailAddress, string recipientName, string token);
        bool PrivateInviteValid(int pledgeCampaignId, string guid, string emailAddress);
        void MarkAsUsed(int pledgeCampaignId, string inviteGuid);
    }
}