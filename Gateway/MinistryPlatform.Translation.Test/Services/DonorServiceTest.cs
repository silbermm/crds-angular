using System;
using System.Collections.Generic;
using System.Configuration;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
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
                {"Statement_Frequency_ID", 1},//default to quarterly
                {"Statement_Type_ID", 1},     //default to individual
                {"Statement_Method_ID", 2},   //default to email/online
                {"Setup_Date", setupDate},    //default to current date/time
                {"Processor_ID", "cus_crds123456"}
            };

           _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
              It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
              It.IsAny<string>(), true)).Returns(expectedDonorId);

           var response = _fixture.CreateDonorRecord(888888, "cus_crds123456", setupDate);

           _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donorPageId, expectedValues, It.IsAny<string>(), true));

            Assert.AreEqual(response, expectedDonorId);

        }

        [Test]
        public void CreateDonorRecordWithNonDefaultValuesTest()
        {
            var donorPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donors"]);
            var expectedDonorId = 585858;
            var setupDate = DateTime.Now;

            var expectedValues = new Dictionary<string, object>
            {
                {"Contact_ID", 888888},
                {"Statement_Frequency_ID", 5},//default to quarterly
                {"Statement_Type_ID", 6},     //default to individual
                {"Statement_Method_ID", 7},   //default to email/online
                {"Setup_Date", setupDate},    //default to current date/time
                {"Processor_ID", "cus_crds123456"}    
            };

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
               It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
               It.IsAny<string>(), true)).Returns(expectedDonorId);

            var response = _fixture.CreateDonorRecord(888888, "cus_crds123456", setupDate, 5, 6, 7);

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donorPageId, expectedValues, It.IsAny<string>(), true));

            Assert.AreEqual(response, expectedDonorId);

        }

        [Test]
        public void CreateDonationAndDistributionRecord()
        {
            var donationAmt = 676767;
            var donorId = 1234567;
            var programId = "3";
            var setupDate = DateTime.Now;
            var charge_id = "ch_crds1234567";
            var pymt_type = "cc";
            var expectedDonationId = 321321;
            var expectedDonationDistributionId = 231231;
            var donationPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donations"]);

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
                {"Transaction_code", charge_id},
                {"Registered_Donor", true}
            };

            var response = _fixture.CreateDonationAndDistributionRecord(donationAmt, donorId, programId, charge_id, pymt_type, setupDate, true);

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationPageId, expectedDonationValues, It.IsAny<string>(), true));

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual(response, expectedDonationDistributionId);
        }

        [Test]
        public void shouldUpdatePaymentProcessorCustomerId()
        {
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(299, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));

            var response = _fixture.UpdatePaymentProcessorCustomerId(123, "456");

            _ministryPlatformService.Verify(mocked => mocked.UpdateRecord(
                299,
                It.Is<Dictionary<string, object>>(
                    d => ((int)d["Donor_ID"]) == 123
                        && ((string)d[DonorService.DONOR_PROCESSOR_ID]).Equals("456")),
                It.IsAny<string>()));
            Assert.AreEqual(123, response);
        }

        [Test]
        public void shouldThrowApplicationExceptionWhenMinistryPlatformUpdateFails()
        {
            var ex = new Exception("Oh no!!!");
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(299, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>())).Throws(ex);

            try
            {
                _fixture.UpdatePaymentProcessorCustomerId(123, "456");
                Assert.Fail("Expected exception was not thrown");
            }
            catch (Exception e)
            {
                Assert.IsInstanceOf(typeof(ApplicationException), e);
                Assert.AreSame(ex, e.InnerException);
            }
        }

        [Test]
        public void TestGetPossibleGuestDonorContact()
        {
            var donorId = 1234567;
            var processorId = "cus_431234";
            var contactId = 565656;
            var email = "cross@crossroads.net";
            var guestDonorPageViewId = "PossibleGuestDonorContact";
            var expectedDonorValues = new List<Dictionary<string, object>>();
            expectedDonorValues.Add(new Dictionary<string, object>
            {
                {"Donor_Record", donorId},
                {"Processor_ID", processorId},
                {"Contact_ID", contactId},
                {"Email_Address", email}
            });
            var donor = new ContactDonor()
            {
                DonorId = donorId,
                ProcessorId = processorId,
                ContactId = contactId,
                Email = email
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(
              guestDonorPageViewId, It.IsAny<string>(),
              It.IsAny<string>(),  "",0)).Returns(expectedDonorValues);

            var response = _fixture.GetPossibleGuestContactDonor(email);

            _ministryPlatformService.Verify(mocked => mocked.GetPageViewRecords(guestDonorPageViewId,It.IsAny<string>(), ","+email,"", 0));

            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(response.DonorId, donor.DonorId);
            Assert.AreEqual(response.ContactId, donor.ContactId);
            Assert.AreEqual(response.Email, donor.Email);
            Assert.AreEqual(response.ProcessorId, donor.ProcessorId);
        }


        [Test]
        public void TestGetDonor()
        {
            var donorId = 1234567;
            var processorId = "cus_431234";
            var contactId = 565656;
            var email = "cross@crossroads.net";
            var guestDonorPageViewId = "DonorByContactId";
            var expectedDonorValues = new List<Dictionary<string, object>>();
            expectedDonorValues.Add(new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Processor_ID", processorId},
                {"Contact_ID", contactId}
            });
            var donor = new ContactDonor()
            {
                DonorId = donorId,
                ProcessorId = processorId,
                ContactId = contactId,
                Email = email
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(
                guestDonorPageViewId, It.IsAny<string>(),
                It.IsAny<string>(), "", 0)).Returns(expectedDonorValues);

            var response = _fixture.GetContactDonor(contactId);

            _ministryPlatformService.Verify(
                mocked => mocked.GetPageViewRecords(guestDonorPageViewId, It.IsAny<string>(), contactId+",", "", 0));

            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(response.DonorId, donor.DonorId);
            Assert.AreEqual(response.ContactId, donor.ContactId);
            Assert.AreEqual(response.ProcessorId, donor.ProcessorId);
        }

        [Test]
        public void TestGetDonorNoExistingDonor()
        {
            var contactId = 565656;
            var guestDonorPageViewId = "DonorByContactId";

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(
                guestDonorPageViewId, It.IsAny<string>(),
                It.IsAny<string>(), "", 0)).Returns((List<Dictionary<string, object>>)null);

            var response = _fixture.GetContactDonor(contactId);

            _ministryPlatformService.Verify(
                mocked => mocked.GetPageViewRecords(guestDonorPageViewId, It.IsAny<string>(), contactId + ",", "", 0));

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual(contactId, response.ContactId);
            Assert.AreEqual(0, response.DonorId);
            Assert.IsNull(response.ProcessorId);
        }

    }
}
