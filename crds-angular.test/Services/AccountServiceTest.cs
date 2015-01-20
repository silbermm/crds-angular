using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation;
using crds_angular.Models.Crossroads;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    public class testclass
    {
        [Test] //- commented out because we can't yet delete contacts with users.
        public void shouldRegisterPerson()
        {
            User newUserData = new User();
            newUserData.firstName = "Automated";
            newUserData.lastName = "Test";
            newUserData.email = "auto02@crossroads.net";
            newUserData.password = "password";

            string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]);

            Dictionary<int, int> newIDs = crds_angular.Services.AccountService.RegisterPerson(newUserData);

            Dictionary<string, object> clearContactIdDict = new Dictionary<string,object>();
            clearContactIdDict["User_Account"] = null;
            clearContactIdDict["Participant_Record"] = null;
            clearContactIdDict["Contact_ID"] = newIDs.First().Value;
            UpdatePageRecordService.UpdateRecord(newIDs.First().Key,clearContactIdDict,token);

            foreach (KeyValuePair<int, int> entry in newIDs)
            {
                DeletePageRecordService.DeleteRecord(entry.Key, entry.Value, null, token);
            }
        }
    }
}