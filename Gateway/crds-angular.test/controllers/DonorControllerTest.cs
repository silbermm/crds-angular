using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace crds_angular.test.controllers
{
    class DonorControllerTest
    {
        private DonorController fixture;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> mpDonorServiceMock;
        private Mock<IPaymentService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private Mock<crds_angular.Services.Interfaces.IDonorService> donorService;
        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            mpDonorServiceMock = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>();
            stripeServiceMock = new Mock<IPaymentService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            donorService = new Mock<crds_angular.Services.Interfaces.IDonorService>();
            fixture = new DonorController(mpDonorServiceMock.Object, stripeServiceMock.Object,
                authenticationServiceMock.Object, donorService.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void TestPostToSuccessfullyCreateAuthenticatedDonor()
        {
            var contactId = 8675309;
            var stripeCustomerId = "cus_test123456";
            var donorId = 394256;
            authenticationServiceMock.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(contactId);

            stripeServiceMock.Setup(mocked => mocked.createCustomer(It.IsAny<string>())).Returns(stripeCustomerId);

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test"
            };

            mpDonorServiceMock.Setup(mocked => mocked.CreateDonorRecord(contactId, stripeCustomerId, It.IsAny<DateTime>())).Returns(donorId);
           
            IHttpActionResult result = fixture.Post(createDonorDto);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual("394256", okResult.Content.id);
            Assert.AreEqual("cus_test123456", okResult.Content.stripe_customer_id);
        }

        [Test]
        public void testPostToSuccessfullyCreateGuestDonor()
        {
            fixture.Request.Headers.Authorization = null;

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test",
                email_address = "me@here.com"
            };

            var lookupDonor = new Donor
            {
                ContactId = 8675309,
                DonorId = 0,
                StripeCustomerId = "cus_test123456"
            };

            var createDonor = new Donor
            {
                ContactId = 8675309,
                DonorId = 394256,
                StripeCustomerId = "cus_test123456"
            };

            donorService.Setup(mocked => mocked.getDonorForEmail(createDonorDto.email_address)).Returns(lookupDonor);
            donorService.Setup(mocked => mocked.createDonor(It.Is<Donor>(d => d == lookupDonor), createDonorDto.email_address, createDonorDto.stripe_token_id, It.IsAny<DateTime>())).Returns(createDonor);

            IHttpActionResult result = fixture.Post(createDonorDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual("394256", okResult.Content.id);
            Assert.AreEqual("cus_test123456", okResult.Content.stripe_customer_id);
        }
    }
}
