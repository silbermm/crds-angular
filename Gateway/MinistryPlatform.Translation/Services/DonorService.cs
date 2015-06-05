using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly int donationDistributionPageId = Convert.ToInt32(AppSettings("Distributions"));

        public const string DONOR_RECORD_ID = "Donor_Record";
        public const string DONOR_PROCESSOR_ID = "Processor_ID";

        private IMinistryPlatformService ministryPlatformService;

        public DonorService(IMinistryPlatformService ministryPlatformService)
        {
            this.ministryPlatformService = ministryPlatformService;
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

        public int CreateDonationAndDistributionRecord(int donationAmt, int donorId, string programId, string charge_id, DateTime setupTime, bool registeredDonor)
        {
            var donationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Payment_Type_ID", 4}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupTime},
                {"Transaction_code", charge_id},
                {"Registered_Donor", registeredDonor}
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
                        RegisteredUser = true
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
                ministryPlatformService.UpdateRecord(donorPageId, parms, apiLogin());
            }
            catch (Exception e)
            {
                throw new ApplicationException(
                    string.Format("UpdatePaymentProcessorCustomerId failed. donorId: {0}, paymentProcessorCustomerId: {1}", donorId, paymentProcessorCustomerId), e);
            }

            return (donorId);
        }


        public void SendConfirmationEmail()
        {

            var templateId = AppSetting("OneTimeGuestGiveTemplate");

            var template = CommunicationService.GetTemplate(templateId);

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                //FromContactId = opp.GroupContactId,
                //FromEmailAddress = fromEmail.Email_Address,
                //ReplyContactId = opp.GroupContactId,
                //ReplyToEmailAddress = fromEmail.Email_Address,
                //ToContactId = contactId,
                //ToEmailAddress = toEmail
            };
        }
    }

}