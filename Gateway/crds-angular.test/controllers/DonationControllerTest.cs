using System;
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
    class DonationControllerTest
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
        public void testPostToCreateDonationAndDistribution()
        {
            var donorId = 394256;
            var amount = 25368;
            var donationId = 6186818;
            var program_id = "3"; //crossroads
            var donationDistributionId = 246810;
            var charge_id = "ch_crdscharge86868";

            var createDonationDTO = new CreateDonationDTO
            {
                program_id = "3", //crossroads
                amount = 86868,
                donor_id = 394256
            };

            donorServiceMock.Setup(mocked => mocked.CreateDonationAndDistributionRecord(amount, donorId, program_id, charge_id, DateTime.Now)).Returns(donationId);
            //TODO fix this up when changes for US1253 are merged in
            //IHttpActionResult result = fixture.Post(createDonationDTO);

            //Assert.IsNotNull(result);
            //Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<DonorDTO>), result);
            //var okResult = (OkNegotiatedContentResult<DonorDTO>)result;
            //Assert.AreEqual(6186818, donationId);
            
        }
    }
}
