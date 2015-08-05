using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class AuthenticationServiceImpl : IAuthenticationService
    {
        private PlatformServiceClient platformService;
        private IMinistryPlatformService ministryPlatformService;

        public AuthenticationServiceImpl(PlatformServiceClient platformService, IMinistryPlatformService ministryPlatformService)
        {
            this.platformService = platformService;
            this.ministryPlatformService = ministryPlatformService;
        }

        public Boolean ChangePassword(string token, string emailAddress, string firstName, string lastName, string password, string mobilephone)
        {
            using (new OperationContextScope((IClientChannel)platformService.InnerChannel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                try
                {
                    UserInfo user = new UserInfo();
                    user.EmailAddress = emailAddress;
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.NewPassword = password;
                    user.MobilePhone = mobilephone;
                    platformService.UpdateCurrentUserInfo(user);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// Change a users password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Boolean ChangePassword(string token, string newPassword)
        {
            try
            {
                var record = ministryPlatformService.GetRecordsDict(Convert.ToInt32(ConfigurationManager.AppSettings["ChangePassword"]), token).Single();
                record["Password"] = newPassword;
                ministryPlatformService.UpdateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["ChangePassword"]), record, token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Get ID of currently logged in user
        public int GetContactId(string token)
        {
            using (new OperationContextScope((IClientChannel)platformService.InnerChannel))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                var contactId = platformService.GetCurrentUserInfo();
                return contactId.ContactId;
            }
        }

        //Get Participant IDs of a contact
        public Participant GetParticipantRecord(string token)
        {
            try
            {
                using (new OperationContextScope(platformService.InnerChannel))
                {
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    var results =
                        ministryPlatformService.GetRecordsDict(
                            Convert.ToInt32(ConfigurationManager.AppSettings["MyParticipantRecords"]), token);
                    var participant = new Participant
                    {
                        ParticipantId = int.Parse(results.Single()["dp_RecordID"].ToString()),
                        EmailAddress = results.Single()["Email_Address"].ToString(),
                        PreferredName = results.Single()["Nickname"].ToString(),
                        DisplayName = results.Single()["Display_Name"].ToString()
                    };

                    return participant;
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains more than one element")
                {
                    throw new MultipleRecordsException("Multiple Participant records found! Only one participant allowed per Contact.");
                }
            }

            return null;
        }
        
    }
}
