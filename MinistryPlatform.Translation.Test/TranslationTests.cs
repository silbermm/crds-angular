using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.PlatformService;
using System.Configuration;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class TranslationTests
    {

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string FIRSTNAME = "Test";
        private const string NEW_PASSWORD = "changemeagain";

        [Test]
        public void ShouldFailLogin()
        {
            var obj = AuthenticationService.authenticate("", "");
            Assert.IsNull(obj, "When not authenticated this should be null");
        }

        [Test]
        public void ShouldLogin()
        {
            var obj = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(obj, "When authenticated this should be a JObject");
        }

        [Test]
        public void ShouldReturnContactId()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var obj = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(obj, "Contact ID shouldn't be null");
        }

        [Test]
        public void ShouldChangePassword()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var changed = AuthenticationService.ChangePassword(token, NEW_PASSWORD);
            Assert.IsTrue(changed);
            var obj = AuthenticationService.authenticate(USERNAME, NEW_PASSWORD);
            Assert.IsNotNull(obj);
            var changedAgain = AuthenticationService.ChangePassword(token, PASSWORD);
            Assert.IsTrue(changedAgain);
        }

        [Test]
        public void Andy()
        {
            var pageId = 474;
            var token = AuthenticationService.authenticate("tmaddox", "crds1234");
            var recordId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null"); ;

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Contact_ID", recordId);
            dictionary.Add("Nickname", "Canterbury");

            MinistryPlatform.Translation.Services.UpdatePageRecordService.UpdateRecord(455, dictionary, token);
        }

        //[Test]
        //public void ShouldGetPageRecords()
        //{
        //    var pageId = 455;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecords(pageId);
        //    Assert.IsNotNull(record);
        //    Assert.IsNotEmpty(record);
        //    Assert.AreEqual( "Tony", record.FirstOrDefault()["First_Name"].ToString());
        //}

        //[Test]
        //public void ShouldGetNoPageRecords()
        //{
        //    var pageId = 0;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecords(pageId);
        //    Assert.IsNull(record);
        //}

        [Test]
        public void ShouldGetPageRecord()
        {
            var pageId = 455;
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var recordId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");
            Dictionary<string,object> record = GetPageRecordService.GetRecordDict(pageId, recordId,token);
            Assert.IsNotNull(record);
            Assert.IsNotEmpty(record);
            Assert.AreEqual(FIRSTNAME, record["First_Name"]);
        }

        [Test]
        public void GetAvailableSkills()
        {
            var pageId = 277;
            var token = AuthenticationService.authenticate("tmaddox", "crds1234");
            //var recordId = AuthenticationService.GetContactId(token);
            //Assert.IsNotNull(recordId, "Contact ID shouldn't be null");
            var records = GetPageRecordService.GetRecords(277, token);
            Assert.IsNotNull(records);
            //Assert.IsNotEmpty(record);
            //Assert.AreEqual(FIRSTNAME, record["First_Name"]);
        }

        [Test]
        public void GetMySkills()
        {
            //setup stuff
            var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var recordId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");

            //the good stuff
            var attributes = GetMyRecords.GetMyAttributes(recordId, token);
            Assert.IsNotNull(attributes);
        }

        //[Test]
        //public void AddASkill()
        //{
        //    //var subPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MySkills"]);
        //    var token = AuthenticationService.authenticate("tmaddox", "crds1234");
        //    var recordId = AuthenticationService.GetContactId(token);
        //    Assert.IsNotNull(recordId, "Contact ID shouldn't be null");

        //    var attribute = new MinistryPlatform.Models.Attribute();
        //    attribute.Start_Date = new DateTime(2013, 7, 1);
        //    attribute.Attribute_ID = 71;

        //    var added = GetMyRecords.CreateAttribute(attribute, recordId, token);
        //    Assert.IsTrue(added);
        //}

        //[Test]
        //public void DeleteASkill()
        //{
        //    var token = AuthenticationService.authenticate("tmaddox", "crds1234");

        //    var deleted = GetMyRecords.DeleteAttribute(679098, token);
        //    Assert.IsTrue(deleted);
        //}
    }
}
