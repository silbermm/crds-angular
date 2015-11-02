using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using MinistryPlatform.Translation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Collections.Generic;
using System;
using MinistryPlatform.Translation.Services;
using System.Configuration;
using MinistryPlatform.Translation.Helpers;
using MinistryPlatform.Translation.PlatformService;


namespace crds_angular.Services
{
    public class TranslationService
    {
        
        private static string GetRecord(int pageId, int recordId, string token)
        {
            var data = MinistryPlatformService.GetRecord(pageId, recordId, token);
            return data.ToString();
        }

        public static string GetMyProfile(int personId, string token)
        {
            return GetRecord(455,personId,token);
        }

        public static string GetMyHousehold(int householdId, string token)
        {
            return GetRecord(465, householdId, token);         
        }

        public static string GetStates(string token)
        {
            var mpObject = MinistryPlatformService.GetRecords(Convert.ToInt32(ConfigurationManager.AppSettings["States"]), token);
            var json = MPFormatConversion.MPFormatToJson(mpObject);
            return json.ToString();
        }


        public static List<Dictionary<string, object>> GetLookupDict(int pageId, string token)
        {
            return MinistryPlatformService.GetRecordsDict(pageId, token);
        }

        public static string GetMyAddress(int addressId, string token)
        {
            return GetRecord(468, addressId, token);
        }

        
        public static Dictionary<string, object> Login(string username, string password)
        {
            return (AuthenticationService.authenticate(username, password));
        }

        public static dynamic DecodeJson(string json)
        {
            var obj = System.Web.Helpers.Json.Decode(json);
            if (obj.GetType() == typeof(System.Web.Helpers.DynamicJsonArray))
            {
                dynamic[] array = obj;
                if (array.Length == 1)
                {
                    return array[0];
                }
            }
            //should probably throw error here
            return null;
        }

    }

}
