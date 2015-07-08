using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _donationsPageId;

        public DonationService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configuration)
        {
            _ministryPlatformService = ministryPlatformService;

            _donationsPageId = configuration.GetConfigIntValue("Donations");
        }

        public void UpdateDonationStatus(int donationId, int statusId, DateTime statusDate,
            string statusNote = null)
        {
            UpdateDonationStatus(new KeyValuePair<string, object>("Donation_ID", donationId), statusId, statusDate, statusNote);
        }

        public void UpdateDonationStatus(string processorPaymentId, int statusId,
            DateTime statusDate, string statusNote = null)
        {
            UpdateDonationStatus(new KeyValuePair<string, object>("Transaction_Code", processorPaymentId), statusId, statusDate, statusNote);
        }

        private void UpdateDonationStatus(KeyValuePair<string, object> recordKey, int statusId, DateTime statusDate,
            string statusNote)
        {
            var parms = new Dictionary<string, object>
            {
                {recordKey.Key, recordKey.Value},
                {"Donation_Status_Date", statusDate},
                {"Donation_Status_Notes", statusNote},
                {"Donation_Status_ID", statusId},
            };

            try
            {
                _ministryPlatformService.UpdateRecord(_donationsPageId, parms, apiLogin());
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "UpdateDonationStatus failed. {0}: {1}, statusId: {2}, statusNote: {3}, statusDate: {4}",
                        recordKey.Key, recordKey.Value, statusId, statusNote, statusDate), e);
            }
        }
    }
}