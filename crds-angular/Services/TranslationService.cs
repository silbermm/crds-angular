using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(pageId, personId);
            return data.ToString();             
        }

        public static string GetMyHousehold(int householdId)
        {
            var pageId = 465;
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(pageId, householdId);
            return data.ToString();            
        }

        public static string GetLookup(int pageId)
        {
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecords(pageId);
            return data.ToString();
        }

        public static string GetMyAddress(int addressId)
        {
            var pageId = 468;
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(pageId, addressId);
            return data.ToString();
        }
    }
}
