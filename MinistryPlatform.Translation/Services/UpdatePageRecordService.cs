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

        //Platform Service - UpdatePageRecord
        //public static JArray Post(int id, string postData)
        //{
        //    var userToken = GetUserToken("tmaddox", "crds1234");

        //    var content = new FormUrlEncodedContent(postData);

        //    var httpClient = new HttpClient();
        //    var url = string.Format("{0}/UpdatePageRecord?pageId={1}", PlatformServiceUri, id);
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        //    var post = httpClient.PostAsync(url, content);
        //    var result = post.Result.Content.ReadAsStringAsync().Result;


        //}

//        public static bool Update()
//        {

//            var userToken = GetUserToken("tmaddox", "crds1234");

//            var jObject = new JObject();
//            JObject person = JObject.Parse(@"{
//                'dp_RecordID': 618590,
//                'dp_RecordName': 'Maddox,  Tony',
//                'dp_Selected': 0,
//                'dp_FileID': null,
//                'dp_RecordStatus': 0,
//                'Display_Name': 'Maddox,  Tony',
//                'Nickname': ' Tony',
//                'First_Name': 'Tony',
//                'Last_Name': 'Maddox',
//                'Contact_Status': 'Active',
//                'Home_Phone': null,
//                'Mobile_Phone': '859-803-2490',
//                'Address_Line_1': null,
//                'City': null,
//                'State': null,
//                'Postal_Code': null,
//                'Email_Address': 'tony.maddox@gmail.com',
//                'Date_of_Birth': '08/06/2010',
//                'Gender': 'Female',
//                'Marital_Status': 'Single',
//                'Congregation_Name': 'Not site specific',
//                'Household_Name': 'Maddox',
//                'Household_Position': null
//              }");

//            //var body = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com", MyCredentials, MyCredentials2);
//            //HttpContent cont = new HttpContent();
//            StringContent content = new StringContent(person.ToString());
//            //HttpResponseMessage aResponse = await client.PostAsync(new Uri("https://login.live.com/accesstoken.srf"), theContent);


//            var httpClient = new HttpClient();
//            var url = string.Format("{0}/UpdatePageRecord?pageId=292", PlatformServiceUri);
//            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
//            var post = httpClient.PostAsync(url, content);
//            var result = post.Result.Content.ReadAsStringAsync().Result;

//            return true;
//        }

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