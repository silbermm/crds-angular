using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
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

        public bool DonorHasPledge(int pledgeCampaignId, int donorId)
        {
            var searchString = string.Format(",{0},{1}", pledgeCampaignId, donorId);
            var records = _ministryPlatformService.GetPageViewRecords("PledgesByDonorId", ApiLogin(), searchString);
            return records.Count != 0;
        }

        public Pledge GetPledgeByCampaignAndDonor(int pledgeCampaignId, int donorId)
        {
            var searchString = string.Format(",{0},{1}", pledgeCampaignId, donorId);
            var records = _ministryPlatformService.GetPageViewRecords("PledgesByDonorId", ApiLogin(), searchString);
            switch (records.Count)
            {
                case 1:
                    var record = records.First();
                    var pledge = new Pledge();
                    pledge.DonorId = record.ToInt("Donor_ID");
                    pledge.PledgeCampaignId = record.ToInt("Pledge_Campaign_ID");
                    pledge.PledgeId = record.ToInt("Pledge_ID");
                    pledge.PledgeStatusId = record.ToInt("Pledge_Status_ID");
                    return pledge;
                case 0:
                    return null;
                default:
                    throw new ApplicationException(string.Format("GetPledgeByCampaignAndDonor returned multiple records. CampaignId: {0}, DonorId: {1}", pledgeCampaignId, donorId));
            }
        }

        public int GetDonorForPledge(int pledgeId)
        {
            var record = _ministryPlatformService.GetRecordDict(_pledgePageId, pledgeId, ApiLogin());
            return record.ToInt("Donor_ID");
        }
    }
}