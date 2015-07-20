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
        private readonly int _batchesPageId;
        private readonly int _depositsPageId;

        private readonly IMinistryPlatformService _ministryPlatformService;

        public DonationService(IMinistryPlatformService ministryPlatformService, IConfigurationWrapper configuration)
        {
            _ministryPlatformService = ministryPlatformService;

            _donationsPageId = configuration.GetConfigIntValue("Donations");
            _batchesPageId = configuration.GetConfigIntValue("Batches");
            _depositsPageId = configuration.GetConfigIntValue("Deposits");
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime statusDate,
            string statusNote = null)
        {
            UpdateDonationStatus(apiLogin(), donationId, statusId, statusDate, statusNote);
            return (donationId);
        }

        public int UpdateDonationStatus(string processorPaymentId, int statusId,
            DateTime statusDate, string statusNote = null)
        {
            return(WithApiLogin(token =>
            {
                var result = _ministryPlatformService.GetRecordsDict(_donationsPageId, token,
                    ",,,,,,," + processorPaymentId);
                int? donationId;
                if (result.Count == 0 || (donationId = result.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                }
                UpdateDonationStatus(token, donationId.Value, statusId, statusDate, statusNote);
                return (donationId.Value);
            }));
        }

        public int CreateDonationBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount,
            int batchEntryType, int? depositId, DateTime finalizedDateTime)
        {
            var parms = new Dictionary<string, object>
            {
                {"Batch_Name", batchName},
                {"Setup_Date", setupDateTime},
                {"Batch_Total", batchTotalAmount},
                {"Item_Count", itemCount},
                {"Batch_Entry_Type_ID", batchEntryType},
                {"Deposit_ID", depositId},
                {"Finalize_Date", finalizedDateTime}
            };
            try
            {
                return (WithApiLogin(token => (_ministryPlatformService.CreateRecord(_batchesPageId, parms, token))));
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "CreateDonationBatch failed. batchName: {0}, setupDateTime: {1}, batchTotalAmount: {2}, itemCount: {3}, batchEntryType: {4}, depositId: {5}, finalizedDateTime: {6}",
                        batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType, depositId, finalizedDateTime), e);
            }
        }

        public void AddDonationToBatch(int batchId, int donationId)
        {
            var parms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Batch_ID", batchId}
            };

            try
            {
                WithApiLogin(token =>
                {
                    _ministryPlatformService.UpdateRecord(_donationsPageId, parms, token);
                    return (true);
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "AddDonationToBatch failed. batchId: {0}, donationId: {1}",
                        batchId, donationId), e);
            }
        }

        public int CreateDeposit(string depositName, decimal depositTotalAmount, DateTime depositDateTime,
            string accountNumber, int batchCount, bool exported, string notes)
        {
            var parms = new Dictionary<string, object>
            {
                {"Deposit_Name", depositName},
                {"Deposit_Total", depositTotalAmount},
                {"Deposit_Date", depositDateTime},
                {"Account_Number", accountNumber},
                {"Batch_Count", batchCount},
                {"Exported", exported},
                {"Notes", notes}
            };

            try
            {
                return (WithApiLogin(token => (_ministryPlatformService.CreateRecord(_depositsPageId, parms, token))));
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "CreateDeposit failed. depositName: {0}, depositTotalAmount: {1}, depositDateTime: {2}, accountNumber: {3}, batchCount: {4}, exported: {5}, notes: {6}",
                        depositName, depositTotalAmount, depositDateTime, accountNumber, batchCount, exported, notes), e);
            }
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