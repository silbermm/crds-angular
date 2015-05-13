using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.PlatformService;
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
        public const string DONOR_STRIPE_CUST_ID = "Stripe_Customer_ID";

        private IMinistryPlatformService ministryPlatformService;

        public DonorService(IMinistryPlatformService ministryPlatformService)
        {
            this.ministryPlatformService = ministryPlatformService;
        }


        public int CreateDonorRecord(int contactId, string stripeCustomerId, DateTime setupTime,
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
                {"Stripe_Customer_ID", stripeCustomerId}
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

        public int CreateDonationAndDistributionRecord(int donationAmt, int donorId, string programId, string charge_id, DateTime setupTime)
        {
            var donationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Payment_Type_ID", 4}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupTime},
                {"Transaction_code", charge_id}
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

        public Donor GetDonorRecord(int contactId)
        {
            Donor donor;
            try
            {
                var searchStr = contactId.ToString() + ",";
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (ministryPlatformService.GetPageViewRecords("DonorByContactId", apiToken, searchStr, "")));
                if (records.Count > 0)
                {
                    var record = records.First();
                    donor = new Donor()
                    {
                        DonorId = record.ToInt(DONOR_RECORD_ID),
                        StripeCustomerId = record.ToString(DONOR_STRIPE_CUST_ID),
                        ContactId = record.ToInt("Contact_ID")
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
                    string.Format("GetDonorRecord failed.  Contact Id: {0}", contactId), ex);
            }

            return donor;

        }
        public Donor GetPossibleGuestDonorContact(string email)
        {
            Donor donor;
            try
            {
                if (email.Equals(String.Empty))
                {
                    return null;
                }
                var searchStr =  "," + email;
                var records =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken => (ministryPlatformService.GetPageViewRecords("PossibleGuestDonorContact", apiToken, searchStr, "")));
                if (records.Count > 0)
                {
                    var record = records.First();
                    donor = new Donor()
                    {
                        DonorId = record.ToInt(DONOR_RECORD_ID),
                        StripeCustomerId = record.ToString(DONOR_STRIPE_CUST_ID),
                        ContactId = record.ToInt("Contact_ID"),
                        Email = record.ToString("Email_Address")
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
                { "dp_RecordID", donorId },
                { DONOR_STRIPE_CUST_ID, paymentProcessorCustomerId },
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
    }
}