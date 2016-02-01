using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ContactAttributeServiceTest
    {
        private ContactAttributeService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<Translation.Services.Interfaces.IApiUserService> _apiUserService;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();            
            _apiUserService = new Mock<IApiUserService>();            
            _apiUserService.Setup(m => m.GetToken()).Returns("something");
        

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});
            _fixture = new ContactAttributeService(_authService.Object, _configWrapper.Object, _ministryPlatformService.Object, _apiUserService.Object);
        }

        [Test]
        public void GetContactAttributes()
        {
            const int contactId = 123456;

            //mock GetSubpageViewRecords
            var getSubpageViewRecordsResponse = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>()
                {
                    {"Contact_Attribute_ID", 1},
                    {"Start_Date", "10/10/2014"},
                    {"End_Date", null},
                    {"Notes", "These are my notes"},
                    {"Attribute_ID", 2},
                    {"Attribute_Type_ID", 3},
                    {"Attribute_Type", "AttributeType #1"}
                },
                new Dictionary<string, object>()
                {
                    {"Contact_Attribute_ID", 4},
                    {"Start_Date", "11/11/2015"},
                    {"End_Date", null},
                    {"Notes", ""},
                    {"Attribute_ID", 5},
                    {"Attribute_Type_ID", 6},
                    {"Attribute_Type", "AttributeType #2"}
                }
            };

            _ministryPlatformService.Setup(
                mocked =>
                    mocked.GetSubpageViewRecords(It.IsAny<int>(), contactId, It.IsAny<string>(), "", "", 0))
                .Returns(getSubpageViewRecordsResponse);
            
            var attributes = _fixture.GetCurrentContactAttributes("fakeToken", contactId, false, null).ToList();

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(attributes);
            Assert.AreEqual(2, attributes.Count());

            var attribute = attributes[0];
            Assert.AreEqual(1, attribute.ContactAttributeId);
            Assert.AreEqual(new DateTime(2014, 10, 10), attribute.StartDate);
            Assert.AreEqual(null, attribute.EndDate);
            Assert.AreEqual("These are my notes", attribute.Notes);
            Assert.AreEqual(2, attribute.AttributeId);
            Assert.AreEqual(3, attribute.AttributeTypeId);

            attribute = attributes[1];
            Assert.AreEqual(4, attribute.ContactAttributeId);
            Assert.AreEqual(new DateTime(2015, 11, 11), attribute.StartDate);
            Assert.AreEqual(null, attribute.EndDate);
            Assert.AreEqual(string.Empty, attribute.Notes);
            Assert.AreEqual(5, attribute.AttributeId);
            Assert.AreEqual(6, attribute.AttributeTypeId);
        }
    }
}