using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using Moq;
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
            var contactId = 8675309;
            var stripeCustomerId = "cus_test123456";
            var donorId = 394256;
            authenticationServiceMock.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(contactId);

            stripeServiceMock.Setup(mocked => mocked.createCustomer(It.IsAny<string>())).Returns(stripeCustomerId);

            var createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test"
            };

            donorServiceMock.Setup(mocked => mocked.CreateDonorRecord(contactId, stripeCustomerId)).Returns(donorId);
           
            IHttpActionResult result = fixture.Post(createDonorDto);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual("394256", okResult.Content.id);
            Assert.AreEqual("cus_test123456", okResult.Content.stripe_customer_id);
        }

        [Test]
        public void testPostToCreateDonationAndDistribution()
        {
            var donorId = 394256;
            var amount = 25368;
            var donationId = 6186818;
            var program = 3; //crossroads
            var donationDistributionId = 246810;

            var createDonationDto = new CreateDonationDTO
            {
                program_id = 3,//crossroads
                amount = 25368,
                donor_id = 6186818
            };

            donorServiceMock.Setup(mocked => mocked.CreateDonationRecord(amount, donorId)).Returns(donationId);
           
            donorServiceMock.Setup(mocked => mocked.CreateDonationDistributionRecord(donationId, amount,
                  program)).Returns(donationDistributionId);

            IHttpActionResult result = fixture.Post(createDonationDto);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            Assert.AreEqual(6186818, donationId);
            
        }
    }
}
