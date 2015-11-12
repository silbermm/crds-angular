using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using crds_angular.Models.Crossroads;
using crds_angular.Models.MP;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class AccountService : MinistryPlatformBaseService, IAccountService
    {
        private IConfigurationWrapper _configurationWrapper;
        private ICommunicationService _communicationService;
        private IAuthenticationService _authenticationService;
        private ISubscriptionsService _subscriptionsService;

        public AccountService(IConfigurationWrapper configurationWrapper, ICommunicationService communicationService, IAuthenticationService authenticationService, ISubscriptionsService subscriptionService)
        {
            this._configurationWrapper = configurationWrapper;
            this._communicationService = communicationService;
            this._authenticationService = authenticationService;
            this._subscriptionsService = subscriptionService;
        }
        public bool ChangePassword(string token, string newPassword)
        {
            return _authenticationService.ChangePassword(token, newPassword);
        }

        //TODO: Put this logic in the Translation Layer?
        public bool SaveCommunicationPrefs(string token, AccountInfo accountInfo)
        {
            var contactId = _authenticationService.GetContactId(token);
            var contact = MinistryPlatformService.GetRecordDict(Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]), contactId, token);
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
                _communicationService.SetEmailSMSPreferences(token, emailsmsDict);
                _communicationService.SetMailPreferences(token, mailDict);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AccountInfo getAccountInfo(string token)
        {
            var contactId = _authenticationService.GetContactId(token);
            CommunicationPreferences contact = _communicationService.GetPreferences(token, contactId);
            var accountInfo = new AccountInfo
            {
                EmailNotifications = contact.Bulk_Email_Opt_Out,
                TextNotifications = contact.Bulk_SMS_Opt_Out,
                PaperlessStatements = contact.Bulk_Mail_Opt_Out
            };
            return accountInfo;


       }

        // TODO  Create a PageIdManager that wraps ConfigurationManager and does the convert for us.

        private static int CreateHouseholdRecord(User newUserData, string token){
            int recordId;

            Dictionary<string, object> householdDictionary = new Dictionary<string, object>();
            householdDictionary["Household_Name"] = newUserData.lastName;
            householdDictionary["Congregation_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Congregation_Default_ID"]);
            householdDictionary["Household_Source_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Household_Default_Source_ID"]);

            recordId = MinistryPlatformService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Households"]), householdDictionary, token);

            return recordId;
        }

        private static int CreateContactRecord(User newUserData, string token, int householdRecordID)
        {
            int recordId;

            Dictionary<string, object> contactDictionary = new Dictionary<string, object>();
            contactDictionary["First_Name"] = newUserData.firstName;
            contactDictionary["Last_Name"] = newUserData.lastName;
            contactDictionary["Email_Address"] = newUserData.email;
            contactDictionary["Company"] = false; // default
            contactDictionary["Display_Name"] = newUserData.lastName + ", " + newUserData.firstName;
            contactDictionary["Nickname"] = newUserData.firstName;
            contactDictionary["Household_Position_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Household_Position_Default_ID"]);
            contactDictionary["Household_ID"] = householdRecordID;

            recordId = MinistryPlatformService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Contacts"]), contactDictionary, token);

            return recordId;
        }

        private static int CreateContactHouseholdRecord(string token, int householdRecordID, int contactRecordID)
        {
            int recordId;

            Dictionary<string, object> contactHouseholdDictionary = new Dictionary<string, object>();
            contactHouseholdDictionary["Contact_ID"] = contactRecordID;
            contactHouseholdDictionary["Household_ID"] = householdRecordID;
            contactHouseholdDictionary["Household_Position_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Household_Position_Default_ID"]);
            contactHouseholdDictionary["Household_Type_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Household_Type_Default_ID"]);

            recordId = MinistryPlatformService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["ContactHouseholds"]), contactHouseholdDictionary, token);

            return recordId;
        }

        private static int CreateUserRecord(User newUserData, string token, int contactRecordID)
        {
            int recordId;

            Dictionary<string, object> userDictionary = new Dictionary<string, object>();
            userDictionary["First_Name"] = newUserData.firstName;
            userDictionary["Last_Name"] = newUserData.lastName;
            userDictionary["User_Email"] = newUserData.email;
            userDictionary["Password"] = newUserData.password;
            userDictionary["Company"] = false; // default
            userDictionary["Display_Name"] = userDictionary["First_Name"];
            userDictionary["Domain_ID"] = 1;
            userDictionary["User_Name"] = userDictionary["User_Email"];
            userDictionary["Contact_ID"] = contactRecordID;

            recordId = MinistryPlatformService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Users"]), userDictionary, token, true);

            return recordId;
        }

        private static int CreateUserRoleSubRecord(string token, int userRecordID)
        {
            int recordId;

            Dictionary<string, object> userRoleDictionary = new Dictionary<string, object>();
            userRoleDictionary["Role_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Role_Default_ID"]);
            recordId = MinistryPlatformService.CreateSubRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Users_Roles"]),userRecordID, userRoleDictionary, token);

            return recordId;
        }

        private static int CreateParticipantRecord(string token, int contactRecordID)
        {
            int recordId;

            Dictionary<string, object> participantDictionary = new Dictionary<string, object>();
            participantDictionary["Participant_Type_ID"] = Convert.ToInt32(ConfigurationManager.AppSettings["Participant_Type_Default_ID"]);

            participantDictionary["Participant_Start_Date"] = DateTime.Now;
            participantDictionary["Contact_Id"] = contactRecordID;

            recordId = MinistryPlatformService.CreateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Participants"]), participantDictionary, token);

            return recordId;
        }
        /*
         * Not needed as long as the triggers are in place on Ministry Platform
         */
        private static void UpdateContactRecord(int contactRecordID, int userRecordID, int participantRecordID, string token)
        {
            Dictionary<string, object> contactDictionary = new Dictionary<string, object>();
            contactDictionary["Contact_ID"] = contactRecordID;
            contactDictionary["User_account"] = userRecordID;
            contactDictionary["Participant_Record"] = participantRecordID;

            MinistryPlatformService.UpdateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Contacts"]), contactDictionary, token);
        }
        public Dictionary<string, string>RegisterPerson(User newUserData)
        {
            var apiUser = this._configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPassword = this._configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData = AuthenticationService.authenticate(apiUser, apiPassword);
            var token = authData["token"].ToString();

            int householdRecordID = CreateHouseholdRecord(newUserData,token);
            int contactRecordID = CreateContactRecord(newUserData,token,householdRecordID);
            int contactHouseholdRecordID = CreateContactHouseholdRecord(token,householdRecordID,contactRecordID);
            int userRecordID = CreateUserRecord(newUserData, token, contactRecordID);
            int userRoleRecordID = CreateUserRoleSubRecord(token, userRecordID);
            int participantRecordID = CreateParticipantRecord(token, contactRecordID);

            CreateNewUserSubscriptions(contactRecordID, token);

            // TODO Contingent on cascading delete via contact
            Dictionary<string, string> returnValues = new Dictionary<string, string>();
            returnValues["firstname"] = newUserData.firstName;
            returnValues["lastname"] = newUserData.lastName;
            returnValues["email"] = newUserData.email;
            returnValues["password"] = newUserData.password; //TODO Conisder encrypting the password on the user model
            return returnValues;
        }

        private void CreateNewUserSubscriptions(int contactRecordId, string token)
        {
            Dictionary<string, object> newSubscription = new Dictionary<string, object>();
            newSubscription["Publication_ID"] = this._configurationWrapper.GetConfigValue("KidsClubPublication");
            newSubscription["Unsubscribed"] = false;
            _subscriptionsService.SetSubscriptions(newSubscription, contactRecordId, token);
            newSubscription["Publication_ID"] = this._configurationWrapper.GetConfigValue("CrossroadsPublication");
            newSubscription["Unsubscribed"] = false;
            _subscriptionsService.SetSubscriptions(newSubscription, contactRecordId, token);
        }

    }
}