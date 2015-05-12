using Moq;
using NUnit.Framework;
using crds_angular.Services;
using MinistryPlatform.Models;
using System;

namespace crds_angular.test.Services
{
    public class DonorServiceTest
    {
        private DonorService fixture;

        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> mpDonorService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService> mpContactService;
        private Mock<crds_angular.Services.Interfaces.IPaymentService> paymentService;

        [SetUp]
        public void SetUp()
        {
            mpDonorService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>(MockBehavior.Strict);
            mpContactService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IContactService>(MockBehavior.Strict);
            paymentService = new Mock<crds_angular.Services.Interfaces.IPaymentService>(MockBehavior.Strict);

            fixture = new DonorService(mpDonorService.Object, mpContactService.Object, paymentService.Object);
        }

        [Test]
        public void shouldGetDonorForEmail()
        {
            var donor = new Donor();
            mpDonorService.Setup(mocked => mocked.GetPossibleGuestDonorContact("me@here.com")).Returns(donor);
            var response = fixture.GetDonorForEmail("me@here.com");

            mpDonorService.VerifyAll();
            Assert.AreSame(donor, response);
        }

        [Test]
        public void shouldReturnExistingDonorWithExistingStripeId()
        {
            var donor = new Donor
            {
                ContactId = 12345,
                DonorId = 67890,
                StripeCustomerId = "stripe_cust_id"
            };

            var response = fixture.CreateDonor(donor, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(donor.ContactId, response.ContactId);
            Assert.AreEqual(donor.DonorId, response.DonorId);
            Assert.AreEqual(donor.StripeCustomerId, response.StripeCustomerId);
        }

        [Test]
        public void shouldCreateNewContactAndDonor()
        {
            mpContactService.Setup(mocked => mocked.CreateContactForGuestGiver("me@here.com", DonorService.GUEST_GIVER_DISPLAY_NAME)).Returns(123);
            paymentService.Setup(mocked => mocked.createCustomer("stripe_token")).Returns("stripe_cust_id");
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(123, "stripe_cust_id", It.IsAny<DateTime>())).Returns(456);

            var response = fixture.CreateDonor(null, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(123, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual("stripe_cust_id", response.StripeCustomerId);
        }

        [Test]
        public void shouldCreateNewDonorForExistingContact()
        {
            var donor = new Donor
            {
                ContactId = 12345,
                DonorId = 0,
            };

            paymentService.Setup(mocked => mocked.createCustomer("stripe_token")).Returns("stripe_cust_id");
            mpDonorService.Setup(mocked => mocked.CreateDonorRecord(12345, "stripe_cust_id", It.IsAny<DateTime>())).Returns(456);

            var response = fixture.CreateDonor(donor, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual("stripe_cust_id", response.StripeCustomerId);
        }

        [Test]
        public void shouldUpdateExistingDonorForExistingContact()
        {
            var donor = new Donor
            {
                ContactId = 12345,
                DonorId = 456,
            };

            paymentService.Setup(mocked => mocked.createCustomer("stripe_token")).Returns("stripe_cust_id");
            mpDonorService.Setup(mocked => mocked.UpdatePaymentProcessorCustomerId(456, "stripe_cust_id")).Returns(456);

            var response = fixture.CreateDonor(donor, "me@here.com", "stripe_token", DateTime.Now);

            mpDonorService.VerifyAll();
            mpContactService.VerifyAll();
            paymentService.VerifyAll();

            Assert.AreEqual(12345, response.ContactId);
            Assert.AreEqual(456, response.DonorId);
            Assert.AreEqual("stripe_cust_id", response.StripeCustomerId);
        }
    }
}
