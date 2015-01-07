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
            return GetPageRecordService.GetLookupRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Emails"]), email, token);
        }

        public static List<Dictionary<string, object>> Genders(string token)
        {
            return GetPageRecordService.GetLookupRecords(Convert.ToInt32(ConfigurationManager.AppSettings["Genders"]), token);
        }

    }
}
