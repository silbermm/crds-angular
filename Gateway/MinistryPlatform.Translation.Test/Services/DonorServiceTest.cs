using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Crossroads.Utilities;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Enum;
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
        private Mock<IProgramService> _programService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IAuthenticationService> _authService;
        private Mock<IConfigurationWrapper> _configuration;
        private Mock<IContactService> _contactService;
        private Mock<ICryptoProvider> _crypto;

        private DonorService _fixture;

        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _programService = new Mock<IProgramService>();
            _communicationService = new Mock<ICommunicationService>();
            _authService = new Mock<IAuthenticationService>();
            _contactService = new Mock<IContactService>();
            _crypto = new Mock<ICryptoProvider>();
            _configuration = new Mock<IConfigurationWrapper>();
            _configuration.Setup(mocked => mocked.GetConfigIntValue("MyHouseholdDonationRecurringGifts")).Returns(523);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Donors")).Returns(299);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Donations")).Returns(297);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("Distributions")).Returns(296);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("DonorAccounts")).Returns(298);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("FindDonorByAccountPageView")).Returns(2015);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("DonorLookupByEncryptedAccount")).Returns(2179);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatus")).Returns(90210);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("MyHouseholdDonationDistributions")).Returns(516);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("RecurringGifts")).Returns(45243);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("RecurringGiftBySubscription")).Returns(45208);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("DonorAccountPageView")).Returns(524);
            _configuration.Setup(mocked => mocked.GetConfigIntValue("MyHouseholdRecurringGiftsApiPageView")).Returns(45209);
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_USER")).Returns("uid");
            _configuration.Setup(m => m.GetEnvironmentVarAsString("API_PASSWORD")).Returns("pwd");

            _authService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(new Dictionary<string, object> { { "token", "ABC" }, { "exp", "123" } });

            _fixture = new DonorService(_ministryPlatformService.Object, _programService.Object, _communicationService.Object, _authService.Object, _contactService.Object, _configuration.Object, _crypto.Object);
        }

        [Test]
        public void CreateDonorRecordTest()
        {
            var donorPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donors"]);
            var expectedDonorId = 585858;
            var setupDate = DateTime.Now;
            var processorId = "cus_crds123456";
            var processorAcctId = "py_asdfghjk4434jjj";

            var expectedValues = new Dictionary<string, object>
            {
                {"Contact_ID", 888888},
                {"Statement_Frequency_ID", 1},//default to quarterly
                {"Statement_Type_ID", 1},     //default to individual
                {"Statement_Method_ID", 2},   //default to email/online
                {"Setup_Date", setupDate},    //default to current date/time
                {"Processor_ID", processorId}
            };

            var acctBytes = Encoding.UTF8.GetBytes("acctNum");
            var rtnBytes = Encoding.UTF8.GetBytes("rtn");
            var expectedEncAcct = Convert.ToBase64String(acctBytes.Concat(rtnBytes).ToArray());

            var donorAccount = new DonorAccount
            {
                AccountNumber = "123",
                RoutingNumber = "456",
                ProcessorAccountId = processorAcctId,
                EncryptedAccount = expectedEncAcct
            };
            
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(
               It.IsAny<int>(), It.IsAny<Dictionary<string, object>>(),
               It.IsAny<string>(), true)).Returns(expectedDonorId);

            var expectedAcctValues = new Dictionary<string, object>
            {
                {"Institution_Name", "Bank"},
                {"Account_Number", "0"},
                {"Routing_Number", "0"},
                {"Encrypted_Account", expectedEncAcct},
                {"Donor_ID", expectedDonorId},
                {"Non-Assignable", false},
                {"Account_Type_ID", (int)donorAccount.Type},
                {"Closed", false},
                {"Processor_Account_ID", donorAccount.ProcessorAccountId},
                {"Prcoessor_ID", processorId}
            };

            var response = _fixture.CreateDonorRecord(888888, processorId, setupDate, 1, 1, 2, donorAccount);

            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donorPageId, expectedValues, It.IsAny<string>(), true));
            _ministryPlatformService.VerifyAll();

            _crypto.VerifyAll();

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

            var response = _fixture.CreateDonorRecord(888888, "cus_crds123456", setupDate, 5, 6, 7, null);

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
            var chargeId = "ch_crds1234567";
            var processorId = "cus_8675309";
            var pymtType = "cc";
            var expectedDonationId = 321321;
            var expectedDonationDistributionId = 231231;
            var checkScannerBatchName = "check scanner batch";
            const string viewKey = "DonorByContactId";
            const string sortString = "";
            const string donorAcctId = "654";
            var searchString = ",\"" + donorId + "\"";
            var donationPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Donations"]);
            var donationDistPageId = Convert.ToInt32(ConfigurationManager.AppSettings["Distributions"]);
            const int donationStatus = 4;

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
                {"Processor_Fee_Amount", feeAmt /Constants.StripeDecimalConversionValue},
                {"Payment_Type_ID", 4}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupDate},
                {"Transaction_code", chargeId},
                {"Registered_Donor", true},
                {"Anonymous", false},
                {"Processor_ID", processorId},
                {"Donation_Status_Date", setupDate},
                {"Donation_Status_ID", donationStatus},
                {"Recurring_Gift_ID", null},
                {"Is_Recurring_Gift", false},
                {"Donor_Account_ID",donorAcctId },
                {"Check_Scanner_Batch", checkScannerBatchName}
            };

            var expectedDistributionValues = new Dictionary<string, object>
            {
                {"Donation_ID", expectedDonationId},
                {"Amount", donationAmt},
                {"Program_ID", programId},
               
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
                {"Donor_ID", donorId},
                {"Processor_ID", "tx_123"},
                {"Email","test@test.com"},
                {"Contact_ID","1234"},
                {"Statement_Type", "Individual"},
                {"Statement_Type_ID", 1},
                {"Statement_Frequency", "Quarterly"},
                {"Statement_Method", "None"},
                {"Household_ID", 1}
            });


            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(dictList);

            var getTemplateResponse = new MessageTemplate()
            {
                Body = "Test Body Content",
                Subject = "Test Email Subject Line"
            };
            _communicationService.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(getTemplateResponse);


            var response = _fixture.CreateDonationAndDistributionRecord(donationAmt, feeAmt, donorId, programId, null, chargeId, pymtType, processorId, setupDate, true, false, false, null, donorAcctId, checkScannerBatchName, donationStatus);

            // Explicitly verify each expectation...
            _communicationService.Verify(mocked => mocked.SendMessage(It.IsAny<Communication>()));
            _programService.Verify(mocked => mocked.GetProgramById(3));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationPageId, expectedDonationValues, It.IsAny<string>(), true));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationDistPageId, expectedDistributionValues, It.IsAny<string>(), true));

           // _ministryPlatformService.VerifyAll();
            _programService.VerifyAll();
            _communicationService.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual(response, expectedDonationId);
        }

        [Test]
        public void CreateDonationAndDistributionRecordWithPledge()
        {
            var donationAmt = 676767;
            var feeAmt = 5656;
            var donorId = 1234567;
            var programId = "3";
            var pledgeId = 123;
            var setupDate = DateTime.Now;
            var chargeId = "ch_crds1234567";
            var processorId = "cus_8675309";
            var pymtType = "cc";
            var expectedDonationId = 321321;
            var expectedDonationDistributionId = 231231;
            var checkScannerBatchName = "check scanner batch";
            const string viewKey = "DonorByContactId";
            const string sortString = "";
            var searchString = ",\"" + donorId + "\"";
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
                {"Processor_Fee_Amount", feeAmt /Constants.StripeDecimalConversionValue},
                {"Payment_Type_ID", 4}, //hardcoded as credit card until ACH stories are worked
                {"Donation_Date", setupDate},
                {"Transaction_code", chargeId},
                {"Registered_Donor", true},
                {"Anonymous", false},
                {"Processor_ID", processorId},
                {"Donation_Status_Date", setupDate},
                {"Donation_Status_ID", 1},
                {"Recurring_Gift_ID", null},
                {"Is_Recurring_Gift", false},
                {"Donor_Account_ID", null},
                {"Check_Scanner_Batch", checkScannerBatchName}
            };

            var expectedDistributionValues = new Dictionary<string, object>
            {
                {"Donation_ID", expectedDonationId},
                {"Amount", donationAmt},
                {"Program_ID", programId},
                {"Pledge_ID", pledgeId}
               
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
                {"Donor_ID", donorId},
                {"Processor_ID", "tx_123"},
                {"Email","test@test.com"},
                {"Contact_ID","1234"},
                {"Statement_Type", "Individual"},
                {"Statement_Type_ID", 1},
                {"Statement_Frequency", "Quarterly"},
                {"Statement_Method", "None"},
                {"Household_ID", 1}
            });


            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(viewKey, It.IsAny<string>(), searchString, sortString, 0)).Returns(dictList);

            var getTemplateResponse = new MessageTemplate()
            {
                Body = "Test Body Content",
                Subject = "Test Email Subject Line"
            };
            _communicationService.Setup(mocked => mocked.GetTemplate(It.IsAny<int>())).Returns(getTemplateResponse);


            var response = _fixture.CreateDonationAndDistributionRecord(donationAmt,
                                                                        feeAmt,
                                                                        donorId,
                                                                        programId,
                                                                        pledgeId,
                                                                        chargeId,
                                                                        pymtType,
                                                                        processorId,
                                                                        setupDate,
                                                                        true,
                                                                        false,
                                                                        false,
                                                                        null,
                                                                        null,
                                                                        checkScannerBatchName);

            // Explicitly verify each expectation...
            _communicationService.Verify(mocked => mocked.SendMessage(It.IsAny<Communication>()));
            _programService.Verify(mocked => mocked.GetProgramById(3));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationPageId, expectedDonationValues, It.IsAny<string>(), true));
            _ministryPlatformService.Verify(mocked => mocked.CreateRecord(donationDistPageId, expectedDistributionValues, It.IsAny<string>(), true));

            // _ministryPlatformService.VerifyAll();
            _programService.VerifyAll();
            _communicationService.VerifyAll();
            Assert.IsNotNull(response);
            Assert.AreEqual(response, expectedDonationId);
        }

        [Test]
        public void ShouldUpdatePaymentProcessorCustomerId()
        {
            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(299, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));

            var response = _fixture.UpdatePaymentProcessorCustomerId(123, "456");

            _ministryPlatformService.Verify(mocked => mocked.UpdateRecord(
                299,
                It.Is<Dictionary<string, object>>(
                    d => ((int)d["Donor_ID"]) == 123
                        && ((string)d[DonorService.DonorProcessorId]).Equals("456")),
                It.IsAny<string>()));
            Assert.AreEqual(123, response);
        }

        [Test]
        public void ShouldThrowApplicationExceptionWhenMinistryPlatformUpdateFails()
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
              It.IsAny<string>(), "", 0)).Returns(expectedDonorValues);

            var response = _fixture.GetPossibleGuestContactDonor(email);

            _ministryPlatformService.Verify(mocked => mocked.GetPageViewRecords(guestDonorPageViewId, It.IsAny<string>(), "," + email, "", 0));

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
                {"Email", email},
                {"Statement_Type", "Individual"},
                {"Statement_Type_ID", 1},
                {"Statement_Frequency", "Quarterly"},
                {"Statement_Method", "None"},
                {"Household_ID", 1}
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
                mocked => mocked.GetPageViewRecords(guestDonorPageViewId, It.IsAny<string>(), "\"" + contactId + "\",", "", 0));

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
                mocked => mocked.GetPageViewRecords(guestDonorPageViewId, It.IsAny<string>(), "\"" + contactId + "\",", "", 0));

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

        [Test]
        public void TestGetContactDonorForDonorAccount()
        {
            const int donorId = 1234567;
            const string processorId = "cus_431234";
            const int contactId = 565656;
            const string email = "cross@crossroads.net";
            const string guestDonorPageViewId = "DonorByContactId";
            const string accountNumber = "1234567";
            const string routingNumber = "042000314";
          
            var expectedEncAcct = _fixture.CreateHashedAccountAndRoutingNumber(accountNumber, routingNumber);
            var queryResults = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Contact_ID", contactId }
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(2015, It.IsAny<string>(), ",\"" + expectedEncAcct + "\"", "", 0)).Returns(queryResults);

            var expectedDonorValues = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donor_ID", donorId},
                    {"Processor_ID", processorId},
                    {"Contact_ID", contactId},
                    {"Email", email},
                    {"Statement_Type", "Individual"},
                    {"Statement_Type_ID", 1},
                    {"Statement_Frequency", "Quarterly"},
                    {"Statement_Method", "None"},
                    {"Household_ID", 1}
                }
            };
            var donor = new ContactDonor()
            {
                DonorId = donorId,
                ProcessorId = processorId,
                ContactId = contactId,
                Email = email
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(
                guestDonorPageViewId, It.IsAny<string>(),
                 "\"" + contactId + "\",", "", 0)).Returns(expectedDonorValues);
           
            var result = _fixture.GetContactDonorForDonorAccount(accountNumber, routingNumber);
            _ministryPlatformService.VerifyAll();
            _crypto.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.DonorId, donor.DonorId);
            Assert.AreEqual(result.ContactId, donor.ContactId);
            Assert.AreEqual(result.ProcessorId, donor.ProcessorId);
        }

        [Test]
        public void TestGetContactDonorForCheckAccount()
        {
            const int contactId = 765567;
            const int donorId = 681806;
            const string displayName = "Victoria Secret";
            const string addr1 = "25 First St";
            const string addr2 = "Suite 101";
            const string city = "San Francisco";
            const string state = "CA";
            const string zip = "91010";
            const string encryptedKey = "pink$010101@knip";
            const string donorLookupByEncryptedAccount = "DonorLookupByEncryptedAccount";

            var donorContact = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "Contact_ID", contactId },
                    {"Display_Name", displayName},
                    {"Donor_ID", donorId}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords("," + donorLookupByEncryptedAccount, It.IsAny<string>(), encryptedKey, "", 0)).Returns(donorContact);

            var myContact = new MyContact
            {
                Address_Line_1 = addr1,
                Address_Line_2 = addr2,
                City = city,
                State = state,
                Postal_Code = zip
            };

            var contactDonor = new ContactDonor
            {
                DonorId = donorId,
                Details = new ContactDetails
                {
                    DisplayName = displayName,
                    Address = new PostalAddress()
                    {
                        Line1 = addr1,
                        Line2 = addr2,
                        City = city,
                        State = state,
                        PostalCode = zip
                    }
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(
              2179, It.IsAny<string>(), "," + encryptedKey, "", 0)).Returns(donorContact);

            _contactService.Setup(mocked => mocked.GetContactById(
              contactId)).Returns(myContact);

            var result = _fixture.GetContactDonorForCheckAccount(encryptedKey);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Details.DisplayName, contactDonor.Details.DisplayName);
            Assert.AreEqual(result.Details.Address.Line1, contactDonor.Details.Address.Line1);
            Assert.AreEqual(result.Details.Address.Line2, contactDonor.Details.Address.Line2);
            Assert.AreEqual(result.Details.Address.City, contactDonor.Details.Address.City);
            Assert.AreEqual(result.Details.Address.State, contactDonor.Details.Address.State);
            Assert.AreEqual(result.Details.Address.PostalCode, contactDonor.Details.Address.PostalCode);
        }

        [Test]
        public void TestGetDonationsForContactId()
        {
            var statuses = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 1},
                    {"Display_On_Giving_History", true},
                    {"Display_On_Statements", true},
                    {"Display_On_MyTrips", true},
                    {"Donation_Status", "Pending"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 2},
                    {"Display_On_Giving_History", false},
                    {"Display_On_Statements", false},
                    {"Display_On_MyTrips", false},
                    {"Donation_Status", "Succeeded"}
                }
            };

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 1000.00M},
                    {"dp_RecordName", "Program 1"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 2000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 2},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 2200},
                    {"Payment_Type_ID", 2220},
                    {"Transaction_Code", "tx_2000"},
                    {"Amount", 2000.00M},
                    {"dp_RecordName", "Program 2"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 9000.00M},
                    {"dp_RecordName", "Program 9"},
                    {"Donor_Display_Name", "Test Name"},
                }
            };

            var searchString = ",,,,,,,,,,\"123\"";
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(296, It.IsAny<string>(), searchString, It.IsAny<string>())).Returns(records);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(90210, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(statuses);
            var result = _fixture.GetDonations(123);

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Distributions.Count);
            Assert.AreEqual(1000000, result[0].donationAmt);
            Assert.AreEqual("Program 1", result[0].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(100000, result[0].Distributions[0].donationDistributionAmt);
            Assert.AreEqual("Program 9", result[0].Distributions[1].donationDistributionProgram);
            Assert.AreEqual(900000, result[0].Distributions[1].donationDistributionAmt);

            Assert.AreEqual(1, result[1].Distributions.Count);
            Assert.AreEqual(200000, result[1].donationAmt);
            Assert.AreEqual("Program 2", result[1].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(200000, result[1].Distributions[0].donationDistributionAmt);
        }

        [Test]
        public void TestGetDonationsForAuthenticatedUser()
        {
            var statuses = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 1},
                    {"Display_On_Giving_History", true},
                    {"Display_On_Statements", true},
                    {"Display_On_MyTrips", true},
                    {"Donation_Status", "Pending"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 2},
                    {"Display_On_Giving_History", false},
                    {"Display_On_Statements", false},
                    {"Display_On_MyTrips", false},
                    {"Donation_Status", "Succeeded"}
                }
            };

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 1000.00M},
                    {"dp_RecordName", "Program 1"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 2000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 2},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 2200},
                    {"Payment_Type_ID", 2220},
                    {"Transaction_Code", "tx_2000"},
                    {"Amount", 2000.00M},
                    {"dp_RecordName", "Program 2"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 9000.00M},
                    {"dp_RecordName", "Program 9"},
                    {"Donor_Display_Name", "Test Name"},
                }
            };

            var searchString = "\"*/2015*\",True";
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(516, "auth token", searchString, It.IsAny<string>())).Returns(records);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(90210, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(statuses);
            var result = _fixture.GetDonationsForAuthenticatedUser("auth token", true, "2015");

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Distributions.Count);
            Assert.AreEqual(1000000, result[0].donationAmt);
            Assert.AreEqual("Program 1", result[0].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(100000, result[0].Distributions[0].donationDistributionAmt);
            Assert.AreEqual("Program 9", result[0].Distributions[1].donationDistributionProgram);
            Assert.AreEqual(900000, result[0].Distributions[1].donationDistributionAmt);

            Assert.AreEqual(1, result[1].Distributions.Count);
            Assert.AreEqual(200000, result[1].donationAmt);
            Assert.AreEqual("Program 2", result[1].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(200000, result[1].Distributions[0].donationDistributionAmt);
            
        }

        [Test]
        public void TestGetDonationsForContactIdAndYear()
        {
            var statuses = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 1},
                    {"Display_On_Giving_History", true},
                    {"Display_On_Statements", true},
                    {"Display_On_MyTrips", true},
                    {"Donation_Status", "Pending"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 2},
                    {"Display_On_Giving_History", false},
                    {"Display_On_Statements", false},
                    {"Display_On_MyTrips", false},
                    {"Donation_Status", "Succeeded"}
                }
            };

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 1000.00M},
                    {"dp_RecordName", "Program 1"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 2000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 2},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 2200},
                    {"Payment_Type_ID", 2220},
                    {"Transaction_Code", "tx_2000"},
                    {"Amount", 2000.00M},
                    {"dp_RecordName", "Program 2"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", null},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 9000.00M},
                    {"dp_RecordName", "Program 9"},
                    {"Donor_Display_Name", "Test Name"},
                }
            };

            var searchString = "\"*/2015*\",,,,,,,,,,\"123\" or \"456\"";
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(296, It.IsAny<string>(), searchString, It.IsAny<string>())).Returns(records);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(90210, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(statuses);
            var result = _fixture.GetDonations(new[] { 123, 456 }, "2015");

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Distributions.Count);
            Assert.AreEqual(1000000, result[0].donationAmt);
            Assert.AreEqual("Program 1", result[0].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(100000, result[0].Distributions[0].donationDistributionAmt);
            Assert.AreEqual("Program 9", result[0].Distributions[1].donationDistributionProgram);
            Assert.AreEqual(900000, result[0].Distributions[1].donationDistributionAmt);

            Assert.AreEqual(1, result[1].Distributions.Count);
            Assert.AreEqual(200000, result[1].donationAmt);
            Assert.AreEqual("Program 2", result[1].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(200000, result[1].Distributions[0].donationDistributionAmt);
        }


        [Test]
        public void TestGetSoftCreditDonations()
        {
            var statuses = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 1},
                    {"Display_On_Giving_History", true},
                    {"Display_On_Statements", true},
                    {"Display_On_MyTrips", true},
                    {"Donation_Status", "Pending"}
                },
                new Dictionary<string, object>
                {
                    {"dp_RecordID", 2},
                    {"Display_On_Giving_History", false},
                    {"Display_On_Statements", false},
                    {"Display_On_MyTrips", false},
                    {"Donation_Status", "Succeeded"}
                }
            };

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", 5511},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 1000.00M},
                    {"dp_RecordName", "Program 1"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 2000},
                    {"Soft_Credit_Donor_ID", 6612},
                    {"Donation_Status_ID", 2},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 2200},
                    {"Payment_Type_ID", 2220},
                    {"Transaction_Code", "tx_2000"},
                    {"Amount", 2000.00M},
                    {"dp_RecordName", "Program 2"},
                    {"Donor_Display_Name", "Test Name"},
                },
                new Dictionary<string, object>
                {
                    {"Donation_Date", DateTime.Now},
                    {"Donation_ID", 1000},
                    {"Soft_Credit_Donor_ID", 5511},
                    {"Donation_Status_ID", 1},
                    {"Donation_Status_Date", DateTime.Now},
                    {"Donor_ID", 1100},
                    {"Payment_Type_ID", 1110},
                    {"Transaction_Code", "tx_1000"},
                    {"Amount", 9000.00M},
                    {"dp_RecordName", "Program 9"},
                    {"Donor_Display_Name", "Test Name"},
                }
            };

            var search = string.Format(",,,,,,,,,,,,,,,,,\"{0}\"", 123);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(296, It.IsAny<string>(), search, It.IsAny<string>())).Returns(records);
            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(90210, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(statuses);
            var result = _fixture.GetSoftCreditDonations(new[] { 123 });

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Distributions.Count);
            Assert.AreEqual(1000000, result[0].donationAmt);
            Assert.AreEqual("Program 1", result[0].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(100000, result[0].Distributions[0].donationDistributionAmt);
            Assert.AreEqual("Program 9", result[0].Distributions[1].donationDistributionProgram);
            Assert.AreEqual(900000, result[0].Distributions[1].donationDistributionAmt);
            Assert.AreEqual(5511, result[0].softCreditDonorId);
            Assert.AreEqual("Test Name", result[0].donorDisplayName);

            Assert.AreEqual(1, result[1].Distributions.Count);
            Assert.AreEqual(200000, result[1].donationAmt);
            Assert.AreEqual("Program 2", result[1].Distributions[0].donationDistributionProgram);
            Assert.AreEqual(200000, result[1].Distributions[0].donationDistributionAmt);
        }

        [Test]
        // This test is pretty much a copy of the code in the service itself, but it does serve a purpose - 
        // making sure if the algorithm changes, this test will fail!
        public void TestCreateHashedAccountAndRoutingNumber()
        {
            const string routingNumber = "110000000";
            const string accountNumber = "000123456789";
            const string hashKey = "Mcc3#e758ebe8Seb1fdeF628dbK796e5";

            var crypt = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(string.Concat(routingNumber, accountNumber, hashKey));
            var crypto = crypt.ComputeHash(bytes, 0, bytes.Length);
            var hashString = Convert.ToBase64String(crypto).Replace('/', '~');

            var result = _fixture.CreateHashedAccountAndRoutingNumber(accountNumber, routingNumber);
            Assert.AreEqual(hashString, result);
        }

        public void TestCreateRecurringGiftRecord()
        {
            const int donorId = 123;
            const int donorAccountId = 456;
            const string planInterval = "week";
            const decimal planAmount = 789.10M;
            var startDate = DateTime.Today;
            const string program = "555";
            const string subscriptionId = "sub_123";
            const int congregationId = 1;

            const int recurringGiftId = 987;

            var expectedParms = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donor_Account_ID", donorAccountId},
                {"Frequency_ID", 1},
                {"Day_Of_Month", (int?)null},
                {"Day_Of_Week_ID", NumericDayOfWeek.GetDayOfWeekID((startDate.DayOfWeek).ToString())},
                {"Amount", planAmount},
                {"Start_Date", startDate},
                {"Program_ID", program},
                {"Congregation_ID", congregationId},
                {"Subscription_ID", subscriptionId}
            };

            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(45243, expectedParms, It.IsAny<string>(), true)).Returns(recurringGiftId);

            var result = _fixture.CreateRecurringGiftRecord("auth", donorId, donorAccountId, planInterval, planAmount, startDate, program, subscriptionId, congregationId);
            _ministryPlatformService.VerifyAll();

            Assert.AreEqual(recurringGiftId, result);
        }

        [Test]
        public void TestGetRecurringGiftForSubscription()
        {
            const decimal amt = 456;

            var lookupResult = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Donor_ID", 123},
                    {"Amount", amt},
                    {"Program_ID", "444"},
                    {"Congregation_ID", 555},
                    {"Account_Type_ID", 3},
                    {"Recurring_Gift_ID", 4},
                    {"Frequency_ID", 1},
                    {"Subscription_ID", "sub_asdf1234"},
                    {"Donor_Account_ID", 5},
                    {"Consecutive_Failure_Count", 1}
                }
            };
            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(45208, It.IsAny<string>(), ",\"sub_123\",", string.Empty, 0)).Returns(lookupResult);

            var result = _fixture.GetRecurringGiftForSubscription("sub_123");
            _ministryPlatformService.VerifyAll();
            Assert.NotNull(result);
            Assert.AreEqual(123, result.DonorId);
            Assert.AreEqual(456, result.Amount);
            Assert.AreEqual("444", result.ProgramId);
            Assert.AreEqual(555, result.CongregationId);
            Assert.AreEqual(PaymentType.CreditCard.abbrv, result.PaymentType);
            Assert.AreEqual(4, result.RecurringGiftId);
            Assert.AreEqual(5, result.DonorAccountId);
        }

        [Test]
        public void TestCreateDonorAccountForBankAccount()
        {
            const string giftType = "Bank";
            const string routingNumber = "110000000";
            const string accountNumber = "7890";
            const int donorId = 123;
            const string processorAcctId = "ba_123";
            const string processorId = "cust_123";
            var encrypt = _fixture.CreateHashedAccountAndRoutingNumber(accountNumber, routingNumber);

            const int donorAccountId = 456;

            var expectedParms = new Dictionary<string, object>
            {
                {"Institution_Name", "Bank"},
                {"Account_Number", accountNumber},
                {"Routing_Number", "0"},
                {"Encrypted_Account", _fixture.CreateHashedAccountAndRoutingNumber(accountNumber, routingNumber)},
                {"Donor_ID", donorId},
                {"Non-Assignable", false},
                {"Account_Type_ID", (int)AccountType.Checking},
                {"Closed", false},
                {"Processor_Account_ID", processorAcctId},
                {"Processor_ID", processorId}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(298, expectedParms, It.IsAny<string>(), false)).Returns(donorAccountId);

            var result = _fixture.CreateDonorAccount(giftType, routingNumber, accountNumber, encrypt, donorId, processorAcctId, processorId);
            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(donorAccountId, result);

        }

        [Test]
        public void TestCreateDonorAccountForCreditCard()
        {
            const string giftType = "Visa";
            const string routingNumber = "0";
            const string accountNumber = "7890";
            const int donorId = 123;
            const string processorAcctId = "ba_123";
            const string processorId = "cust_123";

            const int donorAccountId = 456;

            var expectedParms = new Dictionary<string, object>
            {
                {"Institution_Name", giftType},
                {"Account_Number", "7890"},
                {"Routing_Number", "0"},
                {"Encrypted_Account", null},
                {"Donor_ID", donorId},
                {"Non-Assignable", false},
                {"Account_Type_ID", (int)AccountType.Credit},
                {"Closed", false},
                {"Processor_Account_ID", processorAcctId},
                {"Processor_ID", processorId}
            };
            _ministryPlatformService.Setup(mocked => mocked.CreateRecord(298, expectedParms, It.IsAny<string>(), false)).Returns(donorAccountId);

            var result = _fixture.CreateDonorAccount(giftType, routingNumber, accountNumber, null, donorId, processorAcctId, processorId);
            _ministryPlatformService.VerifyAll();
            Assert.AreEqual(donorAccountId, result);

        }

        [Test]
        public void TestGetRecurringGiftsForAuthenticatedUser()
        {
            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Recurring_Gift_ID", 123},
                    {"Donor_ID", 123123},
                    {"User_Email", "test@example.com"},
                    {"Frequency", "Weekly"},
                    {"Recurrence", "Fridays Weekly"},
                    {"Start_Date", DateTime.Now},
                    {"End_Date", DateTime.Now},
                    {"Amount", 950.0M},
                    {"Program_ID", 2},
                    {"Program_Name", "Beans & Rice"},
                    {"Congregation_Name", "Upton"},
                    {"Account_Type_ID", 3},
                    {"Account_Number", "4433"},
                    {"Institution_Name", "Visa"},
                    {"Subscription_ID", "sub_77L7hDGjQdoxRE"},
                    {"Processor_ID", "cus_123"},
                    {"Processor_Account_ID", "card_123"}
                },
                new Dictionary<string, object>
                {
                    {"Recurring_Gift_ID", 124},
                    {"Donor_ID", 123123},
                    {"User_Email", "test@example.com"},
                    {"Frequency", "Montly"},
                    {"Recurrence", "8th Monthly"},
                    {"Start_Date", DateTime.Now},
                    {"End_Date", DateTime.Now},
                    {"Amount", 190.0M},
                    {"Program_ID", 1},
                    {"Program_Name", "Crossroads"},
                    {"Congregation_Name", ""},
                    {"Account_Type_ID", 1},
                    {"Account_Number", "4093"},
                    {"Institution_Name", "Bank"},
                    {"Subscription_ID", "sub_77uaEIZLssR6xN"},
                    {"Processor_ID", "cus_456"},
                    {"Processor_Account_ID", "card_456"}
                },
                new Dictionary<string, object>
                {
                    {"Recurring_Gift_ID", 125},
                    {"Donor_ID", 123123},
                    {"User_Email", "test@example.com"},
                    {"Frequency", "Weekly"},
                    {"Recurrence", "Tuesdays Weekly"},
                    {"Start_Date", DateTime.Now},
                    {"End_Date", DateTime.Now},
                    {"Amount", 50.0M},
                    {"Program_ID", 3},
                    {"Program_Name", "Old St. George"},
                    {"Congregation_Name", "General"},
                    {"Account_Type_ID", 3},
                    {"Account_Number", "1984"},
                    {"Institution_Name", "American Express"},
                    {"Subscription_ID", "sub_77L8qFUF6QFZsO"},
                    {"Processor_ID", "cus_789"},
                    {"Processor_Account_ID", "card_789"}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetRecordsDict(523, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(records);
            var result = _fixture.GetRecurringGiftsForAuthenticatedUser("afdafsaaatewjrtjeretewtr");

            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(records[0]["Recurring_Gift_ID"], result[0].RecurringGiftId);
            Assert.AreEqual(records[0]["Donor_ID"], result[0].DonorID);
            Assert.AreEqual(records[0]["User_Email"], result[0].EmailAddress);
            Assert.AreEqual(records[0]["Frequency"], result[0].Frequency);
            Assert.AreEqual(records[0]["Recurrence"], result[0].Recurrence);
            Assert.AreEqual(records[0]["Start_Date"], result[0].StartDate);
            Assert.AreEqual(records[0]["End_Date"], result[0].EndDate);
            Assert.AreEqual(records[0]["Amount"], result[0].Amount);
            Assert.AreEqual(records[0]["Program_ID"], result[0].ProgramID);
            Assert.AreEqual(records[0]["Program_Name"], result[0].ProgramName);
            Assert.AreEqual(records[0]["Congregation_Name"], result[0].CongregationName);
            Assert.AreEqual(records[0]["Account_Type_ID"], result[0].AccountTypeID);
            Assert.AreEqual(records[0]["Account_Number"], result[0].AccountNumberLast4);
            Assert.AreEqual(records[0]["Institution_Name"], result[0].InstitutionName);
            Assert.AreEqual(records[0]["Subscription_ID"], result[0].SubscriptionID);
            Assert.AreEqual(records[0]["Processor_ID"], result[0].ProcessorId);
            Assert.AreEqual(records[0]["Processor_Account_ID"], result[0].ProcessorAccountId);

            Assert.AreEqual(records[1]["Recurring_Gift_ID"], result[1].RecurringGiftId);
            Assert.AreEqual(records[1]["Donor_ID"], result[1].DonorID);
            Assert.AreEqual(records[1]["User_Email"], result[1].EmailAddress);
            Assert.AreEqual(records[1]["Frequency"], result[1].Frequency);
            Assert.AreEqual(records[1]["Recurrence"], result[1].Recurrence);
            Assert.AreEqual(records[1]["Start_Date"], result[1].StartDate);
            Assert.AreEqual(records[1]["End_Date"], result[1].EndDate);
            Assert.AreEqual(records[1]["Amount"], result[1].Amount);
            Assert.AreEqual(records[1]["Program_ID"], result[1].ProgramID);
            Assert.AreEqual(records[1]["Program_Name"], result[1].ProgramName);
            Assert.AreEqual(records[1]["Congregation_Name"], result[1].CongregationName);
            Assert.AreEqual(records[1]["Account_Type_ID"], result[1].AccountTypeID);
            Assert.AreEqual(records[1]["Account_Number"], result[1].AccountNumberLast4);
            Assert.AreEqual(records[1]["Institution_Name"], result[1].InstitutionName);
            Assert.AreEqual(records[1]["Subscription_ID"], result[1].SubscriptionID);
            Assert.AreEqual(records[1]["Processor_ID"], result[1].ProcessorId);
            Assert.AreEqual(records[1]["Processor_Account_ID"], result[1].ProcessorAccountId);

            Assert.AreEqual(records[2]["Recurring_Gift_ID"], result[2].RecurringGiftId);
            Assert.AreEqual(records[2]["Donor_ID"], result[2].DonorID);
            Assert.AreEqual(records[2]["User_Email"], result[2].EmailAddress);
            Assert.AreEqual(records[2]["Frequency"], result[2].Frequency);
            Assert.AreEqual(records[2]["Recurrence"], result[2].Recurrence);
            Assert.AreEqual(records[2]["Start_Date"], result[2].StartDate);
            Assert.AreEqual(records[2]["End_Date"], result[2].EndDate);
            Assert.AreEqual(records[2]["Amount"], result[2].Amount);
            Assert.AreEqual(records[2]["Program_ID"], result[2].ProgramID);
            Assert.AreEqual(records[2]["Program_Name"], result[2].ProgramName);
            Assert.AreEqual(records[2]["Congregation_Name"], result[2].CongregationName);
            Assert.AreEqual(records[2]["Account_Type_ID"], result[2].AccountTypeID);
            Assert.AreEqual(records[2]["Account_Number"], result[2].AccountNumberLast4);
            Assert.AreEqual(records[2]["Institution_Name"], result[2].InstitutionName);
            Assert.AreEqual(records[2]["Subscription_ID"], result[2].SubscriptionID);
            Assert.AreEqual(records[2]["Processor_ID"], result[2].ProcessorId);
            Assert.AreEqual(records[2]["Processor_Account_ID"], result[2].ProcessorAccountId);
        }

        [Test]
        public void TestGetRecurringGiftById()
        {
            const string authUserToken = "auth";
            const int recurringGiftId = 123;

            const int donorId = 456;
            const int frequencyId = 789;
            const int dayOfWeekId = 1;
            const int dayOfMonth = 15;
            var startDate = DateTime.Parse("1973-10-15");
            const decimal amount = 123.45M;
            const int programId = 4;
            const int congragationId = 5;
            const int accountTypeId = 2;
            const int donorAccountId = 987;
            const string subscriptionId = "sub_123";
            const string processorId = "cus_123";
            const string accountId = "card_456";
            const string recurrence = "Monthly";
            const string programName = "Game Change";

            var records = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Recurring_Gift_ID", recurringGiftId},
                    {"Donor_ID", donorId},
                    {"Frequency_ID", frequencyId},
                    {"Day_Of_Week_ID", dayOfWeekId},
                    {"Day_Of_Month", dayOfMonth},
                    {"Start_Date", startDate},
                    {"Amount", amount},
                    {"Program_ID", programId},
                    {"Congregation_ID", congragationId},
                    {"Account_Type_ID", accountTypeId},
                    {"Donor_Account_ID", donorAccountId},
                    {"Subscription_ID", subscriptionId},
                    {"Processor_ID", processorId},
                    {"Processor_Account_ID", accountId},
                    {"Recurrence", recurrence},
                    {"Program_Name", programName}
                }
            };

            _ministryPlatformService.Setup(mocked => mocked.GetPageViewRecords(45209, authUserToken, "\"123\",", string.Empty, 0)).Returns(records);

            var result = _fixture.GetRecurringGiftById(authUserToken, recurringGiftId);
            _ministryPlatformService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(recurringGiftId, result.RecurringGiftId);
            Assert.AreEqual(donorId, result.DonorId);
            Assert.AreEqual(frequencyId, result.Frequency);
            Assert.AreEqual(dayOfWeekId, result.DayOfWeek);
            Assert.AreEqual(dayOfMonth, result.DayOfMonth);
            Assert.AreEqual(startDate, result.StartDate);
            Assert.AreEqual(amount * 100, result.Amount);
            Assert.AreEqual(programId+"", result.ProgramId);
            Assert.AreEqual(congragationId, result.CongregationId);
            Assert.AreEqual(PaymentType.CreditCard.abbrv, result.PaymentType);
            Assert.AreEqual(donorAccountId, result.DonorAccountId);
            Assert.AreEqual(subscriptionId, result.SubscriptionId);
            Assert.AreEqual(processorId, result.StripeCustomerId);
            Assert.AreEqual(accountId, result.StripeAccountId);
            Assert.AreEqual(recurrence, result.Recurrence);
            Assert.AreEqual(programName, result.ProgramName);
        }

        [Test]
        public void TestCancelRecurringGift()
        {
            const string authUserToken = "auth";
            const int recurringGiftId = 123;

            var expectedParms = new Dictionary<string, object>
            {
                { "End_Date", DateTime.Now.Date },
                { "Recurring_Gift_ID", recurringGiftId}
            };

            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(523, expectedParms, authUserToken));

            _fixture.CancelRecurringGift(authUserToken, recurringGiftId);
            _ministryPlatformService.VerifyAll();
        }

        [Test]
        public void TestGetDonorAccountPymtType()
        {
            const int donorAcct = 123;
            const int donorId = 12345;
            const int acct = 2;

            var donorAccount = new Dictionary<string, object>
            {
                {"Donor_ID", donorId},
                {"Donor_Account_ID", donorAcct},
                {"Account_Type_ID", acct}
            };

            _ministryPlatformService.Setup(mocked => mocked.GetRecordDict(298, donorAcct, It.IsAny<string>(), false)).Returns(donorAccount);
            var result = _fixture.GetDonorAccountPymtType(donorAcct);

            _ministryPlatformService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(donorAccount["Account_Type_ID"], result);
        }

        public void TestUpdateRecurringGiftDonorAccount()
        {
            const string authUserToken = "auth";
            const int recurringGiftId = 123;
            const int donorAccountId = 123;

            var expectedParms = new Dictionary<string, object>
            {
                { "Donor_Account_ID", donorAccountId },
                { "Recurring_Gift_ID", recurringGiftId}
            };

            _ministryPlatformService.Setup(mocked => mocked.UpdateRecord(523, expectedParms, authUserToken));

            _fixture.UpdateRecurringGiftDonorAccount(authUserToken, recurringGiftId, donorAccountId);
            _ministryPlatformService.VerifyAll();
        }
    }
}
