using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Enum;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonationService : BaseService, IDonationService
    {
        private readonly int _donationsPageId;
        private readonly int _donorMessageTemplateId;
        private readonly int _distributionPageId;
        private readonly int _batchesPageId;
        private readonly int _depositsPageId;
        private readonly int _paymentProcessorErrorsPageId;
        private readonly int _tripDistributionsPageView;
        private readonly int _gpExportPageView;
        private readonly int _processingProgramId;
        private readonly int _scholarshipPaymentTypeId;

        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly IDonorService _donorService;
        private readonly ICommunicationService _communicationService;

        public DonationService(IMinistryPlatformService ministryPlatformService, IDonorService donorService, ICommunicationService communicationService, IConfigurationWrapper configuration, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _donorService = donorService;
            _communicationService = communicationService;

            _donationsPageId = configuration.GetConfigIntValue("Donations");
            _donorMessageTemplateId = configuration.GetConfigIntValue("DonorMessageTemplateId");
            _distributionPageId = configuration.GetConfigIntValue("Distributions");
            _batchesPageId = configuration.GetConfigIntValue("Batches");
            _depositsPageId = configuration.GetConfigIntValue("Deposits");
            _paymentProcessorErrorsPageId = configuration.GetConfigIntValue("PaymentProcessorEventErrors");
            _tripDistributionsPageView = configuration.GetConfigIntValue("TripDistributionsView");
            _gpExportPageView = configuration.GetConfigIntValue("GPExportView");
            _processingProgramId = configuration.GetConfigIntValue("ProcessingProgramId");
            _scholarshipPaymentTypeId = configuration.GetConfigIntValue("ScholarshipPaymentTypeId");
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime statusDate,
            string statusNote = null)
        {
            UpdateDonationStatus(ApiLogin(), donationId, statusId, statusDate, statusNote);
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

        public DonationBatch GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return(WithApiLogin(token =>
            {
                var search = string.Format(",,,,,,,,{0},", processorTransferId);
                var batches = _ministryPlatformService.GetRecordsDict(_batchesPageId, token, search);
                if (batches == null || batches.Count == 0)
                {
                    return (null);
                }

                return (Mapper.Map<Dictionary<string, object>, DonationBatch>(batches[0]));
            }));
            
        }

        public DonationBatch GetDonationBatch(int batchId)
        {
            return (WithApiLogin(token => (Mapper.Map<Dictionary<string,object>, DonationBatch>(_ministryPlatformService.GetRecordDict(_batchesPageId, batchId, token)))));
        }

        public DonationBatch GetDonationBatchByDepositId(int depositId)
        {
            return (WithApiLogin(token =>
            {
                var search = string.Format(",,,,,{0}", depositId);
                var batches = _ministryPlatformService.GetRecordsDict(_batchesPageId, token, search);
                if (batches == null || batches.Count == 0)
                {
                    return (null);
                }

                return (Mapper.Map<Dictionary<string, object>, DonationBatch>(batches[0]));
            }));
        }

        public List<Deposit> GetSelectedDonationBatches(int selectionId, string token)
        {
            var results = _ministryPlatformService.GetSelectionsForPageDict(_depositsPageId, selectionId, token);
            var deposits = new List<Deposit>();

            foreach (var result in results)
            {
                deposits.Add(Mapper.Map<Dictionary<string, object>, Deposit>(result));
            }

            return deposits;
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
                var token = ApiLogin();
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

        public int CreateDeposit(string depositName, decimal depositTotalAmount, decimal depositAmount, decimal depositProcessorFee, DateTime depositDateTime,
            string accountNumber, int batchCount, bool exported, string notes, string processorTransferId)
        {
            var parms = new Dictionary<string, object>
            {
                {"Deposit_Name", depositName},
                {"Deposit_Total", depositTotalAmount},
                {"Deposit_Amount", depositAmount},
                {"Processor_Fee_Total", depositProcessorFee},
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
                        "CreateDeposit failed. depositName: {0}, depositTotalAmount: {1}, depositAmount: {2}, depositProcessorFee: {3}, depositDateTime: {4}, accountNumber: {5}, batchCount: {6}, exported: {7}, notes: {8}",
                        depositName, depositTotalAmount, depositAmount, depositProcessorFee, depositDateTime, accountNumber, batchCount, exported, notes), e);
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
                string apiToken = ApiLogin();
                var result = GetDonationByProcessorPaymentId(processorPaymentId, apiToken);

                var rec = _ministryPlatformService.GetRecordsDict(_distributionPageId, apiToken, ",,,,,,,," + result.donationId);
                
                if (rec.Count == 0 || (rec.Last().ToNullableInt("dp_RecordID")) == null)
                {
                    throw (new ApplicationException("Could not locate donation for charge " + processorPaymentId));
                }
                
                var program = rec.First().ToString("Statement_Title");
                var paymentType = PaymentType.getPaymentType(result.paymentTypeId).name;
                var declineEmailTemplate = PaymentType.getPaymentType(result.paymentTypeId).declineEmailTemplateId;

                _donorService.SendEmail(declineEmailTemplate, result.donorId, result.donationAmt, paymentType, result.donationDate,
                    program, result.donationNotes);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format(
                        "ProcessDeclineEmail failed. processorPaymentId: {0},", processorPaymentId), ex);
            }
        }

        public Donation GetDonationByProcessorPaymentId(string processorPaymentId)
        {
            return (WithApiLogin(token => GetDonationByProcessorPaymentId(processorPaymentId, token)));
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
                paymentTypeId = PaymentType.getPaymentType(dictionary.ToString("Payment_Type")).id,
                donationNotes = dictionary.ToString("Donation_Status_Notes"),
                batchId = dictionary.ToNullableInt("Batch_ID")
            };
            return (d);
        }

        public List<TripDistribution> GetMyTripDistributions(int contactId)
        {
            var results = _ministryPlatformService.GetPageViewRecords(_tripDistributionsPageView, ApiLogin(), contactId.ToString());
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
                    DonorId = result.ToInt("Donor ID"),
                    DonorNickname = result.ToString("Nickname"),
                    DonorFirstName = result.ToString("First Name"),
                    DonorLastName = result.ToString("Last Name"),
                    DonorEmail = result.ToString("Email Address"),
                    DonationDate = result.ToDate("Donation Date"),
                    DonationAmount = Convert.ToInt32(result["Amount"]),
                    AnonymousGift = result.ToBool("Anonymous"),
                    RegisteredDonor = result.ToBool("Registered Donor"),
                    MessageSent = result.ToBool("Message Sent")
                };

                trips.Add(trip);
            }
            return trips;
        }

        public List<GPExportDatum> GetGPExport(int depositId, string token)
        {
            var results = _ministryPlatformService.GetPageViewRecords(_gpExportPageView, token, depositId.ToString());
            var gpExport = new List<GPExportDatum>();

            foreach (var result in results)
            {
                var gp = new GPExportDatum
                {
                    ProccessFeeProgramId = _processingProgramId,
                    ProgramId = result.ToInt("Program_ID"),
                    DocumentType = result.ToString("Document_Type"),
                    DonationId = result.ToInt("Donation_ID"),
                    BatchName = result.ToString("Batch_Name"),
                    DonationDate = result.ToDate("Donation_Date"),
                    DepositDate = result.ToDate("Deposit_Date"),
                    CustomerId = result.ToString("Customer_ID"),
                    DonationAmount = result.ToString("Donation_Amount"),
                    CheckbookId = result.ToString("Checkbook_ID"),
                    CashAccount = result.ToString("Cash_Account"),
                    ReceivableAccount = result.ToString("Receivable_Account"),
                    DistributionAccount = result.ToString("Distribution_Account"),
                    ScholarshipExpenseAccount = result.ToString("Scholarship_Expense_Account"),
                    Amount = result.ToString("Amount"),
                    ScholarshipPaymentTypeId = _scholarshipPaymentTypeId,
                    PaymentTypeId = result.ToInt("Payment_Type_ID")
                };

                gpExport.Add(gp);
            }

            return gpExport;
        }

        public void UpdateDepositToExported(int selectionId, int depositId, string token)
        {
            var paramaters = new Dictionary<string, object>
            {
                {"Deposit_ID", depositId},
                {"Exported", true},
            };

            _ministryPlatformService.UpdateRecord(_depositsPageId, paramaters, token);
            _ministryPlatformService.RemoveSelection(selectionId, new [] {depositId}, token);
        }

        public void SendMessageToDonor(int donorId, int fromContactId, string body, string subject)
        {
            var template = _communicationService.GetTemplate(_donorMessageTemplateId);
            var messageData = new Dictionary<string, object>
            {
                {"TripSubject", subject}, 
                {"DonorMessage", body}
            };
            var toEmail = _donorService.GetEmailViaDonorId(donorId);
            var authorId = _communicationService.GetUserIdFromContactId(fromContactId);
            var fromEmail = _communicationService.GetEmailFromContactId(fromContactId);

            var comm = new Communication
            {
                AuthorUserId = authorId,
                DomainId = 1,
                ToContactId = toEmail.ContactId,
                ToEmailAddress = toEmail.Email,
                FromContactId = fromContactId,
                FromEmailAddress = fromEmail,
                ReplyContactId = fromContactId,
                ReplyToEmailAddress = fromEmail,
                EmailSubject = _communicationService.ParseTemplateBody(template.Subject, messageData),
                EmailBody = _communicationService.ParseTemplateBody(template.Body, messageData),
                MergeData = messageData
            };
            _communicationService.SendMessage(comm);
        }
    }
}
