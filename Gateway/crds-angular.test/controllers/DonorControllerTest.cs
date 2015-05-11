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
using NSubstitute;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    class DonorControllerTest
    {
        private DonorController fixture;
        private Mock<IDonorService> donorServiceMock;
        private Mock<IPaymentService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private string authType;
        private string authToken;
        private static int contactId = 8675309;
        private static string stripeCustomerId = "cus_test123456";
        private static string email = "automatedtest@crossroads.net";
        private static int donorId = 394256;
        private Donor donor = new Donor()
        {
            DonorId = 1,
            StripeCustomerId = "1",
            ContactId = 1,
            Email = "1"
        };

        [SetUp]
        public void SetUp()
        {
            donorServiceMock = new Mock<IDonorService>();
            stripeServiceMock = new Mock<IPaymentService>();
            authenticationServiceMock = new Mock<IAuthenticationService>();
            fixture = new DonorController(donorServiceMock.Object, stripeServiceMock.Object,
                authenticationServiceMock.Object);

            authType = "auth_type";
            authToken = "auth_token";
            fixture.Request = new HttpRequestMessage();
            fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void TestPostToSuccessfullyCreateDonor()
        {
            authenticationServiceMock.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(contactId);

            stripeServiceMock.Setup(mocked => mocked.createCustomer(It.IsAny<string>())).Returns(stripeCustomerId);

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test"
            };

            donorServiceMock.Setup(mocked => mocked.CreateDonorRecord(contactId, stripeCustomerId, It.IsAny<DateTime>())).Returns(donorId);
           
            IHttpActionResult result = fixture.Post(createDonorDto);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.id);
            Assert.AreEqual(stripeCustomerId, okResult.Content.stripe_customer_id);
        }

        [Test]
        public void TestGetSuccessGetDonorAuthenticated()
        {
            Donor donor = new Donor();
            donor.DonorId = 1;
            authenticationServiceMock.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(contactId);
            donorServiceMock.Setup(mocked => mocked.GetPossibleGuestDonorContact("foo").Returns(new Donor()));
            IHttpActionResult result = fixture.Get();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(donorId, okResult.Content.id);
            Assert.AreEqual(stripeCustomerId, okResult.Content.stripe_customer_id);
        }
    }
}
