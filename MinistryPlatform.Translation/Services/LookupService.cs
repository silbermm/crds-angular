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
            var emails = GetPageRecordService.GetLookupRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Emails"]), email, token);
            return emails;
        }


    }
}
