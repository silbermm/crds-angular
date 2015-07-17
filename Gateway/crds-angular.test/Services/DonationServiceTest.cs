using System;
using crds_angular.Services;
using Moq;
using NUnit.Framework;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class DonationServiceTest
    {
        private DonationService _fixture;
        private Mock<MPServices.IDonationService> _mpDonationService;

        [SetUp]
        public void SetUp()
        {
            _mpDonationService = new Mock<MPServices.IDonationService>(MockBehavior.Strict);

            _fixture = new DonationService(_mpDonationService.Object);
        }

        [Test]
        public void TestUpdateDonationByIdWithOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus(123, 4, d, "note")).Returns(456);
            var response = _fixture.UpdateDonationStatus(123, 4, d, "note");
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByIdWithoutOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus(123, 4, It.IsNotNull<DateTime>(), null)).Returns(456);
            var response = _fixture.UpdateDonationStatus(123, 4, null);
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByProcessorIdWithOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus("ch_123", 4, d, "note")).Returns(456);
            var response = _fixture.UpdateDonationStatus("ch_123", 4, d, "note");
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByProcessorIdWithoutOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus("ch_123", 4, It.IsNotNull<DateTime>(), null)).Returns(456);
            var response = _fixture.UpdateDonationStatus("ch_123", 4, null);
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }
    }
}
