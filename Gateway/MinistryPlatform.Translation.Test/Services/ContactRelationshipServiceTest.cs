using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
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
        
        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _fixture = new ContactRelationshipService(_ministryPlatformService.Object,_authService.Object,_configWrapper.Object);
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
                    {"Age", 40}
                },
                new Dictionary<string, object>()
                {
                    {"Contact_ID", 222222},
                    {"Email_Address", "person-two@email.com"},
                    {"Preferred Name", "person-two"},
                    {"Last Name", "person-two"},
                    {"Participant_ID", 3333333},
                    {"Relationship_ID", 1},
                    {"Age", 40}
                }
            };
            _ministryPlatformService.Setup(
                mocked =>
                    mocked.GetSubpageViewRecords("MyContactFamilyRelationshipViewId", contactId, It.IsAny<string>(), "",
                        "", 0))
                .Returns(getSubpageViewRecordsResponse);

            var family = _fixture.GetMyImmediatieFamilyRelationships(contactId, token).ToList();

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