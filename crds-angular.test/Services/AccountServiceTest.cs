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
        public void shouldRegisterPerson()
        {
            string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]);

            Dictionary<string, object> contactDictionary = new Dictionary<string, object>();
            contactDictionary["First_Name"] = "Julius";
            contactDictionary["Last_Name"] = "Caesar";
            contactDictionary["Email_Address"] = "test@test.com";
            contactDictionary["Company"] = false; // default
            contactDictionary["Display_Name"] = "Julius";

            Dictionary<string, object> userDictionary = new Dictionary<string, object>();
            userDictionary["First_Name"] = "Julius";
            userDictionary["Last_Name"] = "Caesar";
            userDictionary["User_Email"] = "usertest11@test.com";
            userDictionary["Company"] = false; // default
            userDictionary["Display_Name"] = "Julius";
            userDictionary["Domain_Id"] = 1;
            userDictionary["User_Name"] = "usertest11@test.com";

            Dictionary<string, object> participantDictionary = new Dictionary<string, object>();
            participantDictionary["Participant_Type_ID"] = "1"; //TODO Use the correct Participant type ID TBD
            participantDictionary["Participant_Start_Date"] = new DateTime(2015, 1, 17);

            Dictionary<int, int> newIDs = crds_angular.Services.AccountService.RegisterPerson(token, contactDictionary, userDictionary, participantDictionary);
            foreach (KeyValuePair<int, int> entry in newIDs)
            {
                DeletePageRecordService.DeleteRecord(entry.Key, entry.Value, null, token);
            }
        }
    }
}