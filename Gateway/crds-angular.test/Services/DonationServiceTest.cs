using System;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using MinistryPlatform.Models;
using Moq;
using Newtonsoft.Json;
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
            AutoMapperConfig.RegisterMappings();

            _mpDonationService = new Mock<MPServices.IDonationService>(MockBehavior.Strict);

            _fixture = new DonationService(_mpDonationService.Object);
        }

        [Test]
        public void TestGetDonationBatch()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatch(123)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789"
            });
            var result = _fixture.GetDonationBatch(123);
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(456, result.DepositId);
            Assert.AreEqual("789", result.ProcessorTransferId);
        }

        [Test]
        public void TestGetDonationBatchReturnsNull()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatch(123)).Returns((DonationBatch) null);
            var result = _fixture.GetDonationBatch(123);
            _mpDonationService.VerifyAll();
            Assert.IsNull(result);
        }

        [Test]
        public void TestGetDonationBatchByProcessorTransferId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByProcessorTransferId("123")).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789"
            });
            var result = _fixture.GetDonationBatchByProcessorTransferId("123");
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(456, result.DepositId);
            Assert.AreEqual("789", result.ProcessorTransferId);
        }

        [Test]
        public void TestGetDonationByProcessorPaymentIdDonationNotFound()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("123")).Returns((Donation) null);
            Assert.IsNull(_fixture.GetDonationByProcessorPaymentId("123"));
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGetDonationByProcessorPaymentId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("123")).Returns(new Donation
            {
                donationId = 123,
                donationAmt = 456,
                batchId = 789
            });
            var result = _fixture.GetDonationByProcessorPaymentId("123");
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123+"", result.donation_id);
            Assert.AreEqual(456, result.amount);
            Assert.AreEqual(789, result.batch_id);
        }

        [Test]
        public void TestGetDonationBatchByDepositDonationIdNotFound()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(12424)).Returns((DonationBatch)null);
            Assert.IsNull(_fixture.GetDonationBatchByDepositId(12424));
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGetDonationBatchByDepositId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(12424)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 12424,
            });
            var result = _fixture.GetDonationBatchByDepositId(12424);
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(12424, result.DepositId);
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
                    mocked.CreateDeposit(dto.DepositName, dto.DepositTotalAmount, dto.DepositAmount, dto.ProcessorFeeTotal, dto.DepositDateTime, dto.AccountNumber,
                        dto.BatchCount, dto.Exported, dto.Notes, dto.ProcessorTransferId)).Returns(987);

            var response = _fixture.CreateDeposit(dto);
            _mpDonationService.VerifyAll();
            Assert.AreSame(dto, response);
            Assert.AreEqual(987, response.Id);
        }

        [Test]
        public void TestCreatePaymentProcessorEventError()
        {
            var stripeEvent = new StripeEvent
            {
                Created = DateTime.Now,
                Data = new StripeEventData
                {
                    Object = new StripeTransfer
                    {
                        Amount = 1000
                    }
                },
                LiveMode = true,
                Id = "123",
                Type = "transfer.paid"
            };

            var stripeEventResponse = new StripeEventResponseDTO
            {
                Exception = new ApplicationException()
            };

            var eventString = JsonConvert.SerializeObject(stripeEvent, Formatting.Indented);
            var responseString = JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented);

            _mpDonationService.Setup(
                mocked =>
                    mocked.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type,
                        eventString, responseString));

            _fixture.CreatePaymentProcessorEventError(stripeEvent, stripeEventResponse);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGPExportFileName()
        {
            var date = DateTime.Today;
            var fileName = string.Format("TestBatchName_{0}{1}.csv", date.ToString("MM"), date.ToString("yy"));

            var batch = new DonationBatchDTO
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789",
                BatchName = "TestBatchName",
            };
            _fixture.GPExportFileName(batch);

            _mpDonationService.VerifyAll();
            Assert.AreEqual(fileName, batch.ExportFileName);
        }

    }
}
