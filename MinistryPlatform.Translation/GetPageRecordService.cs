using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using MinistryPlatform.Translation.Helpers;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class GetPageRecordService
    {
        public static JArray GetRecord(int pageId, int recordId)
        {
            var userToken = GetUserToken("tmaddox", "crds1234");

            var platformServiceClient = new PlatformService.PlatformServiceClient();
            PlatformService.SelectQueryResult result;

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + userToken);

                result = platformServiceClient.GetPageRecord(pageId, recordId, false);

            }
            return MPFormatConversion.MPFormatToJson(result);

        }


        //Platform Service - GetPageRecords
        public static JArray GetRecords(int id)
        {
            var userToken = GetUserToken("tmaddox", "crds1234");

            var platformServiceClient = new PlatformService.PlatformServiceClient();
            PlatformService.SelectQueryResult result;

            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
            {
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + userToken);
                result = platformServiceClient.GetPageRecords(id, "", "", 0);

            }
            return MPFormatConversion.MPFormatToJson(result);

            //var httpClient = new HttpClient();
            //var url = string.Format("{0}/GetPageRecords?pageId={1}", PlatformServiceUri, id);

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            //var post = httpClient.GetAsync(url);
            //var result = post.Result.Content.ReadAsStringAsync().Result;

            //MPFormatConversion.JsonToMPFormat(MPFormatConversion.MPFormatToJson(result));

            //return MPFormatConversion.MPFormatToJson(result);
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