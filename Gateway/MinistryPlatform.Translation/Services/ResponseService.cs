using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class ResponseService : BaseService, IResponseService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformService _ministryPlatformService;

        public ResponseService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService)
            : base(authenticationService, configurationWrapper)
        {
            _authenticationService = authenticationService;
            _configurationWrapper = configurationWrapper;
            _ministryPlatformService = ministryPlatformService;
        }

        public List<MPServeReminders> GetServeReminders(String token)
        {
            var pageId = AppSetting("SignupToServeReminders");
            var dict = _ministryPlatformService.GetPageViewRecords(pageId, token, "", "", 0);

            return dict.Select(rec =>
            {
                var mp = new MPServeReminders()
                {
                    Event_End_Date = (DateTime) rec["Event_End_Date"],
                    Event_Start_Date = (DateTime) rec["Event_Start_Date"],
                    Event_Title = (String) rec["Event_Title"],
                    Opportunity_Contact_Id = (int) rec["Opportunity_Contact_ID"],
                    Opportunity_Email_Address = (String) rec["Opportunity_Contact_Email_Address"],
                    Opportunity_Title = (String) rec["Opportunity_Title"],
                    Shift_End = (TimeSpan) rec["Shift_End"],
                    Shift_Start = (TimeSpan) rec["Shift_Start"],
                    Signedup_Contact_Id = (int) rec["Contact_ID"],
                    Signedup_Email_Address = (String) rec["Email_Address"]
                };
                if (rec["Communication_ID"] != null)
                {
                    mp.Template_Id = (int) rec["Communication_ID"];
                }
                return mp;
            }).ToList();
        }

        
    }
}
