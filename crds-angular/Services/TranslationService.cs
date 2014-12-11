using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Threading.Tasks;

namespace crds_angular.Services
{
    public class Product
    {
        
    }

    public class TranslationService
    {
        private const string TranslationUri = "http://my.crossroads.net/translation/api/";

        public static JArray GetMyProfile()
        {
            var pageId = 455;
            var personId = 618590;
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(pageId, personId);
            return data;             
        }

        public static JArray GetLookup(int pageId)
        {
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecords(pageId);
            return data;
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
