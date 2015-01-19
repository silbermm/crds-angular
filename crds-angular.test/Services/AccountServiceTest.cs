using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation;

namespace crds_angular.test.Services
{
    [TestClass]
    public class testclass
    {
        //[TestMethod] - commented out because we can't yet delete contacts with users.
        //TODO When DeleteRecord is available cleanup and delete this test record
        public void shouldRegisterPerson(User newUserData)
        {
            

           

            Dictionary<int, int> newIDs = crds_angular.Services.AccountService.RegisterPerson(newUserData);
            foreach (KeyValuePair<int, int> entry in newIDs)
            {
                DeletePageRecordService.DeleteRecord(entry.Key, entry.Value, null, token);
            }
        }
    }
}