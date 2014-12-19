using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.PlatformService;
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
            UserInfo userInfo = new UserInfo
            {
                FirstName = FIRSTNAME,
                LastName = "User",
                MobilePhone = "513-555-5555",
                EmailAddress = "testme@test.com",
                NewPassword = NEW_PASSWORD
            };
            var changed = AuthenticationService.ChangePassword(token, userInfo);
            Assert.IsTrue(changed);
            var obj = AuthenticationService.authenticate(USERNAME, NEW_PASSWORD);
            Assert.IsNotNull(obj);
            userInfo.NewPassword = PASSWORD;
            var changedAgain = AuthenticationService.ChangePassword(token, userInfo);
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
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");;
            Dictionary<string,object> record = GetPageRecordService.GetRecordDict(pageId, recordId,token);
            Assert.IsNotNull(record);
            Assert.IsNotEmpty(record);
            Assert.AreEqual(FIRSTNAME, record["First_Name"]);
        }

        //[Test]
        //public void ShouldGetNoPageRecord()
        //{
        //    var pageId = 292;
        //    var recordId = 0;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, recordId);
        //    Assert.IsNull(record);

        //}
    }
}
