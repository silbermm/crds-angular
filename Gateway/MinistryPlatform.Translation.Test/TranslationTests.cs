using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;
using Attribute = MinistryPlatform.Models.Attribute;

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
        public void ShouldGetPageRecord()
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var recordId = AuthenticationService.GetContactId(token);
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
            var token = AuthenticationService.authenticate(uid, pwd);
            var recordId = AuthenticationService.GetContactId(token);

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

        [Test]
        [Ignore("Must have certain rights to access subpage.  Ask team best way to handle")]
        public void ShouldGetSubPageRecord()
        {
            var subGroupPageId = Convert.ToInt32(ConfigurationManager.AppSettings["GroupsSubgroups"]);
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var recordId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");
            var record = MinistryPlatformService.GetSubPageRecords(subGroupPageId, 6717,
                token);
            Assert.IsNotNull(record);
            Assert.IsNotEmpty(record);
        }


        [Test]
        public void GetAvailableSkills()
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Attributes"]);
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var records = MinistryPlatformService.GetRecords(pageId, token);
            Assert.IsNotNull(records);
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

        [Test]
        public void UpdateMySkills()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var recordId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(recordId, "Contact ID shouldn't be null");

            var attributePageId = Convert.ToInt32(ConfigurationManager.AppSettings["Attributes"]);
            var dentist = MinistryPlatformService.GetLookupRecord(attributePageId,
                "Dentist", token, 1);
            Assert.IsNotNull(dentist);

            var attribute = new Attribute();
            attribute.Start_Date = new DateTime(2013, 7, 1);
            attribute.Attribute_ID = Convert.ToInt32(dentist["dp_RecordID"]);
            var added = GetMyRecords.CreateAttribute(attribute, recordId, token);
            Assert.IsNotNull(added);
            Assert.IsFalse(added == 0);

            //now try to delete just added attribute
            var deleted = GetMyRecords.DeleteAttribute(added, token);
            Assert.IsTrue(deleted);
        }

        [Test]
        public void CreateOpportunityResponse()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            const int opportunityId = 113;
            const string comment = "Test Comment";
            Assert.DoesNotThrow(() => OpportunityService.RespondToOpportunity(token, opportunityId, comment));
        }

        [Test]
        public void ShouldNotCreateOpportunityResponse()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            const int opportunityId = 10000000;
            const string comment = "Fail Test Comment";

            Assert.Throws<FaultException<ExceptionDetail>>(
                () => OpportunityService.RespondToOpportunity(token, opportunityId, comment));
        }

        [Test]
        public void GetParticipants()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            var participants = AuthenticationService.GetParticipantRecord(token);

            Assert.IsNotNull(participants);
        }

        [Test]
        public void ShouldNotDeleteResponse()
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["OpportunityResponses"]);
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            const int opportunityId = 113;
            const string comment = "Test Comment";

            //Create a Response to delete
            var responseId = OpportunityService.RespondToOpportunity(token, opportunityId, comment);

            //Try to delete Response, should fail
            Assert.Throws<FaultException<ExceptionDetail>>(
                () => MinistryPlatformService.DeleteRecord(pageId, responseId, null, token));
        }

        [Test]
        public void ShouldReturnTodaysEvents()
        {
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["TodaysEventLocationRecords"]);
            string token = AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"],
                ConfigurationManager.AppSettings["ApiPass"]);

            var todaysEvents = MinistryPlatformService.GetRecordsDict(pageId, token, ",Mason", "5");

            Assert.IsNotNull(todaysEvents);
        }
    }
}