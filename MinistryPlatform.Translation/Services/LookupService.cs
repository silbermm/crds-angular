using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Services
{
    public class LookupService
    {


        public static Dictionary<string,object> EmailSearch(String email, string token)
        {
            return MinistryPlatformService.GetLookupRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Emails"]), email, token);
        }

        public static List<Dictionary<string, object>> Genders(string token)
        {
            return MinistryPlatformService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["Genders"]), token);
        }

        public static List<Dictionary<string,object>> MaritalStatus(string token)
        {
            return MinistryPlatformService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["MaritalStatus"]), token);
        }

        public static List<Dictionary<string, object>> ServiceProviders(string token)
        {
            return MinistryPlatformService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["ServiceProviders"]), token);
        }

        public static List<Dictionary<string, object>> States(string token)
        {
            return MinistryPlatformService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["States"]), token);
        }

        public static List<Dictionary<string, object>> Countries(string token)
        {
            return MinistryPlatformService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["Countries"]), token);
        }

        public static List<Dictionary<string, object>> CrossroadsLocations(string token)
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["CrossroadsLocations"]);
            if (pageId == 0) { throw new Exception("page id not found!"); }


            return MinistryPlatformService.GetLookupRecords(pageId, token);

        }

    }
}
