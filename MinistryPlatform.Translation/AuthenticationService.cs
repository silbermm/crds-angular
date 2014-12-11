using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation
{
    public class AuthenticationService
    {

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
    }
}
