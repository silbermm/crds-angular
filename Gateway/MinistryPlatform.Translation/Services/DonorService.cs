using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DonorService : BaseService, IEventService
    {
        private readonly log4net.ILog logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int DonorPageId = Convert.ToInt32(AppSettings("Donors"));
        
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
                "Contact_Id" = contactId,
                "Statement_Frequency_ID" = "1", //quarterly
                "Statement_Type_ID" = "1", //individual
                "Statement_Method_ID" = '2', //email/online
                "Setup_Date" = DateTime
            }
        }
        
    }
}