using System;
using System.Collections.Generic;
using crds_angular.App_Start;
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
        private Mock<IDonorService> _donorService;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _ministryPlatformService = new Mock<IMinistryPlatformService>(MockBehavior.Strict);
            _donorService = new Mock<IDonorService>(MockBehavior.Strict);

            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigIntValue("Donations")).Returns(9090);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Batches")).Returns(8080);
            configuration.Setup(mocked => mocked.GetConfigIntValue("Distributions")).Returns(1234);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DefaultGiveDeclineEmailTemplate")).Returns(999999);
            configuration.Setup(mocked => mocked.GetConfigIntValue("CheckGiveDeclineEmailTemplate")).Returns(12200);
            configuration.Setup(mocked => mocked.GetConfigValue("Bank")).Returns("5:Bank");
            configuration.Setup(mocked => mocked.GetConfigValue("CreditCard")).Returns("4:CreditCard");
            configuration.Setup(mocked => mocked.GetConfigValue("Check")).Returns("1:Check");
            configuration.Setup(mocked => mocked.GetConfigIntValue("Deposits")).Returns(7070);
            configuration.Setup(mocked => mocked.GetConfigIntValue("PaymentProcessorEventErrors")).Returns(6060);

            _fixture = new DonationService(_ministryPlatformService.Object, _donorService.Object, configuration.Object);
        }

        [Test]
        public void TestGetDonationBatchByProcessorTransferId()
        {
            const string processorTransferId = "123";
            const int depositId = 456;
            const int batchId = 789;
            var searchResult = new List<Dictionary<string, object>>
            {
                {
                    new Dictionary<string, object>
                    {
                        {"dp_RecordID", batchId},
                        {"Processor_Transfer_ID", processorTransferId},
                        {"Deposit_ID", depositId},
                    }
                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(8080, It.IsAny<string>(), string.Format(",,,,,,,,{0},", processorTransferId), "")).Returns(searchResult);

            var result = _fixture.GetDonationBatchByProcessorTransferId(processorTransferId);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(processorTransferId, result.ProcessorTransferId);
            Assert.AreEqual(batchId, result.Id);
            Assert.AreEqual(depositId, result.DepositId);
        }

        [Test]
        public void TestGetDonationBatch()
        {
            const string processorTransferId = "123";
            const int depositId = 456;
            const int batchId = 789;
            var getResult = new Dictionary<string, object>
                {
                    {"Batch_ID", batchId},
                    {"Processor_Transfer_ID", processorTransferId},
                    {"Deposit_ID", depositId},
                };
            _ministryPlatformService.Setup(mocked => mocked.GetRecordDict(8080, batchId, It.IsAny<string>(), false)).Returns(getResult);

            var result = _fixture.GetDonationBatch(batchId);
            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(processorTransferId, result.ProcessorTransferId);
            Assert.AreEqual(batchId, result.Id);
            Assert.AreEqual(depositId, result.DepositId);
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
            const int donorId = 9876;
            const int donationAmt = 4343;
            const string paymentType = "Bank";
            const int batchId = 9090;

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
                        {"dp_RecordID", donationId},
                        {"Donor_ID", donorId},
                        {"Donation_Amount", donationAmt},
                        {"Donation_Date", donationStatusDate},
                        {"Donation_Status_Notes", donationStatusNotes},
                        {"Payment_Type", paymentType},
                        {"Batch_ID", batchId}
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
            const string processorTransferId = "transfer 1";

            var expectedParms = new Dictionary<string, object>
            {
                {"Batch_Name", batchName},
                {"Setup_Date", setupDateTime},
                {"Batch_Total", batchTotalAmount},
                {"Item_Count", itemCount},
                {"Batch_Entry_Type_ID", batchEntryType},
                {"Deposit_ID", depositId},
                {"Finalize_Date", finalizedDateTime},
                {"Processor_Transfer_ID", processorTransferId}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(8080, expectedParms, It.IsAny<string>(), false))
                .Returns(513);

            var expectedUpdateParms = new Dictionary<string, object>
            {
                {"Batch_ID", 513},
                {"Currency", null},
                {"Default_Payment_Type", null}
            };
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(8080, expectedUpdateParms, It.IsAny<string>()));
            var batchId = _fixture.CreateDonationBatch(batchName, setupDateTime, batchTotalAmount, itemCount, batchEntryType,
                depositId, finalizedDateTime, processorTransferId);
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

        [Test]
        public void TestCreateDeposit()
        {
            const string depositName = "MP12345";
            const decimal depositTotalAmount = 456.78M;
            var depositDateTime = DateTime.Now;
            const string accountNumber = "8675309";
            const int batchCount = 55;
            const bool exported = true;
            const string notes = "C Sharp";
            const string processorTransferId = "transfer 1";

            var expectedParms = new Dictionary<string, object>
            {
                {"Deposit_Name", depositName},
                {"Deposit_Total", depositTotalAmount},
                {"Deposit_Date", depositDateTime},
                {"Account_Number", accountNumber},
                {"Batch_Count", batchCount},
                {"Exported", exported},
                {"Notes", notes},
                {"Processor_Transfer_ID", processorTransferId}
            };

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(7070, expectedParms, It.IsAny<string>(), false))
                .Returns(513);
            var depositId = _fixture.CreateDeposit(depositName, depositTotalAmount, depositDateTime, accountNumber,
                batchCount, exported, notes, processorTransferId);
            Assert.AreEqual(513, depositId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestCreatePaymentProcessorEventError()
        {
            var dateTime = DateTime.Now;
            const string eventId = "123";
            const string eventType = "456";
            const string message = "message";
            const string response = "response";
            var expectedParms = new Dictionary<string, object>
            {
                {"Event_Date_Time", dateTime},
                {"Event_ID", eventId},
                {"Event_Type", eventType},
                {"Event_Message", message},
                {"Response_Message", response}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(6060, expectedParms, It.IsAny<string>(), false)).Returns(513);

            _fixture.CreatePaymentProcessorEventError(dateTime, eventId, eventType, message, response);

            _ministryPlatformService.VerifyAll();
        }

    }
}
