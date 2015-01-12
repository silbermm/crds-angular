using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class UpdatePageRecordService
    {
       

        public static void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token)
        {
            try
            {
                var platformServiceClient = new PlatformService.PlatformServiceClient();
                PlatformService.SelectQueryResult result;

                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    platformServiceClient.UpdatePageRecord(pageId, dictionary, false);                    
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

  
        private const string PlatformServiceUri = "https://my.crossroads.net/ministryplatformapi/PlatformService.svc";

        //get token using logged in user's credentials
        private static string GetUserToken(string username, string password)
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
            var result = message.Result.Content.ReadAsStringAsync().Result;

            var obj = JObject.Parse(result);
            var token = (string)obj["access_token"];
            //ignorning refreshToken for now
            var refreshToken = (string)obj["refresh_token"];

            return token;
        }
    }
}