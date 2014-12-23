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

namespace crds_angular.Services
{
    public static class AccountService
    {
        public static bool ChangePassword(string token, string newPassword)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            Dictionary<string, object> contact = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecordDict(455, contactId, token); 
            return AuthenticationService.ChangePassword(token, 
                (string)contact["Email_Address"], 
                (string)contact["First_Name"], 
                (string)contact["Last_Name"],
                newPassword,
                (string)contact["Mobile_Phone"]
            );
        }

        public static Models.Json.Communication SaveCommunicationPrefs(string token, Models.Json.Communication communicationPrefs)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            return communicationPrefs;
        }

        public static AccountInfo getAccountInfo(string token)
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

    }
}