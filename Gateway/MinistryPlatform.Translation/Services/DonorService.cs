using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonorService : BaseService, IDonorService
    {
        private readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int donorPageId = Convert.ToInt32(AppSettings("Donors"));
        private readonly int donationPageId = Convert.ToInt32((AppSettings("Donations")));
        private readonly int donationStatusPageId = Convert.ToInt32((AppSettings("DonationStatus")));
        private readonly int donationDistributionPageId = Convert.ToInt32(AppSettings("Distributions"));

        public const string DONOR_RECORD_ID = "Donor_Record";
        public const string DONOR_PROCESSOR_ID = "Processor_ID";
        public const string EMAILREASON = "None";

        private IMinistryPlatformService ministryPlatformService;
        private IProgramService programService;
        private ICommunicationService communicationService;

        public DonorService(IMinistryPlatformService ministryPlatformService, IProgramService programService, ICommunicationService communicationService, IAuthenticationService authenticationService,IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            this.ministryPlatformService = ministryPlatformService;
            this.programService = programService;
            this.communicationService = communicationService;
        }


        public int CreateDonorRecord(int contactId, string processorId, DateTime setupTime,
            int? statementFrequencyId = 1, // default to quarterly
            int? statementTypeId = 1, //default to individual
            int? statementMethodId = 2 // default to email/online
            )
        {
            //this assumes that you do not already have a donor record - new giver

            var values = new Dictionary<string, object>
            {
                {"Contact_ID", contactId},
                {"Statement_Frequency_ID", statementFrequencyId},
                {"Statement_Type_ID", statementTypeId}, 
                {"Statement_Method_ID", statementMethodId},
                {"Setup_Date", setupTime},    //default to current date/time
                {"Processor_ID", processorId}
            };

            int donorId;

            try
            {
                donorId = WithApiLogin<int>(apiToken =>
                {
                    return (ministryPlatformService.CreateRecord(donorPageId, values, apiToken, true));
                });
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreateDonorRecord failed.  Contact Id: {0}", contactId), e);
            }
            return donorId;

        }

        public int CreateDonationAndDistributionRecord(int donationAmt, int? feeAmt, int donorId, string programId, string charge_id, string pymtType, string processorId, DateTime setupTime, bool registeredDonor)
        {
            var pymt_id = (pymtType == "bank") ? "5" : "4";
            var fee = feeAmt.HasValue ? feeAmt/100M : null;
            
            var donationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Processor_Fee_Amount", fee},
                {"Payment_Type_ID", pymt_id},
                {"Donation_Date", setupTime},
                {"Transaction_code", charge_id},
                {"Registered_Donor", registeredDonor},
                {"Processor_ID", processorId },
                {"Donation_Status_Date", setupTime},
                {"Donation_Status_ID", 1} //hardcoded to pending 
            };

            int donationId;

            try
            {
                donationId = WithApiLogin<int>(apiToken => (ministryPlatformService.CreateRecord(donationPageId, donationValues, apiToken, true)));
            }
            catch (Exception e)
            {
                throw new ApplicationException(string.Format("CreateDonationRecord failed.  Donor Id: {0}", donorId), e);
            }
            
            var distributionValues = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Amount", donationAmt},
                {"Program_ID", programId}
            };

            int donationDistributionId;

            try
            {
                donationDistributionId =
                    WithApiLogin<int>(
                        apiToken =>
                            (ministryPlatformService.CreateRecord(donationDistributionPageId, distributionValues, apiToken, true)));
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("CreateDonationDistributionRecord failed.  Donation Id: {0}", donationId), e);
            }

            try
            {
                SetupConfirmationEmail(Convert.ToInt32(programId), donorId, donationAmt, setupTime, pymtType);
            }
            catch (Exception )
            {
                logger.Error(string.Format("Failed when processing the template for Donation Id: {0}", donationId));
            }

            return donationDistributionId;
        }

        public ContactDonor GetContactDonor(int contactId)
        {
            ContactDonor donor;
            try
            {
                var searchStr = contactId.ToString() + ",";
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (ministryPlatformService.GetPageViewRecords("DonorByContactId", apiToken, searchStr, "")));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();
                    donor = new ContactDonor()
                    {
                        DonorId = record.ToInt("Donor_ID"),
                        ProcessorId = record.ToString(DONOR_PROCESSOR_ID),
                        ContactId = record.ToInt("Contact_ID"),
                        RegisteredUser = true,
                        Email = record.ToString("Email")
                    };
                }
                else
                {
                    donor = new ContactDonor {
                        ContactId = contactId,
                        RegisteredUser = true
                    };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetDonorRecord failed.  Contact Id: {0}", contactId), ex);
            }

            return donor;

        }
        public ContactDonor GetPossibleGuestContactDonor(string email)
        {
            ContactDonor donor;
            try
            {
                if (String.IsNullOrWhiteSpace(email))
                {
                    return null;
                }
                var searchStr =  "," + email;
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (ministryPlatformService.GetPageViewRecords("PossibleGuestDonorContact", apiToken, searchStr, "")));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();
                    donor = new ContactDonor()
                    {
                        
                        DonorId = record.ToInt(DONOR_RECORD_ID),
                        ProcessorId = record.ToString(DONOR_PROCESSOR_ID),
                        ContactId = record.ToInt("Contact_ID"),
                        Email = record.ToString("Email_Address"),
                        RegisteredUser = false
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetPossibleGuestDonorContact failed. Email: {0}", email), ex);
            }

            return donor;

        }

        public int UpdatePaymentProcessorCustomerId(int donorId, string paymentProcessorCustomerId)
        {
            var parms = new Dictionary<string, object> {
                { "Donor_ID", donorId },
                { DONOR_PROCESSOR_ID, paymentProcessorCustomerId },
            };

            try
            {
                ministryPlatformService.UpdateRecord(donorPageId, parms, ApiLogin());
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("UpdatePaymentProcessorCustomerId failed. donorId: {0}, paymentProcessorCustomerId: {1}", donorId, paymentProcessorCustomerId), e);
            }

            return (donorId);
        }


        public void SetupConfirmationEmail(int programId, int donorId, int donationAmount, DateTime setupDate, string pymtType)
        {
            var program = programService.GetProgramById(programId);
            //If the communcations admin does not link a message to the program, the default template will be used.
            int communicationTemplateId = program.CommunicationTemplateId == 0 ? AppSetting("DefaultGiveConfirmationEmailTemplate") : program.CommunicationTemplateId;

            SendEmail(communicationTemplateId, donorId, donationAmount, pymtType, setupDate, program.Name, EMAILREASON);
        }

        public ContactDonor GetEmailViaDonorId(int donorId)
        {
            ContactDonor donor = new ContactDonor();
            try
            {
                var searchStr = "," + donorId.ToString();
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (ministryPlatformService.GetPageViewRecords("DonorByContactId", apiToken, searchStr, "")));
                if (records != null && records.Count > 0)
                {
                    var record = records.First();

                    donor.Email = record.ToString("Email");
                    donor.ContactId = record.ToInt("Contact_ID");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("GetEmailViaDonorId failed.  Donor Id: {0}", donorId), ex);
            }

            return donor;
        }

        public void SendEmail(int communicationTemplateId, int donorId, int donationAmount, string paymentType, DateTime setupDate, string program, string EMAILREASON)
        {
            MessageTemplate template = communicationService.GetTemplate(communicationTemplateId);

            ContactDonor contact = GetEmailViaDonorId(donorId);

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContactId = 5,
                FromEmailAddress = "giving@crossroads.net",
                ReplyContactId = 5,
                ReplyToEmailAddress = "giving@crossroads.net",
                ToContactId = contact.ContactId,
                ToEmailAddress = contact.Email
            };

            comm.MergeData = new Dictionary<string, object>
            {
                {"Program_Name", program},
                {"Donation_Amount", donationAmount},
                {"Donation_Date", setupDate},
                {"Payment_Method", paymentType},
                {"Decline_Reason", EMAILREASON}
            };

            communicationService.SendMessage(comm);
        }
    }

}