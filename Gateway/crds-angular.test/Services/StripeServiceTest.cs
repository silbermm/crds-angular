using System;
using System.Configuration;
using System.Net;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using RestSharp;

namespace crds_angular.test.Services
{
    class StripeServiceTest
    {
        private Mock<IRestClient> restClient;
        private StripeService fixture;

        [SetUp]
        public void Setup()
        {
            restClient = new Mock<IRestClient>(MockBehavior.Strict);
            fixture = new StripeService(restClient.Object);
        }

        [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest);

            var request = new RestRequest("customers", Method.POST);
            request.AddParameter("description", "testing customers");
            request.AddParameter("source", "src");
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<RestRequest>())).Returns(stripeResponse.Object);

            Assert.Throws<StripeException>(() => fixture.createCustomer("tok_is_bad"));



        }

    }

}
