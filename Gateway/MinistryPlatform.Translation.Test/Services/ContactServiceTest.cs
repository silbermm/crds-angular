using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
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
        private ContactService _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _fixture = new ContactService(_ministryPlatformService.Object);
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
                    {"City", "Cincinnati"},
                    {"State", "OH"},
                    {"Postal_Code", "45208"},
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
                    {"Nickname", "nickname"}
                }
            };

            _ministryPlatformService.Setup(m => m.GetRecordsDict("MyProfile", It.IsAny<string>(), "", ""))
                .Returns(dictionaryList);

            var myProfile = _fixture.GetMyProfile(It.IsAny<string>());

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(myProfile);
            Assert.AreEqual(3, myProfile.Contact_ID);
            Assert.AreEqual(100, myProfile.Address_ID);
        }
    }
}