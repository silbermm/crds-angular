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
        
        private IMinistryPlatformService ministryPlatformService;

        public DonorService(IMinistryPlatformService ministryPlatformService)
        {
            this.ministryPlatformService = ministryPlatformService;
        }

        
        public int CreateDonorRecord(int contactId, string stripeCustomerId)
        {
            //this assumes that you do not already have a donor record - new giver

            var values = new Dictionary<string, object>
            {
                {"Contact_ID", contactId},
                {"Statement_Frequency_ID", "1"},//default to quarterly
                {"Statement_Type_ID", "1"},     //default to individual
                {"Statement_Method_ID", 2},   //default to email/online
                {"Setup_Date", DateTime.Now},    //default to current date/time
                {"Stripe_Customer_ID", stripeCustomerId}
            }; 

            int donorId;

            try
            {
              donorId = WithApiLogin<int>(apiToken => (ministryPlatformService.CreateRecord(donorPageId, values, apiToken, true)));
            }
            catch (Exception e)
            {
               throw new ApplicationException(string.Format("CreateDonorRecord failed.  Contact Id: {0}", contactId), e);
            }
            return donorId;
        
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
                var record = records.Single();
                donor = new Donor()
                {
                    DonorId = record.ToInt("dp_RecordID"),
                    StripeCustomerId = record.ToString("Stripe Customer ID"),
                    ContactId = record.ToInt("Contact ID"),
                    StatementFreq = record.ToString("Statement Frequency"),
                    StatementType = record.ToString("Statement Type"),
                    StatementMethod = record.ToString("Statement Method"),
                    SetupDate = record.ToDate("Setup Date")
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("CreateDonorRecord failed.  Contact Id: {0}", contactId), ex);
            }

            return donor;

        }
    }
}