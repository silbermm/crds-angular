using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class DonorServiceTest
    {
        private DonorService fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private readonly int _donorPageId = 299;

        [SetUp]
        public void SetUp()
        {
            //fixture = new DonorService(new MinistryPlatformServiceImpl(new PlatformServiceClient(), ));

            _ministryPlatformService = new Mock<IMinistryPlatformService>();

        }

        [Test]
        public void TestCreateDonorRecord()
        {
            // Arrange - need contactId, StripeCutomerId, pageId??
            const int contactId = 123456;
            const string stripeCustomerId = "CRDS987654";
            
            // Act - execute createDonorRecord
            // need to mock this
           // donorId = WithApiLogin<int>(apiToken => (ministryPlatformService.CreateRecord(donorPageId, values, apiToken, true)));

            // Assert - call is successful and donorId is returned
            //  ministryPlatformService.VerifyAll();
            //  Assert.IsNotNull(donorId);

        }
    }
}
