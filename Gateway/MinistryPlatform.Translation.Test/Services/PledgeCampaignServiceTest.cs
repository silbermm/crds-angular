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
    public class PledgeCampaignServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private IPledgeService _fixture;

        [SetUp]
        public void SetUp()
        {
            const int mockPledgesPageId = 9876;

            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _authService = new Mock<IAuthenticationService>();
            _configWrapper = new Mock<IConfigurationWrapper>();

            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configWrapper.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");
            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> {{"token", "ABC"}, {"exp", "123"}});
            _configWrapper.Setup(m => m.GetConfigIntValue("Pledges")).Returns(mockPledgesPageId);
            _configWrapper.Setup(mocked => mocked.GetConfigIntValue("MyHouseholdPledges")).Returns(525);

            _fixture = new PledgeService(_ministryPlatformService.Object, _authService.Object, _configWrapper.Object);
        }

        [Test]
        public void CreatePledgeRecordTest()
        {
            const int mockPageId = 9876;
            const int mockDonorId = 11111;
            const int mockCampaignId = 22222;
            const int mockPledgeAmount = 259;
            const int expectedPledgeId = 676767;

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
                It.IsAny<int>(),
                It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(),
                true)).Returns(expectedPledgeId);

            var response = _fixture.CreatePledge(mockDonorId, mockCampaignId, mockPledgeAmount);

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(mockPageId, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>(), true));

            Assert.AreEqual(response, expectedPledgeId);
        }

        [Test]
        public void GetPledgesForAuthUserTest()
        {
            const int _myHouseholdPledges = 525;

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Pledge_ID", 123},
                    {"Pledge_Campaign_ID", 678},
                    {"Pledge_Status", "Active"},
                    {"Donor_ID", 432},
                    {"Campaign_Name", "Winners Win"},
                    {"Total_Pledge", 1000.00M},
                    {"Donation_Total", 125.00M},
                    {"Display_Name", " Dr McSteamy"},
                    {"Start_Date", DateTime.Now.Date},
                    {"End_Date", DateTime.Now.AddYears(5).Date}
                },
                new Dictionary<string, object>
                {
                    {"Pledge_ID", 321},
                    {"Pledge_Campaign_ID", 876},
                    {"Pledge_Status", "Active"},
                    {"Donor_ID", 111},
                    {"Campaign_Name", "Chartreuse Caboose"},
                    {"Total_Pledge", 3000.00M},
                    {"Donation_Total", 1215.00M},
                    {"Display_Name", "Ice Creamy"},
                    {"Start_Date", DateTime.Now.AddMonths(10).Date},
                    {"End_Date", DateTime.Now.AddYears(3).Date}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(_myHouseholdPledges, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(records);
            var record = _fixture.GetPledgesForAuthUser("iamausertoken");

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(record);
            Assert.AreEqual(records[0]["Pledge_ID"], record[0].PledgeId);
            Assert.AreEqual(records[0]["Pledge_Campaign_ID"], record[0].PledgeCampaignId);
            Assert.AreEqual(records[0]["Pledge_Status"], record[0].PledgeStatus);
            Assert.AreEqual(records[0]["Campaign_Name"], record[0].CampaignName);
            Assert.AreEqual(records[0]["Total_Pledge"], record[0].PledgeTotal);
            Assert.AreEqual(records[0]["Donation_Total"], record[0].PledgeDonations);
            Assert.AreEqual(records[0]["Display_Name"], record[0].DonorDisplayName);
            Assert.AreEqual(records[0]["Start_Date"], record[0].CampaignStartDate);
            Assert.AreEqual(records[0]["End_Date"], record[0].CampaignEndDate);

            Assert.AreEqual(records[1]["Pledge_ID"], record[1].PledgeId);
            Assert.AreEqual(records[1]["Pledge_Campaign_ID"], record[1].PledgeCampaignId);
            Assert.AreEqual(records[1]["Pledge_Status"], record[1].PledgeStatus);
            Assert.AreEqual(records[1]["Campaign_Name"], record[1].CampaignName);
            Assert.AreEqual(records[1]["Total_Pledge"], record[1].PledgeTotal);
            Assert.AreEqual(records[1]["Donation_Total"], record[1].PledgeDonations);
            Assert.AreEqual(records[1]["Display_Name"], record[1].DonorDisplayName);
            Assert.AreEqual(records[1]["Start_Date"], record[1].CampaignStartDate);
            Assert.AreEqual(records[1]["End_Date"], record[1].CampaignEndDate);
        }
    }
}