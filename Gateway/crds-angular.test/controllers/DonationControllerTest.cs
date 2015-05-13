using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;

namespace crds_angular.test.controllers
{
    class DonationControllerTest
    {
        private DonationController fixture;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> donorServiceMock;
        private Mock<IPaymentService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IDonorService> gatewayDonorServiceMock;
        private string authToken;
        private string authType;

        [SetUp]
        public void SetUp()
        {
            donorServiceMock = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>();
            gatewayDonorServiceMock = new Mock<IDonorService>();
            stripeServiceMock = new Mock<IPaymentService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            fixture = new DonationController(donorServiceMock.Object, stripeServiceMock.Object,
                authenticationServiceMock.Object, gatewayDonorServiceMock.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void testPostToCreateDonationAndDistributionAuthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge_id = "ch_crdscharge86868";

            var createDonationDTO = new CreateDonationDTO
            {
                program_id = "3", //crossroads
                amount = 86868,
                donor_id = 394256
            };

            var donor = new Donor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                StripeCustomerId = "cus_test1234567"
            };

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(authType + " " + authToken)).Returns(contactId);
            
            donorServiceMock.Setup(mocked => mocked.GetDonorRecord(contactId))
                .Returns(donor);

            stripeServiceMock.Setup(
                mocked => mocked.chargeCustomer(donor.StripeCustomerId, createDonationDTO.amount, donor.DonorId))
                .Returns(charge_id);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.amount, donor.DonorId, 
                    createDonationDTO.program_id, charge_id, It.IsAny<DateTime>()))
                    .Returns(donationId);
            
            IHttpActionResult result = fixture.Post(createDonationDTO);

            authenticationServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);
            
        }

        [Test]
        public void testPostToCreateDonationAndDistributionUnauthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge_id = "ch_crdscharge86868";


            var createDonationDTO = new CreateDonationDTO
            {
                program_id = "3", //crossroads
                amount = 86868,
                donor_id = 394256,
                email_address = "test@test.com"
            };

            var donor = new Donor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                StripeCustomerId = "cus_test1234567"
            };

            fixture.Request.Headers.Authorization = null;
            gatewayDonorServiceMock.Setup(mocked => mocked.GetDonorForEmail(createDonationDTO.email_address)).Returns(donor);
            stripeServiceMock.Setup(
    mocked => mocked.chargeCustomer(donor.StripeCustomerId, createDonationDTO.amount, donor.DonorId))
    .Returns(charge_id);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.amount, donor.DonorId,
                    createDonationDTO.program_id, charge_id, It.IsAny<DateTime>()))
                    .Returns(donationId);

            IHttpActionResult result = fixture.Post(createDonationDTO);

            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

        }

    }
}
