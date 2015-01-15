using crds_angular.Models;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Translation;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.PlatformService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using MinistryPlatform.Translation.Models;
using crds_angular.Models.MP;
using System.Configuration;

namespace crds_angular.Services
{
    public class AccountService : MinistryPlatformBaseService
    {
        public bool ChangePassword(string token, string newPassword)
        {
            return AuthenticationService.ChangePassword(token, newPassword);
        }

        //TODO: Put this logic in the Translation Layer?
        public bool SaveCommunicationPrefs(string token, AccountInfo accountInfo)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            var contact = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecordDict(Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]), contactId, token);
            try
            {                
                var emailsmsDict = getDictionary(new EmailSMSOptOut
                    {
                        Contact_ID = contactId,
                        Bulk_Email_Opt_Out = accountInfo.EmailNotifications,
                        Bulk_SMS_Opt_Out = accountInfo.TextNotifications
                    });
                var mailDict = getDictionary(new MailOptOut 
                {
                    Household_ID = (int)contact["Household_ID"],
                    Bulk_Mail_Opt_Out = accountInfo.PaperlessStatements 
                });
                CommunicationService.SetEmailSMSPreferences(token, emailsmsDict);
                CommunicationService.SetMailPreferences(token, mailDict);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AccountInfo getAccountInfo(string token)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            CommunicationPreferences contact = CommunicationService.GetPreferences(token, contactId);
            var accountInfo = new AccountInfo
            {
                EmailNotifications = contact.Bulk_Email_Opt_Out,
                TextNotifications = contact.Bulk_SMS_Opt_Out,
                PaperlessStatements = contact.Bulk_Mail_Opt_Out
            };
            return accountInfo;


       }
        public static void RegisterPerson()
        {
            string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]);

            Dictionary<string, object> contactDictionary = new Dictionary<string, object>();
            contactDictionary["First_Name"]="Julius";
            contactDictionary["Last_Name"]="Caesar";
            contactDictionary["Email_Address"] = "test@test.com";
            contactDictionary["Company"] = false; // default
            contactDictionary["Display_Name"] = "Julius";

            int newRecordId = MinistryPlatform.Translation.Services.CreatePageRecordService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Contacts"]), contactDictionary, token);

            Dictionary<string, object> userDictionary = new Dictionary<string, object>();
            userDictionary["First_Name"] = "Julius";
            userDictionary["Last_Name"] = "Caesar";
            userDictionary["User_Email"] = "usertest@test.com";
            userDictionary["Company"] = false; // default
            userDictionary["Display_Name"] = "Julius";
            userDictionary["Contact_Id"] = newRecordId;
            userDictionary["Domain_Id"] = 1;
            userDictionary["User_Name"] = "usertest@test.com";

            MinistryPlatform.Translation.Services.CreatePageRecordService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Users"]), userDictionary, token);
        }

    }
}