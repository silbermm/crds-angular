using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    public class DonationServiceTest
    {
        private DonationService _fixture;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);

            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigIntValue("Donations")).Returns(9090);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Batches")).Returns(8080);

            _fixture = new DonationService(_ministryPlatformService.Object, configuration.Object);
        }

        [Test]
        public void TestUpdateDonationStatusById()
        {
            const int donationId = 987;
            var donationStatusDate = DateTime.Now.AddDays(-1);
            const string donationStatusNotes = "note";
            const int donationStatusId = 654;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", donationStatusDate},
                {"Donation_Status_Notes", donationStatusNotes},
                {"Donation_Status_ID", donationStatusId}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));

            _fixture.UpdateDonationStatus(donationId, donationStatusId, donationStatusDate, donationStatusNotes);

            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationStatusByProcessorPaymentId()
        {
            const int donationId = 987;
            var donationStatusDate = DateTime.Now.AddDays(-1);
            const string donationStatusNotes = "note";
            const int donationStatusId = 654;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Donation_Status_Date", donationStatusDate},
                {"Donation_Status_Notes", donationStatusNotes},
                {"Donation_Status_ID", donationStatusId}
            };

            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));

            var searchResult = new List<Dictionary<string, object>>
            {
                {
                    new Dictionary<string, object>
                    {
                        {"dp_RecordID", donationId}
                    }
                }
            };
            _ministryPlatformService.Setup(
                mocked => mocked.GetRecordsDict(9090, It.IsAny<string>(), ",,,,,,,ch_123", It.IsAny<string>()))
                .Returns(searchResult);

            _fixture.UpdateDonationStatus("ch_123", donationStatusId, donationStatusDate, donationStatusNotes);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCreateDonationBatch()
        {
            const string batchName = "MP12345";
            var setupDateTime = DateTime.Now;
            const decimal batchTotalAmount = 456.78M;
            const int itemCount = 55;
            const int batchEntryType = 44;
            const int depositId = 987;
            var finalizedDateTime = DateTime.Now;

            var expectedParms = new Dictionary<string, object>
            {
                {"Batch_Name", batchName},
                {"Setup_Date", setupDateTime},
                {"Batch_Total", batchTotalAmount},
                {"Item_Count", itemCount},
                {"Batch_Entry_Type_ID", batchEntryType},
                {"Deposit_ID", depositId},
                {"Finalize_Date", finalizedDateTime}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(8080, expectedParms, It.IsAny<string>(), false))
                .Returns(513);
            var batchId = _fixture.CreateDonationBatch(batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType,
                depositId, finalizedDateTime);
            Assert.AreEqual(513, batchId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestAddDonationToBatch()
        {
            const int batchId = 123;
            const int donationId = 456;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donation_ID", donationId},
                {"Batch_ID", batchId}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(9090, expectedParms, It.IsAny<string>()));
            _fixture.AddDonationToBatch(batchId, donationId);
            _ministryPlatformService.VerifyAll();
        }
    }
}