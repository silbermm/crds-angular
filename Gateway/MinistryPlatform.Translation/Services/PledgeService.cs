using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class PledgeService : BaseService, IPledgeService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        private readonly int _pledgePageId;

        public PledgeService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;

            _pledgePageId = configurationWrapper.GetConfigIntValue("Pledges");
        }

        public int CreatePledge(int donorId, int pledgeCampaignId, decimal totalPledge)
        {
            var values = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Pledge_Campaign_ID", pledgeCampaignId},
                {"Pledge_Status_ID", 1},
                {"Total_Pledge", totalPledge},
                {"Installments_Planned", 0},
                {"Installments_Per_Year", 0},
                {"First_Installment_Date", DateTime.Now}
            };

            int pledgeId;

            try
            {
                pledgeId = WithApiLogin<int>(apiToken => (_ministryPlatformService.CreateRecord(_pledgePageId, values, apiToken, true)));
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreatePledge failed.  Donor Id: {0}", donorId), e);
            }
            return pledgeId;
        }
    }
}