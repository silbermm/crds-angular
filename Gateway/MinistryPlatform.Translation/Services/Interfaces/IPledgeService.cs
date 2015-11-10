using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IPledgeService
    {
        int CreatePledge(int donorId, int pledgeCampaignId, decimal totalPledge);
        bool DonorHasPledge(int pledgeCampaignId, int donorId);
        Pledge GetPledgeByCampaignAndDonor(int pledgeCampaignId, int donorId);
        int GetDonorForPledge(int pledgeId);
        List<Pledge> GetPledgesForAuthUser(string userToken, int[] campaignTypeIds = null);
    }
}