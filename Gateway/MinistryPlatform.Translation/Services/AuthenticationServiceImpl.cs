using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Crossroads.Utilities.Services;
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

        //get token using logged in user's credentials
        public Dictionary<string, object> Authenticate(string username, string password)
        {
            var userCredentials =
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"username", username},
                    {"password", password},
                    {"client_id", "client"},
                    {"client_secret", "secret"},
                    {"grant_type", "password"}
                });
            var client = new HttpClient();
            var tokenUrl = ConfigurationManager.AppSettings["TokenURL"];
            var message = client.PostAsync(tokenUrl, userCredentials);
            try
            {
                var result = message.Result.Content.ReadAsStringAsync().Result;
                var obj = JObject.Parse(result);
                var token = (string) obj["access_token"];
                var exp = (string) obj["expires_in"];
                //ignoring refreshToken for now
                var refreshToken = (string) obj["refresh_token"];
                var authData = new Dictionary<string, object>
                {
                    {"token", token},
                    {"exp", exp}
                };
                return authData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Boolean ChangePassword(string token, string emailAddress, string firstName, string lastName, string password, string mobilephone)
        {
            using (new OperationContextScope((IClientChannel) platformService.InnerChannel))
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
                catch (Exception)
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
            return ministryPlatformService.GetContactInfo(token).ContactId;
        }
    }
}