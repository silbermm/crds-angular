using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class AuthenticationService
    {
       //get token using logged in user's credentials
        public static Dictionary<string, object> authenticate(string username, string password)
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
                var token = (string)obj["access_token"];
                var exp = (string)obj["expires_in"];
                var refreshToken = (string)obj["refresh_token"];
                var authData = new Dictionary<string, object>
                {
                    {"token", token},
                    {"exp", exp},
                    {"refreshToken", refreshToken}
                };
                return authData;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Dictionary<string, object> RefreshToken(string refreshToken)
        {
            var userCredentials =
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"refresh_token", refreshToken},
                    {"client_id", "client"},
                    {"client_secret", "secret"},
                    {"grant_type", "refresh_token"}
                });
            var client = new HttpClient();
            var tokenUrl = ConfigurationManager.AppSettings["TokenURL"];
            var message = client.PostAsync(tokenUrl, userCredentials);
            try
            {
                var result = message.Result.Content.ReadAsStringAsync().Result;
                var obj = JObject.Parse(result);
                var token = (string)obj["access_token"];
                var exp = (string)obj["expires_in"];
                var refreshTokenResponse = (string)obj["refresh_token"];
                var authData = new Dictionary<string, object>
                {
                    {"token", token},
                    {"exp", exp},
                    {"refreshToken", refreshTokenResponse}
                };
                return authData;
            }
            catch (Exception)
            {
                return null;
            }
        }
   }
}
