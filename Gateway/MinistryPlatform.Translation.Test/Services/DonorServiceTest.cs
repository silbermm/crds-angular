using System;
using System.Collections.Generic;
using System.Configuration;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using Newtonsoft.Json.Bson;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class DonorServiceTest
    {
        private Mock<IMinistryPlatformService> _ministryPlatformService;
        private Mock<IProgramService> _programService;
        private Mock<ICommunicationService> _communicationService;
        private DonorService _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _programService = new Mock<IProgramService>();
            _communicationService = new Mock<ICommunicationService>();

            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigValue("Bank")).Returns("5:Bank");
            configuration.Setup(mocked => mocked.GetConfigValue("CreditCard")).Returns("4:CreditCard");
            configuration.Setup(mocked => mocked.GetConfigValue("Check")).Returns("1:Check");

            _fixture = new DonorService(_ministryPlatformService.Object, _programService.Object, _communicationService.Object, configuration.Object);
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
            var feeAmt = 5656;
            var donorId = 1234567;
            var programId = "3";
            var setupDate = DateTime.Now;
            var charge_id = "ch_crds1234567";
            var processorId = "cus_8675309";
            var pymtType = "CreditCard";
            var expectedDonationId = 321321;
            var expectedDonationDistributionId = 231231;
            const string viewKey = "DonorByContactId";
            const string sortString = "";
            var searchString = "," + donorId;
            var donationPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donations"]);
            var donationDistPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Distributions"]);
           
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
              donationPageId, It.IsAny<Dictionary<string, object>>(),
              It.IsAny<string>(), true)).Returns(expectedDonationId);

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
                donationDistPageId, It.IsAny<Dictionary<string, object>>(),
                It.IsAny<string>(), true)).Returns(expectedDonationDistributionId);

            _communicationService.Setup(mocked => mocked.SendMessage(It.IsAny<Communication>()));

            var expectedDonationValues = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donation_Amount", donationAmt},
                {"Processor_Fee_Amount", feeAmt / 100M},
                {"Payment_Type_ID", "4"}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupDate},
                {"Transaction_code", charge_id},
                {"Registered_Donor", true}, 
                {"Processor_ID", processorId},
                {"Donation_Status_Date", setupDate},
                {"Donation_Status_ID", 1}
            };
            
            var expectedDistributionValues = new Dictionary<string, object>
            {
                {"Donation_ID", expectedDonationId},
                {"Amount", donationAmt},
                {"Program_ID", programId}
            };

            var programServiceResponse = new Program
            {
                CommunicationTemplateId = 1234,
                ProgramId = 3,
                Name = "Crossroads"
            };

            _programService.Setup(mocked => mocked.GetProgramById(It.IsAny<int>())).Returns(programServiceResponse);

            var dictList = new List<Dictionary<string, object>>();
            dictList.Add(new Dictionary<string, object>()
            {
                {"Email","test@test.com"},
                {"Contact_ID","1234"}
            });


            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(dictList);

            var getTemplateResponse = new MessageTemplate()
            {
                Body = "Test Body Content",
                Subject = "Test Email Subject Line"
            };
            _communicationService.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(getTemplateResponse);


            var response = _fixture.CreateDonationAndDistributionRecord(donationAmt, feeAmt, donorId, programId, charge_id, pymtType, processorId, setupDate, true);

            // Explicitly verify each expectation...
            _communicationService.Verify(mocked => mocked.SendMessage(It.IsAny<Communication>()));
            _programService.Verify(mocked => mocked.GetProgramById(3));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationPageId, expectedDonationValues, It.IsAny<string>(), true));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationDistPageId, expectedDistributionValues, It.IsAny<string>(), true));

            _ministryPlatformService.VerifyAll();
            _programService.VerifyAll();
            _communicationService.VerifyAll();
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
                {"Contact_ID", contactId},
                {"Email", email}
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

        [Test]
        public void TestSendEmail()
        {
            const string program = "Crossroads";
            const int declineEmailTemplate = 11940;
            var donationDate = DateTime.Now;
            const string emailReason = "rejected: lack of funds";
            const int donorId = 9876;
            const int donationAmt = 4343;
            const string paymentType = "Bank";

            var getTemplateResponse = new MessageTemplate()
            {
                Body = "Your payment was rejected.  Darn.",
                Subject = "Test Decline Email"
            };
            _communicationService.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(getTemplateResponse);

            _fixture.SendEmail(declineEmailTemplate, donorId, donationAmt, paymentType, donationDate, program,
                emailReason);

            _ministryPlatformService.VerifyAll();
            _communicationService.VerifyAll();
 
        }

    }
}
