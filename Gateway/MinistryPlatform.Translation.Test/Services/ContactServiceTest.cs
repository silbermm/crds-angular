using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class ContactServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private ContactService _fixture;
        private Mock<IConfigurationWrapper> _configuration;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configuration = new Mock<IConfigurationWrapper>();
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Contacts")).Returns(292);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Households")).Returns(327);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("SecurityRolesSubPageId")).Returns(363);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Congregation_Default_ID")).Returns(5);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Household_Default_Source_ID")).Returns(30);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Household_Position_Default_ID")).Returns(1);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Addresses")).Returns(271);
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });


            _fixture = new ContactService(_ministryPlatformService.Object, _authService.Object, _configuration.Object);
        }

        [Test]
        public void GetMyProfile()
        {
            var dictionaryList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Address_ID", 100},
                    {"Address_Line_1", "address-line-1"},
                    {"Address_Line_2", "address-line-2"},
                    {"Congregation_ID", 5},
                    {"Household_ID", 4},
                    {"Household_Name", "hh name"},
                    {"City", "Cincinnati"},
                    {"State", "OH"},
                    {"Postal_Code", "45208"},
                    {"County", "Hamilton"},
                    {"Anniversary_Date", new DateTime(2013, 8, 5)},
                    {"Contact_ID", 3},
                    {"Date_of_Birth", new DateTime(2007, 5, 29)},
                    {"Email_Address", "email-address@email.com"},
                    {"Employer_Name", "Crossroads"},
                    {"First_Name", "first-name"},
                    {"Foreign_Country", "USA"},
                    {"Gender_ID", 2},
                    {"Home_Phone", "513-555-1234"},
                    {"Last_Name", "last-name"},
                    {"Maiden_Name", "maiden-name"},
                    {"Marital_Status_ID", 3},
                    {"Middle_Name", "middle-name"},
                    {"Mobile_Carrier_ID", 2},
                    {"Mobile_Phone", "513-555-9876"},
                    {"Nickname", "nickname"},
                    {"Age", 30},
                    {"Passport_Number", "12345"},
                    {"Passport_Firstname", "first-name"},
                    {"Passport_Lastname", "last-name"},
                    {"Passport_Country", "USA"},
                    {"Passport_Middlename", "middle-name"},
                    {"Passport_Expiration", "02/21/2020"}             
                }
            };

            _ministryPlatformService.Setup(m => m.GetRecordsDict("MyProfile", It.IsAny<string>(), "", ""))
                .Returns(dictionaryList);

            var myProfile = _fixture.GetMyProfile(It.IsAny<string>());

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(myProfile);
            Assert.AreEqual(3, myProfile.Contact_ID);
            Assert.AreEqual(100, myProfile.Address_ID);
            Assert.AreEqual("hh name", myProfile.Household_Name);
        }

        [Test]
        public void GetMyProfileWithOptionalNullableFields()
        {
            var dictionaryList = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Address_ID", null},
                    {"Address_Line_1", "address-line-1"},
                    {"Address_Line_2", "address-line-2"},
                    {"Congregation_ID", null},
                    {"Household_ID", 4},
                    {"Household_Name", "hh name"},
                    {"City", "Cincinnati"},
                    {"State", "OH"},
                    {"Postal_Code", "45208"},
                    {"County", "Hamilton"},
                    {"Anniversary_Date", new DateTime(2013, 8, 5)},
                    {"Contact_ID", 3},
                    {"Date_of_Birth", new DateTime(2007, 5, 29)},
                    {"Email_Address", "email-address@email.com"},
                    {"Employer_Name", "Crossroads"},
                    {"First_Name", "first-name"},
                    {"Foreign_Country", "USA"},
                    {"Gender_ID", null},
                    {"Home_Phone", "513-555-1234"},
                    {"Last_Name", "last-name"},
                    {"Maiden_Name", "maiden-name"},
                    {"Marital_Status_ID", null},
                    {"Middle_Name", "middle-name"},
                    {"Mobile_Carrier_ID", null},
                    {"Mobile_Phone", "513-555-9876"},
                    {"Nickname", "nickname"},
                    {"Age", 30},
                    {"Passport_Number", "12345"},
                    {"Passport_Firstname", "first-name"},
                    {"Passport_Lastname", "last-name"},
                    {"Passport_Country", "USA"},
                    {"Passport_Middlename", "middle-name"},
                    {"Passport_Expiration", "02/21/2020"}
                }
            };

            _ministryPlatformService.Setup(m => m.GetRecordsDict("MyProfile", It.IsAny<string>(), "", ""))
                .Returns(dictionaryList);

            var myProfile = _fixture.GetMyProfile(It.IsAny<string>());

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(myProfile);
            Assert.IsNull(myProfile.Address_ID);
            Assert.IsNull(myProfile.Congregation_ID);
            Assert.IsNull(myProfile.Gender_ID);
            Assert.IsNull(myProfile.Marital_Status_ID);
            Assert.IsNull(myProfile.Mobile_Carrier);
            Assert.AreEqual("hh name", myProfile.Household_Name);
        }

        [Test]
        public void shouldCreateContactForGuestGiver()
        {
            _ministryPlatformService.Setup(
                mocked => mocked.CreateRecord(292, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), false))
                .Returns(123);

            var contactId = _fixture.CreateContactForGuestGiver("me@here.com", "display name");

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(292,
                                                                          It.Is<Dictionary<string, object>>(d =>
                                                                                                                d["Email_Address"].Equals("me@here.com")
                                                                                                                && d["Company"].Equals(false)
                                                                                                                && d["Display_Name"].Equals("display name")
                                                                                                                && d["Nickname"].Equals("display name")
                                                                                                                && d["Household_Position_ID"].Equals(1)
                                                                              ),
                                                                          It.IsAny<string>(),
                                                                          false));

            Assert.AreEqual(123, contactId);
        }

        [Test]
        public void shouldThrowApplicationExceptionWhenGuestGiverCreationFails()
        {
            Exception ex = new Exception("Danger, Will Robinson!");
            _ministryPlatformService.Setup(
                mocked => mocked.CreateRecord(292, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), false))
                .Throws(ex);

            try
            {
                _fixture.CreateContactForGuestGiver("me@here.com", "display");
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof (ApplicationException), e);
                Assert.AreSame(ex, e.InnerException);
            }
        }

        [Test]
        public void shouldGetContactFromParticipant()
        {
            var participantId = 2375529;
            var contactId = 2562386;
            var mockContact = new Dictionary<string, object>
            {
                {"Contact_ID", contactId}
            };

            _ministryPlatformService.Setup(mocked => mocked.GetRecordDict(It.IsAny<int>(), participantId, It.IsAny<string>(), It.IsAny<bool>())).Returns(mockContact);

            var result = _fixture.GetContactIdByParticipantId(participantId);
            Assert.AreEqual(contactId, result);
        }
    }
}