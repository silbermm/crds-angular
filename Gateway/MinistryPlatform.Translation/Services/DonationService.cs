using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;
using RestSharp.Extensions;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly int _donationsPageId;
        private readonly int _donorPageId;
        private readonly int _distributionPageId;
        private readonly int _batchesPageId;
        private readonly int _declineEmailTemplate;
        private readonly int _creditCardPaymentType;
        private readonly int _depositsPageId;

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IDonorService _donorService;
      
        public DonationService(IMinistryPlatformService ministryPlatformService, IDonorService donorService, IConfigurationWrapper configuration)
        {
            _ministryPlatformService = ministryPlatformService;
            _donorService = donorService;

            _donationsPageId = configuration.GetConfigIntValue("Donations");
            _distributionPageId = configuration.GetConfigIntValue("Distributions");
            _batchesPageId = configuration.GetConfigIntValue("Batches");
            _declineEmailTemplate = configuration.GetConfigIntValue("DefaultGiveDeclineEmailTemplate");
            _creditCardPaymentType = configuration.GetConfigIntValue("CreditCard");
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
            return (WithApiLogin(token =>
            {
                //var result = _ministryPlatformService.GetRecordsDict(_donationsPageId, token,
                //    ",,,,,,," + processorPaymentId);
                //int? donationId;
                //if (result.Count == 0 || (donationId = result.Last().ToNullableInt("dp_RecordID")) == null)
                //{
                //    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                //}

                var result = GetDonationByProcessorPaymentId(processorPaymentId, token);

                UpdateDonationStatus(token, result.donationId, statusId, statusDate, statusNote);
                return (result.donationId);
            }));
        }

        public int CreateDonationBatch(string batchName, DateTime setupDateTime, decimal batchTotalAmount, int itemCount,
            int batchEntryType, int? depositId, DateTime finalizedDateTime, string processorTransferId)
        {
            
            var parms = new Dictionary<string, object>
            {
                {"Batch_Name", batchName},
                {"Setup_Date", setupDateTime},
                {"Batch_Total", batchTotalAmount},
                {"Item_Count", itemCount},
                {"Batch_Entry_Type_ID", batchEntryType},
                {"Deposit_ID", depositId},
                {"Finalize_Date", finalizedDateTime},
                {"Processor_Transfer_ID", processorTransferId}
            };
            try
            {
                var token = apiLogin();
                var batchId = _ministryPlatformService.CreateRecord(_batchesPageId, parms, token);

                // Important! These two fields have to be set on an update, not on the initial
                // create.  They are nullable fields with default values, but setting a null
                // value on the CreateRecord call has no effect (the default values still get used).
                var updateParms = new Dictionary<string, object>
                {
                    {"Batch_ID", batchId},
                    {"Currency", null},
                    {"Default_Payment_Type", null}
                };
                _ministryPlatformService.UpdateRecord(_batchesPageId, updateParms, token);

                return (batchId);
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format(
                        "CreateDonationBatch failed. batchName: {0}, setupDateTime: {1}, batchTotalAmount: {2}, itemCount: {3}, batchEntryType: {4}, depositId: {5}, finalizedDateTime: {6}",
                        batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType, depositId,
                        finalizedDateTime), e);
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
            string accountNumber, int batchCount, bool exported, string notes, string processorTransferId)
        {
            var parms = new Dictionary<string, object>
            {
                {"Deposit_Name", depositName},
                {"Deposit_Total", depositTotalAmount},
                {"Deposit_Date", depositDateTime},
                {"Account_Number", accountNumber},
                {"Batch_Count", batchCount},
                {"Exported", exported},
                {"Notes", notes},
                {"Processor_Transfer_ID", processorTransferId}
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

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            try
            {
                string apiToken = apiLogin();
                var result = GetDonationByProcessorPaymentId(processorPaymentId, apiToken);

                var rec = _ministryPlatformService.GetRecordsDict(_distributionPageId, apiToken, ",,,,,,,," + result.donationId);
                int? distributionId;
                if (rec.Count == 0 || (distributionId = rec.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                }
                
                var program = rec.First().ToString("Statement_Title");

                var paymentType = (result.paymentTypeId == _creditCardPaymentType)
                    ? "Credit Card"
                    : "Bank";

                _donorService.SendEmail(_declineEmailTemplate, result.donorId, result.donationAmt, paymentType, result.donationDate,
                    program, result.donationNotes);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format(
                        "ProcessDeclineEmail failed. processorPaymentId: {0},", processorPaymentId), ex);
            }
        }


        private Donation GetDonationByProcessorPaymentId(string processorPaymentId, string apiToken)
        {
            var result = _ministryPlatformService.GetRecordsDict(_donationsPageId, apiToken,
                ",,,,,,," + processorPaymentId);
            int? donationId;

            if (result.Count == 0 || (donationId = result.Last().ToNullableInt("dp_RecordID")) == null)
            {
                throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
            }

            var dictionary = result.First();
          
            var d = new Donation()
            {
                donationId = dictionary.ToInt("dp_RecordID"),
                donorId = dictionary.ToInt("Donor_ID"),
                donationDate = dictionary.ToDate("Donation_Date"),
                donationAmt = Convert.ToInt32(dictionary["Donation_Amount"]),
                paymentTypeId = (dictionary.ToString("Payment_Type") == "Bank") ? 4 : 5,
                donationNotes = dictionary.ToString("Donation_Status_Notes")
            };
            return (d);
        }
    }

   
}
