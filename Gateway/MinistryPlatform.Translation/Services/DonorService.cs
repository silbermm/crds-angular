using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
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

        
        public int CreateDonorRecord(int contactId)
        {
            //this assumes that you do not already have a donor record - new giver

            var values = new Dictionary<string, object>
            {
                {"Contact_Id", contactId},
                {"Statement_Frequency_ID", "1"},//default to quarterly
                {"Statement_Type_ID", "1"},     //default to individual
                {"Statement_Method_ID", '2'},   //default to email/online
                {"Setup_Date", DateTime.Now}    //defaukl to current date/time
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
        
    }
}