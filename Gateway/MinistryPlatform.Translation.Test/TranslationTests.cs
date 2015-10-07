using System;
using System.Collections.Generic;
using System.Configuration;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    [Category("IntegrationTests")]
    public class TranslationTests
    {
        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string FIRSTNAME = "Test";
        private const string NEW_PASSWORD = "changemeagain";

        private AuthenticationServiceImpl _fixture;
        private PlatformServiceClient _platformService;
        private MinistryPlatformServiceImpl _ministryPlatformService;
        private IConfigurationWrapper _configurationWrapper;

        [SetUp]
        public void SetUp()
        {
            _configurationWrapper = new ConfigurationWrapper();
            _platformService = new PlatformServiceClient();
            _ministryPlatformService = new MinistryPlatformServiceImpl(_platformService, _configurationWrapper);
            _fixture = new AuthenticationServiceImpl(_platformService, _ministryPlatformService);
        }

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
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var token = authData["token"].ToString();
            var obj = _fixture.GetContactId(token);
            Assert.IsNotNull(obj, "Contact ID shouldn't be null");
        }

        [Test]
        public void ShouldChangePassword()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var token = authData["token"].ToString();
            var changed = _fixture.ChangePassword(token, NEW_PASSWORD);
            Assert.IsTrue(changed);
            var obj = AuthenticationService.authenticate(USERNAME, NEW_PASSWORD);
            Assert.IsNotNull(obj);
            var changedAgain = _fixture.ChangePassword(token, PASSWORD);
            Assert.IsTrue(changedAgain);
        }

        [Test]
        public void ShouldGetPageRecord()
        {

            
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var token = authData["token"].ToString();
            var recordId = _fixture.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");
            Dictionary<string, object> record = MinistryPlatformService.GetRecordDict(pageId, recordId, token);
            Assert.IsNotNull(record);
            Assert.IsNotEmpty(record);
            Assert.AreEqual(FIRSTNAME, record["First_Name"]);
        }

        [Test]
        public void ShouldCreatePageRecord()
        {
            var uid = USERNAME;
            var pwd = PASSWORD;
            var authData = AuthenticationService.authenticate(uid, pwd);
            var token = authData["token"].ToString();
            var recordId = _fixture.GetContactId(token);

            var householdDict = new Dictionary<string, object>
            {
                {"Contact_ID", recordId},
                {"Congregation_ID", 5},
                {"Household_Name", "API Household"}
            };
            var hhPageId = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);

            var newRecordId = MinistryPlatformService.CreateRecord(hhPageId, householdDict, token);
            Assert.IsNotNull(newRecordId);
            Assert.AreNotEqual(0, newRecordId);

            //TODO: Determine how to clean up after tests
            //MinistryPlatformService.DeleteRecord(hhPageId, newRecordId, null, adminToken);
        }

    }
}