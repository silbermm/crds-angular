using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly int _donationsPageId;
        private readonly int _donorPageId;
        private readonly int _distributionPageId;
        private readonly int _batchesPageId;
        private readonly int _declineEmailTemplate;
        private readonly string _creditCardPaymentType;
        private readonly string _bankPaymentType;
        private readonly int _depositsPageId;
        private readonly int _paymentProcessorErrorsPageId;
        private readonly int _tripDistributionsPageView;

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
            _creditCardPaymentType = configuration.GetConfigValue("CreditCard");
            _bankPaymentType = configuration.GetConfigValue("Bank");
            _depositsPageId = configuration.GetConfigIntValue("Deposits");
            _paymentProcessorErrorsPageId = configuration.GetConfigIntValue("PaymentProcessorEventErrors");
            _tripDistributionsPageView = configuration.GetConfigIntValue("TripDistributionsView");
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

        public void CreatePaymentProcessorEventError(DateTime? eventDateTime, string eventId, string eventType, string eventMessage,
            string responseMessage)
        {
            var parms = new Dictionary<string, object>
            {
                {"Event_Date_Time", eventDateTime ?? DateTime.Now},
                {"Event_ID", eventId},
                {"Event_Type", eventType},
                {"Event_Message", eventMessage},
                {"Response_Message", responseMessage}
            };
            try
            {
                WithApiLogin(token => _ministryPlatformService.CreateRecord(_paymentProcessorErrorsPageId, parms, token));
            }
            catch (Exception e)
            {
                throw (new ApplicationException(string.Format("Could not insert event error dateTime: {0}, eventId: {1}, eventType: {2}, eventMessage: {3}, responseMessage: {4}, Error: {5}", eventDateTime, eventId, eventType, eventMessage, responseMessage, e.Message)));
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
                
                if (rec.Count == 0 || (rec.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                }
                
                var program = rec.First().ToString("Statement_Title");

                var paymentType = (result.paymentTypeId.ToString() == _creditCardPaymentType.Substring(0,1))
                    ? _creditCardPaymentType.Substring(2,11)
                    : _bankPaymentType.Substring(2,4);

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
          
            if (result.Count == 0 || (result.Last().ToNullableInt("dp_RecordID")) == null)
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
                paymentTypeId = (dictionary.ToString("Payment_Type") == "Bank") ? 5 : 4,
                donationNotes = dictionary.ToString("Donation_Status_Notes")
            };
            return (d);
        }

        public List<TripDistribution> GetMyTripDistributions(int contactId)
        {
            var apiToken = apiLogin();
            var results = _ministryPlatformService.GetPageViewRecords(_tripDistributionsPageView, apiToken, contactId.ToString());
            var trips = new List<TripDistribution>();
            foreach (var result in results)
            {
                var trip = new TripDistribution
                {
                    EventId = result.ToInt("Event ID"),
                    EventTypeId = result.ToInt("Event Type ID"),
                    EventTitle = result.ToString("Event Title"),
                    EventStartDate = result.ToDate("Event Start Date"),
                    EventEndDate = result.ToDate("Event End Date"),
                    TotalPledge = Convert.ToInt32(result["Total Pledge"]),
                    CampaignStartDate = result.ToDate("Start Date"),
                    CampaignEndDate = result.ToDate("End Date"),
                    DonorNickname = result.ToString("Nickname"),
                    DonorFirstName = result.ToString("First Name"),
                    DonorLastName = result.ToString("Last Name"),
                    DonorEmail = result.ToString("Email Address"),
                    DonationDate = result.ToDate("Donation Date"),
                    DonationAmount = Convert.ToInt32(result["Amount"])
                };

                trips.Add(trip);
            }
            return trips;
        }
    }
}
