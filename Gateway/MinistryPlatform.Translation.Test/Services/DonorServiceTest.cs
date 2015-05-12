using System;
using System.Collections.Generic;
using System.Configuration;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class DonorServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private DonorService _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _fixture = new DonorService(_ministryPlatformService.Object);
        }

        [Test]
        public void CreateDonorRecordTest()
        {
            var donorPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donors"]);
            var expectedDonorId = 585858;
            var setupDate = DateTime.Now;

            var expectedValues = new Dictionary<string, object>
            {
                {"Contact_ID", 888888},
                {"Statement_Frequency_ID", "1"},//default to quarterly
                {"Statement_Type_ID", "1"},     //default to individual
                {"Statement_Method_ID", 2},   //default to email/online
                {"Setup_Date", setupDate},    //default to current date/time
                {"Stripe_Customer_ID", "cus_crds123456"}    
            };

           _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
              It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
              It.IsAny<string>(), true)).Returns(expectedDonorId);

           var response = _fixture.CreateDonorRecord(888888, "cus_crds123456", setupDate);

           _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donorPageId, expectedValues, It.IsAny<string>(), true));
          
            Assert.AreEqual(response, expectedDonorId);
  
        }

        [Test]
        public void CreateDonationAndDistributionRecord()
        {
            var donationAmt = 676767;
            var donorId = 1234567;
            var donationId = 7654321;
            var programId = "3";
            var setupDate = DateTime.Now;
            var charge_id = "ch_crds1234567";
            var expectedDonationId = 321321;
            var expectedDonationDistributionId = 231231;
            var donationPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donations"]);
            var donationDistributionPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Distributions"]);
            

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
              It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
              It.IsAny<string>(), true)).Returns(expectedDonationId);

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
              It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
              It.IsAny<string>(), true)).Returns(expectedDonationDistributionId);

            var expectedDonationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Payment_Type_ID", 4}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupDate},
                {"Transaction_code", charge_id}
            };
            
            var response = _fixture.CreateDonationAndDistributionRecord(donationAmt, donorId, programId, charge_id, setupDate);

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationPageId, expectedDonationValues, It.IsAny<string>(), true));

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual(response, expectedDonationDistributionId);
        }

       
    }
}