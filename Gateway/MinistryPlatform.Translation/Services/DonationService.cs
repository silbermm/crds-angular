using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly int _donationsPageId;
        private readonly IMinistryPlatformService _ministryPlatformService;

        public DonationService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configuration)
        {
            _ministryPlatformService = ministryPlatformService;

            _donationsPageId = configuration.GetConfigIntValue("Donations");
        }

        public void UpdateDonationStatus(int donationId, int statusId, DateTime statusDate,
            string statusNote = null)
        {
            UpdateDonationStatus(apiLogin(), donationId, statusId, statusDate, statusNote);
        }

        public void UpdateDonationStatus(string processorPaymentId, int statusId,
            DateTime statusDate, string statusNote = null)
        {
            WithApiLogin(token =>
            {
                var result = _ministryPlatformService.GetRecordsDict(_donationsPageId, token,
                    ",,,,,,," + processorPaymentId);
                int? donationId;
                if (result.Count == 0 || (donationId = result.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                }
                UpdateDonationStatus(token, donationId.Value, statusId, statusDate, statusNote);
                return (true);
            });
        }

        private void UpdateDonationStatus(string apiToken, int donationId, int statusId, DateTime statusDate,
            string statusNote)
        {
            var parms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", statusDate},
                {"Donation_Status_Notes", statusNote},
                {"Donation_Status_ID", statusId}
            };

            try
            {
                _ministryPlatformService.UpdateRecord(_donationsPageId, parms, apiToken);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "UpdateDonationStatus failed. donationId: {0}, statusId: {1}, statusNote: {2}, statusDate: {3}",
                        donationId, statusId, statusNote, statusDate), e);
            }
        }
    }
}