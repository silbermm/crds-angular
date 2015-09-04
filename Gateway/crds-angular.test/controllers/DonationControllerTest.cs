using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;
using IDonationService = crds_angular.Services.Interfaces.IDonationService;

namespace crds_angular.test.controllers
{
    class DonationControllerTest
    {
        private DonationController fixture;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> donorServiceMock;
        private Mock<IPaymentService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<IDonorService> gatewayDonorServiceMock;
        private Mock<IDonationService> gatewayDonationServiceMock;
        private string authToken;
        private string authType;

        [SetUp]
        public void SetUp()
        {
            donorServiceMock = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>();
            gatewayDonorServiceMock = new Mock<IDonorService>();
            stripeServiceMock = new Mock<IPaymentService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            gatewayDonationServiceMock = new Mock<IDonationService>();

            fixture = new DonationController(donorServiceMock.Object, stripeServiceMock.Object,
                authenticationServiceMock.Object, gatewayDonorServiceMock.Object, gatewayDonationServiceMock.Object);

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
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };

            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            authenticationServiceMock.Setup(mocked => mocked.GetContactId(authType + " " + authToken)).Returns(contactId);

            donorServiceMock.Setup(mocked => mocked.GetContactDonor(contactId))
                .Returns(donor);

            stripeServiceMock.Setup(
                mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId))
                .Returns(charge);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), true, null))
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

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>) result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.email);
        }

        [Test]
        public void testPostToCreateDonationAndDistributionUnauthenticated()
        {
            var contactId = 999999;
            var donationId = 6186818;
            var charge = new StripeCharge()
            {
                Id = "ch_crdscharge86868",
                BalanceTransaction = new StripeBalanceTransaction()
                {
                    Fee = 987
                }
            };
           
            var createDonationDTO = new CreateDonationDTO
            {
                ProgramId = "3", //crossroads
                Amount = 86868,
                DonorId = 394256,
                EmailAddress = "test@test.com"
            };

            var donor = new ContactDonor
            {
                ContactId = contactId,
                DonorId = 424242,
                SetupDate = new DateTime(),
                StatementFreq = "1",
                StatementMethod = "2",
                StatementType = "3",
                ProcessorId = "cus_test1234567",
                Email = "moc.tset@tset"
            };

            fixture.Request.Headers.Authorization = null;
            gatewayDonorServiceMock.Setup(mocked => mocked.GetContactDonorForEmail(createDonationDTO.EmailAddress)).Returns(donor);
            
            stripeServiceMock.Setup(mocked => mocked.ChargeCustomer(donor.ProcessorId, createDonationDTO.Amount, donor.DonorId)).
                Returns(charge);

            donorServiceMock.Setup(mocked => mocked.
                CreateDonationAndDistributionRecord(createDonationDTO.Amount, charge.BalanceTransaction.Fee, donor.DonorId,
                    createDonationDTO.ProgramId, charge.Id, createDonationDTO.PaymentType, donor.ProcessorId, It.IsAny<DateTime>(), false, null))
                    .Returns(donationId);

            IHttpActionResult result = fixture.Post(createDonationDTO);

            donorServiceMock.VerifyAll();
            stripeServiceMock.VerifyAll();
            donorServiceMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonationDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonationDTO>)result;
            Assert.AreEqual(6186818, donationId);

            var resultDto = ((OkNegotiatedContentResult<DonationDTO>)result).Content;
            Assert.IsNotNull(resultDto);
            Assert.AreEqual(donor.Email, resultDto.email);
        }

    }
}
