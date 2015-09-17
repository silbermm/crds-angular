using crds_angular.Services;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.test.Services
{
    public class DonorServiceTest
    {
        private DonorService fixture;

        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> mpDonorService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService> mpContactService;
        private Mock<crds_angular.Services.Interfaces.IPaymentService> paymentService;
        private Mock<IConfigurationWrapper> configurationWrapper;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService> authenticationService;

        private const string GUEST_GIVER_DISPLAY_NAME = "Guest Giver";

        private const int STATEMENT_FREQUENCY_NEVER = 9;
        private const int STATEMENT_FREQUENCY_QUARTERLY = 99;
        private const int STATEMENT_TYPE_INDIVIDUAL = 8;
        private const int STATEMENT_METHOD_NONE = 7;
        private const int STATEMENT_METHOD_POSTAL_MAIL = 77;
        private const string ENCRYPTED_KEY =  "S/Hhsdgsydgs67733+jjdhjsdbnv332387uhrjfk";

        [SetUp]
        public void SetUp()
        {
            mpDonorService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>(MockBehavior.Strict);
            mpContactService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService>(MockBehavior.Strict);
            paymentService = new Mock<crds_angular.Services.Interfaces.IPaymentService>(MockBehavior.Strict);
            authenticationService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IAuthenticationService>(MockBehavior.Strict);

            configurationWrapper = new Mock<IConfigurationWrapper>();
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyNever")).Returns(STATEMENT_FREQUENCY_NEVER);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementFrequencyQuarterly")).Returns(STATEMENT_FREQUENCY_QUARTERLY);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementTypeIndividual")).Returns(STATEMENT_TYPE_INDIVIDUAL);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodNone")).Returns(STATEMENT_METHOD_NONE);
            configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementMethodPostalMail")).Returns(STATEMENT_METHOD_POSTAL_MAIL);
            configurationWrapper.Setup(mocked => mocked.GetConfigValue("GuestGiverContactDisplayName")).Returns(GUEST_GIVER_DISPLAY_NAME);

            fixture = new DonorService(mpDonorService.Object, mpContactService.Object, paymentService.Object, configurationWrapper.Object, authenticationService.Object);

        }

        [Test]
        public void shouldGetDonorForEmail()
        {
            var donor = new ContactDonor();
            mpDonorService.Setup(mocked => mocked.GetPossibleGuestContactDonor("me@here.com")).Returns(donor);
            var response = fixture.GetContactDonorForEmail("me@here.com");

            mpDonorService.VerifyAll();
            Assert.AreSame(donor, response);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void shouldGetDonorForAuthenticatedUser()
        {
            var donor = new ContactDonor();
            authenticationService.Setup(mocked => mocked.GetContactId("authToken")).Returns(123);
            mpDonorService.Setup(mocked => mocked.GetContactDonor(123)).Returns(donor);
            var response = fixture.GetContactDonorForAuthenticatedUser("authToken");

            authenticationService.VerifyAll();
            mpDonorService.VerifyAll();

            Assert.AreSame(donor, response);
        }

        [Test]
        public void shouldReturnExistingDonorWithExistingStripeId()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 67890,
                ProcessorId = "Processor_ID",
                RegisteredUser = true
            };

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(donor.ContactId, response.ContactId);
            Assert.AreEqual(donor.DonorId, response.DonorId);
            Assert.AreEqual(donor.ProcessorId, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void shouldCreateNewContactAndDonor()
        {
            var stripeCust = new StripeCustomer
            {
                id = "856",
                sources = new Sources
                {
                    data = new List<SourceData>
                    {
                       new SourceData
                        {
                            id = "123",
                            @object = "credit_card",
                            brand = "mcc",
                            last4 = "0002",
                            exp_month = "2",
                            exp_year = "2024",
                            address_zip = "10101"
                        },
                    }
                },
                default_source = "123",
            };
            
            mpContactService.Setup(mocked => mocked.CreateContactForGuestGiver("me@here.com", GUEST_GIVER_DISPLAY_NAME)).Returns(123);
            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(123, stripeCust.id, It.IsAny<DateTime>(), STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE, null)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(null, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(123, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.IsFalse(response.RegisteredUser);
        }

        [Test]
        public void shouldCreateNewDonorForExistingContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 0,
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                sources = new Sources
                {
                    data = new List<SourceData>
                    {
                       new SourceData
                        {
                            id = "123",
                            @object = "credit_card",
                            brand = "mcc",
                            last4 = "0002",
                            exp_month = "2",
                            exp_year = "2024",
                            address_zip = "10101"
                        },
                    }
                },
                default_source = "123",
            };

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), STATEMENT_FREQUENCY_NEVER, STATEMENT_TYPE_INDIVIDUAL, STATEMENT_METHOD_NONE, null)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
        }

        [Test]
        public void shouldCreateNewDonorForExistingRegisteredContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 0,
                RegisteredUser = true,
                Email = "me@here.com"
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                sources = new Sources
                {
                    data = new List<SourceData>
                    {
                       new SourceData
                        {
                            id = "123",
                            @object = "credit_card",
                            brand = "mcc",
                            last4 = "0002",
                            exp_month = "2",
                            exp_year = "2024",
                            address_zip = "10101"
                        },
                    }
                },
                default_source = "123",
            };

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, stripeCust.id, It.IsAny<DateTime>(), 1, 1, 2, null)).Returns(456);
            mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(456)).Returns(donor);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
            Assert.AreEqual(donor.RegisteredUser, response.RegisteredUser);
        }

        [Test]
        public void shouldUpdateExistingDonorForExistingContact()
        {
            var donor = new ContactDonor
            {
                ContactId = 12345,
                DonorId = 456,
            };

            var stripeCust = new StripeCustomer
            {
                id = "856",
                sources = new Sources
                {
                    data = new List<SourceData>
                    {
                       new SourceData
                        {
                            id = "123",
                            @object = "credit_card",
                            brand = "mcc",
                            last4 = "0002",
                            exp_month = "2",
                            exp_year = "2024",
                            address_zip = "10101"
                        },
                    }
                },
                default_source = "123",
            };

            paymentService.Setup(mocked => mocked.CreateCustomer("stripe_token")).Returns(stripeCust);
            mpDonorService.Setup(mocked => mocked.UpdatePaymentProcessorCustomerId(456, stripeCust.id)).Returns(456);
            paymentService.Setup(mocked => mocked.UpdateCustomerDescription(stripeCust.id, 456)).Returns("456");

            var response = fixture.CreateOrUpdateContactDonor(donor, ENCRYPTED_KEY, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual(stripeCust.id, response.ProcessorId);
        }
    }
}
