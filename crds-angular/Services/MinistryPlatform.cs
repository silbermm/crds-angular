using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace crds_angular.Services
{
    public class Product
    {
        
    }
    public class MinistryPlatform
    {
        private const string TranslationUri = "http://my.crossroads.net/translation/api/";

        public static string GetMyProfile()
        {
            var pageId = 455;
            var url = string.Format("{0}{1}{2}", TranslationUri, "getpagerecords/", pageId);

            return FetchData(url);
        }

        public static string GetLookup(int pageId)
        {
            var url = string.Format("{0}{1}{2}", TranslationUri, "GetPageLookupRecords/", pageId);

            return FetchData(url);
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