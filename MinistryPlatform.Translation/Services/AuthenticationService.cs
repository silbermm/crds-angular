using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation
{
    public class AuthenticationService
    {
        public static Boolean ChangePassword(string token, string emailAddress, string firstName, string lastName, string password, string mobilephone)
        {
            var platformService = new PlatformService.PlatformServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
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
        public static Boolean ChangePassword(string token, string newPassword)
        {
            try
            {
                var record = GetPageRecordService.GetRecordsDict(Convert.ToInt32(ConfigurationManager.AppSettings["ChangePassword"]), token);
                record["Password"] = newPassword;
                UpdatePageRecordService.UpdateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["ChangePassword"]), record, token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //get token using logged in user's credentials
        public static String authenticate(string username, string password)
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
            var message = client.PostAsync("https://my.crossroads.net/ministryplatform/oauth/token", userCredentials);
            try
            {
                var result = message.Result.Content.ReadAsStringAsync().Result;

                var obj = JObject.Parse(result);
                var token = (string)obj["access_token"];
                //ignorning refreshToken for now
                var refreshToken = (string)obj["refresh_token"];
                return token;
            }
            catch
            {
                return null;
            }
        }

        //Get ID of currently logged in user
        public static int GetContactId(string token)
        {
            var platformService = new PlatformService.PlatformServiceClient();
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformService.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                var contactId = platformService.GetCurrentUserInfo();
                return contactId.ContactId;
            }
        }
    }
}
