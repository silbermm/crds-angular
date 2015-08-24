using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICampaignService
    {
        PledgeCampaign GetPledgeCampaign(int campaignId);
    }
}
