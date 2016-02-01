using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class MinistryPlatform
    {
        //Platform Service - GetPageRecords
        public static JArray GetMyPageRecords(int pageId)
        {
            var userToken = GetUserToken("tmaddox", "crds1234");

            var httpClient = new HttpClient();
            var url = string.Format("{0}/GetPageRecords?pageId={1}", PlatformServiceUri, pageId);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            var post = httpClient.GetAsync(url);
            var result = post.Result.Content.ReadAsStringAsync().Result;
            PlatformService.SelectQueryResult mpObject;

            try
            {
                mpObject = JsonConvert.DeserializeObject<PlatformService.SelectQueryResult>(result);
            }
            catch
            {
                //TO-DO: add proper error handler
                return null;
            }

            //map the reponse into name/value pairs
            var j = new JArray();
            foreach (var dataItem in mpObject.Data)
            {
                var jObject = new JObject();
                foreach (var mpField in mpObject.Fields)
                {
                    var jProperty = new JProperty(mpField.Name, dataItem[mpField.Index]);
                    jObject.Add(jProperty);
                }
                j.Add(jObject);
            }

            return j;
        }

        //Platform Service - GetPageRecord
        public static JArray GetMyPageRecord(int pageId, int recordId)
        {
            var userToken = GetUserToken("tmaddox", "crds1234");

            var httpClient = new HttpClient();
            var url = string.Format("{0}/GetPageRecord?pageId={1}&recordId={2}", PlatformServiceUri, pageId, recordId);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
            var post = httpClient.GetAsync(url);
            var result = post.Result.Content.ReadAsStringAsync().Result;
            PlatformService.SelectQueryResult mpObject;

            try
            {
                mpObject = JsonConvert.DeserializeObject<PlatformService.SelectQueryResult>(result);

                //map the reponse into name/value pairs
                var j = new JArray();
                foreach (var dataItem in mpObject.Data)
                {
                    var jObject = new JObject();
                    foreach (var mpField in mpObject.Fields)
                    {
                        var jProperty = new JProperty(mpField.Name, dataItem[mpField.Index]);
                        jObject.Add(jProperty);
                    }
                    j.Add(jObject);
                }
                return j;
            }
            catch
            {
                //TO-DO: add proper error handler
                return null;
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