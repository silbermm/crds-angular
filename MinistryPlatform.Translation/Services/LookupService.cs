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

        public static Dictionary<string, object> Genders(string token)
        {
            return GetPageRecordService.GetRecordsDict(Convert.ToInt32(ConfigurationManager.AppSettings["Genders"]), token);
        }

        //public static string Genders(string token)
        //{
        //    var jarray = GetPageRecordService.GetRecords(Convert.ToInt32(ConfigurationManager.AppSettings["Genders"]), token);
        //    return jarray.ToString(); 
        //}


    }
}
