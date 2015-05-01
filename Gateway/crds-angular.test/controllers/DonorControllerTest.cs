using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;
using RestSharp.Authenticators.OAuth;
using Rhino.Mocks;

namespace crds_angular.test.controllers
{
    class DonorControllerTest
    {
        private DonorController fixture;
        private Mock<IDonorService> donorServiceMock;
        private Mock<IStripeService> stripeServiceMock;
        private Mock<IAuthenticationService> authenticationServiceMock;
        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            donorServiceMock = new Mock<IDonorService>();
            stripeServiceMock = new Mock<IStripeService>();
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
            CreateDonorDTO createDonorDto = new CreateDonorDTO
            {
                stripe_token_id = "tok_test"
            };

           // donorServiceMock.Setup(mocked => mocked.CreateDonorRecord());



            IHttpActionResult result = fixture.Post(createDonorDto);
            
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            var okResult = (OkNegotiatedContentResult<DonorDTO>)result;

            Assert.AreEqual("123456", okResult.Content.id);
            Assert.AreEqual("cus_test123456", okResult.Content.stripe_customer_id);
        }
    }
}
