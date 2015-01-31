using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MinistryPlatform.Translation.Models;

namespace MinistryPlatform.Translation.Services
{
    public class CommunicationService
    {

        public static CommunicationPreferences GetPreferences(String token, int userId)
        {
            int pNum = Convert.ToInt32( ConfigurationManager.AppSettings["MyContact"]);
            int hNum = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
            var profile = MinistryPlatformService.GetRecordDict(pNum, userId, token);
            var household = MinistryPlatformService.GetRecordDict(hNum, (int)profile["Household_ID"], token);
            return new CommunicationPreferences
            {
                Bulk_Email_Opt_Out = (bool)profile["Bulk_Email_Opt_Out"],
                Bulk_Mail_Opt_Out = (bool)household["Bulk_Mail_Opt_Out"],
                Bulk_SMS_Opt_Out = (bool)profile["Bulk_SMS_Opt_Out"]
            };
            //return MinistryPlatformService.GetRecordsDict(Convert.ToInt32(pageNumber), token);
        }

        public static bool SetEmailSMSPreferences(String token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public static bool SetMailPreferences(string token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
