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
    }
}