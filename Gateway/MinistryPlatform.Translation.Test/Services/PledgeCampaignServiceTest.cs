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
    }
}