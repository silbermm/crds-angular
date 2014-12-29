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


namespace crds_angular.Services
{
    public class TranslationService
    {
        
        private static string GetRecord(int pageId, int recordId, string token)
        {
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(pageId, recordId, token);
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

        public static string GetMySkills(int personId, string token)
        {
            return null;
        }

        public static string GetLookup(int pageId, string token)
        {
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecords(pageId, token);
            return data.ToString();
        }

        public static Dictionary<string, object> GetLookupDict(int pageId, string token)
        {
            return MinistryPlatform.Translation.Services.GetPageRecordService.GetRecordsDict(pageId, token);
        }

        public static string GetMyAddress(int addressId, string token)
        {
            return GetRecord(468, addressId, token);
        }

        
        public static string Login(string username, string password)
        {
            return MinistryPlatform.Translation.AuthenticationService.authenticate(username, password);   
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
