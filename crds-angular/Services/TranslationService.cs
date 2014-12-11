using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
<<<<<<< HEAD
using MinistryPlatform.Translation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
=======
//using System.Threading.Tasks;
>>>>>>> 4e2d621c72ee6bf0133baa1468960c6119ffe5ac

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

<<<<<<< HEAD
        public static string GetMyHousehold(int householdId)
        {
            var pageId = 465;
            var data = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, householdId);
            return data.ToString();            
        }

        public static string GetLookup(int pageId)
=======
        public static JArray GetLookup(int pageId)
>>>>>>> 4e2d621c72ee6bf0133baa1468960c6119ffe5ac
        {
            var data = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecords(pageId);
            return data;
        }

        public static string GetMyAddress(int addressId)
        {
            var pageId = 468;
            var data = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, addressId);
            return data.ToString();
        }
    }
}