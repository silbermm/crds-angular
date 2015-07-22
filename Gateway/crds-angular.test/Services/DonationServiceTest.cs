using System;
using crds_angular.Models.Crossroads.Stewardship;
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

        [Test]
        public void TestCreateDonationBatch()
        {
            var dto = new DonationBatchDTO
            {
                DepositId = 123,
                BatchEntryType = 2,
                BatchName = "batch name",
                BatchTotalAmount = 456.78M,
                FinalizedDateTime = DateTime.Now,
                ItemCount = 5,
                SetupDateTime = DateTime.Now,
                ProcessorTransferId = "transfer 1",
                Id = 999 // Should be overwritten in service
            };
            dto.Donations.Add(new DonationDTO { donation_id = "102030"});
            _mpDonationService.Setup(
                mocked =>
                    mocked.CreateDonationBatch(dto.BatchName, dto.SetupDateTime, dto.BatchTotalAmount, dto.ItemCount,
                        dto.BatchEntryType, dto.DepositId, dto.FinalizedDateTime, dto.ProcessorTransferId)).Returns(987);
            _mpDonationService.Setup(mocked => mocked.AddDonationToBatch(987, 102030));

            var response = _fixture.CreateDonationBatch(dto);
            _mpDonationService.VerifyAll();
            Assert.AreSame(dto, response);
            Assert.AreEqual(987, response.Id);
        }

        [Test]
        public void TestCreateDeposit()
        {
            var dto = new DepositDTO
            {
                AccountNumber = "8675309",
                BatchCount = 5,
                DepositDateTime = DateTime.Now,
                DepositName = "deposit name",
                DepositTotalAmount = 456.78M,
                Exported = true,
                Notes = "blah blah blah",
                ProcessorTransferId = "transfer 1",
                Id = 123 // should be overwritten in service
            };

            _mpDonationService.Setup(
                mocked =>
                    mocked.CreateDeposit(dto.DepositName, dto.DepositTotalAmount, dto.DepositDateTime, dto.AccountNumber,
                        dto.BatchCount, dto.Exported, dto.Notes, dto.ProcessorTransferId)).Returns(987);

            var response = _fixture.CreateDeposit(dto);
            _mpDonationService.VerifyAll();
            Assert.AreSame(dto, response);
            Assert.AreEqual(987, response.Id);
        }

    }
}
