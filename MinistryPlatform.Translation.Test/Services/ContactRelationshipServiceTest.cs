﻿using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ContactRelationshipServiceTest
    {
        private ContactRelationshipService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;

        private readonly int CONTACT_RELATIONSHIP_PAGE = 265;
        private readonly int CONTACT_RELATIONSHIP_ID = 110;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _fixture = new ContactRelationshipService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object);
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});
            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("ContactRelationships")).Returns(CONTACT_RELATIONSHIP_PAGE);
            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("ContactRelationshipsIds")).Returns(CONTACT_RELATIONSHIP_ID);
        }

        [Test]
        public void AddRelationship()
        {
            const int childId = 4384766;
            const int myId = 2186211;


            Relationship r = new Relationship
            {
                RelationshipID = 43,
                EndDate = null,
                StartDate = DateTime.Now,
                RelatedContactID = childId
            };

            var dict = new Dictionary<string, object>
            {
                {"Relationship_ID", r.RelationshipID},
                {"Related_Contact_ID", r.RelatedContactID},
                {"Start_Date", r.StartDate},
                {"End_Date", r.EndDate}
            };


            _ministryPlatformService.Setup(mocked =>
                                               mocked.CreateSubRecord(CONTACT_RELATIONSHIP_PAGE,
                                                                      myId,
                                                                      dict,
                                                                      It.IsAny<string>(),
                                                                      true)).Returns(1);

            var id = _fixture.AddRelationship(r, myId);
            Assert.AreEqual(1, id);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void GetSomeonesRelationships()
        {
            const int childId = 4384766;
            const int myId = 2186211;

            var relationships = new List<Relationship>
            {
                new Relationship
                {
                    RelationshipID = 43,
                    RelatedContactID = childId,
                    StartDate = DateTime.Today,
                    EndDate = null
                }
            };

            var subpageDict = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Relationship_ID", 43},
                    {"Related_Contact_ID", childId},
                    {"End_Date", null},
                    {"Start_Date", DateTime.Today}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetSubpageViewRecords("ContactRelationshipsIds", myId, It.IsAny<string>(), "", "", 0)).Returns(subpageDict);

            var result = _fixture.GetMyCurrentRelationships(myId).ToList();
            Assert.AreEqual(relationships[0].RelatedContactID, result[0].RelatedContactID);
            Assert.AreEqual(relationships[0].RelationshipID, result[0].RelationshipID);
            Assert.AreEqual(relationships[0].StartDate, result[0].StartDate);
            Assert.AreEqual(relationships[0].EndDate, result[0].EndDate);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void GetMyFamily()
        {
            const int contactId = 123456;
            const string token = "some-string";

            //mock GetSubpageViewRecords
            var getSubpageViewRecordsResponse = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    {"Contact_ID", 123456},
                    {"Email_Address", "person-one@email.com"},
                    {"Preferred Name", "person-one"},
                    {"Last Name", "person-one"},
                    {"Participant_ID", 654321},
                    {"Relationship_ID", 1},
                    {"Age", 40},
                    {"HS_Graduation_Year", 0}
                },
                new Dictionary<string, object>()
                {
                    {"Contact_ID", 222222},
                    {"Email_Address", "person-two@email.com"},
                    {"Preferred Name", "person-two"},
                    {"Last Name", "person-two"},
                    {"Participant_ID", 3333333},
                    {"Relationship_ID", 1},
                    {"Age", 40},
                    {"HS_Graduation_Year", 0}
                }
            };
            _ministryPlatformService.Setup(
                mocked =>
                    mocked.GetSubpageViewRecords("MyContactFamilyRelationshipViewId",
                                                 contactId,
                                                 It.IsAny<string>(),
                                                 "",
                                                 "",
                                                 0))
                .Returns(getSubpageViewRecordsResponse);

            var family = _fixture.GetMyImmediateFamilyRelationships(contactId, token).ToList();

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(family);
            Assert.AreEqual(2, family.Count());

            Assert.AreEqual(123456, family[0].Contact_Id);
            Assert.AreEqual("person-one@email.com", family[0].Email_Address);
            Assert.AreEqual("person-one", family[0].Preferred_Name);
            Assert.AreEqual("person-one", family[0].Last_Name);

            Assert.AreEqual(222222, family[1].Contact_Id);
            Assert.AreEqual("person-two@email.com", family[1].Email_Address);
            Assert.AreEqual("person-two", family[1].Preferred_Name);
            Assert.AreEqual("person-two", family[1].Last_Name);
        }
    }
}