namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IPledgeService
    {
        int CreatePledge(int donorId, int pledgeCampaignId, decimal totalPledge);
    }
}