using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Threading.Tasks;
using MinistryPlatform.Translation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace crds_angular.Services
{
    public class Product
    {
        
    }
    public class TranslationService
    {
        private const string TranslationUri = "http://my.crossroads.net/translation/api/";

        public static string GetMyProfile()
        {
            var pageId = 455;
            var personId = 618590;
            var data = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, personId);
            return data.ToString();             
        }

        public static string GetLookup(int pageId)
        {
            return MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecords(pageId).ToString();
        }

        private static string FetchData(string url)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("url");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(url);
                var result = response.Result.Content.ReadAsStringAsync().Result;

                return result;
            }
        }
    }
}