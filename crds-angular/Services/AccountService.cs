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
        public static Dictionary<int, int> RegisterPerson(string token, Dictionary<string, object> contactDictionary, Dictionary<string, object> userDictionary, Dictionary<string, object> participantDictionary)
        {
            int contactsPageID = Convert.ToInt32(ConfigurationManager.AppSettings["Contacts"]);
            int usersPageID = Convert.ToInt32(ConfigurationManager.AppSettings["Users"]);
            int participantsPageID = Convert.ToInt32(ConfigurationManager.AppSettings["Participants"]);

            int contactRecordID = MinistryPlatform.Translation.Services.CreatePageRecordService.CreateRecord(contactsPageID, contactDictionary, token);
            userDictionary["Contact_Id"] = contactRecordID;
            participantDictionary["Contact_Id"] = contactRecordID;
            int userRecordID = MinistryPlatform.Translation.Services.CreatePageRecordService.CreateRecord(usersPageID, userDictionary, token);
            int participantRecordID = MinistryPlatform.Translation.Services.CreatePageRecordService.CreateRecord(participantsPageID, participantDictionary, token);
            Dictionary<int, int> returnValues = new Dictionary<int, int>();
            returnValues[contactsPageID] = contactRecordID;
            returnValues[participantsPageID] = participantRecordID;
            returnValues[usersPageID] = userRecordID;
            return returnValues;
        }

    }
}